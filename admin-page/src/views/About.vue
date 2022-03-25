<script setup>
import { useUserStore } from "../stores/user";

const user = useUserStore();
</script>

<template>
    <h1>{{ user.email }}</h1>
    <h1>{{ user.token }}</h1>
    <button @click="user.login('admin@gmail.com', 'admin')">Logged In</button>
    <button @click="user.logout()">Logout</button>
</template>

<style lang="scss" scoped></style>
