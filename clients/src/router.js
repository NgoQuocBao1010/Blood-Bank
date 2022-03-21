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
        {
            path: "/events-management",
            name: "Events Management",
            component: () => import("./views/management/Events.vue"),
        },
        // Activity
        {
            path: "/donation-requests",
            name: "Donation Requests",
            component: () => import("./views/acitivity/DonationRequests.vue"),
        },
        {
            path: "/blood-requests",
            name: "Blood Requests",
            component: () => import("./views/acitivity/BloodRequests.vue"),
        },
        // Information
        {
            path: "/about",
            name: "About",
            component: () => import("./views/About.vue"),
        },
    ],
});

/* Changing the name of tab */
router.beforeEach((to, _, next) => {
    document.title = `${to.name} | Judoh Admin`;
    return next();
});

export default router;
