import { beforeEach, describe, expect, it } from 'vitest'
import { loadState, saveState } from './storage'

type TestState = {
  value: number
  label: string
}

const KEY = 'test-storage'

describe('storage utils', () => {
  beforeEach(() => {
    window.localStorage.clear()
  })

  it('saves and loads state with versioned payload', () => {
    const state: TestState = { value: 42, label: 'ok' }

    const saved = saveState(KEY, state)
    const loaded = loadState<TestState>(KEY)

    expect(saved).toBe(true)
    expect(loaded).toEqual(state)
  })

  it('returns null when payload has invalid version', () => {
    window.localStorage.setItem(KEY, JSON.stringify({ version: 999, state: { value: 1, label: 'x' } }))

    const loaded = loadState<TestState>(KEY)

    expect(loaded).toBeNull()
  })

  it('returns null when payload is invalid json', () => {
    window.localStorage.setItem(KEY, '{bad-json')

    const loaded = loadState<TestState>(KEY)

    expect(loaded).toBeNull()
  })
})
