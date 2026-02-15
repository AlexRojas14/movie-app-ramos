import { create } from 'zustand'
import { createId } from '../../../shared/utils/id'
import { wait } from '../../../shared/utils/wait'
import { createInitialBoardData } from './fixtures'
import { loadBoardFromStorage, saveBoardToStorage } from './persistence'
import type { BoardData, ColumnId } from './types'

const INITIAL_LOADING_MS = 5000

type MoveTaskInput = {
  taskId: string
  fromColumnId: ColumnId
  toColumnId: ColumnId
  toIndex: number
}

type BoardStore = {
  data: BoardData
  isHydrating: boolean
  activeTaskId: string | null
  hydrate: () => Promise<void>
  addTask: (columnId: ColumnId, title: string) => void
  moveTask: (input: MoveTaskInput) => void
  setActiveTaskId: (taskId: string | null) => void
  resetForTests: (data?: BoardData) => void
}

function clampIndex(value: number, max: number): number {
  return Math.max(0, Math.min(value, max))
}

function moveTaskInData(data: BoardData, input: MoveTaskInput): BoardData {
  const { taskId, fromColumnId, toColumnId } = input
  const fromTaskIds = [...data.columns[fromColumnId].taskIds]
  const toTaskIds = fromColumnId === toColumnId ? fromTaskIds : [...data.columns[toColumnId].taskIds]

  const sourceIndex = fromTaskIds.indexOf(taskId)
  if (sourceIndex === -1) {
    return data
  }

  fromTaskIds.splice(sourceIndex, 1)
  const boundedIndex = clampIndex(input.toIndex, toTaskIds.length)
  toTaskIds.splice(boundedIndex, 0, taskId)

  return {
    ...data,
    columns: {
      ...data.columns,
      [fromColumnId]: {
        ...data.columns[fromColumnId],
        taskIds: fromTaskIds,
      },
      [toColumnId]: {
        ...data.columns[toColumnId],
        taskIds: toTaskIds,
      },
    },
  }
}

export const useBoardStore = create<BoardStore>((set, get) => ({
  data: createInitialBoardData(),
  isHydrating: true,
  activeTaskId: null,

  hydrate: async () => {
    if (!get().isHydrating) {
      return
    }

    await wait(INITIAL_LOADING_MS)

    const stored = loadBoardFromStorage()
    const data = stored ?? createInitialBoardData()
    saveBoardToStorage(data)

    set({
      data,
      isHydrating: false,
    })
  },

  addTask: (columnId, rawTitle) => {
    const title = rawTitle.trim()
    if (!title) {
      return
    }

    set((state) => {
      const taskId = createId('task')
      const nextData: BoardData = {
        ...state.data,
        tasks: {
          ...state.data.tasks,
          [taskId]: {
            id: taskId,
            title,
            createdAt: new Date().toISOString(),
          },
        },
        columns: {
          ...state.data.columns,
          [columnId]: {
            ...state.data.columns[columnId],
            taskIds: [...state.data.columns[columnId].taskIds, taskId],
          },
        },
      }

      saveBoardToStorage(nextData)
      return { data: nextData }
    })
  },

  moveTask: (input) => {
    set((state) => {
      const nextData = moveTaskInData(state.data, input)
      if (nextData === state.data) {
        return {}
      }

      saveBoardToStorage(nextData)
      return { data: nextData }
    })
  },

  setActiveTaskId: (taskId) => {
    set({ activeTaskId: taskId })
  },

  resetForTests: (data) => {
    const nextData = data ?? createInitialBoardData()
    set({
      data: nextData,
      isHydrating: false,
      activeTaskId: null,
    })
  },
}))
