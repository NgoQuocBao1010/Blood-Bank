<script setup>
import { onMounted } from "vue";
import { RouterView } from "vue-router";
import AppNavbar from "./components/navbar/AppNavbar.vue";
import AppSidebar from "./components/sidebar/AppSidebar.vue";
import Footer from "./components/Footer.vue";

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
        <div class="layout-sidebar">
            <AppSidebar />
        </div>

        <!-- Main -->
        <div class="layout-main-container">
            <div class="layout-main">
                <router-view />
            </div>
            <Footer />
        </div>
    </main>
</template>

<style lang="scss"></style>
