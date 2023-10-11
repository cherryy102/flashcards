<template>
  <div>
    <q-list class="text-bold">
      <q-item clickable>
        <q-item-section @click="onSelect(WordGroup.All)">
          {{ EN.All }}
        </q-item-section>
      </q-item>
      <q-separator />
      <q-item clickable>
        <q-item-section @click="onSelect(WordGroup.Correct)">
          {{ EN.Correct }}
        </q-item-section>
      </q-item>
      <q-separator />
      <q-item clickable>
        <q-item-section @click="onSelect(WordGroup.InCorrect)">
          {{ EN.Incorrect }}
        </q-item-section>
      </q-item>
      <q-separator />
    </q-list>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { EN } from '@/translation';
import { WordGroup } from '@/types/enums';

const EMIT_SELECT = 'select';

export default defineComponent({
  emits: [EMIT_SELECT],
  setup(props, { emit }) {
    function onSelect(mode: WordGroup) {
      emit(EMIT_SELECT, mode);
    }

    return {
      EN,
      WordGroup,

      onSelect,
    };
  },
});
</script>
<style>
.size-18 {
  font-size: 18px;
}
</style>
