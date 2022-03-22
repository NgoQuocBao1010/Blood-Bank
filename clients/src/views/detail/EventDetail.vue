<script setup>
import Breadcrumb from "primevue/breadcrumb";

const props = defineProps({
    _id: String,
});

const home = $ref({
    icon: "fa-solid fa-calendar-days",
    to: { name: "Events Management" },
});

const items = $ref([{ label: "NAME event" }]);
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <Breadcrumb
                :home="home"
                :model="items"
                style="margin-bottom: 1rem; border-radius: 15px"
            />
            <div class="card">
                <h2>Event ID: {{ props._id }}</h2>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped></style>
