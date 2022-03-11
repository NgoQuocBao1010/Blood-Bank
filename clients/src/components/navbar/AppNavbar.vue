<script setup>
const emit = defineEmits(["toggleSidebar"]);
</script>

<template>
    <div class="layout-topbar">
        <!-- Logo -->
        <router-link to="/" class="layout-topbar-logo">
            <img alt="Logo" src="../../assets/images/logo.png" />
            <span>JUDOH</span>
        </router-link>

        <!-- Mobile icon toggle for settings -->
        <button
            class="p-link layout-topbar-menu-button layout-topbar-button"
            v-styleclass="{
                selector: '@next',
                enterClass: 'hidden',
                enterActiveClass: 'scalein',
                leaveToClass: 'hidden',
                leaveActiveClass: 'fadeout',
                hideOnOutsideClick: true,
            }"
        >
            <i class="pi pi-ellipsis-v"></i>
        </button>

        <!-- Top bar Menu -->
        <ul
            class="layout-topbar-menu hidden lg:flex origin-top"
            style="margin-top: 0.5rem"
        >
            <li>
                <button
                    class="p-link layout-topbar-button"
                    @click="emit('toggleSidebar')"
                >
                    <i class="pi pi-bars"></i>
                    <span>Menu</span>
                </button>
            </li>
            <li>
                <button class="p-link layout-topbar-button">
                    <i class="pi pi-user"></i>
                    <span>Profile</span>
                </button>
            </li>
        </ul>
    </div>
</template>

<style lang="scss" scoped>
.layout-topbar-logo {
    img {
        height: 5rem;
    }
}
</style>
