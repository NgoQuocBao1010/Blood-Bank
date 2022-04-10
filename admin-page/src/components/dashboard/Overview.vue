<script setup>
const apiCall = () => {
    return new Promise((resolve, reject) => {
        setTimeout(() => resolve({ name: "Quoc Bao" }), 2000);
    });
};

let data = $ref(await apiCall());
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
                    <span class="block text-500 font-medium mb-3">Events</span>
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
</template>

<style lang="scss" scoped>
.text-decreased {
    color: var(--primary-color) !important;
}
</style>
