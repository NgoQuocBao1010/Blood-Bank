<script setup>
import InputSwitch from "primevue/inputswitch";
import ProgressBar from "primevue/progressbar";
import { onBeforeMount, watch } from "vue";

import DonorRepo from "../../api/DonorRepo";
import DonorTable from "../../components/tables/DonorTable.vue";

let donorsData = $ref(null);
let fetchingDonors = $ref(false);

let isRejectParticipants = $ref(false);
watch(
    () => isRejectParticipants,
    async () => {
        await updateParticipants(false);
    }
);

const updateParticipants = async (reset = true) => {
    if (reset) isRejectParticipants = false;

    fetchingDonors = true;
    try {
        donorsData = null;
        const { data } = isRejectParticipants
            ? await DonorRepo.getReject()
            : await DonorRepo.getAll();
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
                <div class="flex header">
                    <h2>Donation Requests Monitor</h2>

                    <div class="switch">
                        <p>Pending Donors</p>
                        <InputSwitch v-model="isRejectParticipants" />
                        <p>Rejected Donors</p>
                    </div>
                </div>

                <!-- Donors table -->
                <DonorTable
                    v-if="donorsData"
                    :donorsData="donorsData"
                    :participants="true"
                    :isReject="isRejectParticipants"
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

<style lang="scss" scoped>
.header {
    align-items: center;
    justify-content: space-between;

    .switch {
        display: flex;
        justify-content: center;
        gap: 1em;
    }
}
</style>
