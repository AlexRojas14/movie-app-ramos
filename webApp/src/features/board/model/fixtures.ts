import { createId } from '../../../shared/utils/id'
import type { BoardData, ColumnId, Task } from './types'

function createTask(title: string): Task {
  return {
    id: createId('task'),
    title,
    createdAt: new Date().toISOString(),
  }
}

function addTaskToColumn(data: BoardData, columnId: ColumnId, title: string): void {
  const task = createTask(title)
  data.tasks[task.id] = task
  data.columns[columnId].taskIds.push(task.id)
}

export function createInitialBoardData(): BoardData {
  const data: BoardData = {
    tasks: {},
    columns: {
      todo: { id: 'todo', title: 'To do', taskIds: [] },
      'in-progress': { id: 'in-progress', title: 'In progress', taskIds: [] },
      done: { id: 'done', title: 'Done', taskIds: [] },
    },
    columnOrder: ['todo', 'in-progress', 'done'],
  }

  addTaskToColumn(data, 'todo', 'Define MVP scope')
  addTaskToColumn(data, 'todo', 'Design card interaction')
  addTaskToColumn(data, 'in-progress', 'Implement drag and drop')
  addTaskToColumn(data, 'done', 'Set up project tooling')

  return data
}
