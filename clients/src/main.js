import "primevue/resources/themes/lara-dark-blue/theme.css";
import "primevue/resources/primevue.min.css";
import "primeicons/primeicons.css";

import "./assets/styles/layout.scss";

import { createApp } from "vue";
import { createPinia } from "pinia";
import PrimeVue from "primevue/config";
import ConfirmationService from "primeVue/confirmationservice";
import ToastService from "primeVue/toastservice";

import App from "./App.vue";
import router from "./router";

const app = createApp(App);

app.use(createPinia());
app.use(PrimeVue, { ripple: true });
app.use(ConfirmationService);
app.use(ToastService);
app.use(router);

app.mount("#app");
