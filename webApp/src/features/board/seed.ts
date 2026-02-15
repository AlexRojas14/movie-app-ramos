import type { BoardState, Card, CardId, ColumnId } from './types'

const now = '2026-02-15T12:00:00.000Z'

function createCard(id: CardId, title: string, description?: string): Card {
  return {
    id,
    title,
    description,
    createdAt: now,
  }
}

const columns: Record<ColumnId, { id: ColumnId; title: string; cardIds: CardId[] }> = {
  todo: {
    id: 'todo',
    title: 'To do',
    cardIds: ['card-001', 'card-002', 'card-003', 'card-004', 'card-005'],
  },
  doing: {
    id: 'doing',
    title: 'Doing',
    cardIds: ['card-006', 'card-007', 'card-008', 'card-009'],
  },
  done: {
    id: 'done',
    title: 'Done',
    cardIds: ['card-010', 'card-011', 'card-012'],
  },
}

const cards: Record<CardId, Card> = {
  'card-001': createCard('card-001', 'Define MVP scope', 'List user stories and acceptance criteria.'),
  'card-002': createCard('card-002', 'Create wireframes', 'Draft low-fidelity board and card layouts.'),
  'card-003': createCard('card-003', 'Plan state model'),
  'card-004': createCard('card-004', 'Design keyboard interactions'),
  'card-005': createCard('card-005', 'Write API contracts for persistence'),
  'card-006': createCard('card-006', 'Implement drag and drop'),
  'card-007': createCard('card-007', 'Add optimistic updates'),
  'card-008': createCard('card-008', 'Build filtering controls'),
  'card-009': createCard('card-009', 'Create empty state UI'),
  'card-010': createCard('card-010', 'Bootstrap Vite + React + TS'),
  'card-011': createCard('card-011', 'Configure Tailwind'),
  'card-012': createCard('card-012', 'Set up Vitest + Testing Library'),
}

export const boardSeed: BoardState = {
  columns,
  cards,
  columnOrder: ['todo', 'doing', 'done'],
}
