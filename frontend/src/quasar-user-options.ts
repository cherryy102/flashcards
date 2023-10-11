import lang from 'quasar/lang/pl';
import '@quasar/extras/roboto-font/roboto-font.css';
import '@quasar/extras/material-icons/material-icons.css';
import '@quasar/extras/fontawesome-v5/fontawesome-v5.css';
import { Notify, Dialog, Loading, Cookies } from 'quasar';

// To be used on app.use(Quasar, { ... })
export default {
  config: {},
  plugins: {
    Notify,
    Dialog,
    Loading,
    Cookies,
  },
  lang,
};
