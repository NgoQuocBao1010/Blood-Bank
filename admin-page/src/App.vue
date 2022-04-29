<script setup>
import { defineAsyncComponent, markRaw, watch, provide } from "vue";
import { useRoute, useRouter } from "vue-router";
import Toast from "primevue/toast";

import { useUserStore } from "./stores/user";
import { useAppStore } from "./stores/app";
import AppLoadingPage from "./components/AppLoadingPage.vue";

const userStore = useUserStore();
const route = useRoute();
const router = useRouter();
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

            layout = markRaw(layoutComponent?.default);
        } catch (e) {
            router.replace({ name: "Server Error" });
            layout = "section";
            throw "Layout Error";
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
let isShowErrorDialog = $ref(null);
const showErrorDialog = (title, message) => {
    dialogProps = { title, message };
    isShowErrorDialog = true;
};
const closeErrorDialog = () => (isShowErrorDialog = false);

provide("errorDialog", { showErrorDialog, closeErrorDialog });
</script>

<template>
    <component :is="layout">
        <router-view :key="route.fullPath" />
    </component>

    <!-- Loading screen -->
    <transition name="fade">
        <div class="loading-wrapper" v-if="useAppStore().loading">
            <AppLoadingPage />
        </div>
    </transition>

    <!-- Error Dialog -->
    <template v-if="isShowErrorDialog">
        <AsyncErrorDialog
            :header="dialogProps.title"
            v-model:visible="isShowErrorDialog"
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

<style lang="scss" scoped>
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

.loading-wrapper {
    position: fixed;
    inset: 0;
    margin: 0;
    padding: 0;
    max-height: 100vh !important;
    z-index: 9999;
    background: rgba(0, 0, 0, 0.7);
    display: flex;
    justify-content: center;
    align-items: center;
    overflow: hidden;
}

// Animation
.fade-enter,
.fade-leave-to {
    opacity: 0;
}

.fade-leave,
.fade-enter-to {
    opacity: 1;
}

.fade-enter-active,
.fade-leave-active {
    transition: all ease 700ms;
}
</style>
