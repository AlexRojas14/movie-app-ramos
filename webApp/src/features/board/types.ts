export type ColumnId = string
export type CardId = string

export type Card = {
  id: CardId
  title: string
  description?: string
  createdAt: string
}

export type Column = {
  id: ColumnId
  title: string
  cardIds: CardId[]
}

export type BoardState = {
  columns: Record<ColumnId, Column>
  cards: Record<CardId, Card>
  columnOrder: ColumnId[]
}
