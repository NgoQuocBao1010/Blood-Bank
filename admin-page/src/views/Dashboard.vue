<script setup>
import Chart from "primevue/chart";
import Menu from "primevue/menu";

import RecentActivities from "../components/dashboard/Recent.vue";
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
        <!-- Blood Receive -->
        <div class="col-12 lg:col-6 xl:col-3">
            <div class="card mb-0">
                <div class="flex justify-content-between mb-3">
                    <div>
                        <span class="block text-500 font-medium mb-3">
                            Blood Receive
                        </span>
                        <div class="text-900 font-medium text-xl">25.00 L</div>
                    </div>
                    <div
                        class="flex align-items-center justify-content-center bg-orange-100 border-round"
                        style="width: 2.5rem; height: 2.5rem"
                    >
                        <i
                            class="text-orange-500 text-xl fa-solid fa-hand-holding-medical"
                        ></i>
                    </div>
                </div>
                <span class="text-green-500 font-medium">%52+ </span>
                <span class="text-500">since last quater</span>
            </div>
        </div>

        <!-- Blood donated -->
        <div class="col-12 lg:col-6 xl:col-3">
            <div class="card mb-0">
                <div class="flex justify-content-between mb-3">
                    <div>
                        <span class="block text-400 font-medium mb-3"
                            >Blood Donated</span
                        >
                        <div class="text-900 font-medium text-xl">500.00 L</div>
                    </div>
                    <div
                        class="flex align-items-center justify-content-center bg-cyan-100 border-round"
                        style="width: 2.5rem; height: 2.5rem"
                    >
                        <i
                            class="text-cyan-500 text-xl fa-solid fa-hand-holding-droplet"
                        ></i>
                    </div>
                </div>
                <span class="text-500 font-medium text-decreased"> %17- </span>
                <span class="text-500">since last quater</span>
            </div>
        </div>

        <!-- Donators -->
        <div class="col-12 lg:col-6 xl:col-3">
            <div class="card mb-0">
                <div class="flex justify-content-between mb-3">
                    <div>
                        <span class="block text-500 font-medium mb-3"
                            >Donators</span
                        >
                        <div class="text-900 font-medium text-xl">152</div>
                    </div>
                    <div
                        class="flex align-items-center justify-content-center bg-blue-100 border-round"
                        style="width: 2.5rem; height: 2.5rem"
                    >
                        <i class="text-blue-500 text-xl fa-solid fa-users"></i>
                    </div>
                </div>
                <span class="text-green-500 font-medium">24 new </span>
                <span class="text-500">since last quater</span>
            </div>
        </div>

        <!-- Events -->
        <div class="col-12 lg:col-6 xl:col-3">
            <div class="card mb-0">
                <div class="flex justify-content-between mb-3">
                    <div>
                        <span class="block text-500 font-medium mb-3"
                            >Events</span
                        >
                        <div class="text-900 font-medium text-xl">30</div>
                    </div>
                    <div
                        class="flex align-items-center justify-content-center bg-purple-100 border-round"
                        style="width: 2.5rem; height: 2.5rem"
                    >
                        <i class="pi pi-calendar text-purple-500 text-xl"></i>
                    </div>
                </div>
                <span class="text-green-500 font-medium">5 new </span>
                <span class="text-500">since last quater</span>
            </div>
        </div>

        <!-- Recent Activities && Blood Type Storage -->
        <div class="col-12 xl:col-6">
            <!-- Activity table -->
            <RecentActivities />

            <!-- Blood Type Storage -->
            <div class="card">
                <div
                    class="flex justify-content-between align-items-center mb-5"
                >
                    <h5>Blood Type Storage</h5>
                    <div>
                        <PrimeVueButton
                            icon="pi pi-ellipsis-v"
                            class="p-button-text p-button-plain p-button-rounded"
                            @click="$refs.menu2.toggle($event)"
                        ></PrimeVueButton>
                        <Menu
                            ref="menu2"
                            :popup="true"
                            :model="menuItems"
                        ></Menu>
                    </div>
                </div>
                <ul class="list-none p-0 m-0">
                    <li
                        class="flex flex-column md:flex-row md:align-items-center md:justify-content-between mb-4"
                    >
                        <div>
                            <span
                                class="text-900 font-medium mr-2 mb-1 md:mb-0"
                            >
                                Type A
                            </span>
                            <div class="mt-1 text-600">A +, A -</div>
                        </div>
                        <div class="mt-2 md:mt-0 flex align-items-center">
                            <div
                                class="surface-300 border-round overflow-hidden w-10rem lg:w-6rem"
                                style="height: 8px"
                            >
                                <div
                                    class="bg-orange-500 h-full"
                                    style="width: 50%"
                                ></div>
                            </div>
                            <span class="text-orange-500 ml-3 font-medium">
                                500.000 ml
                            </span>
                        </div>
                    </li>
                    <li
                        class="flex flex-column md:flex-row md:align-items-center md:justify-content-between mb-4"
                    >
                        <div>
                            <span
                                class="text-900 font-medium mr-2 mb-1 md:mb-0"
                            >
                                Type O
                            </span>
                            <div class="mt-1 text-600">O +, O -</div>
                        </div>
                        <div
                            class="mt-2 md:mt-0 ml-0 md:ml-8 flex align-items-center"
                        >
                            <div
                                class="surface-300 border-round overflow-hidden w-10rem lg:w-6rem"
                                style="height: 8px"
                            >
                                <div
                                    class="bg-cyan-500 h-full"
                                    style="width: 16%"
                                ></div>
                            </div>
                            <span class="text-cyan-500 ml-3 font-medium">
                                160.000 ml
                            </span>
                        </div>
                    </li>
                    <li
                        class="flex flex-column md:flex-row md:align-items-center md:justify-content-between mb-4"
                    >
                        <div>
                            <span
                                class="text-900 font-medium mr-2 mb-1 md:mb-0"
                            >
                                Type B
                            </span>
                            <div class="mt-1 text-600">B+, B-</div>
                        </div>
                        <div
                            class="mt-2 md:mt-0 ml-0 md:ml-8 flex align-items-center"
                        >
                            <div
                                class="surface-300 border-round overflow-hidden w-10rem lg:w-6rem"
                                style="height: 8px"
                            >
                                <div
                                    class="bg-pink-500 h-full"
                                    style="width: 67%"
                                ></div>
                            </div>
                            <span class="text-pink-500 ml-3 font-medium">
                                670.000 ml
                            </span>
                        </div>
                    </li>
                    <li
                        class="flex flex-column md:flex-row md:align-items-center md:justify-content-between mb-4"
                    >
                        <div>
                            <span class="text-900 font-medium mr-2 mb-1 md:mb-0"
                                >Type AB</span
                            >
                            <div class="mt-1 text-600">...</div>
                        </div>
                        <div
                            class="mt-2 md:mt-0 ml-0 md:ml-8 flex align-items-center"
                        >
                            <div
                                class="surface-300 border-round overflow-hidden w-10rem lg:w-6rem"
                                style="height: 8px"
                            >
                                <div
                                    class="bg-green-500 h-full"
                                    style="width: 35%"
                                ></div>
                            </div>
                            <span class="text-green-500 ml-3 font-medium">
                                350.000 ml
                            </span>
                        </div>
                    </li>
                </ul>
            </div>
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
    .text-decreased {
        color: var(--primary-color) !important;
    }
}
</style>
