import { create } from 'zustand'
import { wait } from '../../shared/utils/wait'
import { createId } from '../../shared/utils/id'
import { loadState, saveState } from '../../shared/utils/storage'
import { boardSeed } from './seed'
import type { BoardState, CardId, ColumnId } from './types'

const INITIAL_LOADING_MS = 5000
export const BOARD_STORAGE_KEY = 'kanban-board-state'

type AddCardInput = {
  title: string
  description?: string
}

type UpdateCardPatch = {
  title?: string
  description?: string
}

type BoardStore = {
  board: BoardState
  isLoading: boolean
  isInitializing: boolean
  hasInitialized: boolean
  lastSavedAt: number
  init: () => Promise<void>
  addCard: (columnId: ColumnId, data: AddCardInput) => void
  updateCard: (cardId: CardId, patch: UpdateCardPatch) => void
  deleteCard: (cardId: CardId) => void
  moveCard: (cardId: CardId, fromColumnId: ColumnId, toColumnId: ColumnId, toIndex: number) => void
  reorderWithinColumn: (columnId: ColumnId, fromIndex: number, toIndex: number) => void
}

function cloneBoardState(state: BoardState): BoardState {
  return JSON.parse(JSON.stringify(state)) as BoardState
}

function clampIndex(index: number, max: number): number {
  return Math.max(0, Math.min(index, max))
}

function persistBoardState(state: BoardState): void {
  saveState(BOARD_STORAGE_KEY, state)
}

export const useBoardStore = create<BoardStore>((set, get) => ({
  board: cloneBoardState(boardSeed),
  isLoading: true,
  isInitializing: false,
  hasInitialized: false,
  lastSavedAt: 0,

  init: async () => {
    if (get().hasInitialized || get().isInitializing) {
      return
    }

    set({ isInitializing: true })
    await wait(INITIAL_LOADING_MS)

    const persisted = loadState<BoardState>(BOARD_STORAGE_KEY)
    const board = persisted ?? cloneBoardState(boardSeed)
    persistBoardState(board)

    set({
      board,
      isLoading: false,
      isInitializing: false,
      hasInitialized: true,
    })
  },

  addCard: (columnId, data) => {
    const trimmedTitle = data.title.trim()
    if (!trimmedTitle) {
      return
    }

    set((state) => {
      const column = state.board.columns[columnId]
      if (!column) {
        return state
      }

      const cardId = createId('card')
      const nextBoard: BoardState = {
        ...state.board,
        cards: {
          ...state.board.cards,
          [cardId]: {
            id: cardId,
            title: trimmedTitle,
            description: data.description?.trim() || undefined,
            createdAt: new Date().toISOString(),
          },
        },
        columns: {
          ...state.board.columns,
          [columnId]: {
            ...column,
            cardIds: [...column.cardIds, cardId],
          },
        },
      }

      persistBoardState(nextBoard)
      return {
        board: nextBoard,
        lastSavedAt: Date.now(),
      }
    })
  },

  updateCard: (cardId, patch) => {
    set((state) => {
      const card = state.board.cards[cardId]
      if (!card) {
        return state
      }

      const nextTitle = patch.title?.trim()
      const nextBoard: BoardState = {
        ...state.board,
        cards: {
          ...state.board.cards,
          [cardId]: {
            ...card,
            ...(typeof patch.title === 'string' ? { title: nextTitle || card.title } : {}),
            ...(Object.prototype.hasOwnProperty.call(patch, 'description')
              ? { description: patch.description?.trim() || undefined }
              : {}),
          },
        },
      }

      persistBoardState(nextBoard)
      return {
        board: nextBoard,
        lastSavedAt: Date.now(),
      }
    })
  },

  deleteCard: (cardId) => {
    set((state) => {
      if (!state.board.cards[cardId]) {
        return state
      }

      const nextCards = { ...state.board.cards }
      delete nextCards[cardId]

      const nextColumns = Object.entries(state.board.columns).reduce(
        (acc, [id, column]) => {
          acc[id] = {
            ...column,
            cardIds: column.cardIds.filter((idInColumn) => idInColumn !== cardId),
          }
          return acc
        },
        {} as BoardState['columns'],
      )

      const nextBoard: BoardState = {
        ...state.board,
        cards: nextCards,
        columns: nextColumns,
      }

      persistBoardState(nextBoard)
      return {
        board: nextBoard,
        lastSavedAt: Date.now(),
      }
    })
  },

  moveCard: (cardId, fromColumnId, toColumnId, toIndex) => {
    set((state) => {
      const fromColumn = state.board.columns[fromColumnId]
      const toColumn = state.board.columns[toColumnId]
      if (!fromColumn || !toColumn) {
        return state
      }

      const fromIds = [...fromColumn.cardIds]
      const sourceIndex = fromIds.indexOf(cardId)
      if (sourceIndex === -1) {
        return state
      }

      fromIds.splice(sourceIndex, 1)

      const sameColumn = fromColumnId === toColumnId
      const targetIds = sameColumn ? fromIds : [...toColumn.cardIds]
      const boundedIndex = clampIndex(toIndex, targetIds.length)
      targetIds.splice(boundedIndex, 0, cardId)

      const nextBoard: BoardState = {
        ...state.board,
        columns: {
          ...state.board.columns,
          [fromColumnId]: {
            ...fromColumn,
            cardIds: fromIds,
          },
          [toColumnId]: {
            ...toColumn,
            cardIds: targetIds,
          },
        },
      }

      persistBoardState(nextBoard)
      return {
        board: nextBoard,
        lastSavedAt: Date.now(),
      }
    })
  },

  reorderWithinColumn: (columnId, fromIndex, toIndex) => {
    set((state) => {
      const column = state.board.columns[columnId]
      if (!column) {
        return state
      }

      if (fromIndex === toIndex) {
        return state
      }

      const ids = [...column.cardIds]
      if (fromIndex < 0 || fromIndex >= ids.length) {
        return state
      }

      const [cardId] = ids.splice(fromIndex, 1)
      const boundedIndex = clampIndex(toIndex, ids.length)
      ids.splice(boundedIndex, 0, cardId)

      const nextBoard: BoardState = {
        ...state.board,
        columns: {
          ...state.board.columns,
          [columnId]: {
            ...column,
            cardIds: ids,
          },
        },
      }

      persistBoardState(nextBoard)
      return {
        board: nextBoard,
        lastSavedAt: Date.now(),
      }
    })
  },
}))
