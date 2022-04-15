<script setup>
import Chart from "primevue/chart";

import Overview from "../components/dashboard/Overview.vue";
import OverviewSkeleton from "../components/dashboard/OverviewSkeleton.vue";
import RecentActivities from "../components/dashboard/Recent.vue";
import BloodStorage from "../components/dashboard/BloodStorage.vue";
import BloodStorageSkeleton from "../components/dashboard/BloodStorageSkeleton.vue";
import Notification from "../components/dashboard/Notification.vue";
import { THEMES } from "../constants/index.js";

// Chart data
let data = $ref({
    labels: ["January", "February", "March", "April", "May", "June", "July"],
    datasets: [
        {
            label: "Blood Received",
            data: [65, 59, 80, 81, 56, 55, 40],
            fill: false,
            backgroundColor: THEMES.primaryColor,
            borderColor: THEMES.primaryColor,
            tension: 0.4,
        },
        {
            label: "Blood Donated",
            data: [28, 48, 40, 19, 86, 27, 90],
            fill: false,
            backgroundColor: THEMES.darkColor,
            borderColor: THEMES.darkColor,
            tension: 0.4,
        },
    ],
});
</script>

<template>
    <div class="grid">
        <!-- General Information -->
        <Suspense>
            <template #default>
                <Overview />
            </template>
            <template #fallback>
                <OverviewSkeleton />
            </template>
        </Suspense>

        <!-- Recent Activities && Blood Type Storage -->
        <div class="col-12 xl:col-6">
            <!-- Activity table -->
            <RecentActivities />

            <!-- Blood Storage -->
            <Suspense>
                <template #default>
                    <BloodStorage />
                </template>
                <template #fallback>
                    <BloodStorageSkeleton />
                </template>
            </Suspense>
        </div>

        <!-- Blood Activities && Notification Panel -->
        <div class="col-12 xl:col-6">
            <!-- Blood Chart Activities -->
            <div class="card">
                <h5>Blood Activites</h5>
                <Chart type="line" :data="data" :options="null" />
            </div>

            <!-- Notification Panel -->
            <Notification />
        </div>
    </div>
</template>

<style lang="scss" scoped>
.grid {
    .card > span {
        @media screen and (max-width: 1300px) {
            font-size: 0.9rem;
        }
    }
}
</style>
