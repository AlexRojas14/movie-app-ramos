import type { SelectHTMLAttributes } from 'react'
import { cn } from '../utils/cn'

type SelectProps = SelectHTMLAttributes<HTMLSelectElement>

export function Select({ className, ...props }: SelectProps) {
  return (
    <select
      className={cn('rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900', className)}
      {...props}
    />
  )
}
