import {
  DndContext,
  KeyboardSensor,
  PointerSensor,
  closestCenter,
  useSensor,
  useSensors,
} from '@dnd-kit/core'
import { sortableKeyboardCoordinates } from '@dnd-kit/sortable'
import { useMemo, useState } from 'react'
import type { DragEndEvent, DragOverEvent, DragStartEvent } from '@dnd-kit/core'
import { useBoardStore } from '../store'
import type { Card, ColumnId } from '../types'
import { Column } from './Column'

type DragCardData = {
  type: 'card'
  columnId: ColumnId
}

type DropColumnData = {
  type: 'column'
  columnId: ColumnId
}

type BoardProps = {
  searchTerm?: string
}

function matchesSearch(card: Card, searchTerm: string): boolean {
  if (!searchTerm) {
    return true
  }

  const normalizedTerm = searchTerm.toLowerCase()
  const title = card.title.toLowerCase()
  const description = card.description?.toLowerCase() ?? ''
  return title.includes(normalizedTerm) || description.includes(normalizedTerm)
}

export function Board({ searchTerm = '' }: BoardProps) {
  const board = useBoardStore((state) => state.board)
  const moveCard = useBoardStore((state) => state.moveCard)
  const reorderWithinColumn = useBoardStore((state) => state.reorderWithinColumn)
  const [, setActiveCardId] = useState<string | null>(null)
  const [overColumnId, setOverColumnId] = useState<ColumnId | null>(null)

  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: { distance: 4 },
    }),
    useSensor(KeyboardSensor, {
      coordinateGetter: sortableKeyboardCoordinates,
    }),
  )

  const cardToColumnMap = useMemo(() => {
    const mapping: Record<string, ColumnId> = {}

    for (const columnId of board.columnOrder) {
      const column = board.columns[columnId]
      for (const cardId of column.cardIds) {
        mapping[cardId] = columnId
      }
    }

    return mapping
  }, [board])

  const resolveOverColumnId = (overId: string, overData: unknown): ColumnId | null => {
    const typedOver = overData as Partial<DropColumnData | DragCardData> | undefined

    if (typedOver?.type === 'column' && typedOver.columnId) {
      return typedOver.columnId
    }

    if (typedOver?.type === 'card' && typedOver.columnId) {
      return typedOver.columnId
    }

    return cardToColumnMap[overId] ?? null
  }

  const onDragStart = (event: DragStartEvent) => {
    setActiveCardId(String(event.active.id))
  }

  const onDragOver = (event: DragOverEvent) => {
    if (!event.over) {
      setOverColumnId(null)
      return
    }

    const nextOverColumn = resolveOverColumnId(String(event.over.id), event.over.data.current)
    setOverColumnId(nextOverColumn)
  }

  const onDragEnd = (event: DragEndEvent) => {
    setOverColumnId(null)
    setActiveCardId(null)

    if (!event.over) {
      return
    }

    const activeId = String(event.active.id)
    const activeData = event.active.data.current as DragCardData | undefined
    if (!activeData || activeData.type !== 'card') {
      return
    }

    const fromColumnId = activeData.columnId
    const fromColumn = board.columns[fromColumnId]
    if (!fromColumn) {
      return
    }

    const fromIndex = fromColumn.cardIds.indexOf(activeId)
    if (fromIndex === -1) {
      return
    }

    const toColumnId = resolveOverColumnId(String(event.over.id), event.over.data.current)
    if (!toColumnId) {
      return
    }

    const toColumn = board.columns[toColumnId]
    if (!toColumn) {
      return
    }

    const overData = event.over.data.current as Partial<DropColumnData | DragCardData> | undefined
    const overId = String(event.over.id)
    const overCardIndex = toColumn.cardIds.indexOf(overId)
    const toIndex =
      overData?.type === 'column' ? toColumn.cardIds.length : overCardIndex === -1 ? toColumn.cardIds.length : overCardIndex

    if (fromColumnId === toColumnId) {
      if (fromIndex === toIndex) {
        return
      }

      reorderWithinColumn(fromColumnId, fromIndex, toIndex)
      return
    }

    moveCard(activeId, fromColumnId, toColumnId, toIndex)
  }

  return (
    <DndContext
      sensors={sensors}
      collisionDetection={closestCenter}
      onDragStart={onDragStart}
      onDragOver={onDragOver}
      onDragEnd={onDragEnd}
      onDragCancel={() => {
        setOverColumnId(null)
        setActiveCardId(null)
      }}
    >
      <div className="overflow-x-auto pb-2">
        <div className="flex min-w-max gap-4">
        {board.columnOrder.map((columnId) => {
          const column = board.columns[columnId]
          const hasSearch = searchTerm.trim().length > 0
          const cards = column.cardIds
            .map((cardId) => board.cards[cardId])
            .filter((card): card is Card => Boolean(card))
            .filter((card) => matchesSearch(card, searchTerm))

          const visibleCardIds = cards.map((card) => card.id)
          const emptyMessage = hasSearch ? 'No matches' : 'Drop cards here'

          return (
            <Column
              key={column.id}
              column={column}
              cards={cards}
              visibleCardIds={visibleCardIds}
              isOverColumn={overColumnId === column.id}
              emptyMessage={emptyMessage}
            />
          )
        })}
      </div>
      </div>
    </DndContext>
  )
}
