import { useEffect } from 'react'
import { Board } from './components/Board'
import { BoardSkeleton } from './components/BoardSkeleton'
import { useBoardStore } from './model/store'

export function BoardFeature() {
  const hydrate = useBoardStore((state) => state.hydrate)
  const isHydrating = useBoardStore((state) => state.isHydrating)

  useEffect(() => {
    void hydrate()
  }, [hydrate])

  return isHydrating ? <BoardSkeleton /> : <Board />
}
