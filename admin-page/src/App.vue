<script setup>
import { markRaw, watch } from "vue";
import { useRoute } from "vue-router";
import Toast from "primevue/toast";

import { useUserStore } from "./stores/user";

const userStore = useUserStore();
const route = useRoute();
let layout = $ref(null);

watch(
    () => route.meta?.layoutName,
    async (layoutName, oldLayout) => {
        try {
            // Catch the first undefined changes
            if (!oldLayout && !layoutName) return;

            // Come to error page
            // If there is old layout, use if
            // Else detect layout base on auth status
            if (layoutName === "__dynamic") {
                if (oldLayout) {
                    return;
                } else {
                    // Verify if token found but user has not logged in
                    if (userStore.token && !userStore.isLoggedIn)
                        await userStore.verifyToken();

                    layoutName = userStore.isLoggedIn
                        ? "LayoutDefault"
                        : "LayoutUnauth";
                }
            }

            // Come from error pages
            // Use the current layout
            if (oldLayout === "__dynamic") return;

            const layoutComponent =
                layoutName && (await import(`./layouts/${layoutName}.vue`));

            layout = markRaw(layoutComponent?.default || "section");
        } catch (e) {
            layout = "section";
        }
    },
    { immediate: true }
);
</script>

<template>
    <component :is="layout">
        <router-view />
    </component>

    <Toast position="bottom-right" />
</template>

<style lang="scss"></style>
