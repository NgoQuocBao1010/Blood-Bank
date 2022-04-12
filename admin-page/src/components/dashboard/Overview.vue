<script setup>
import AppRepo from "../../api/AppRepo";

const response = await AppRepo.getOverview();
let data = $ref(response.data);

const formatBloodAmount = (value) => {
    if (value < 1000) return `${value} ML`;

    return `${value / 1000} L`;
};

const formatData = $computed(() => {
    let transformed = {};

    for (const key in data) {
        let obj = { ...data[key] };

        obj["text-class"] =
            obj["lastQuarter"] >= 0 ? "text-green-500" : "text-decreased";

        if (["bloodDonated", "bloodReceive"].includes(key)) {
            obj["total"] = formatBloodAmount(obj["total"]);

            obj["compared"] =
                obj["lastQuarter"] >= 0
                    ? `%${obj["lastQuarter"] * 100}+ `
                    : `%${Math.abs(obj["lastQuarter"]) * 100}- `;
        } else {
            obj["compared"] =
                obj["lastQuarter"] >= 0
                    ? `${obj["lastQuarter"]} new `
                    : `${Math.abs(obj["lastQuarter"])} less `;
        }

        transformed[key] = obj;
    }

    return transformed;
});
</script>

<template>
    <!-- Blood Receive -->
    <div class="col-12 lg:col-6 xl:col-3">
        <div class="card mb-0">
            <div class="flex justify-content-between mb-3">
                <div>
                    <span class="block text-500 font-medium mb-3">
                        Blood Receive
                    </span>
                    <div class="text-900 font-medium text-xl">
                        {{ formatData["bloodReceive"].total }}
                    </div>
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
            <span
                class="font-medium"
                :class="[formatData['bloodReceive']['text-class']]"
            >
                {{ formatData["bloodReceive"].compared }}
            </span>
            <span class="text-500">since last quater</span>
        </div>
    </div>

    <!-- Blood Giveaway -->
    <div class="col-12 lg:col-6 xl:col-3">
        <div class="card mb-0">
            <div class="flex justify-content-between mb-3">
                <div>
                    <span class="block text-400 font-medium mb-3">
                        Blood Giveaway
                    </span>
                    <div class="text-900 font-medium text-xl">
                        {{ formatData["bloodDonated"].total }}
                    </div>
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
            <span
                class="font-medium"
                :class="[formatData['bloodDonated']['text-class']]"
            >
                {{ formatData["bloodDonated"].compared }}
            </span>
            <span class="text-500">since last quater</span>
        </div>
    </div>

    <!-- Donators -->
    <div class="col-12 lg:col-6 xl:col-3">
        <div class="card mb-0">
            <div class="flex justify-content-between mb-3">
                <div>
                    <span class="block text-500 font-medium mb-3">
                        Donators
                    </span>
                    <div class="text-900 font-medium text-xl">
                        {{ formatData["donators"].total }}
                    </div>
                </div>
                <div
                    class="flex align-items-center justify-content-center bg-blue-100 border-round"
                    style="width: 2.5rem; height: 2.5rem"
                >
                    <i class="text-blue-500 text-xl fa-solid fa-users"></i>
                </div>
            </div>
            <span
                class="font-medium"
                :class="[formatData['donators']['text-class']]"
            >
                {{ formatData["donators"].compared }}
            </span>
            <span class="text-500">since last quater</span>
        </div>
    </div>

    <!-- Events -->
    <div class="col-12 lg:col-6 xl:col-3">
        <div class="card mb-0">
            <div class="flex justify-content-between mb-3">
                <div>
                    <span class="block text-500 font-medium mb-3">Events</span>
                    <div class="text-900 font-medium text-xl">
                        {{ formatData["events"].total }}
                    </div>
                </div>
                <div
                    class="flex align-items-center justify-content-center bg-purple-100 border-round"
                    style="width: 2.5rem; height: 2.5rem"
                >
                    <i class="pi pi-calendar text-purple-500 text-xl"></i>
                </div>
            </div>
            <span
                class="font-medium"
                :class="[formatData['events']['text-class']]"
                >{{ formatData["events"].compared }}</span
            >
            <span class="text-500">since last quater</span>
        </div>
    </div>
</template>

<style lang="scss" scoped>
.text-decreased {
    color: var(--primary-color) !important;
}
</style>
