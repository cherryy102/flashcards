<template>
  <BaseCard
    v-model="flashcard"
    :cards="cards"
    @reverse="onReverse"
    @close="onClose"
    @update="onUpdate"
  >
    <template #text>
      <div v-if="flashcard.showAnswer">
        <div>
          <span class="text-green text-bold text-uppercase">{{
            EN.CorrectAnswer
          }}</span>
          <p v-if="reverse" class="answer">
            {{ currentCardObject.definition }}
          </p>
          <p v-else class="answer">{{ currentCardObject.term }}</p>
        </div>
      </div>
    </template>
    <template #action>
      <div v-if="!isEmpty" class="answer-button">
        <q-btn
          v-if="!flashcard.showAnswer && !flashcard.showSummary"
          :label="EN.Answer"
          size="16px"
          padding="10px 30px"
          color="white"
          text-color="black"
          rounded
          @click="onShowAnswer"
        />
        <div v-if="flashcard.showAnswer">
          <q-btn
            :label="EN.Incorrect"
            :disable="disable"
            size="16px"
            padding="10px 30px"
            text-color="grey-6"
            rounded
            outline
            @click="showAnswerWithInCorrect"
          />
          <q-btn
            :disable="disable"
            :label="EN.Correct"
            size="16px"
            class="q-ml-sm"
            padding="10px 30px"
            color="white"
            text-color="black"
            rounded
            @click="showAnswerWithCorrect"
          />
        </div>
      </div>
    </template>
  </BaseCard>
</template>

<script lang="ts">
import {
  computed,
  defineComponent,
  onMounted,
  reactive,
  ref,
  watch,
} from 'vue';
import { Card, defCard, UpdateCard } from '@/types/services/flashcards/cards';
import { defFlashcard, Flashcard } from './models';
import { EN } from '@/translation';
import { useAxios } from '@/composables';
import { useLocalStorage } from '@vueuse/core';
import { WordGroup } from '@/types/enums';
import BaseCard from './BaseCard.vue';

const EMIT_CLOSE = 'close';

export default defineComponent({
  components: {
    BaseCard,
  },
  props: {
    cards: {
      type: Array as () => Card[],
      required: true,
    },
  },
  emits: [EMIT_CLOSE],
  setup(props, { emit }) {
    const disable = ref(false);
    const flashcard: Flashcard = reactive<Flashcard>(defFlashcard());
    const reverse = useLocalStorage<boolean>('reverse', false);

    const isEmpty = computed(() => props.cards.length === 0);
    const currentCardObject = computed(() => {
      if (props.cards[flashcard.index]) {
        return props.cards[flashcard.index];
      }
      return defCard();
    });

    function setCard() {
      if (reverse.value) {
        flashcard.word = currentCardObject.value.term;
        return;
      }
      flashcard.word = currentCardObject.value.definition;
    }

    async function setCorrect() {
      disable.value = true;
      flashcard.correct += 1;
      const correctness: UpdateCard = {
        id: currentCardObject.value.id,
        correctness: WordGroup.Correct,
      };
      await useAxios<UpdateCard, void>(
        '/api/flashcards/v1/cards',
        'PUT',
        correctness
      );
      disable.value = false;
    }

    async function setInCorrect() {
      disable.value = true;
      flashcard.inCorrect += 1;
      const correctness: UpdateCard = {
        id: currentCardObject.value.id,
        correctness: WordGroup.InCorrect,
      };
      await useAxios<UpdateCard, void>(
        '/api/flashcards/v1/cards',
        'PUT',
        correctness
      );
      disable.value = false;
    }

    function onUpdate(word: UpdateCard) {
      currentCardObject.value.definition = word.definition
        ? word.definition
        : currentCardObject.value.definition;
      currentCardObject.value.term = word.term
        ? word.term
        : currentCardObject.value.term;
      if (reverse.value) {
        flashcard.word = currentCardObject.value.term;
        return;
      }
      flashcard.word = currentCardObject.value.term;
    }

    function onClose() {
      emit(EMIT_CLOSE);
    }

    function onReverse() {
      reverse.value = !reverse.value;
      setCard();
    }

    function nextCard() {
      flashcard.index += 1;
      flashcard.showAnswer = false;
      if (flashcard.index >= props.cards.length) {
        flashcard.showSummary = true;
      }
    }

    function showAnswerWithCorrect() {
      setCorrect();
      nextCard();
    }

    function showAnswerWithInCorrect() {
      setInCorrect();
      nextCard();
    }

    function onShowAnswer() {
      flashcard.showAnswer = true;
    }

    watch(currentCardObject, () => {
      setCard();
    });

    watch(
      () => reverse,
      () => {
        setCard();
      }
    );

    onMounted(() => {
      setCard();
    });

    return {
      currentCardObject,
      disable,
      EN,
      flashcard,
      isEmpty,
      reverse,

      onClose,
      onReverse,
      onShowAnswer,
      onUpdate,
      showAnswerWithCorrect,
      showAnswerWithInCorrect,
    };
  },
});
</script>
