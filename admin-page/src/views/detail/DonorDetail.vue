<script setup>
import { defineAsyncComponent, onBeforeMount } from "vue";
import Breadcrumb from "primevue/breadcrumb";

import DonorRepo from "../../api/DonorRepo";
import DonorTransactionRepo from "../../api/DonorTransaction";
import { formatDate } from "../../utils";

const props = defineProps({
    _id: String,
    filterData: {
        type: String,
        default: "",
    },
});

let donor = $ref(null);

onBeforeMount(async () => {
    const { data } = await DonorRepo.getById(props._id);
    donor = data;

    if (props.filterData) {
        await getListTransaction();
    }
});

let listDonation = $ref(null);
let fetchingDonations = $ref(false);
const AsyncTransactionTable = defineAsyncComponent({
    loader: () => import("../../components/tables/TransactionTable.vue"),
});
const getListTransaction = async () => {
    fetchingDonations = true;
    const { data } = await DonorTransactionRepo.getListTransactionByDonor(
        props._id
    );
    listDonation = data;
    fetchingDonations = false;
};

// Naviagtion settings
const home = $ref({
    icon: "fa-solid fa-user-group",
    to: { name: "Donors Management" },
});
let items = [{ label: "Donor Detail" }];
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <!-- Navigation -->
            <Breadcrumb
                :home="home"
                :model="items"
                style="margin-bottom: 1rem; border-radius: 15px"
            />

            <!-- Personal Information -->
            <div class="card" v-if="donor">
                <h3 class="app-highlight">{{ donor.name }}</h3>

                <!-- Details -->
                <div class="card__information">
                    <!-- Personal ID, Gender, Date of birth, Address -->
                    <div class="section">
                        <p>
                            <i class="fa-solid fa-id-card"></i>
                            {{ donor._id }}
                        </p>
                        <p style="text-transform: capitalize">
                            <i class="fa-solid fa-mars"></i>
                            {{ donor.gender }}
                        </p>
                        <p>
                            <i class="fa-solid fa-cake-candles"></i>
                            {{ formatDate(parseInt(donor.dob)) }}
                        </p>
                        <p>
                            <i class="fa-solid fa-location-pin"></i>
                            {{ donor.address }}
                        </p>
                    </div>

                    <!-- Phone, Email, Blood Type -->
                    <div class="section">
                        <p>
                            <i class="fa-solid fa-phone"></i>
                            {{ donor.phone }}
                        </p>
                        <p>
                            <i class="fa-solid fa-envelope"></i>
                            {{ donor.email }}
                        </p>
                        <p>
                            <i class="fa-solid fa-hand-holding-droplet"></i>
                            <span
                                :class="'blood-badge type-' + donor.blood.name"
                            >
                                {{ donor.blood.name }} {{ donor.blood.type }}
                            </span>
                        </p>
                    </div>
                </div>
            </div>

            <!-- List of transaction (Donate times) -->
            <div class="card" v-if="donor">
                <div
                    class="flex-center"
                    style="width: 100%"
                    v-if="!listDonation"
                >
                    <PrimeVueButton
                        label="Show Donor Donations"
                        @click="getListTransaction"
                        :loading="fetchingDonations"
                    />
                </div>

                <template v-else>
                    <AsyncTransactionTable
                        :transactionData="listDonation"
                        :filterData="filterData"
                    />
                </template>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";
.card {
    &__information {
        display: flex;

        .section {
            flex: 1 1 50%;
            padding-top: 1rem;

            p {
                i {
                    color: var(--primary-color);
                    font-size: 1.2rem;
                    padding-inline: 1rem;
                }
            }
        }
    }
}
</style>
