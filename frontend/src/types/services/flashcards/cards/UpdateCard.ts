import { WordGroup } from '@/types/enums';

interface UpdateCard {
  id: number;
  definition?: string;
  term?: string;
  correctness?: WordGroup;
}

export { UpdateCard };
