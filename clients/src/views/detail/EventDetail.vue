<script setup>
const props = defineProps({
    id: String,
});
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <h2>Event ID: {{ props.id }}</h2>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped></style>
