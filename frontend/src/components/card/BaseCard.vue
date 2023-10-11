<template>
  <div>
    <div class="header-bar">
      <div>
        <q-btn round flat @click="onClose">
          <q-icon name="close" size="md" />
        </q-btn>
      </div>
      <div class="progress-bar">
        <q-linear-progress
          :value="progress"
          size="10px"
          color="secondary"
          rounded
        />
      </div>
      <div>
        <q-btn flat round @click="onReverse">
          <q-icon name="settings_backup_restore" size="md" />
        </q-btn>
      </div>
    </div>
    <q-card class="word-card" dark flat>
      <div v-if="!propValue.showSummary" class="display-word">
        <p>{{ propValue.word }}</p>
        <p v-if="isEmpty">{{ EN.FlashcardIsEmpty }}</p>
      </div>
      <div v-if="!propValue.showSummary" class="q-pl-lg q-pb-lg">
        <slot name="text" />
      </div>
      <LearningSummary
        v-if="propValue.showSummary"
        :correct="propValue.correct"
        :in-correct="propValue.inCorrect"
        :total="total"
      />
    </q-card>
    <q-card class="action-card" dark flat>
      <div class="action-button">
        <slot name="action" />
      </div>
    </q-card>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent } from 'vue';
import { EN } from '@/translation';
import { Flashcard } from './models';
import { LearningSummary } from './atoms';
import { UpdateCard, Card } from '@/types/services/flashcards/cards';

const EMIT_CLOSE = 'close';
const EMIT_REVERSE = 'reverse';
const EMIT_UPDATE = 'update';
const EMIT_UPDATE_VALUE = 'update:modelValue';

export default defineComponent({
  components: {
    LearningSummary,
  },
  props: {
    modelValue: {
      type: Object as () => Flashcard,
      required: true,
    },
    cards: {
      type: Array as () => Card[],
      required: true,
    },
  },
  emits: [EMIT_CLOSE, EMIT_REVERSE, EMIT_UPDATE_VALUE, EMIT_UPDATE],
  setup(props, { emit }) {
    const propValue = computed({
      get() {
        return props.modelValue;
      },
      set(value: Flashcard) {
        emit(EMIT_UPDATE_VALUE, value);
      },
    });

    const total = computed(
      () => propValue.value.correct + propValue.value.inCorrect
    );
    const isEmpty = computed(() => props.cards.length === 0);

    const progress = computed(() => propValue.value.index / props.cards.length);

    function onClose() {
      emit(EMIT_CLOSE);
    }

    function onReverse() {
      emit(EMIT_REVERSE);
    }

    function onUpdate(word: UpdateCard) {
      emit(EMIT_UPDATE, word);
    }

    return {
      EN,
      isEmpty,
      progress,
      propValue,
      total,

      onClose,
      onReverse,
      onUpdate,
    };
  },
});
</script>
<style>
.word-card {
  max-width: 600px;
  min-height: 100px;
  margin: 20px auto;
}
.action-card {
  max-width: 600px;
  min-height: 100px;
  margin: auto;
  padding: 10px 0 0 0;
}
.display-word {
  width: 100%;
  overflow: auto;
  padding: 5px;
}
.display-word p {
  margin-top: 30px;
  margin-left: 20px;
  text-align: left;
  font-size: 23px;
  text-transform: uppercase;
  font-weight: bold;
  overflow-wrap: anywhere;
  word-break: break-word;
}
.action-button {
  width: 100%;
  text-align: center;
}
.progress-bar {
  width: 70%;
}
.header-bar {
  padding: 10px;
  display: flex;
  color: white;
  max-width: 600px;
  align-items: center;
  justify-content: space-between;
  margin: auto;
}
</style>
