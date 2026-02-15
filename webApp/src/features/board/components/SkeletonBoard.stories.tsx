import type { Meta, StoryObj } from '@storybook/react-vite'
import { SkeletonBoard } from './SkeletonBoard'

const meta: Meta<typeof SkeletonBoard> = {
  title: 'Board/SkeletonBoard',
  component: SkeletonBoard,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
}

export default meta
type Story = StoryObj<typeof SkeletonBoard>

export const Loading: Story = {}
