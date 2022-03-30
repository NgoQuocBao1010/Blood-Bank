<script setup>
import { useUserStore } from "../stores/user";

const user = useUserStore();
</script>

<template>
    <p>{{ user.email }}</p>
    <p style="max-width: 50ch; white-space: initial; word-wrap: break-word">
        {{ user.token }}
    </p>
</template>

<style lang="scss" scoped></style>
