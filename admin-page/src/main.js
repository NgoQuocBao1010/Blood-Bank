import "primevue/resources/themes/saga-purple/theme.css";
import "primevue/resources/primevue.min.css";
import "primeflex/primeflex.css";
import "primeicons/primeicons.css";
import "./assets/styles/layout.scss";

import { createApp } from "vue";
import { createPinia } from "pinia";
import PrimeVue from "primevue/config";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";
import ConfirmationService from "primeVue/confirmationservice";
import ToastService from "primeVue/toastservice";
import Ripple from "primevue/ripple";
import Tooltip from "primevue/tooltip";
import StyleClass from "primevue/styleclass";

import App from "./App.vue";
import router from "./router.js";

const app = createApp(App);

app.use(createPinia());
app.use(router);

// PrimeVue directives, services and components
app.use(PrimeVue, { ripple: true });
app.directive("ripple", Ripple);
app.directive("tooltip", Tooltip);
app.directive("styleclass", StyleClass);
app.use(ConfirmationService);
app.use(ToastService);
app.component("PrimeVueTable", DataTable);
app.component("PrimeVueColumn", Column);
app.component("PrimeVueButton", Button);

app.mount("#app");

// ** Warn handler
app.config.warnHandler = (msg, instance, trace) => {
    console.log("🚀 ~ file: main.js ~ line 42 ~ msg", msg);
};

// ** Error handler
app.config.errorHandler = (err, instance, info) => {
    if (!err.response && err.message === "Network Error") {
        // Handle server cannot reach error (Ex: forget to start the server)
        router.push({ name: "Server Error" });
        return;
    }

    if (err.response) {
        console.log("FROM MAIN.JS", err.response);
        if (err.response.status === 500) router.push({ name: "Server Error" });
        else if (err.response.status === 401)
            router.push({ name: "Unauthorized Error" });
        else if (err.response.status === 404)
            router.push({ name: "404 Error" });
        return;
    }

    console.log("FROM MAIN.JS", err, instance, info);
    throw err;
};
