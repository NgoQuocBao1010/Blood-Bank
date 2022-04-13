<script setup></script>

<template>
    <!-- Blood Type Storage -->
    <div class="card">
        <div class="flex justify-content-between align-items-center mb-5">
            <h5>Blood Type Storage</h5>
        </div>

        <!-- Detail -->
        <ul class="list-none p-0 m-0">
            <li
                class="flex flex-column md:flex-row md:align-items-center md:justify-content-between mb-4"
                v-for="_ in 5"
            >
                <!-- Blood Name -->
                <div style="width: 8rem">
                    <span class="mr-2 mb-1 md:mb-0 blood__title"> </span>
                    <div class="mt-1 text-600 blood__type"></div>
                </div>

                <!-- Blood Value -->
                <div class="mt-2 md:mt-0 flex align-items-center">
                    <div
                        class="border-round overflow-hidden w-10rem lg:w-6rem blood__bar"
                        style="height: 8px"
                    ></div>
                    <span
                        class="ml-3 font-medium blood__detail"
                        style="width: 5rem"
                    >
                    </span>
                </div>
            </li>
        </ul>
    </div>
</template>

<style lang="scss" scoped>
.blood {
    &__title {
        display: block;
        width: 8rem !important;
        height: 1.5rem !important;
        animation: 1.5s loading_skeleton linear infinite;
    }

    &__type {
        width: 4rem !important;
        height: 1rem !important;
        animation: 1.5s loading_skeleton linear infinite;
    }

    &__bar {
        animation: 1.5s loading_skeleton linear infinite;
    }

    &__detail {
        width: 5rem !important;
        height: 1.5rem !important;
        animation: 1.5s loading_skeleton linear infinite;
    }
}
.bg-color {
    &__A {
        background: #256029;
        color: #256029 !important;
    }

    &__B {
        background: #c63737;
        color: #c63737 !important;
    }

    &__AB {
        background: #8a5340;
        color: #8a5340 !important;
    }

    &__O {
        background: #23547b;
        color: #23547b !important;
    }

    &__Rh {
        background: #694382;
        color: #694382 !important;
    }
}
</style>
