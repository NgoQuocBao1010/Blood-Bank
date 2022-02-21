<script setup>
import { RouterView, RouterLink } from "vue-router";
import Button from "primevue/button";
import Dialog from "primevue/dialog";

const name = $ref("Quoc Bao Ngo");
const clickMe = () => {
    if (name.length > 25) name = "Quoc Bao Ngo";
    else name += " Changed";
};

let isOpenDialog = $ref(false);

let isOpenConfirm = $ref(false);
</script>

<template>
    <router-link :to="{ name: 'Home' }">Home</router-link>
    <router-link :to="{ name: 'About' }">About</router-link>
    <main>
        <p>
            Dynamic Content:
            <span>{{ name }}</span>
        </p>

        <Button
            @click="clickMe"
            label="PrimeVue Button"
            class="p-button-success my-btn"
        />

        <Button @click="() => (isOpenDialog = true)" label="Open Dialog" class="my-btn" />

        <Button
            @click="() => (isOpenConfirm = true)"
            label="Open Confirm"
            class="my-btn"
            style="background-color: lightcoral; border: none"
        />

        <RouterView />
    </main>

    <Dialog
        v-model:visible="isOpenDialog"
        modal="true"
        header="My Dialog"
        style="max-width: 80%"
    >
        Lorem ipsum dolor sit amet, consectetur adipisicing elit. Esse nihil id
        perferendis doloribus dolores deserunt et iste ut quam iure, dolor sint culpa
        facilis dignissimos eos quibusdam modi, facere nesciunt, voluptatum impedit enim.
        Impedit nulla aliquid blanditiis magnam dolorem minus?
    </Dialog>
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

.my-btn {
    margin: 1rem;

    &:enabled:focus {
        border: none !important;
        outline: none;
    }
}
</style>
