<script setup>
import { defineAsyncComponent, onBeforeMount } from "vue";
import Breadcrumb from "primevue/breadcrumb";

import DonorRepo from "../../api/DonorRepo";
import DonorTransactionRepo from "../../api/DonorTransaction";
import { formatDate } from "../../utils";

// *** Mock data ***
const transactionData = [
    {
        _id: "911fdf65-2913-4705-8df1-7cba4a0a9355",
        _event: {
            _id: "1440f35b-0db5-484b-9370-872cb3c7f519",
            name: "event",
        },
        amount: 500,
        dateDonated: new Date("2021-09-13").getTime().toString(),
    },
    {
        _id: "6f769818-5060-435e-835b-ab7ab3cfdaec",
        _event: {
            _id: "de169f18-226d-48e0-9579-b18184e2c260",
            name: "event1",
        },
        amount: 420,
        dateDonated: new Date("2021-09-13").getTime(),
    },
    {
        _id: "6f05348f-5e55-408f-9c70-dddc105e8c7a",
        _event: {
            _id: "7cae7784-7523-47ae-b1a4-42308f8fb348",
            name: "event",
        },
        amount: 421,
        dateDonated: new Date("2021-09-13").getTime(),
    },
];
// *** END of mock data **
const props = defineProps({
    _id: String,
});

let donor = $ref(null);

onBeforeMount(async () => {
    const { data } = await DonorRepo.getById(props._id);
    donor = data;
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
                    <AsyncTransactionTable :transactionData="listDonation" />
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
