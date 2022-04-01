<script setup>
import ProgressBar from "primevue/progressbar";
import { onBeforeMount } from "vue";

import DonorRepo from "../../api/DonorRepo";
import DonorTable from "../../components/tables/DonorTable.vue";

let donorsData = $ref(null);
let fetchingDonors = $ref(false);

const updateParticipants = async () => {
    fetchingDonors = true;
    try {
        donorsData = null;
        const { data } = await DonorRepo.getAll();
        donorsData = data;
    } catch (e) {
        throw e;
    } finally {
        fetchingDonors = false;
    }
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

                <!-- Progress bar -->
                <div
                    class="flex flex-center"
                    style="flex-direction: column; margin-block: 3rem"
                    v-else
                >
                    <h5>Loading ...</h5>
                    <ProgressBar
                        mode="indeterminate"
                        style="height: 0.5em; width: 100%"
                    />
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped></style>
