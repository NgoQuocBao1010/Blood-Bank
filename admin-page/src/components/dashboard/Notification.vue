<script setup>
import AppRepo from "../../api/AppRepo";

try {
    let data = $ref(await AppRepo.getEventNotifications());
    console.log(data);
} catch (err) {
    console.log(err.response);
}
</script>

<template>
    <div class="card">
        <div class="flex align-items-center justify-content-between mb-4">
            <h5>Notifications</h5>
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
                    <i class="pi pi-question text-xl text-orange-500"></i>
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
                    <i class="fa-solid fa-gift text-xl text-blue-500"></i>
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
                    <span class="text-700 font-medium"> has started. </span>
                </span>
            </li>
        </ul>
    </div>
</template>

<style lang="scss" scoped></style>
