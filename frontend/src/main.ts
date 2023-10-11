import { createApp } from 'vue';
import { Quasar } from 'quasar';
import 'quasar/src/css/index.sass';
import { configure } from '@/services/axios';
import KeyCloakService from './services/keycloak/KeycloakService';
import quasarUserOptions from './quasar-user-options';
import router from './router/index';
import App from './App.vue';

KeyCloakService.callLogin(() =>
  createApp(App).use(router).use(Quasar, quasarUserOptions).mount('#app')
);
configure();
