<template>
  <div>
    <div v-if="isStartMenu" class="choose-mode">
      <div class="column">
        <q-btn-dropdown
          :label="EN.Flashcards"
          color="grey-10"
          icon="style"
          class="q-mb-sm"
          align="left"
          padding="15px 10px"
          dropdown-icon="expand_more"
          split
          @click="onFlashcardSelect(WordGroup.All)"
        >
          <ChooseMode @select="onFlashcardSelect" />
        </q-btn-dropdown>

        <q-btn-dropdown
          :label="EN.Writing"
          color="grey-10"
          align="left"
          padding="15px 10px"
          dropdown-icon="expand_more"
          icon="keyboard"
          split
          @click="onWritingSelect(WordGroup.All)"
        >
          <ChooseMode @select="onWritingSelect" />
        </q-btn-dropdown>
      </div>
    </div>
    <div class="card">
      <WritingCard
        v-if="cardMode === WorkModeEnum.Typing"
        :cards="cardOptions"
        @close="onClose"
        @correct="setCorrect"
        @in-correct="setInCorrect"
        @filter="onFilter"
      />
      <Flashcard
        v-if="cardMode === WorkModeEnum.Classic"
        :cards="cardOptions"
        @close="onClose"
      />
    </div>
  </div>
</template>

<script lang="ts">
import { WordGroup, RouterNameEnum } from '@/types/enums';
import { computed, defineComponent, onMounted, reactive, ref } from 'vue';
import ChooseMode from '@/components/choose-mode/ChooseMode.vue';
import WritingCard from '@/components/card/WritingCard.vue';
import Flashcard from '@/components/card/Flashcard.vue';
import { Card, UpdateCard } from '@/types/services/flashcards/cards';
import { WorkModeEnum } from '@/types/models';
import { useArray, useAxios } from '@/composables';
import { EN } from '@/translation';

export default defineComponent({
  components: {
    ChooseMode,
    WritingCard,
    Flashcard,
  },
  props: {
    setId: {
      type: Number,
      required: true,
    },
  },
  setup(props) {
    const cards: Card[] = reactive([]);
    const wordGroup = ref<WordGroup>(WordGroup.None);
    const cardMode = ref<WorkModeEnum>(WorkModeEnum.None);
    const modeClassic = ref(false);
    const modeTyping = ref(false);
    const { shuffleArray } = useArray();

    const cardOptions = computed(() => {
      if (wordGroup.value === WordGroup.All) {
        return cards;
      }
      return cards.filter((x) => x.correctness === wordGroup.value);
    });

    const isStartMenu = computed(() => cardMode.value === WorkModeEnum.None);

    async function getCards() {
      const response = await useAxios<void, Card[]>(
        `/api/flashcards/v1/cards/${props.setId}`,
        'GET'
      );
      const result = response?.data ?? [];
      shuffleArray(result);
      cards.splice(0, cards.length, ...result);
    }

    async function onClose() {
      cardMode.value = WorkModeEnum.None;
      await getCards();
    }

    function hideExpandItem() {
      modeClassic.value = false;
      modeTyping.value = false;
    }

    function onFlashcardSelect(mode: WordGroup) {
      wordGroup.value = mode;
      cardMode.value = WorkModeEnum.Classic;
      hideExpandItem();
    }

    function onWritingSelect(mode: WordGroup) {
      wordGroup.value = mode;
      cardMode.value = WorkModeEnum.Typing;
      hideExpandItem();
    }

    async function setCorrect(id: number) {
      const correctness: UpdateCard = {
        id,
        correctness: WordGroup.Correct,
      };
      await useAxios<UpdateCard, void>(
        '/api/flashcards/v1/cards',
        'PUT',
        correctness
      );
    }

    async function setInCorrect(id: number) {
      const correctness: UpdateCard = {
        id,
        correctness: WordGroup.InCorrect,
      };
      await useAxios<UpdateCard, void>(
        '/api/flashcards/v1/cards',
        'PUT',
        correctness
      );
    }

    function onFilter(ids: number[]) {
      const result = cards.filter((x) => ids.includes(x.id));
      cards.splice(0, cards.length, ...result);
    }

    onMounted(async () => {
      await getCards();
    });

    return {
      cardMode,
      cardOptions,
      EN,
      isStartMenu,
      modeClassic,
      modeTyping,
      RouterNameEnum,
      WordGroup,
      WorkModeEnum,

      onClose,
      onFilter,
      onFlashcardSelect,
      onWritingSelect,
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
