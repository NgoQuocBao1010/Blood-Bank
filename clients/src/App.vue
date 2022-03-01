<script setup>
import Button from "primevue/button";
import AppSidebar from "./layouts/AppSidebar.vue";

let sidebarVisibility = $ref(true);
</script>

<template>
    <main
        class="layout-wrapper layout-static"
        :class="{ 'layout-static-sidebar-inactive': sidebarVisibility }"
    >
        <!-- Sidebar -->
        <div class="layout-sidebar">
            <AppSidebar />
        </div>
        <div class="layout-main-container">
            <Button
                class="p-button-primary my-btn"
                label="Toggle Sidebar"
                @click="sidebarVisibility = !sidebarVisibility"
            />
        </div>
    </main>
</template>

<style lang="scss">
.my-btn {
    margin: 1rem;
    width: max-content;
    &:focus {
        box-shadow: none !important;
    }
}
</style>
