import { createRouter, createWebHistory } from "vue-router";

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: "/",
            name: "Dashboard",
            component: () => import("./views/Dashboard.vue"),
        },
        // Management
        {
            path: "/donors-management",
            name: "Donors Management",
            component: () => import("./views/management/Donors.vue"),
        },
        {
            path: "/blood-management",
            name: "Blood Management",
            component: () => import("./views/management/Blood.vue"),
        },
        // Information
        {
            path: "/about",
            name: "About",
            component: () => import("./views/About.vue"),
        },
    ],
});

export default router;
