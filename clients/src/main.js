import "primevue/resources/themes/saga-purple/theme.css";
import "primevue/resources/primevue.min.css";
import "primeicons/primeicons.css";

import "./assets/styles/layout.scss";

import { createApp } from "vue";
import { createPinia } from "pinia";
import PrimeVue from "primevue/config";
import ConfirmationService from "primeVue/confirmationservice";
import ToastService from "primeVue/toastservice";
import Ripple from "primevue/ripple";
import StyleClass from "primevue/styleclass";

import App from "./App.vue";
import router from "./router";

const app = createApp(App);

app.use(createPinia());
app.use(router);

// PrimeVue directives and services
app.use(PrimeVue, { ripple: true });
app.directive("ripple", Ripple);
app.directive("styleclass", StyleClass);
app.use(ConfirmationService);
app.use(ToastService);

app.mount("#app");
