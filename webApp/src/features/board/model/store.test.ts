import { beforeEach, describe, expect, it, vi } from 'vitest'
import { createInitialBoardData } from './fixtures'
import { clearBoardStorage } from './persistence'
import { useBoardStore } from './store'

describe('board store', () => {
  beforeEach(() => {
    clearBoardStorage()
    useBoardStore.getState().resetForTests(createInitialBoardData())
  })

  it('adds a task to selected column', () => {
    const beforeCount = useBoardStore.getState().data.columns.todo.taskIds.length

    useBoardStore.getState().addTask('todo', 'Ship feature')

    const state = useBoardStore.getState().data
    expect(state.columns.todo.taskIds).toHaveLength(beforeCount + 1)
    const createdTaskId = state.columns.todo.taskIds.at(-1)
    expect(createdTaskId).toBeDefined()
    expect(state.tasks[createdTaskId!].title).toBe('Ship feature')
  })

  it('moves task between columns', () => {
    const stateBefore = useBoardStore.getState().data
    const taskId = stateBefore.columns.todo.taskIds[0]

    useBoardStore.getState().moveTask({
      taskId,
      fromColumnId: 'todo',
      toColumnId: 'done',
      toIndex: 0,
    })

    const stateAfter = useBoardStore.getState().data
    expect(stateAfter.columns.todo.taskIds).not.toContain(taskId)
    expect(stateAfter.columns.done.taskIds[0]).toBe(taskId)
  })

  it('keeps loading skeleton state until hydration delay completes', async () => {
    vi.useFakeTimers()
    useBoardStore.setState({ isHydrating: true })

    const hydration = useBoardStore.getState().hydrate()
    expect(useBoardStore.getState().isHydrating).toBe(true)

    await vi.advanceTimersByTimeAsync(5000)
    await hydration

    expect(useBoardStore.getState().isHydrating).toBe(false)
    vi.useRealTimers()
  })
})
