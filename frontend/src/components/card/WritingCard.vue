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
        <div v-if="!displayUserAnswer">
          <span class="text-red text-bold text-uppercase">{{
            EN.YourAnswer
          }}</span>
          <p class="answer">{{ answer.toLocaleLowerCase() }}</p>
        </div>
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
      <div v-if="!flashcard.showSummary && !isEmpty">
        <q-form
          class="answer-form"
          @submit.prevent="verifyCorrectness"
          @reset="nextCard"
        >
          <q-btn
            v-if="flashcard.showAnswer"
            ref="nextButton"
            :label="EN.Next"
            class="answer-button"
            size="16px"
            color="white"
            text-color="black"
            type="reset"
            padding="10px 30px"
            rounded
            @keydown.enter="nextCard"
          />
          <q-input
            v-if="!flashcard.showAnswer"
            ref="answerInput"
            v-model="answer"
            :label="EN.Answer"
            class="answer-input"
            input-class="text-white"
            label-color="white"
            color="white"
          />
          <q-btn
            v-if="!flashcard.showAnswer"
            :label="EN.Answer"
            class="answer-button"
            color="white"
            size="16px"
            padding="10px 30px"
            text-color="black"
            type="submit"
            rounded
          />
        </q-form>
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
import { useLocalStorage } from '@vueuse/core';
import BaseCard from './BaseCard.vue';

const EMIT_CLOSE = 'close';
const EMIT_CORRECT = 'correct';
const EMIT_FILTER = 'filter';
const EMIT_INCORRECT = 'inCorrect';

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
  emits: [EMIT_CLOSE, EMIT_CORRECT, EMIT_INCORRECT, EMIT_FILTER],
  setup(props, { emit }) {
    const answer = ref('');
    const answerInput = ref<HTMLInputElement>();
    const disable = ref(false);
    const flashcard: Flashcard = reactive<Flashcard>(defFlashcard());
    const inCorrectWords = ref<number[]>([]);
    const nextButton = ref<HTMLFormElement>();
    const reverse = ref(useLocalStorage<boolean>('reverse', false));

    const displayUserAnswer = computed(() => checkWord());
    const total = computed(() => props.cards.length);
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

    function checkWord() {
      const parseAnswer = answer.value.toLowerCase().trim();
      if (reverse.value) {
        return parseAnswer === currentCardObject.value.definition;
      }
      return parseAnswer === currentCardObject.value.term;
    }

    function verifyCorrectness() {
      const correctness = checkWord();
      setTimeout(() => {
        nextButton.value?.$el.focus();
      }, 100);
      if (correctness) {
        flashcard.showAnswer = true;
        disable.value = true;
        emit(EMIT_CORRECT, currentCardObject.value.id);
        if (!inCorrectWords.value.includes(currentCardObject.value.id)) {
          flashcard.correct += 1;
        }
        const index = inCorrectWords.value.indexOf(currentCardObject.value.id);
        if (index !== -1) {
          inCorrectWords.value.splice(index, 1);
        }
        disable.value = false;
        return;
      }
      if (!inCorrectWords.value.includes(currentCardObject.value.id)) {
        flashcard.inCorrect += 1;
        inCorrectWords.value.push(currentCardObject.value.id);
      }
      flashcard.showAnswer = true;
      disable.value = true;
      emit(EMIT_INCORRECT, currentCardObject.value.id);
      disable.value = false;
    }

    function focusInputWithoutScrolling() {
      answerInput.value?.focus({ preventScroll: true });
    }

    function onReverse() {
      reverse.value = !reverse.value;
      setCard();
      focusInputWithoutScrolling();
    }

    function onClose() {
      emit(EMIT_CLOSE);
    }

    function nextCard() {
      flashcard.index += 1;
      flashcard.showAnswer = false;
      answer.value = '';
      setTimeout(() => {
        focusInputWithoutScrolling();
      }, 100);
      if (flashcard.index >= total.value) {
        if (inCorrectWords.value.length === 0) {
          flashcard.showSummary = true;
          return;
        }
        flashcard.index = 0;
        emit(EMIT_FILTER, inCorrectWords.value);
      }
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
      focusInputWithoutScrolling();
    });

    return {
      answer,
      answerInput,
      currentCardObject,
      displayUserAnswer,
      EN,
      flashcard,
      isEmpty,
      nextButton,
      reverse,

      nextCard,
      onClose,
      onReverse,
      onUpdate,
      verifyCorrectness,
    };
  },
});
</script>
<style>
.answer-input {
  width: 80%;
  margin: 0 auto;
  color: white !important;
}
.answer {
  font-size: 20px;
}
.answer-form {
  width: 100%;
  min-height: 180px;
}
.answer-button {
  position: absolute;
  bottom: 25px;
  right: 25px;
}
</style>
