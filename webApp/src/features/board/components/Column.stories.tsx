import { DndContext } from '@dnd-kit/core'
import type { Meta, StoryObj } from '@storybook/react-vite'
import type { Card, Column as BoardColumn } from '../types'
import { Column } from './Column'

const cards: Card[] = [
  {
    id: 'card-col-01',
    title: 'Implement search',
    description: 'Filter by title and description.',
    createdAt: '2026-02-15T12:00:00.000Z',
  },
  {
    id: 'card-col-02',
    title: 'Build modal flows',
    description: 'Create and edit cards from column UI.',
    createdAt: '2026-02-15T12:00:00.000Z',
  },
  {
    id: 'card-col-03',
    title: 'Wire drag and drop',
    description: 'Use @dnd-kit sortable context.',
    createdAt: '2026-02-15T12:00:00.000Z',
  },
]

const column: BoardColumn = {
  id: 'doing',
  title: 'Doing',
  cardIds: cards.map((card) => card.id),
}

const meta: Meta<typeof Column> = {
  title: 'Board/Column',
  component: Column,
  tags: ['autodocs'],
  decorators: [
    (Story) => (
      <DndContext>
        <div className="w-[22rem]">
          <Story />
        </div>
      </DndContext>
    ),
  ],
}

export default meta
type Story = StoryObj<typeof Column>

export const WithCards: Story = {
  args: {
    column,
    cards,
    visibleCardIds: cards.map((card) => card.id),
    isOverColumn: false,
  },
}
