import { useState } from 'react'
import type { FormEvent } from 'react'
import { useDroppable } from '@dnd-kit/core'
import { SortableContext, verticalListSortingStrategy } from '@dnd-kit/sortable'
import { Button } from '../../../shared/ui/Button'
import { Modal } from '../../../shared/ui/Modal'
import { TextArea } from '../../../shared/ui/TextArea'
import { TextInput } from '../../../shared/ui/TextInput'
import { cn } from '../../../shared/utils/cn'
import { useBoardStore } from '../store'
import { CardItem } from './CardItem'
import type { Card, Column as ColumnModel, ColumnId } from '../types'

type ColumnProps = {
  column: ColumnModel
  cards: Card[]
  visibleCardIds: string[]
  isOverColumn?: boolean
  emptyMessage?: string
}

export function Column({
  column,
  cards,
  visibleCardIds,
  isOverColumn = false,
  emptyMessage = 'Drop cards here',
}: ColumnProps) {
  const addCard = useBoardStore((state) => state.addCard)
  const updateCard = useBoardStore((state) => state.updateCard)
  const deleteCard = useBoardStore((state) => state.deleteCard)

  const [isCreateOpen, setIsCreateOpen] = useState(false)
  const [createTitle, setCreateTitle] = useState('')
  const [createDescription, setCreateDescription] = useState('')

  const [editingCard, setEditingCard] = useState<Card | null>(null)
  const [editTitle, setEditTitle] = useState('')
  const [editDescription, setEditDescription] = useState('')

  const { setNodeRef, isOver } = useDroppable({
    id: `column-${column.id}`,
    data: {
      type: 'column',
      columnId: column.id as ColumnId,
    },
  })

  const openEditModal = (card: Card) => {
    setEditingCard(card)
    setEditTitle(card.title)
    setEditDescription(card.description ?? '')
  }

  const closeCreateModal = () => {
    setIsCreateOpen(false)
    setCreateTitle('')
    setCreateDescription('')
  }

  const closeEditModal = () => {
    setEditingCard(null)
    setEditTitle('')
    setEditDescription('')
  }

  const handleCreate = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    if (!createTitle.trim()) {
      return
    }

    addCard(column.id, { title: createTitle, description: createDescription })
    closeCreateModal()
  }

  const handleUpdate = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    if (!editingCard || !editTitle.trim()) {
      return
    }

    updateCard(editingCard.id, { title: editTitle, description: editDescription })
    closeEditModal()
  }

  const handleDelete = () => {
    if (!editingCard) {
      return
    }

    const shouldDelete = window.confirm('Are you sure you want to delete this card?')
    if (!shouldDelete) {
      return
    }

    deleteCard(editingCard.id)
    closeEditModal()
  }

  return (
    <>
      <section className="flex max-h-[calc(100vh-14rem)] min-h-[28rem] w-[20rem] flex-col rounded-2xl border border-slate-200 bg-slate-50/90 p-3 shadow-sm">
        <header className="mb-3 flex items-center justify-between gap-2">
          <h2 className="text-sm font-semibold uppercase tracking-wide text-slate-700">{column.title}</h2>
          <div className="flex items-center gap-2">
            <span className="rounded-full bg-slate-200 px-2 py-0.5 text-xs font-medium text-slate-700">{cards.length}</span>
            <Button type="button" variant="ghost" className="px-2 py-1 text-xs" onClick={() => setIsCreateOpen(true)}>
              + Add
            </Button>
          </div>
        </header>

        <SortableContext items={visibleCardIds} strategy={verticalListSortingStrategy}>
          <div
            ref={setNodeRef}
            className={cn(
              'flex min-h-[10rem] flex-1 flex-col gap-2 overflow-y-auto rounded-lg px-1 py-1 transition-colors',
              (isOver || isOverColumn) && 'bg-sky-100/60 ring-2 ring-inset ring-sky-200',
            )}
            aria-label={`${column.title} cards`}
          >
            {cards.map((card) => (
              <CardItem key={card.id} card={card} columnId={column.id} onOpen={openEditModal} />
            ))}

            {cards.length === 0 && (
              <p className="mt-6 rounded-lg border border-dashed border-slate-300 px-3 py-4 text-center text-xs text-slate-500">
                {emptyMessage}
              </p>
            )}
          </div>
        </SortableContext>
      </section>

      <Modal isOpen={isCreateOpen} title={`Add card to ${column.title}`} onClose={closeCreateModal}>
        <form className="space-y-4" onSubmit={handleCreate}>
          <h3 className="text-lg font-semibold text-slate-900">Add Card</h3>

          <div className="space-y-1">
            <label className="text-sm font-medium text-slate-700" htmlFor={`create-title-${column.id}`}>
              Title
            </label>
            <TextInput
              id={`create-title-${column.id}`}
              value={createTitle}
              onChange={(event) => setCreateTitle(event.target.value)}
              placeholder="Card title"
              required
              autoFocus
            />
          </div>

          <div className="space-y-1">
            <label className="text-sm font-medium text-slate-700" htmlFor={`create-description-${column.id}`}>
              Description
            </label>
            <TextArea
              id={`create-description-${column.id}`}
              value={createDescription}
              onChange={(event) => setCreateDescription(event.target.value)}
              placeholder="Optional description"
              rows={4}
            />
          </div>

          <div className="flex justify-end gap-2">
            <Button type="button" variant="ghost" onClick={closeCreateModal}>
              Cancel
            </Button>
            <Button type="submit">Save</Button>
          </div>
        </form>
      </Modal>

      <Modal isOpen={Boolean(editingCard)} title="Edit card" onClose={closeEditModal}>
        <form className="space-y-4" onSubmit={handleUpdate}>
          <h3 className="text-lg font-semibold text-slate-900">Edit Card</h3>

          <div className="space-y-1">
            <label className="text-sm font-medium text-slate-700" htmlFor={`edit-title-${column.id}`}>
              Title
            </label>
            <TextInput
              id={`edit-title-${column.id}`}
              value={editTitle}
              onChange={(event) => setEditTitle(event.target.value)}
              placeholder="Card title"
              required
              autoFocus
            />
          </div>

          <div className="space-y-1">
            <label className="text-sm font-medium text-slate-700" htmlFor={`edit-description-${column.id}`}>
              Description
            </label>
            <TextArea
              id={`edit-description-${column.id}`}
              value={editDescription}
              onChange={(event) => setEditDescription(event.target.value)}
              placeholder="Optional description"
              rows={4}
            />
          </div>

          <div className="flex items-center justify-between gap-2">
            <Button type="button" className="bg-red-600 text-white hover:bg-red-700" onClick={handleDelete}>
              Delete
            </Button>
            <div className="flex gap-2">
              <Button type="button" variant="ghost" onClick={closeEditModal}>
                Cancel
              </Button>
              <Button type="submit">Update</Button>
            </div>
          </div>
        </form>
      </Modal>
    </>
  )
}
