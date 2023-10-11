import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import { RouterUrlEnum, RouterNameEnum } from '@/types/enums';
import EditSet from '@/views/EditSet.vue';
import ErrorPage from '@/views/ErrorPage.vue';
import Review from '@/views/Review.vue';
import Sets from '@/views/Sets.vue';
import Study from '@/views/Study.vue';

const routes: Array<RouteRecordRaw> = [
  {
    path: RouterUrlEnum.Sets,
    name: RouterNameEnum.Sets,
    component: Sets,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: RouterUrlEnum.EditSet,
    name: RouterNameEnum.EditSet,
    component: EditSet,
    props(route) {
      return {
        setId: Number(route.params.setId),
      };
    },
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: RouterUrlEnum.Study,
    name: RouterNameEnum.Study,
    component: Study,
    props(route) {
      return {
        setId: Number(route.params.setId),
      };
    },
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: RouterUrlEnum.Review,
    name: RouterNameEnum.Review,
    component: Review,
  },
  {
    path: RouterUrlEnum.ErrorPage,
    name: RouterNameEnum.ErrorPage,
    component: ErrorPage,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
