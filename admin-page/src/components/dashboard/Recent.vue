<script setup>
import { onBeforeMount } from "vue";

import AppRepo from "../../api/AppRepo.js";
import { timeDifference } from "../../utils";

// Activitiy data
let activities = $ref([]);
let fetchingData = $ref(false);

const getDetailRoute = (data) => {
    if (!["Hospital", "Donor"].includes(data.trigger))
        throw "Unidentified activities on recent activities table";

    const detail = {};

    return {
        name: `${data.trigger} Detail`,
        params: {
            _id: data.triggerId,
            filterData: data.detailId,
        },
    };
};

onBeforeMount(async () => {
    fetchingData = true;
    const { data, status } = await AppRepo.getRecentActivities();

    activities = JSON.parse(JSON.stringify(data));
    fetchingData = false;
});
</script>

<template>
    <div class="card">
        <h5>Blood Activities</h5>
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
                            v-if="data.type === 'plus'"
                            style="color: lightgreen"
                        ></i>
                        <i
                            class="fa-solid fa-circle-minus icon-type"
                            v-if="data.type === 'minus'"
                            style="color: lightcoral"
                        ></i>
                        <!-- Mock data -->
                        {{ data.amount }} ml
                    </span>
                </template>
            </PrimeVueColumn>

            <!-- Date -->
            <PrimeVueColumn field="date">
                <template #body="{ data }">
                    {{ timeDifference(parseInt(data.date)) }}
                </template>
            </PrimeVueColumn>

            <!-- View Icon -->
            <PrimeVueColumn>
                <template #header> View </template>
                <template #body="{ data }">
                    <router-link
                        :to="getDetailRoute(data)"
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
