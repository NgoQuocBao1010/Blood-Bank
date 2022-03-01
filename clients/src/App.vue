<script setup>
import { RouterView } from "vue-router";
import Button from "primevue/button";
import AppNavbar from "./layouts/navbar/AppNavbar.vue";
import AppSidebar from "./layouts/sidebar/AppSidebar.vue";

let sidebarVisibility = $ref(true);
</script>

<template>
    <main
        class="layout-wrapper layout-static"
        :class="{ 'layout-static-sidebar-inactive': sidebarVisibility }"
    >
        <!-- Navbar -->
        <AppNavbar @toggleSidebar="sidebarVisibility = !sidebarVisibility" />

        <!-- Sidebar -->
        <div class="layout-sidebar">
            <AppSidebar />
        </div>

        <!-- Main -->
        <div class="layout-main-container">
            <div class="layout-main">
                <router-view />
            </div>
        </div>
    </main>
</template>

<style lang="scss">
.my-btn {
    margin: 1rem 0;
    width: max-content;
    &:focus {
        box-shadow: none !important;
    }
}
</style>
