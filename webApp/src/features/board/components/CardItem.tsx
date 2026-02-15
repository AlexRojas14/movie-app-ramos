import { useSortable } from '@dnd-kit/sortable'
import { CSS } from '@dnd-kit/utilities'
import { cn } from '../../../shared/utils/cn'
import type { Card, ColumnId } from '../types'

type CardItemProps = {
  card: Card
  columnId: ColumnId
  onOpen?: (card: Card) => void
  forceDragging?: boolean
}

export function CardItem({ card, columnId, onOpen, forceDragging }: CardItemProps) {
  const { attributes, listeners, setNodeRef, transform, transition, isDragging } = useSortable({
    id: card.id,
    data: {
      type: 'card',
      columnId,
    },
  })

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
  }

  const dragging = forceDragging ?? isDragging

  return (
    <article
      ref={setNodeRef}
      style={style}
      onClick={() => onOpen?.(card)}
      className={cn(
        'cursor-grab rounded-xl border border-slate-200 bg-white p-3 shadow-sm transition',
        'hover:-translate-y-0.5 hover:shadow-md active:cursor-grabbing',
        'focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-sky-500 focus-visible:ring-offset-2',
        dragging && 'z-10 opacity-55 shadow-lg ring-2 ring-sky-300',
      )}
      {...attributes}
      {...listeners}
    >
      <p className="text-sm font-medium text-slate-900">{card.title}</p>
      {card.description ? <p className="mt-2 text-xs leading-relaxed text-slate-600">{card.description}</p> : null}
      <p className="mt-3 text-[11px] font-medium uppercase tracking-wide text-slate-400">
        {new Date(card.createdAt).toLocaleDateString(undefined, { month: 'short', day: 'numeric' })}
      </p>
    </article>
  )
}
