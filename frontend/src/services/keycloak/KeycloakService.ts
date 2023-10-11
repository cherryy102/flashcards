/* eslint-disable @typescript-eslint/no-explicit-any */
import Keycloak from 'keycloak-js';

const initOptions = {
  url: import.meta.env.VITE_APP_KEYCLOAK_URL,
  realm: import.meta.env.VITE_APP_KEYCLOAK_REALM,
  clientId: import.meta.env.VITE_APP_KEYCLOAK_CLIENT,
};

const keycloakInstance = new Keycloak(initOptions);

const login = (onAuthenticatedCallback: () => any) => {
  keycloakInstance
    .init({ onLoad: 'login-required' })
    .then(function (authenticated) {
      authenticated ? onAuthenticatedCallback() : alert('non authenticated');
    })
    .catch((e) => {
      console.dir(e);
      console.log(`keycloak init exception: ${e}`);
    });
};

const logout = keycloakInstance.logout;

const isLoggedIn = () => !!keycloakInstance.token;

const getToken = () => keycloakInstance.token;

const doLogin = keycloakInstance.login;

const updateToken = (successCallback: () => any) =>
  keycloakInstance.updateToken(5).then(successCallback).catch(doLogin);

const KeyCloakService = {
  callLogin: login,
  callLogout: logout,
  isLoggedIn: isLoggedIn,
  getToken: getToken,
  updateToken: updateToken,
  keycloakInstance,
};

export default KeyCloakService;
