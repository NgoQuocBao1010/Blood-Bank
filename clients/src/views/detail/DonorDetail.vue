<script setup>
import { onBeforeMount } from "vue";
import Breadcrumb from "primevue/breadcrumb";
import { formatDate } from "../../utils";

const data = {
    _id: "3dbf5ab8-4a39-43d9-8fda-2e1b4de09ca2",
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

const props = defineProps({
    _id: String,
});

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

            <div class="card">
                <h3 class="app-highlight">{{ data.name }}</h3>

                <!-- Details -->
                <ul class="event-overview">
                    <!-- Duration -->
                    <li>
                        <b>Phone: </b>
                        <span class="info">{{ data.phone }}</span>
                    </li>
                    <!-- Email -->
                    <li>
                        <b>Email: </b>
                        <span class="info">{{ data.email }}</span>
                    </li>
                    <!-- Gender -->
                    <li>
                        <b>Gender: </b>
                        <span class="info">{{ data.gender }}</span>
                    </li>
                    <!-- Address -->
                    <li>
                        <b>Address: </b>
                        <span class="info">{{ data.address }}</span>
                    </li>
                    <!-- Date of birth -->
                    <li>
                        <b>Date of birth: </b>
                        <span class="info">
                            {{ formatDate(data.dob) }}
                        </span>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
.event-overview {
    list-style: none;
    padding: 0;
    margin: 0;

    li {
        line-height: 2;

        span.info {
            margin-left: 0.5rem;
        }
    }
}
</style>
