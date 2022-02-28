<script setup>
import Button from "primevue/button";
import AppSidebar from "./layouts/AppSidebar.vue";

let sidebarHide = $ref(true);
</script>

<template>
    <main
        class="layout-wrapper layout-static"
        :class="{ 'layout-static-sidebar-inactive': sidebarHide }"
    >
        <!-- Sidebar -->
        <div class="layout-sidebar">
            <AppSidebar />
        </div>
        <div class="layout-main-container">
            <Button class="p-button-secondary my-btn" @click="sidebarHide = !sidebarHide">
                Toggle Sidebar
            </Button>
        </div>
    </main>
</template>

<style lang="scss">
.my-btn {
    margin: 1rem;
    width: 500px;
    &:focus {
        box-shadow: none !important;
    }
}
</style>
