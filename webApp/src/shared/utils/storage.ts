const STORAGE_VERSION = 1

type PersistedState<T> = {
  version: number
  state: T
}

export function loadState<T>(key: string): T | null {
  if (typeof window === 'undefined') {
    return null
  }

  try {
    const rawValue = window.localStorage.getItem(key)
    if (!rawValue) {
      return null
    }

    const parsed = JSON.parse(rawValue) as PersistedState<T>
    if (!parsed || parsed.version !== STORAGE_VERSION) {
      return null
    }

    return parsed.state
  } catch {
    return null
  }
}

export function saveState<T>(key: string, state: T): boolean {
  if (typeof window === 'undefined') {
    return false
  }

  try {
    const payload: PersistedState<T> = {
      version: STORAGE_VERSION,
      state,
    }

    window.localStorage.setItem(key, JSON.stringify(payload))
    return true
  } catch {
    return false
  }
}
