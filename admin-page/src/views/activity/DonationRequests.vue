<script setup>
import { onBeforeMount, watch } from "vue";

import DonorRepo from "../../api/DonorRepo";
import DonorTable from "../../components/tables/DonorTable.vue";
import AppProgressBar from "../../components/AppProgressBar.vue";

let donorsData = $ref(null);
const fetchData = (type) => {
    return {
        pending: () => DonorRepo.getAllDonations(),
        rejected: () => DonorRepo.getRejectDonations(),
        approved: () => DonorRepo.getSuccessDonations(),
    }[type];
};

const tabs = ["approved", "rejected", "pending"];
let donorType = $ref("pending");
watch(
    () => donorType,
    async () => {
        await fetchParticipants();
    }
);

const fetchParticipants = async () => {
    try {
        donorsData = null;
        const { data } = await fetchData(donorType)();
        donorsData = data;
    } catch (e) {
        throw e;
    }
};

const updateParticipants = async () => {
    donorType = "pending";
    await fetchParticipants();
};

onBeforeMount(async () => {
    await fetchParticipants();
});
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <!-- Page headers -->
                <div class="flex header">
                    <h2>Donation Requests Monitor</h2>
                </div>

                <!-- Donor Type Selections -->
                <ul class="tab-wrapper">
                    <li
                        v-for="tab in tabs"
                        v-ripple
                        class="p-ripple tab"
                        :class="{ active: tab === donorType }"
                        @click="donorType = tab"
                    >
                        <p class="content">{{ tab }}</p>
                    </li>
                </ul>

                <!-- Donor Table -->
                <div id="donor-table" v-if="donorsData">
                    <DonorTable
                        :donorsData="donorsData"
                        :participants="true"
                        :isRejectParticipant="donorType === 'rejected'"
                        :isApproveParticipant="donorType === 'approved'"
                        @updateParticipants="updateParticipants"
                    />
                </div>

                <!-- Progress bar -->
                <AppProgressBar v-else />
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
.tab-wrapper {
    padding-top: 1rem;
    margin: 0;
    border-top: 1px solid rgb(236, 236, 236);
    display: flex;
    justify-content: flex-end;
    gap: 0.2rem;
    .tab {
        padding: 0.75rem 2rem;
        cursor: pointer;
        border-top-left-radius: 30px;
        border-top-right-radius: 30px;
        text-transform: capitalize;
        font-weight: 700;
        transition: all 0.3s ease;
        color: lightgray;
        position: relative;

        &:hover {
            background-color: #f8f9fa;
        }

        &.active {
            background-color: #f8f9fa;
            color: var(--primary-color);
        }

        .content {
            margin: 0;
            padding: 0;
        }
    }
}
</style>
