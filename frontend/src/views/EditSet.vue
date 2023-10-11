<template>
  <div class="edit-set">
    <SetHeader :set="set" @update="getSet" />
    <div class="edit-set-header q-mt-lg q-mb-lg">
      <div class="text-h4 text-white text-bold">
        {{ EN.Cards }}
        <q-badge align="top">{{ cardsQuantity }}</q-badge>
      </div>
    </div>
    <div>
      <q-input
        v-model="searchValue"
        color="white"
        input-class="text-white"
        class="edit-set-input q-mb-md q-px-lg"
        label-color="grey-5"
        label="Search"
        outlined
        hide-bottom-space
      >
        <template #prepend>
          <q-icon name="search" color="white" />
        </template>
      </q-input>
      <q-scroll-area
        id="scroll-area-with-virtual-scroll-1"
        :thumb-style="thumbStyle"
        :bar-style="barStyle"
        style="height: 554px"
        class="q-px-lg"
      >
        <q-virtual-scroll
          v-slot="{ item }"
          :items="cards"
          scroll-target="#scroll-area-with-virtual-scroll-1 > .scroll"
        >
          <q-item class="edit-card q-pa-md q-mb-md">
            <div class="edit-set-inputs">
              <q-input
                v-model="item.definition"
                :label="EN.Definition"
                :rules="[
                  (val) => (val && val.length > 0) || EN.TheFieldCannotBeEmpty,
                ]"
                color="white"
                input-class="text-white"
                class="edit-set-input q-mr-md q-my-xs"
                label-color="grey-5"
                outlined
                hide-bottom-space
              />
              <q-input
                v-model="item.term"
                :label="EN.Term"
                :rules="[
                  (val) => (val && val.length > 0) || EN.TheFieldCannotBeEmpty,
                ]"
                color="white"
                input-class="text-white"
                class="edit-set-input q-mr-md q-my-xs"
                label-color="grey-5"
                outlined
                hide-bottom-space
              />
            </div>
            <div class="column justify-center items-center">
              <q-btn
                icon="save"
                class="btn-hover q-mb-sm"
                color="grey-10"
                text-color="grey-5"
                round
                @click="editCard(item)"
              />
              <q-btn
                icon="delete"
                class="btn-hover"
                color="grey-10"
                text-color="grey-5"
                round
                @click="deleteCard(item.id)"
              />
            </div>
          </q-item>
        </q-virtual-scroll>
      </q-scroll-area>
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, onMounted, ref } from 'vue';
import { Dialog, Notify, QSpinnerFacebook, useQuasar } from 'quasar';
import { EN } from '@/translation';
import {
  Card,
  defSet,
  Set,
} from '@/types/services/flashcards/sets/get-with-cards';
import { ResponseStatusEnum } from '@/types/enums';
import { UpdateCard } from '@/types/services/flashcards/cards';
import { useAxios } from '@/composables';
import SetHeader from '@/components/set-header/SetHeader.vue';

export default defineComponent({
  components: {
    SetHeader,
  },
  props: {
    setId: {
      type: Number,
      required: true,
    },
  },
  setup(props) {
    const quasar = useQuasar();

    const set = ref<Set>(defSet());
    const thumbStyle: Partial<CSSStyleDeclaration> = {
      right: '5px',
      borderRadius: '8px',
      backgroundColor: 'white',
      width: '10px',
      opacity: '0.75',
    };
    const searchValue = ref('');
    const barStyle: Partial<CSSStyleDeclaration> = {
      backgroundColor: 'transparent',
    };

    const cardsQuantity = computed(() => set.value?.cards.length ?? 0);
    const cards = computed(() =>
      set.value.cards.filter(
        (x) =>
          x.term.includes(searchValue.value.toLowerCase().trim()) ||
          x.definition.includes(searchValue.value.toLowerCase().trim())
      )
    );

    async function getSet() {
      const result = await useAxios<void, Set>(
        `/api/flashcards/v1/sets/with-cards/${props.setId}`,
        'GET'
      );
      set.value = result?.data ?? defSet();
    }

    async function editCard(card: Card) {
      if (!card.definition || !card.term) {
        return;
      }
      const update: UpdateCard = {
        id: card.id,
        definition: card.definition.trim().toLowerCase(),
        term: card.term.trim().toLowerCase(),
      };
      const result = await useAxios<UpdateCard, void>(
        '/api/flashcards/v1/cards',
        'PUT',
        update
      );
      if (result?.status === ResponseStatusEnum.Ok) {
        await getSet();
        Notify.create({
          message: EN.Saved,
          icon: 'check_circle',
          color: 'green-6',
          textColor: 'white',
          timeout: 2000,
          position: 'top-right',
        });
      }
    }

    async function deleteCard(id: number) {
      Dialog.create({
        dark: true,
        title: EN.Confirm,
        message: EN.AreYouSureYouWantToDelete,
        cancel: EN.Cancel,
        ok: EN.Delete,
      }).onOk(async () => {
        await useAxios<void, void>(`/api/flashcards/v1/cards/${id}`, 'DELETE');
        await getSet();
      });
    }

    onMounted(async () => {
      quasar.loading.show({
        spinner: QSpinnerFacebook,
        spinnerColor: 'blue-9',
        message: EN.DataLoading,
      });
      await getSet();
      quasar.loading.hide();
    });

    return {
      barStyle,
      cards,
      cardsQuantity,
      EN,
      searchValue,
      set,
      thumbStyle,

      deleteCard,
      editCard,
      getSet,
    };
  },
});
</script>
<style>
.edit-set-header {
  width: 100%;
  display: flex;
  justify-content: space-between;
  align-items: center;
}
.edit-set {
  max-width: 700px;
  margin: 0 auto;
  padding: 0px 10px 10px 10px;
}
.edit-set h4 {
  color: white;
  font-weight: bold;
}
.edit-set h5 {
  color: white;
  font-weight: bold;
}
.edit-set-input.q-field--outlined .q-field__control {
  border: 1px solid #6e6e6e;
}
.edit-card {
  border: 1px solid #6e6e6e;
  border-radius: 5px;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-around;
}
.btn-hover:hover {
  background-color: white !important;
  color: black !important;
}
.q-btn.btn-hover-save {
  transition: 0.3s;
}
.q-btn.btn-hover-save .q-focus-helper {
  display: none;
}
.q-btn.btn-hover-save:hover {
  background-color: black !important;
  color: white !important;
}
.edit-set-inputs {
  display: flex;
  width: 100%;
  justify-content: space-between;
  flex-wrap: wrap;
}
.edit-set-input {
  flex-grow: 1;
}
</style>
