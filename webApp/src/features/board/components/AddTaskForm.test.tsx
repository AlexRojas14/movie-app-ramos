import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it, vi } from 'vitest'
import { AddTaskForm } from './AddTaskForm'

describe('AddTaskForm', () => {
  it('submits title and selected column', async () => {
    const user = userEvent.setup()
    const onAddTask = vi.fn()

    render(<AddTaskForm onAddTask={onAddTask} />)

    await user.type(screen.getByLabelText(/task title/i), 'Review PR')
    await user.selectOptions(screen.getByLabelText(/column/i), 'in-progress')
    await user.click(screen.getByRole('button', { name: /add task/i }))

    expect(onAddTask).toHaveBeenCalledWith('in-progress', 'Review PR')
  })
})
