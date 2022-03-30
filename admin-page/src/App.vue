<script setup>
import { useRoute } from "vue-router";
import Toast from "primevue/toast";
import { markRaw, watch } from "vue";

let layout = $ref("section");
const route = useRoute();
watch(
    () => route.meta?.layoutName,
    async (layoutName) => {
        try {
            const layoutComponent =
                layoutName && (await import(`./layouts/${layoutName}.vue`));

            layout = markRaw(layoutComponent?.default || "section");
        } catch (e) {
            layout = "section";
        }
    },
    { immediate: true }
);
</script>

<template>
    <component :is="layout">
        <router-view />
    </component>

    <Toast position="bottom-right" />
</template>

<style lang="scss"></style>
