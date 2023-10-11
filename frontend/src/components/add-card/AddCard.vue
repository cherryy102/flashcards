<template>
  <q-dialog v-model="propValue" flat @hide="onHide">
    <q-card style="min-width: 350px" class="q-pa-lg" dark flat>
      <q-form @submit.prevent="addCard">
        <div class="row items-center justify-between">
          <div class="add-card-input">
            <q-input
              ref="firstInput"
              v-model="definition"
              :label="EN.Definition"
              :rules="[
                (val) => (val && val.length > 0) || EN.TheFieldCannotBeEmpty,
              ]"
              color="white"
              input-class="text-white"
              label-color="grey-5"
              lazy-rules="ondemand"
              class="q-mb-md"
              debounce="500"
              hide-bottom-space
              autofocus
              @update:model-value="translate"
            >
              <template #append>
                <q-chip color="grey-9">
                  <span :class="sourceLang.icon" />
                </q-chip>
              </template>
            </q-input>
            <q-input
              v-model="term"
              :loading="loading"
              :label="EN.Term"
              :rules="[
                (val) => (val && val.length > 0) || EN.TheFieldCannotBeEmpty,
              ]"
              color="white"
              input-class="text-white"
              class="q-mb-md"
              label-color="grey-5"
              lazy-rules="ondemand"
              hide-bottom-space
            >
              <template #append>
                <q-chip color="grey-9">
                  <span :class="targetLang.icon" />
                </q-chip>
              </template>
            </q-input>
          </div>
          <div class="switch-lang">
            <q-icon
              :color="translateIconColor"
              name="g_translate"
              size="md"
              class="cursor-pointer"
              @click="toggleTranslation"
            />
            <q-icon
              name="compare_arrows"
              class="rotate-icon cursor-pointer q-mt-sm"
              size="lg"
              color="white"
              @click="switchLang"
            />
          </div>
        </div>
        <div class="text-right">
          <q-btn v-close-popup :label="EN.Cancel" color="amber-6" flat />
          <q-btn :label="EN.Add" color="amber-6" type="submit" flat />
        </div>
      </q-form>
    </q-card>
  </q-dialog>
</template>

<script lang="ts">
import { computed, defineComponent, ref, watch } from 'vue';
import { Notify } from 'quasar';
import { useLocalStorage } from '@vueuse/core';
import { Set } from '@/types/services/flashcards/sets';
import { AddCard } from '@/types/services/flashcards/cards';
import { LanguageCode, ResponseStatusEnum } from '@/types/enums';
import { EN } from '@/translation';
import {
  Translate,
  TranslateResult,
} from '@/types/services/flashcards/translate';
import { useAbortRequest, useAxios } from '@/composables';

const EMIT_UPDATE_VALUE = 'update:modelValue';
const EMIT_UPDATE = 'update';

