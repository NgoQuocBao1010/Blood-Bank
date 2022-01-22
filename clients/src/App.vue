<script setup>
import { RouterView, RouterLink } from "vue-router";
const name = $ref("Quoc Bao Ngo");
</script>

<template>
    <router-link :to="{ name: 'Home' }">Home</router-link>
    <router-link :to="{ name: 'About' }">About</router-link>
    <main>
        <p>
            New Compostion API Template
            <span>{{ name }}</span>
        </p>

        <RouterView />
    </main>
</template>

<style lang="scss">
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

html {
    background-color: black;
    color: #fff;
}

a {
    color: white;
    padding: 1rem 2rem;
    transition: all 0.4s ease;
    &.router-link-active {
        color: rgba(73, 255, 88, 0.74);
    }
}

main {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);

    p {
        font-size: 2rem;
        font-weight: bold;
        span {
            color: rgba(73, 255, 88, 0.74);
        }
    }
}
</style>
