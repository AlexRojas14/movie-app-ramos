import type { ButtonHTMLAttributes } from 'react'
import { cn } from '../utils/cn'

type ButtonProps = ButtonHTMLAttributes<HTMLButtonElement> & {
  variant?: 'primary' | 'ghost'
}

export function Button({ className, variant = 'primary', ...props }: ButtonProps) {
  return (
    <button
      className={cn(
        'inline-flex items-center justify-center rounded-md px-3 py-2 text-sm font-medium transition-colors',
        'focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-sky-500 focus-visible:ring-offset-2',
        'disabled:cursor-not-allowed disabled:opacity-60',
        variant === 'primary' && 'bg-sky-600 text-white hover:bg-sky-700',
        variant === 'ghost' && 'bg-white text-slate-700 hover:bg-slate-100',
        className,
      )}
      {...props}
    />
  )
}
