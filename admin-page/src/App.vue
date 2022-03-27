<script setup>
import { onMounted } from "vue";
import { RouterView } from "vue-router";
import Toast from "primevue/toast";

import AppNavbar from "./components/navbar/AppNavbar.vue";
import AppSidebar from "./components/sidebar/AppSidebar.vue";
import AppFooter from "./components/footer/AppFooter.vue";

let sidebarHide = $ref(null);

const winWidth =
    window.innerWidth ||
    document.documentElement.clientWidth ||
    document.body.clientWidth;

onMounted(() => {
    // Hide sidebar if window's width is lower than 1280
    sidebarHide = winWidth <= 1280 ? true : false;
});
</script>

<template>
    <main
        class="layout-wrapper layout-static"
        :class="{ 'layout-static-sidebar-inactive': sidebarHide }"
    >
        <!-- Navbar -->
        <AppNavbar @toggleSidebar="sidebarHide = !sidebarHide" />

        <!-- Sidebar -->
        <div class="layout-sidebar scrollbar-style">
            <AppSidebar />
        </div>

        <!-- Main -->
        <div class="layout-main-container">
            <div class="layout-main">
                <router-view />
            </div>
            <AppFooter v-once />
        </div>
    </main>

    <Toast position="bottom-right" />
</template>

<style lang="scss"></style>
