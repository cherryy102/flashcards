<template>
  <div class="summary">
    <div class="text-left">
      <q-knob
        v-model="correctPercent"
        :thickness="0.2"
        size="90px"
        color="positive"
        track-color="red-8"
        class="text-white-9 q-ma-xs"
        readonly
        show-value
      >
        {{ correctPercent }}%
      </q-knob>
    </div>
    <div class="q-mt-md summary-button">
      <q-chip icon="done" color="positive" text-color="white">{{ correct }}</q-chip>
      <q-chip icon="close" color="red" text-color="white">
        {{ inCorrect }}
      </q-chip>
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent } from 'vue';
import { EN } from '@/translation';

export default defineComponent({
  props: {
    total: {
      type: Number,
      required: true,
    },
    correct: {
      type: Number,
      required: true,
    },
    inCorrect: {
      type: Number,
      required: true,
    },
  },
  setup(props) {
    const correctPercent = computed(() =>
      Number(((props.correct / props.total) * 100).toFixed(0))
    );

    return {
      EN,
      correctPercent,
    };
  },
});
</script>
<style>
.summary {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  width: 100%;
  min-height: 200px;
}
</style>
