import { DndContext } from '@dnd-kit/core'
import { SortableContext, verticalListSortingStrategy } from '@dnd-kit/sortable'
import type { Meta, StoryObj } from '@storybook/react-vite'
import { CardItem } from './CardItem'

const sampleCard = {
  id: 'card-story-01',
  title: 'Design drag state',
  description: 'Check ring, shadow and opacity while dragging.',
  createdAt: '2026-02-15T12:00:00.000Z',
}

const meta: Meta<typeof CardItem> = {
  title: 'Board/CardItem',
  component: CardItem,
  tags: ['autodocs'],
  decorators: [
    (Story, context) => (
      <DndContext>
        <SortableContext items={[context.args.card.id]} strategy={verticalListSortingStrategy}>
          <div className="w-72">
            <Story />
          </div>
        </SortableContext>
      </DndContext>
    ),
  ],
}

export default meta
type Story = StoryObj<typeof CardItem>

export const Normal: Story = {
  args: {
    card: sampleCard,
    columnId: 'todo',
  },
}

export const Dragging: Story = {
  args: {
    card: sampleCard,
    columnId: 'todo',
    forceDragging: true,
  },
}
