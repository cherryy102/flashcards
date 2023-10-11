<template>
  <div class="row items-center">
    <q-btn
      :to="RouterUrlEnum.Sets"
      text-color="white"
      class="q-mr-sm"
      icon="arrow_back"
      round
    />
    <h4>{{ EN.EditSet }}</h4>
  </div>
  <div class="text-right">
    <q-input
      v-model="setName"
      :label="EN.Title"
      :rules="[(val) => (val && val.length > 0) || EN.TheFieldCannotBeEmpty]"
      color="white"
      input-class="text-white"
      class="edit-set-input q-mb-md"
      label-color="grey-5"
      outlined
      hide-bottom-space
    />
    <q-btn
      :label="EN.Delete"
      size="16px"
      padding="8px 30px"
      class="q-mr-sm"
      text-color="grey-6"
      rounded
      outline
      @click="deleteSet"
    />
    <q-btn
      :label="EN.Save"
      color="white"
      text-color="black"
      class="btn-hover-save"
      size="16px"
      padding="8px 30px"
      rounded
      no-caps
      @click="renameSet"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, watch } from 'vue';
import { Dialog, Notify } from 'quasar';
import { useRouter } from 'vue-router';
import { EN } from '@/translation';
import { Rename } from '@/types/services/flashcards/sets';
import {
  ResponseStatusEnum,
  RouterNameEnum,
  RouterUrlEnum,
} from '@/types/enums';
import { Set } from '@/types/services/flashcards/sets/get-with-cards';
import { useAxios } from '@/composables';

const EMIT_UPDATE = 'update';

export default defineComponent({
  props: {
    set: {
      type: Object as PropType<Set>,
      default: null,
    },
  },
  emits: [EMIT_UPDATE],
  setup(props, { emit }) {
    const router = useRouter();
    const setName = ref('');

    async function renameSet() {
      const newName: Rename = {
        id: props.set.id,
        name: setName.value.trim().toLowerCase(),
      };
      const result = await useAxios<Rename, void>(
        '/api/flashcards/v1/sets',
        'PUT',
        newName
      );
      if (result?.status === ResponseStatusEnum.Ok) {
        emit(EMIT_UPDATE);
        Notify.create({
          message: EN.Renamed,
          icon: 'check_circle',
          color: 'green-6',
          textColor: 'white',
          timeout: 2000,
          position: 'top-right',
        });
      }
    }

    async function deleteSet() {
      Dialog.create({
        dark: true,
        title: EN.Confirm,
        message: EN.AreYouSureYouWantToDelete,
        cancel: EN.Cancel,
        ok: EN.Delete,
      }).onOk(async () => {
        await useAxios<void, void>(
          `/api/flashcards/v1/sets/${props.set.id}`,
          'DELETE'
        );
        router.push({ name: RouterNameEnum.Sets });
      });
    }

    watch(
      () => props.set,
      () => {
        setName.value = props.set.name;
      }
    );

    return {
      EN,
      RouterUrlEnum,
      setName,

      deleteSet,
      renameSet,
    };
  },
});
</script>
