const columnCardCount = [4, 3, 5]

function SkeletonCard() {
  return (
    <div className="rounded-xl border border-slate-200 bg-white p-3 shadow-sm">
      <div className="h-4 w-3/4 animate-pulse rounded bg-slate-200" />
      <div className="mt-2 h-3 w-11/12 animate-pulse rounded bg-slate-200" />
      <div className="mt-1 h-3 w-2/3 animate-pulse rounded bg-slate-200" />
      <div className="mt-3 h-3 w-1/4 animate-pulse rounded bg-slate-200" />
    </div>
  )
}

function SkeletonColumn({ cards }: { cards: number }) {
  return (
    <div className="flex min-h-[28rem] w-[20rem] flex-col rounded-2xl border border-slate-200 bg-slate-50/90 p-3 shadow-sm">
      <div className="mb-3 flex items-center justify-between">
        <div className="h-4 w-24 animate-pulse rounded bg-slate-200" />
        <div className="h-5 w-6 animate-pulse rounded-full bg-slate-200" />
      </div>
      <div className="flex flex-1 flex-col gap-2">
        {Array.from({ length: cards }).map((_, index) => (
          <SkeletonCard key={index} />
        ))}
      </div>
    </div>
  )
}

export function SkeletonBoard() {
  return (
    <div className="overflow-x-auto pb-2" aria-label="Loading board">
      <div className="flex min-w-max gap-4">
        {columnCardCount.map((cards, index) => (
          <SkeletonColumn key={index} cards={cards} />
        ))}
      </div>
    </div>
  )
}
