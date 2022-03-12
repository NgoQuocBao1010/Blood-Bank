import "primeflex/primeflex.css";
import "primeicons/primeicons.css";
import "primevue/resources/primevue.min.css";
import "primevue/resources/themes/lara-light-blue/theme.css";
import "./styles/override.scss";

import { createPinia } from "pinia";
import { createApp } from "vue";
import PrimeVue from "primevue/config";
import ConfirmationService from "primeVue/confirmationservice";
import ToastService from "primeVue/toastservice";
import router from "./router";
import App from "./App.vue";

const app = createApp(App);

app.use(createPinia());
app.use(PrimeVue, { ripple: true });
app.use(ConfirmationService);
app.use(ToastService);
app.use(router);

app.mount("#app");
