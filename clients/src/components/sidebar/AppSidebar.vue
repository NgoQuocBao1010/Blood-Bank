<script setup>
import SidebarMenu from "./SidebarMenu.vue";
const menu = [
    {
        label: "Overview",
        items: [
            {
                label: "Dashboard",
                icon: "pi pi-fw pi-home",
                to: "/",
            },
        ],
    },
    {
        label: "Management",
        items: [
            {
                label: "Donors",
                icon: "fa-solid fa-user-group",
                to: { name: "Donors" },
            },
        ],
    },
    {
        label: "Information",
        items: [
            {
                label: "About",
                icon: "pi pi-fw pi-info-circle",
                to: { name: "About" },
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
