import { createRouter, createWebHistory } from "vue-router";

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: "/",
            name: "Dashboard",
            component: () => import("./views/Dashboard.vue"),
        },
        {
            path: "/donors-management",
            name: "Donors",
            component: () => import("./views/management/Donors.vue"),
        },
        {
            path: "/about",
            name: "About",
            component: () => import("./views/About.vue"),
        },
    ],
});

export default router;
