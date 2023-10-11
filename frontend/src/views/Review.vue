<template>
  <div>
    <div class="card">
      <WritingCard
        :cards="cards"
        @close="onClose"
        @correct="setCorrect"
        @in-correct="setInCorrect"
        @filter="onFilter"
      />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { Card, SetReview } from '@/types/services/flashcards/cards';
import { EN } from '@/translation';
import { useArray } from '@/composables';
import { useAxios } from '@/composables';
import { WordGroup, RouterNameEnum } from '@/types/enums';
import { WorkModeEnum } from '@/types/models';
import WritingCard from '@/components/card/WritingCard.vue';

export default defineComponent({
  components: {
    WritingCard,
  },
  setup() {
    const { shuffleArray } = useArray();
    const router = useRouter();

    const cards = ref<Card[]>([]);
    const modeClassic = ref(false);
    const modeTyping = ref(false);

    async function getCards() {
      const result = await useAxios<void, Card[]>(
        '/api/flashcards/v1/cards/reviews',
        'GET'
      );
      const cardsResult = result?.data ?? [];
      shuffleArray(cardsResult);
      cards.value = cardsResult;
    }

    async function onClose() {
      router.back();
    }

    function onFilter(ids: number[]) {
      const result = cards.value.filter((x) => ids.includes(x.id));
      cards.value = result;
    }

    async function setCorrect(id: number) {
      const review: SetReview = {
        cardId: id,
        correctness: WordGroup.Correct,
      };
      await useAxios<SetReview, void>(
        '/api/flashcards/v1/cards/review',
        'PUT',
        review
      );
    }

    async function setInCorrect(id: number) {
      const review: SetReview = {
        cardId: id,
        correctness: WordGroup.InCorrect,
      };
      await useAxios<SetReview, void>(
        '/api/flashcards/v1/cards/review',
        'PUT',
        review
      );
    }

    onMounted(async () => {
      await getCards();
    });

    return {
      cards,
      EN,
      modeClassic,
      modeTyping,
      RouterNameEnum,
      WordGroup,
      WorkModeEnum,

      onClose,
      onFilter,
      setCorrect,
      setInCorrect,
    };
  },
});
</script>
<style>
.choose-mode {
  position: absolute;
  top: 50%;
  left: 50%;
  max-width: 600px;
  min-width: 300px;
  transform: translate(-50%, -50%);
}
.q-btn-dropdown--split .q-btn-dropdown__arrow-container:not(.q-btn--outline) {
  border: none;
}
</style>
