import { CSS } from '@dnd-kit/utilities'
import { useSortable } from '@dnd-kit/sortable'
import { cn } from '../../../shared/utils/cn'
import type { ColumnId, Task } from '../model/types'

type TaskCardProps = {
  task: Task
  columnId: ColumnId
  isDragOverlay?: boolean
}

export function TaskCard({ task, columnId, isDragOverlay = false }: TaskCardProps) {
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({
    id: task.id,
    disabled: isDragOverlay,
    data: {
      type: 'task',
      columnId,
    },
  })

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
  }

  return (
    <article
      ref={setNodeRef}
      style={style}
      aria-label={`Task ${task.title}`}
      className={cn(
        'rounded-lg border border-slate-200 bg-white p-3 shadow-sm',
        'focus-visible:ring-2 focus-visible:ring-sky-500',
        isDragging && !isDragOverlay && 'opacity-50',
        isDragOverlay && 'shadow-lg',
      )}
      {...attributes}
      {...listeners}
    >
      <p className="text-sm font-medium text-slate-800">{task.title}</p>
      <p className="mt-2 text-xs text-slate-500">
        {new Date(task.createdAt).toLocaleDateString(undefined, { month: 'short', day: 'numeric' })}
      </p>
    </article>
  )
}
