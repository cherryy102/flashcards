interface Card {
  id: number;
  setId: number;
  definition: string;
  term: string;
  correctness: number, 
}

const defCard = (): Card => ({
  definition: '',
  setId: 0,
  id: 0,
  term: '',
  correctness: 0,
});

export { Card, defCard};
