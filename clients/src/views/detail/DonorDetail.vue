<script setup>
import { defineAsyncComponent } from "vue";
import Breadcrumb from "primevue/breadcrumb";

import { formatDate } from "../../utils";

// *** Mock data ***
const data = {
    _id: "080011114356",
    name: "Quoc Bao 1",
    phone: "0945127866",
    address: "1st street, Can Tho city, VietNam",
    gender: "male",
    email: "baobao@gmail.com",
    dob: new Date("05/11/2000").getTime().toString(),
    blood: {
        name: "A",
        type: "Negative",
    },
};
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

const AsyncTransactionTable = defineAsyncComponent({
    loader: () => import("../../components/tables/TransactionTable.vue"),
});

const props = defineProps({
    _id: String,
});

let showTransactions = $ref(false);

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
            <div class="card">
                <h3 class="app-highlight">{{ data.name }}</h3>

                <!-- Details -->
                <div class="card__information">
                    <!-- Personal ID, Gender, Date of birth, Address -->
                    <div class="section">
                        <p>
                            <i class="fa-solid fa-id-card"></i>
                            {{ data._id }}
                        </p>
                        <p style="text-transform: capitalize">
                            <i class="fa-solid fa-mars"></i>
                            {{ data.gender }}
                        </p>
                        <p>
                            <i class="fa-solid fa-cake-candles"></i>
                            {{ formatDate(parseInt(data.dob)) }}
                        </p>
                        <p>
                            <i class="fa-solid fa-location-pin"></i>
                            {{ data.address }}
                        </p>
                    </div>

                    <!-- Phone, Email, Blood Type -->
                    <div class="section">
                        <p>
                            <i class="fa-solid fa-phone"></i>
                            {{ data.phone }}
                        </p>
                        <p>
                            <i class="fa-solid fa-envelope"></i>
                            {{ data.email }}
                        </p>
                        <p>
                            <i class="fa-solid fa-hand-holding-droplet"></i>
                            <span
                                :class="'blood-badge type-' + data.blood.name"
                            >
                                {{ data.blood.name }} {{ data.blood.type }}
                            </span>
                        </p>
                    </div>
                </div>
            </div>

            <!-- List of transaction (Donate times) -->
            <div class="card">
                <div
                    class="flex-center"
                    style="width: 100%"
                    v-if="!showTransactions"
                >
                    <PrimeVueButton
                        label="Show Donor Donations"
                        @click="showTransactions = !showTransactions"
                    />
                </div>

                <template v-else>
                    <h3>Donations Table</h3>
                    <AsyncTransactionTable :transactionData="transactionData" />
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
