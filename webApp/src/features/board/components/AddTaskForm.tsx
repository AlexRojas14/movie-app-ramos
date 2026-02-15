import { useState } from 'react'
import type { FormEvent } from 'react'
import { Button } from '../../../shared/ui/Button'
import { Select } from '../../../shared/ui/Select'
import { TextInput } from '../../../shared/ui/TextInput'
import type { ColumnId } from '../model/types'

type AddTaskFormProps = {
  onAddTask: (columnId: ColumnId, title: string) => void
}

const columnOptions: Array<{ value: ColumnId; label: string }> = [
  { value: 'todo', label: 'To do' },
  { value: 'in-progress', label: 'In progress' },
  { value: 'done', label: 'Done' },
]

export function AddTaskForm({ onAddTask }: AddTaskFormProps) {
  const [title, setTitle] = useState('')
  const [columnId, setColumnId] = useState<ColumnId>('todo')

  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    if (!title.trim()) {
      return
    }

    onAddTask(columnId, title)
    setTitle('')
  }

  return (
    <form className="rounded-xl border border-slate-200 bg-white p-4 shadow-sm" onSubmit={handleSubmit}>
      <div className="grid gap-3 sm:grid-cols-[2fr_1fr_auto] sm:items-end">
        <div>
          <label className="mb-1 block text-sm font-medium text-slate-700" htmlFor="task-title-input">
            Task title
          </label>
          <TextInput
            id="task-title-input"
            name="task-title-input"
            placeholder="Write a new task"
            value={title}
            onChange={(event) => setTitle(event.target.value)}
            aria-label="Task title"
          />
        </div>

        <div>
          <label className="mb-1 block text-sm font-medium text-slate-700" htmlFor="task-column-select">
            Column
          </label>
          <Select
            id="task-column-select"
            name="task-column-select"
            value={columnId}
            onChange={(event) => setColumnId(event.target.value as ColumnId)}
            aria-label="Column"
            className="w-full"
          >
            {columnOptions.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </Select>
        </div>

        <Button type="submit">Add task</Button>
      </div>
    </form>
  )
}
