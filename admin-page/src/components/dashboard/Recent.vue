<script setup>
import { onBeforeMount } from "vue";

import AppRepo from "../../api/AppRepo";
import { timeDiffernet } from "../../utils";

// Activitiy data
let activities = $ref([]);
let fetchingData = $ref(false);

onBeforeMount(async () => {
    fetchingData = true;
    const { data, status } = await AppRepo.getRecentActivities();

    activities = JSON.parse(JSON.stringify(data));
    fetchingData = false;
});
</script>

<template>
    <div class="card">
        <h5>5 Recent Activites</h5>
        <PrimeVueTable
            :value="activities"
            :loading="fetchingData"
            :rows="5"
            responsiveLayout="scroll"
        >
            <!-- Type -->
            <PrimeVueColumn field="type" header="Type"></PrimeVueColumn>

            <!-- Detail -->
            <PrimeVueColumn
                field="detail"
                header="Detail"
                :sortable="true"
            ></PrimeVueColumn>

            <!-- Date -->
            <PrimeVueColumn field="date">
                <template #body="{ data }">
                    {{ timeDiffernet(parseInt(data.date)) }}
                </template>
            </PrimeVueColumn>

            <!-- View Icon -->
            <PrimeVueColumn>
                <template #header> View </template>
                <template #body="{ data }">
                    <router-link
                        :to="{
                            name: 'Donor Detail',
                            params: { _id: data._id },
                            query: { donation: true },
                        }"
                        class="p-button p-button-sm app-router-link-icon btn-icon-only"
                    >
                        <i class="fa-solid fa-magnifying-glass"></i>
                    </router-link>
                </template>
            </PrimeVueColumn>
        </PrimeVueTable>
    </div>
</template>

<style lang="scss" scoped></style>
