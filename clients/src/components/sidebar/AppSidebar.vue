<script setup>
import SidebarMenu from "./SidebarMenu.vue";
const menu = [
    {
        label: "Home",
        items: [
            {
                label: "Dashboard",
                icon: "pi pi-fw pi-home",
                to: "/",
            },
        ],
    },
    {
        label: "Information",
        items: [
            {
                label: "About",
                icon: "pi pi-fw pi-info-circle",
                to: "/about",
            },
            {
                label: "View Source",
                icon: "pi pi-fw pi-search",
                newTab: true,
                url: "https://github.com/NgoQuocBao1010/Blood-Bank",
            },
        ],
    },
];
</script>

<template>
    <div class="layout-menu-container">
        <SidebarMenu class="layout-menu" :items="menu" :root="true" />
    </div>
</template>

<style lang="scss" scoped></style>
