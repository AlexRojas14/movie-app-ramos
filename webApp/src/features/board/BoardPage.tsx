import { useEffect, useState } from 'react'
import { Board } from './components/Board'
import { SkeletonBoard } from './components/SkeletonBoard'
import { useBoardStore } from './store'

export function BoardPage() {
  const [searchTerm, setSearchTerm] = useState('')
  const [showSaved, setShowSaved] = useState(false)
  const init = useBoardStore((state) => state.init)
  const isLoading = useBoardStore((state) => state.isLoading)
  const lastSavedAt = useBoardStore((state) => state.lastSavedAt)

  useEffect(() => {
    void init()
  }, [init])

  useEffect(() => {
    if (!lastSavedAt) {
      return
    }

    setShowSaved(true)
    const timeoutId = window.setTimeout(() => setShowSaved(false), 1000)
    return () => window.clearTimeout(timeoutId)
  }, [lastSavedAt])

  return (
    <section className="space-y-5">
      <header className="rounded-2xl border border-slate-200 bg-white p-4 shadow-sm sm:p-5">
        <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
          <h1 className="text-2xl font-semibold tracking-tight text-slate-900">Kanban Board</h1>
          <label className="w-full sm:max-w-sm">
            <span className="sr-only">Search cards</span>
            <input
              type="search"
              value={searchTerm}
              onChange={(event) => setSearchTerm(event.target.value)}
              placeholder="Search by title or description..."
              className="w-full rounded-xl border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 shadow-sm transition focus:border-sky-500 focus:outline-none focus:ring-2 focus:ring-sky-500/40"
            />
          </label>
        </div>
      </header>

      {isLoading ? <SkeletonBoard /> : <Board searchTerm={searchTerm} />}

      {showSaved ? (
        <div className="fixed bottom-4 right-4 z-40 rounded-lg border border-emerald-300 bg-emerald-50 px-3 py-2 text-xs font-semibold text-emerald-700 shadow-sm">
          Saved
        </div>
      ) : null}
    </section>
  )
}
