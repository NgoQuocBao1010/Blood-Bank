<script setup>
import { defineAsyncComponent, markRaw, watch, provide } from "vue";
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
                layoutName = userStore.isLoggedIn
                    ? "LayoutDefault"
                    : "LayoutUnauth";
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

// Global Error Dialog
const AsyncErrorDialog = defineAsyncComponent({
    loader: () => import("primevue/dialog"),
});
let dialogProps = $ref({
    title: null,
    message: null,
});
let isShowErroDialog = $ref(null);
const showErrorDialog = (title, message) => {
    dialogProps = { title, message };
    isShowErroDialog = true;
};
const closeErrorDialog = () => (isShowErroDialog = false);

provide("errorDialog", { showErrorDialog, closeErrorDialog });
</script>

<template>
    <component :is="layout">
        <router-view />
    </component>

    <!-- Error Dialog -->
    <template v-if="isShowErroDialog">
        <AsyncErrorDialog
            :header="dialogProps.title"
            v-model:visible="isShowErroDialog"
            :style="{ width: '50vw' }"
            :modal="true"
        >
            <div class="error-dialog__body">
                <i class="fa-solid fa-circle-exclamation"></i>
                <p>{{ dialogProps.message }}</p>
            </div>
        </AsyncErrorDialog>
    </template>

    <!-- Toast notification -->
    <Toast position="bottom-right" />
</template>

<style lang="scss">
.error-dialog {
    &__header {
        text-align: center;
    }

    &__body {
        width: 100%;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        i {
            font-size: 2rem;
            color: red;
        }

        p {
            text-align: center;
            font-size: 1.1rem;
            margin: 1rem 0;
        }
    }
}
</style>
