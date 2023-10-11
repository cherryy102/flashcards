/* eslint-disable @typescript-eslint/no-explicit-any */
import axios from 'axios';
import KeyCloakService from '../keycloak/KeycloakService';

const _axios = axios.create();

const configure = () => {
  _axios.interceptors.request.use(async (config: any) => {
    if (KeyCloakService.isLoggedIn()) {
      const cb = () => {
        config.headers.Authorization = `Bearer ${KeyCloakService.getToken()}`;
        return Promise.resolve(config);
      };
      return KeyCloakService.updateToken(cb);
    }
  });
};

export { _axios, configure };
