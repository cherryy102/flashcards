import { Card } from '@/types/services/flashcards/cards';

export function useArray() {
  function shuffleArray(array: Card[]) {
    for (let i = array.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [array[i], array[j]] = [array[j], array[i]];
    }
  }

  return {
    shuffleArray,
  };
}