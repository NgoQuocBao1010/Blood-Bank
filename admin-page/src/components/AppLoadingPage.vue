<script setup>
import ProgressBar from "primevue/progressbar";
</script>

<template>
    <div class="wrapper flex-center">
        <img src="../assets/images/logo.png" alt="logo" class="logo" />

        <ProgressBar mode="indeterminate" style="height: 0.5em; width: 100%" />
    </div>
</template>

<style lang="scss" scoped>
.wrapper {
    max-width: 50vw;
    padding: 1rem 3em;
    aspect-ratio: 1 / 1;
    background-color: #fff;
    border-radius: 15px;
    flex-direction: column;
    justify-content: space-around;
    .logo {
        width: 150px;
        aspect-ratio: 1 / 1;
    }
}

@keyframes spin {
    0% {
        transform: translateY(0) rotate(0deg);
    }
    100% {
        transform: translateY(-100%) rotate(400deg);
    }
}
</style>
