<template>
  <div class="not-found">
    <div class="text">
      <h1>404</h1>
      <h2>{{ EN.PageNotFound }}</h2>
      <q-btn :to="{ name: RouterNameEnum.Sets }" color="black" size="xl">
        {{ EN.BackToHomePage }}
      </q-btn>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { EN } from '@/translation';
import { RouterNameEnum } from '@/types/enums';

export default defineComponent({
  setup() {
    return {
      EN,
      RouterNameEnum,
    };
  },
});
</script>
<style>
.not-found {
  display: flex;
  width: 100%;
  height: 100vh;
  flex-flow: column wrap;
  justify-content: center;
  align-items: center;
  text-align: center;
}
.text h1 {
  color: white;
  margin-top: -200px;
  font-size: 10em;
  text-align: center;
  text-shadow: -5px 5px 0px rgba(255, 255, 255, 0.7),
    -10px 10px 0px rgba(255, 255, 255, 0.4),
    -15px 15px 0px rgba(255, 255, 255, 0.2);
  font-weight: bold;
}

.text h2 {
  color: white;
  font-size: 2em;
  text-align: center;
  margin-top: 50px;
  font-weight: bold;
}
</style>
