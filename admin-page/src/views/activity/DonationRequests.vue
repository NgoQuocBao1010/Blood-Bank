<script setup>
import { onBeforeMount } from "vue";

import DonorRepo from "../../api/DonorRepo";
import DonorTable from "../../components/tables/DonorTable.vue";

let donorsData = $ref(null);

onBeforeMount(async () => {
    const { data } = await DonorRepo.getAll();
    donorsData = data;
});
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <!-- Page headers -->
                <h2>Donation Requests Monitor</h2>

                <!-- Donors table -->
                <DonorTable
                    v-if="donorsData"
                    :donorsData="donorsData"
                    :participants="true"
                />
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped></style>
