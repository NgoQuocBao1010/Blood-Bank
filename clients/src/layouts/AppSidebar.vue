<script setup>
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
        label: "Get Started",
        items: [
            {
                label: "Documentation",
                icon: "pi pi-fw pi-question",
                command: () => {
                    window.location = "#/documentation";
                },
            },
            {
                label: "View Source",
                icon: "pi pi-fw pi-search",
                command: () => {
                    window.location = "https://github.com/primefaces/sakai-vue";
                },
            },
        ],
    },
];
</script>

<template>
    <p>Sidebar</p>
</template>

<style lang="scss" scoped></style>
