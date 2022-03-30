<script setup>
import { useUserStore } from "../stores/user";

const user = useUserStore();
let errorMessage = $ref(null);

const login = async () => {
    try {
        await user.login("admin@gmail.com", "admin");
        errorMessage = null;
    } catch (e) {
        if (e.response) {
            const { status } = e.response;
            if (status === 400) {
                errorMessage = "Email Or Password is invalid";
            } else throw e;
        } else {
            throw e;
        }
    }
};
</script>

<template>
    <p>{{ user.email }}</p>
    <p style="max-width: 50ch; white-space: initial; word-wrap: break-word">
        {{ user.token }}
    </p>
    <p v-if="errorMessage">{{ errorMessage }}</p>
    <button @click="login()">Logged In</button>
    <button @click="user.logout()">Logout</button>
</template>

<style lang="scss" scoped></style>
