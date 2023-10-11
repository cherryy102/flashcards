<template>
  <TopMenu :count="reviewsCount" />
  <div class="sets">
    <div class="searcher-container q-mb-md">
      <h1 class="title">
        {{ EN.Sets }} <q-badge align="top">{{ setsCount }}</q-badge>
      </h1>
      <q-btn
        :label="EN.NewSet"
        icon="add"
        color="white"
        outline
        no-caps
        rounded
        @click="onCreateSet"
      />
    </div>
    <q-list class="text-white">
      <q-item
        v-for="set in filteredSets"
        :key="set.id"
        v-ripple
        class="set"
        clickable
      >
        <q-item-section @click="goToStudyPage(set.id)">
          <span class="set-name">
            {{ set.name }}
          </span>
        </q-item-section>
        <q-item-section avatar>
          <div class="row">
            <q-btn
              :to="`edit-set/${set.id}`"
              class="btn-hover q-mr-sm"
              color="grey-10"
              text-color="grey-5"
              icon="edit"
              round
            />
            <q-btn
              class="btn-hover"
              color="grey-10"
              text-color="grey-5"
              icon="add"
              round
              @click="onShowAddCard(set)"
            />
          </div>
        </q-item-section>
      </q-item>
    </q-list>
    <AddSet
      v-model="showAddSet"
      :title="EN.CreateSet"
      :ok-title="EN.Add"
      :cancel-title="EN.Cancel"
      @ok="addSet"
    />
    <AddCard v-model="showAddCard" :set="currentSet" @update="getReviews" />
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useQuasar, QSpinnerFacebook } from 'quasar';
import { Add, Set } from '@/types/services/flashcards/sets';
import { EN } from '@/translation';
import { RouterNameEnum } from '@/types/enums';
import { useAxios } from '@/composables';
import { Card } from '@/types/services/flashcards/cards';
import AddSet from '@/components/modals/AddSet.vue';
import AddCard from '@/components/add-card/AddCard.vue';
import TopMenu from '@/components/menu/TopMenu.vue';
interface SetEdit extends Set {
  edit: boolean;
}

export default defineComponent({
  components: {
    AddSet,
    AddCard,
    TopMenu,
  },
  setup() {
    const router = useRouter();
    const quasar = useQuasar();

    const currentSet = ref<Set | null>(null);
    const filter = ref('');
    const reviewsCount = ref(0);
    const setsOptions = ref<SetEdit[]>([]);
    const showAddCard = ref(false);
    const showAddSet = ref(false);

    const setsCount = computed(() => setsOptions.value.length);
    const filteredSets = computed(() => {
      const parsedFilter = filter.value.trim().toLowerCase();
      return setsOptions.value.filter((x) => x.name.includes(parsedFilter));
    });

    function mapSetOptions(sets: Set[]) {
      setsOptions.value = sets.map<SetEdit>((x) => {
        const show = currentSet.value?.id === x.id;
        return { ...x, edit: show };
      });
    }

    async function getSets() {
      quasar.loading.show({
        spinner: QSpinnerFacebook,
        spinnerColor: 'blue-9',
        message: EN.DataLoading,
      });
      const result = await useAxios<void, Set[]>(
        '/api/flashcards/v1/sets',
        'GET'
      );
      mapSetOptions(result?.data ?? []);
      quasar.loading.hide();
    }

    async function getReviews() {
      const reviews = await useAxios<void, Card[]>(
        '/api/flashcards/v1/cards/reviews',
        'GET'
      );
      reviewsCount.value = reviews?.data.length ?? 0;
    }

    function onShow(set: Set) {
      currentSet.value = set;
      mapSetOptions(setsOptions.value);
    }

    function onCreateSet() {
      showAddSet.value = true;
    }

    async function addSet(value: string) {
      const add: Add = {
        name: value,
      };
      currentSet.value = null;
      await useAxios<Add, void>('/api/flashcards/v1/sets', 'POST', add);
      await getSets();
      showAddSet.value = false;
    }

    function onShowAddCard(value: Set) {
      currentSet.value = value;
      showAddCard.value = true;
    }

    function goToStudyPage(id: number) {
      router.push({
        name: RouterNameEnum.Study,
        params: { setId: id.toString() },
      });
    }

    onMounted(async () => {
      await Promise.all([getSets(), getReviews()]);
    });

    return {
      currentSet,
      EN,
      filter,
      filteredSets,
      reviewsCount,
      setsCount,
      setsOptions,
      showAddCard,
      showAddSet,

      addSet,
      getReviews,
      getSets,
      goToStudyPage,
      onCreateSet,
      onShow,
      onShowAddCard,
    };
  },
});
</script>
<style>
.sets {
  margin: 70px auto 0 auto;
  box-sizing: content-box;
  max-width: 600px;
  padding: 20px;
}
.set-name {
  font-size: 20px;
}
.set {
  border: 1px solid #6e6e6e;
  border-radius: 5px;
  margin-bottom: 10px;
}
.set-name::first-letter {
  text-transform: uppercase;
}
.searcher-container {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.searcher {
  width: 50%;
  padding-right: 10px;
}
.title {
  padding: 0;
  margin: 0;
  color: white;
  font-size: 45px;
  font-weight: bold;
  line-height: 3rem;
}
@media screen and (max-width: 768px) {
  .sets {
    margin: 0 auto 0 auto;
  }
}
</style>
