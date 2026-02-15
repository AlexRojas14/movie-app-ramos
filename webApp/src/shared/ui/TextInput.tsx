import type { InputHTMLAttributes } from 'react'
import { cn } from '../utils/cn'

type TextInputProps = InputHTMLAttributes<HTMLInputElement>

export function TextInput({ className, ...props }: TextInputProps) {
  return (
    <input
      className={cn(
        'w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 placeholder:text-slate-400',
        'focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-sky-500 focus-visible:ring-offset-2',
        className,
      )}
      {...props}
    />
  )
}
