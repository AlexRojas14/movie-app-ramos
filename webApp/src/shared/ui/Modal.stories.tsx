import { useState } from 'react'
import type { Meta, StoryObj } from '@storybook/react-vite'
import { Button } from './Button'
import { Modal } from './Modal'
import { TextArea } from './TextArea'
import { TextInput } from './TextInput'

function ModalDemo() {
  const [open, setOpen] = useState(true)

  return (
    <div className="w-[26rem]">
      <Button type="button" onClick={() => setOpen(true)}>
        Open modal
      </Button>

      <Modal isOpen={open} title="Card editor" onClose={() => setOpen(false)}>
        <div className="space-y-4">
          <h3 className="text-lg font-semibold text-slate-900">Edit Card</h3>

          <div className="space-y-1">
            <label className="text-sm font-medium text-slate-700" htmlFor="story-title">
              Title
            </label>
            <TextInput id="story-title" defaultValue="Refactor board component" />
          </div>

          <div className="space-y-1">
            <label className="text-sm font-medium text-slate-700" htmlFor="story-description">
              Description
            </label>
            <TextArea id="story-description" rows={3} defaultValue="Split drag logic into smaller hooks." />
          </div>

          <div className="flex justify-end gap-2">
            <Button type="button" variant="ghost" onClick={() => setOpen(false)}>
              Cancel
            </Button>
            <Button type="button" onClick={() => setOpen(false)}>
              Save
            </Button>
          </div>
        </div>
      </Modal>
    </div>
  )
}

const meta: Meta<typeof Modal> = {
  title: 'Shared/Modal',
  component: Modal,
  tags: ['autodocs'],
}

export default meta
type Story = StoryObj<typeof Modal>

export const Default: Story = {
  render: () => <ModalDemo />,
}
