import { beforeEach, describe, expect, it } from 'vitest'
import { createInitialBoardData } from './fixtures'
import { clearBoardStorage, loadBoardFromStorage, saveBoardToStorage } from './persistence'

describe('board persistence', () => {
  beforeEach(() => {
    clearBoardStorage()
    window.localStorage.clear()
  })

  it('loads saved data when version is valid', () => {
    const initial = createInitialBoardData()
    saveBoardToStorage(initial)

    const loaded = loadBoardFromStorage()

    expect(loaded).toEqual(initial)
  })

  it('returns null when storage payload is invalid', () => {
    window.localStorage.setItem('kanban-board-state', '{"broken":true}')

    const loaded = loadBoardFromStorage()

    expect(loaded).toBeNull()
  })
})
