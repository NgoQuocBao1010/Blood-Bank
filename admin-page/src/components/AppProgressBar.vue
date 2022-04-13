<script setup>
import ProgressBar from "primevue/progressbar";
</script>

<template>
    <div
        class="flex flex-center"
        style="
            flex-direction: column;
            padding: 3rem 1rem;
            background-color: #f8f9fa;
        "
    >
        <h5>Loading ...</h5>
        <ProgressBar mode="indeterminate" style="height: 0.5em; width: 100%" />
    </div>
</template>
