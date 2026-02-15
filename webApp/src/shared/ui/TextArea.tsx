import type { TextareaHTMLAttributes } from 'react'
import { cn } from '../utils/cn'

type TextAreaProps = TextareaHTMLAttributes<HTMLTextAreaElement>

export function TextArea({ className, ...props }: TextAreaProps) {
  return (
    <textarea
      className={cn(
        'w-full rounded-md border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 placeholder:text-slate-400',
        'focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-sky-500 focus-visible:ring-offset-2',
        className,
      )}
      {...props}
    />
  )
}
