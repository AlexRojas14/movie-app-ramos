export type ColumnId = 'todo' | 'in-progress' | 'done'

export type Task = {
  id: string
  title: string
  createdAt: string
}

export type Column = {
  id: ColumnId
  title: string
  taskIds: string[]
}

export type BoardData = {
  tasks: Record<string, Task>
  columns: Record<ColumnId, Column>
  columnOrder: ColumnId[]
}

export type SortDirection = 'asc' | 'desc'
