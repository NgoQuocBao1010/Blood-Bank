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
    console.log(activities);
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
            <!-- Detail -->
            <PrimeVueColumn field="detail" header="Detail"></PrimeVueColumn>

            <!-- Type -->
            <PrimeVueColumn field="type" header="Amount">
                <template #body="{ data }">
                    <span class="flex" style="align-items: center">
                        <i
                            class="fa-solid fa-circle-plus icon-type"
                            v-if="data.type === 'Receive'"
                            style="color: lightgreen"
                        ></i>
                        <i
                            class="fa-solid fa-circle-minus icon-type"
                            v-else
                            style="color: lightcoral"
                        ></i>
                        <!-- Mock data -->
                        200ml
                    </span>
                </template>
            </PrimeVueColumn>

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
                        <i
                            class="fa-solid fa-magnifying-glass"
                            style="font-size: 0.7rem"
                        ></i>
                    </router-link>
                </template>
            </PrimeVueColumn>
        </PrimeVueTable>
    </div>
</template>

<style lang="scss" scoped>
.icon-type {
    font-size: 1.5rem;
    margin-right: 0.4em;
}
</style>
