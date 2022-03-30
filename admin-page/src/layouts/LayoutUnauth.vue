<script setup>
import AppNavbar from "../components/navbar/AppNavbar.vue";
import AppFooter from "../components/footer/AppFooter.vue";
</script>

<template>
    <main>
        <!-- Navbar -->
        <nav class="navbar">
            <router-link to="/" class="navbar-logo">
                <img alt="Logo" src="../assets/images/logo.png" />
                <span class="title">JUDOH Blood Bank Operations</span>
            </router-link>
        </nav>

        <!-- Main -->
        <slot />

        <!-- Footer -->
        <AppFooter v-once />
    </main>
</template>

<style lang="scss" scoped>
main {
    overflow-x: hidden;
}
.navbar {
    display: flex;
    align-items: center;
    justify-content: center;
    background: #fff;
    padding: 0.3rem 0;
    box-shadow: rgba(0, 0, 0, 0.15) 1.95px 1.95px 2.6px;
    margin-bottom: 1rem;

    .navbar-logo {
        display: flex;
        align-items: center;

        img {
            width: 80px;
        }

        span.title {
            margin: 0 1rem;
            font-size: 1.8rem;
            font-weight: 900;
            text-transform: capitalize;
        }
    }
}
</style>
