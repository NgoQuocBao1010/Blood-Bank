import { createRouter, createWebHistory } from "vue-router";

import Welcome from "../views/Welcome/Welcome.vue";
import About from "../views/About.vue";
import Donate from "../views/Donate.vue";
import Login from "../views/Login.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      component: Welcome,
    },
    {
      path: "/about",
      component: About,
    },
    {
      path: "/donate",
      component: Donate,
    },
    {
      path: "/login",
      component: Login,
    },
  ],
});

export default router;
