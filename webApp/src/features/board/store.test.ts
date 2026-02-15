import { beforeEach, describe, expect, it } from 'vitest'
import { loadState } from '../../shared/utils/storage'
import { boardSeed } from './seed'
import { BOARD_STORAGE_KEY, useBoardStore } from './store'
import type { BoardState } from './types'

function cloneSeed(): BoardState {
  return JSON.parse(JSON.stringify(boardSeed)) as BoardState
}

describe('kanban board store', () => {
  beforeEach(() => {
    window.localStorage.clear()
    useBoardStore.setState({
      board: cloneSeed(),
      isLoading: false,
      isInitializing: false,
      hasInitialized: true,
    })
  })

  it('moves a card across columns and persists the result', () => {
    useBoardStore.getState().moveCard('card-001', 'todo', 'doing', 1)

    const state = useBoardStore.getState().board
    const persisted = loadState<BoardState>(BOARD_STORAGE_KEY)

    expect(state.columns.todo.cardIds).not.toContain('card-001')
    expect(state.columns.doing.cardIds[1]).toBe('card-001')
    expect(persisted).toEqual(state)
  })

  it('adds a card to the target column and persists', () => {
    const beforeState = useBoardStore.getState().board
    const beforeIds = new Set(Object.keys(beforeState.cards))

    useBoardStore.getState().addCard('todo', {
      title: 'New card',
      description: 'New description',
    })

    const state = useBoardStore.getState().board
    const persisted = loadState<BoardState>(BOARD_STORAGE_KEY)
    const newCardId = Object.keys(state.cards).find((id) => !beforeIds.has(id))

    expect(newCardId).toBeDefined()
    expect(state.columns.todo.cardIds).toContain(newCardId as string)
    expect(state.cards[newCardId as string].title).toBe('New card')
    expect(state.cards[newCardId as string].description).toBe('New description')
    expect(persisted).toEqual(state)
  })

  it('updates an existing card and persists', () => {
    useBoardStore.getState().updateCard('card-001', {
      title: 'Updated title',
      description: 'Updated description',
    })

    const state = useBoardStore.getState().board
    const persisted = loadState<BoardState>(BOARD_STORAGE_KEY)

    expect(state.cards['card-001'].title).toBe('Updated title')
    expect(state.cards['card-001'].description).toBe('Updated description')
    expect(persisted).toEqual(state)
  })

  it('reorders cards within the same column and persists the result', () => {
    useBoardStore.getState().reorderWithinColumn('todo', 0, 2)

    const state = useBoardStore.getState().board
    const persisted = loadState<BoardState>(BOARD_STORAGE_KEY)

    expect(state.columns.todo.cardIds.slice(0, 3)).toEqual(['card-002', 'card-003', 'card-001'])
    expect(persisted).toEqual(state)
  })

  it('deletes a card from cards map and every column list, then persists', () => {
    useBoardStore.getState().deleteCard('card-006')

    const state = useBoardStore.getState().board
    const persisted = loadState<BoardState>(BOARD_STORAGE_KEY)

    expect(state.cards['card-006']).toBeUndefined()
    expect(state.columns.doing.cardIds).not.toContain('card-006')
    expect(persisted).toEqual(state)
  })
})
