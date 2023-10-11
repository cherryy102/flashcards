interface Flashcard {
  index: number;
  correct: number;
  inCorrect: number;
  showSummary: boolean;
  word: string;
  showAnswer: boolean;
}

const defFlashcard = (): Flashcard => ({
  index: 0,
  correct: 0,
  inCorrect: 0,
  showSummary: false,
  word: '',
  showAnswer: false,
});

export { Flashcard, defFlashcard };
