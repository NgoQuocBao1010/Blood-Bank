<script setup>
import Chart from "primevue/chart";
import Menu from "primevue/menu";

import Overview from "../components/dashboard/Overview.vue";
import OverviewSkeleton from "../components/dashboard/OverviewSkeleton.vue";
import RecentActivities from "../components/dashboard/Recent.vue";
import BloodStorage from "../components/dashboard/BloodStorage.vue";
import BloodStorageSkeleton from "../components/dashboard/BloodStorageSkeleton.vue";
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

// Menu items
let menuItems = $ref([
    {
        label: "Add New",
        icon: "pi pi-fw pi-plus",
        command: () => {
            console.log("Add Blood");
        },
    },
    {
        label: "Remove",
        icon: "pi pi-fw pi-minus",
        command: () => {
            console.log("Remove Blood");
        },
    },
]);
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
            <div class="card">
                <div
                    class="flex align-items-center justify-content-between mb-4"
                >
                    <h5>Notifications</h5>
                    <div>
                        <PrimeVueButton
                            icon="pi pi-ellipsis-v"
                            class="p-button-text p-button-plain p-button-rounded"
                            @click="$refs.menu1.toggle($event)"
                        ></PrimeVueButton>
                        <Menu
                            ref="menu1"
                            :popup="true"
                            :model="menuItems"
                        ></Menu>
                    </div>
                </div>

                <span class="block text-600 font-medium mb-3">TODAY</span>
                <ul class="p-0 mx-0 mt-0 mb-4 list-none">
                    <li
                        class="flex align-items-center py-2 border-bottom-1 surface-border"
                    >
                        <div
                            class="w-3rem h-3rem flex align-items-center justify-content-center bg-blue-100 border-circle mr-3 flex-shrink-0"
                        >
                            <i
                                class="fa-solid fa-hand-holding-droplet text-xl text-blue-500"
                            ></i>
                        </div>
                        <span class="text-900 line-height-3">
                            Event Máu Nhân Đạo
                            <span class="text-700">
                                has received
                                <span class="text-blue-500 font-medium">
                                    50000 ml of blood
                                </span>
                            </span>
                        </span>
                    </li>
                    <li class="flex align-items-center py-2">
                        <div
                            class="w-3rem h-3rem flex align-items-center justify-content-center bg-orange-100 border-circle mr-3 flex-shrink-0"
                        >
                            <i
                                class="pi pi-question text-xl text-orange-500"
                            ></i>
                        </div>
                        <span class="text-700 line-height-3">
                            Chợ Rẫy hospital requested
                            <span class="text-blue-500 font-medium">
                                2500 ml of blood
                            </span>
                            .
                        </span>
                    </li>
                </ul>

                <span class="block text-600 font-medium mb-3">YESTERDAY</span>
                <ul class="p-0 m-0 list-none">
                    <li
                        class="flex align-items-center py-2 border-bottom-1 surface-border"
                    >
                        <div
                            class="w-3rem h-3rem flex align-items-center justify-content-center bg-blue-100 border-circle mr-3 flex-shrink-0"
                        >
                            <i
                                class="fa-solid fa-gift text-xl text-blue-500"
                            ></i>
                        </div>
                        <span class="text-900 line-height-3">
                            Ô Môn hospital
                            <span class="text-700">
                                has been transported
                                <span class="text-blue-500 font-medium">
                                    45000 ml of Blood Type A
                                </span>
                            </span>
                        </span>
                    </li>
                    <li
                        class="flex align-items-center py-2 border-bottom-1 surface-border"
                    >
                        <div
                            class="w-3rem h-3rem flex align-items-center justify-content-center bg-pink-100 border-circle mr-3 flex-shrink-0"
                        >
                            <i
                                class="fa-solid fa-flag-checkered text-xl text-pink-500"
                            ></i>
                        </div>
                        <span class="text-900 line-height-3">
                            Event Máu Nhân Đạo
                            <span class="text-700 font-medium">
                                has started.
                            </span>
                        </span>
                    </li>
                </ul>
            </div>
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
