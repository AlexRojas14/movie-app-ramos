function SkeletonCard() {
  return (
    <div className="rounded-lg border border-slate-200 bg-white p-3">
      <div className="h-4 w-3/4 animate-pulse rounded bg-slate-200" />
      <div className="mt-3 h-3 w-1/3 animate-pulse rounded bg-slate-200" />
    </div>
  )
}

function SkeletonColumn() {
  return (
    <div className="flex min-h-[22rem] flex-col rounded-xl border border-slate-200 bg-slate-50 p-3">
      <div className="mb-3 flex items-center justify-between">
        <div className="h-4 w-24 animate-pulse rounded bg-slate-200" />
        <div className="h-5 w-6 animate-pulse rounded-full bg-slate-200" />
      </div>
      <div className="flex flex-1 flex-col gap-2">
        <SkeletonCard />
        <SkeletonCard />
        <SkeletonCard />
      </div>
    </div>
  )
}

export function BoardSkeleton() {
  return (
    <div aria-label="Loading board" className="space-y-4">
      <div className="h-20 animate-pulse rounded-xl border border-slate-200 bg-white p-4" />
      <div className="grid gap-4 md:grid-cols-3">
        <SkeletonColumn />
        <SkeletonColumn />
        <SkeletonColumn />
      </div>
    </div>
  )
}
