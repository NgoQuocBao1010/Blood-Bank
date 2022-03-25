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
            component: () => import("./views/activity/DonationRequests.vue"),
        },
        {
            path: "/blood-requests",
            name: "Blood Requests",
            component: () => import("./views/activity/BloodRequests.vue"),
        },
        // Detail
        {
            path: "/events/detail/:_id",
            name: "Event Detail",
            component: () => import("./views/detail/EventDetail.vue"),
            props: true,
        },
        {
            path: "/donors/detail/:_id",
            name: "Donor Detail",
            component: () => import("./views/detail/DonorDetail.vue"),
            props: true,
        },
        // Form
        {
            path: "/event/new/",
            name: "Event Create",
            component: () => import("./views/form/EventForm.vue"),
        },
        {
            path: "/event/edit/:_id",
            name: "Event Edit",
            component: () => import("./views/form/EventForm.vue"),
            props: true,
        },
        // Information
        {
            path: "/about",
            name: "About",
            component: () => import("./views/About.vue"),
        },
        // Error Page
        {
            path: "/error/server",
            name: "Server Error",
            component: () => import("./views/error/ServerError.vue"),
        },
        {
            path: "/:pathMatch(.*)*",
            name: "404 Not found",
            component: () => import("./views/error/404Error.vue"),
        },
    ],
    scrollBehavior(to, from, savedPosition) {
        if (savedPosition) {
            return savedPosition;
        } else {
            return { top: 0, behavior: "smooth" };
        }
    },
});

/* Changing the name of tab */
router.beforeEach((to, _, next) => {
    document.title = `${to.name} | Judoh Admin`;
    return next();
});

export default router;