interface Lang {
  type: string;
  icon: string;
}

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
    set: {
      type: Object as () => Set | null,
      required: false,
      default: null,
    },
  },
  emits: [EMIT_UPDATE_VALUE, EMIT_UPDATE],
  setup(props, { emit }) {
    const propValue = computed({
      get() {
        return props.modelValue;
      },
      set(value: boolean) {
        emit(EMIT_UPDATE_VALUE, value);
      },
    });

    const { abortRequest } = useAbortRequest();

    const isTranslationOn = useLocalStorage<boolean>('isTranslationOn', true);
    const sourceLang = useLocalStorage<Lang>('sourceLang', {
      type: LanguageCode.EN,
      icon: getIcon(LanguageCode.EN),
    });
    const targetLang = useLocalStorage<Lang>('targetLang', {
      type: LanguageCode.PL,
      icon: getIcon(LanguageCode.PL),
    });

    const definition = ref('');
    const firstInput = ref<HTMLElement>();
    const loading = ref(false);
    const setName = ref(props.set?.name ?? '');
    const term = ref('');

    const translateIconColor = computed(() =>
      isTranslationOn.value ? 'blue-6' : 'grey-8'
    );
    const definitionFormatted = computed(() => {
      if (sourceLang.value.type === LanguageCode.EN) {
        return definition.value.toLowerCase().trim();
      }
      return term.value.toLowerCase().trim();
    });

    const termFormatted = computed(() => {
      if (targetLang.value.type === LanguageCode.EN_US) {
        return definition.value.toLowerCase().trim();
      }
      return term.value.toLowerCase().trim();
    });

    function toggleTranslation() {
      isTranslationOn.value = !isTranslationOn.value;
    }

    function getIcon(code: string) {
      if (code === LanguageCode.EN) {
        return 'fi fi-gb';
      }
      if (code === LanguageCode.EN_US) {
        return 'fi fi-gb';
      }
      return 'fi fi-pl';
    }

    function getLang(code: string) {
      const lang: Lang = {
        type: code,
        icon: getIcon(code),
      };
      return lang;
    }

    async function translate(value: string | number | null) {
      const text = value?.toString();
      if (!text) {
        return;
      }
      if (isTranslationOn.value) {
        const translateText: Translate = {
          text,
          sourceLanguageCode: sourceLang.value.type,
          targetLanguageCode: targetLang.value.type,
        };
        if (value !== '') {
          loading.value = true;
          const response = await abortRequest((signal) =>
            useAxios<Translate, TranslateResult>(
              '/api/flashcards/v1/translate',
              'POST',
              translateText,
              signal
            )
          );
          term.value = response?.data.text ?? '';
          loading.value = false;
          return;
        }
        term.value = '';
      }
    }

    async function switchLang() {
      const termCopy = term.value;
      const definitionCopy = definition.value;
      if (sourceLang.value.type === LanguageCode.EN) {
        definition.value = termCopy;
        term.value = definitionCopy;
        sourceLang.value = getLang(LanguageCode.PL);
        targetLang.value = getLang(LanguageCode.EN_US);
        await translate(definition.value);
        return;
      }
      definition.value = termCopy;
      term.value = definitionCopy;
      sourceLang.value = getLang(LanguageCode.EN);
      targetLang.value = getLang(LanguageCode.PL);
      await translate(definition.value);
    }

    async function addCard() {
      if (!definition.value || !term.value) {
        return;
      }
      const add: AddCard = {
        setId: props.set?.id ?? 0,
        definition: definitionFormatted.value,
        term: termFormatted.value,
      };
      const result = await useAxios<AddCard, void>(
        '/api/flashcards/v1/cards',
        'POST',
        add
      );
      if (result?.status === ResponseStatusEnum.Ok) {
        Notify.create({
          message: EN.AddedSuccessfully,
          icon: 'check_circle',
          color: 'green-6',
          textColor: 'white',
          timeout: 2000,
          position: 'top-right',
        });
        firstInput.value?.focus();
        definition.value = '';
        term.value = '';
        emit(EMIT_UPDATE);
        return;
      }
      Notify.create({
        message: EN.SomethingWentWrong,
        icon: 'priority_high',
        color: 'deep-orange-8',
        textColor: 'white',
        timeout: 2000,
        position: 'top-right',
      });
    }

    function onHide() {
      definition.value = '';
      term.value = '';
    }

    watch(definition, () => {
      if (definition.value === '') {
        term.value = '';
      }
    });

    return {
      definition,
      EN,
      firstInput,
      loading,
      propValue,
      setName,
      sourceLang,
      targetLang,
      term,
      translateIconColor,

      addCard,
      onHide,
      switchLang,
      toggleTranslation,
      translate,
    };
  },
});
</script>
<style>
.rotate-icon {
  transform: rotate(90deg);
}
.add-card-input {
  width: 90%;
}
.switch-lang {
  width: 10%;
  padding: 10px;
}
</style>
