import { Card } from './Card';

interface Set {
  id: number;
  name: string;
  cards: Card[];
}

const defSet = (): Set => ({
  id: 0,
  name: '',
  cards: [],
});

export { defSet, Set };
