import { createRouter, createWebHistory } from "vue-router";

import Welcome from "../views/Welcome/Welcome.vue";
import About from "../views/About/About.vue";
import Login from "../views/Login/Login.vue";
import Donate from "../views/Donate/Donate.vue";
import EventDetails from "../views/Donate/EventDetails.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "Home",
      component: Welcome,
    },
    {
      path: "/about",
      name: "About",
      component: About,
    },
    {
      path: "/donate",
      name: "Donate",
      component: Donate,
    },
    {
      path: "/donate/:eventId",
      name: "Event Details",
      component: EventDetails,
    },
    {
      path: "/login",
      name: "Login",
      component: Login,
    },
  ],
});

export default router;
