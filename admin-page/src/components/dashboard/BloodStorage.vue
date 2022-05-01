<script setup>
import { useBloodStore } from "../../stores/blood";

const bloodStore = useBloodStore();
await bloodStore.getData();
let data = $ref(bloodStore.summaryData);

const maxQuantity = $computed(() => {
    const quantity = data.map((row) => row.quantity);

    return Math.max(...quantity) + 10000;
});
</script>

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
                v-for="blood in data"
            >
                <!-- Blood Name -->
                <div>
                    <span
                        class="text-900 font-medium mr-2 mb-1 md:mb-0"
                        :class="`bg-color__${blood.name}`"
                        style="background: #fff !important"
                    >
                        Type {{ blood.name }}
                    </span>
                    <div class="mt-1 text-600">
                        {{ blood.name }} +, {{ blood.name }} -
                    </div>
                </div>

                <!-- Blood Value -->
                <div class="mt-2 md:mt-0 flex align-items-center">
                    <div
                        class="surface-300 border-round overflow-hidden w-10rem lg:w-6rem"
                        style="height: 8px"
                    >
                        <div
                            class="h-full"
                            :class="`bg-color__${blood.name}`"
                            :style="{
                                width: `${Math.floor(
                                    (blood.quantity / maxQuantity) * 100
                                )}%`,
                            }"
                        ></div>
                    </div>
                    <span
                        class="ml-3 font-medium"
                        :class="`bg-color__${blood.name}`"
                        style="width: 5rem; background: #fff !important"
                    >
                        {{ blood.quantity }} ml
                    </span>
                </div>
            </li>
        </ul>
    </div>
</template>

<style lang="scss" scoped>
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
