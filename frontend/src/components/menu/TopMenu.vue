<template>
  <div class="top-menu">
    <q-tabs class="w-100 text-white bg-primary menu-tabs">
      <q-tab :label="EN.Logout" icon="logout" @click="logout" />
      <q-tab
        :label="`${EN.Review}(${count})`"
        icon="event_repeat"
        @click="goToReview"
      />
    </q-tabs>
    <div class="d-flex">
      <q-btn
        :label="EN.Logout"
        class="q-mr-md"
        icon="logout"
        color="white"
        flat
        rounded
        no-caps
        @click="logout"
      />
      <q-btn
        color="white"
        text-color="black"
        rounded
        no-caps
        @click="goToReview"
      >
        {{ EN.Review }} ({{ count }})
      </q-btn>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useRouter } from 'vue-router';
import { EN } from '@/translation';
import { RouterNameEnum } from '@/types/enums';
import KeyCloakService from '@/services/keycloak/KeycloakService';

export default defineComponent({
  props: {
    count: {
      type: Number,
      default: 0,
    },
  },
  setup() {
    const router = useRouter();
    const { callLogout } = KeyCloakService;

    function logout() {
      callLogout();
    }

    function goToReview() {
      router.push({ name: RouterNameEnum.Review });
    }

    return {
      EN,

      logout,
      goToReview,
    };
  },
});
</script>
<style>
.d-flex {
  display: flex;
  justify-content: flex-end;
  align-items: center;
}
.top-menu {
  height: 50px;
  width: 100%;
  position: fixed;
  max-width: 600px;
  top: 0;
  left: 50%;
  box-sizing: content-box;
  padding-top: 10px;
  padding-right: 20px;
  transform: translateX(-50%);
  z-index: 1;
}
.menu-icon {
  position: absolute;
  top: 1px;
  cursor: pointer;
  right: 7px;
}
.menu-tabs {
  display: none;
}
@media screen and (max-width: 768px) {
  .top-menu {
    top: auto;
    bottom: 0;
    max-width: 100%;
    padding-right: 0;
    padding-bottom: 20px;
  }
  .d-flex {
    display: none;
  }
  .menu-tabs {
    display: block;
  }
}
.w-100 {
  width: 100%;
}
</style>
