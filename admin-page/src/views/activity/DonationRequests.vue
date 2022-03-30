<script setup>
import { onBeforeMount } from "vue";

import DonorRepo from "../../api/DonorRepo";
import DonorTable from "../../components/tables/DonorTable.vue";

let donorsData = $ref(null);

const updateParticipants = async () => {
    donorsData = null;
    const { data } = await DonorRepo.getAll();
    donorsData = data;
};

onBeforeMount(async () => {
    await updateParticipants();
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
                    @updateParticipants="updateParticipants"
                />
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped></style>
