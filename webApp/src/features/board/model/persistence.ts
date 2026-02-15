import type { BoardData } from './types'

const STORAGE_KEY = 'kanban-board-state'
const STORAGE_VERSION = 1

type PersistedBoard = {
  version: number
  data: BoardData
}

export function loadBoardFromStorage(): BoardData | null {
  if (typeof window === 'undefined') {
    return null
  }

  const raw = window.localStorage.getItem(STORAGE_KEY)
  if (!raw) {
    return null
  }

  try {
    const parsed: PersistedBoard = JSON.parse(raw)
    if (parsed.version !== STORAGE_VERSION) {
      return null
    }

    return parsed.data
  } catch {
    return null
  }
}

export function saveBoardToStorage(data: BoardData): void {
  if (typeof window === 'undefined') {
    return
  }

  const payload: PersistedBoard = {
    version: STORAGE_VERSION,
    data,
  }

  window.localStorage.setItem(STORAGE_KEY, JSON.stringify(payload))
}

export function clearBoardStorage(): void {
  if (typeof window === 'undefined') {
    return
  }

  window.localStorage.removeItem(STORAGE_KEY)
}
