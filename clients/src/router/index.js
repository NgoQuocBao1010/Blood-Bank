import { createRouter, createWebHistory } from "vue-router";

import Dashboard from "../views/Dashboard.vue";

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: "/",
            name: "Dashboard",
            component: Dashboard,
        },
        {
            path: "/home",
            name: "Home",
            component: () => import("../views/Home.vue"),
        },
        {
            path: "/about",
            name: "About",
            component: () => import("../views/About.vue"),
        },
    ],
});

export default router;
