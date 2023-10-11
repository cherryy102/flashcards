<template>
  <div>
    <q-dialog v-model="propValue" flat @hide="onHide">
      <q-card style="min-width: 350px" dark flat>
        <q-card-section>
          <div class="text-h6">{{ title }}</div>
        </q-card-section>
        <q-form @submit.prevent="onOk">
          <q-card-section class="q-pt-none">
            <q-input
              v-model="text"
              :rules="[
                (val) => (val && val.length > 0) || EN.TheFieldCannotBeEmpty,
              ]"
              lazy-rules="ondemand"
              input-class="text-white"
              label-color="white"
              color="white"
              dense
              autofocus
            />
          </q-card-section>
          <q-card-actions align="right" class="text-primary">
            <q-btn v-close-popup :label="cancelTitle" color="amber-6" flat />
            <q-btn :label="okTitle" color="amber-6" type="submit" flat />
          </q-card-actions>
        </q-form>
      </q-card>
    </q-dialog>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from 'vue';
import { EN } from '@/translation';

const EMIT_OK = 'ok';
const EMIT_UPDATE_VALUE = 'update:modelValue';

export default defineComponent({
  model: {
    prop: 'modelValue',
    event: EMIT_UPDATE_VALUE,
  },
  props: {
    modelValue: {
      type: Boolean,
      required: false,
      default: false,
    },
    title: {
      type: String,
      required: false,
      default: '',
    },
    okTitle: {
      type: String,
      required: false,
      default: '',
    },
    cancelTitle: {
      type: String,
      required: false,
      default: '',
    },
  },
  emits: [EMIT_OK, EMIT_UPDATE_VALUE],
  setup(props, { emit }) {
    const propValue = computed({
      get() {
        return props.modelValue;
      },
      set(value: boolean) {
        emit(EMIT_UPDATE_VALUE, value);
      },
    });

    const text = ref('');

    async function onOk() {
      if (text.value) {
        const parsedValue = text.value.toLowerCase().trim();
        emit(EMIT_OK, parsedValue);
        text.value = '';
        return;
      }
      return;
    }

    function onHide() {
      text.value = '';
    }

    return {
      EN,
      propValue,
      text,

      onHide,
      onOk,
    };
  },
});
</script>
