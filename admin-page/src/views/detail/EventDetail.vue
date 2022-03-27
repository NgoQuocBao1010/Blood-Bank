<script setup>
import { defineAsyncComponent, onBeforeMount } from "vue";
import { RouterLink } from "vue-router";
import dayjs from "dayjs";
import Breadcrumb from "primevue/breadcrumb";
import Divider from "primevue/divider";

import EventRepo from "../../api/EventRepo";
import { formatDate } from "../../utils";

const AsyncDonorTable = defineAsyncComponent({
    loader: () => import("../../components/tables/DonorTable.vue"),
});

// *** Mock data ***
const donorsData = [
    {
        _id: "800000100001",
        name: "Quoc Bao 1",
        transaction: {
            _id: "911fdf65-2913-4705-8df1-7cba4a0a9355",
            blood: {
                name: "O",
                type: "Negative",
            },
            _event: {
                _id: "1440f35b-0db5-484b-9370-872cb3c7f519",
                name: "event",
            },
            amount: 500,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000001",
        name: "Quoc Bao",
        transaction: {
            _id: "6f769818-5060-435e-835b-ab7ab3cfdaec",
            blood: {
                name: "A",
                type: "Positive",
            },
            _event: {
                _id: "de169f18-226d-48e0-9579-b18184e2c260",
                name: "event1",
            },
            amount: 420,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000002",
        name: "Quoc Bao",
        transaction: {
            _id: "6f05348f-5e55-408f-9c70-dddc105e8c7a",
            blood: {
                name: "A",
                type: "Negative",
            },
            _event: {
                _id: "7cae7784-7523-47ae-b1a4-42308f8fb348",
                name: "event",
            },
            amount: 421,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000003",
        name: "Quoc Bao",
        transaction: {
            _id: "f1a2b6d8-cd3b-41ce-b313-d2388e6ed898",
            blood: {
                name: "A",
                type: "Positive",
            },
            _event: {
                _id: "c31675bc-09c6-40d8-8a1f-6a0bc86e5def",
                name: "event1",
            },
            amount: 422,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000004",
        name: "Quoc Bao",
        transaction: {
            _id: "1bbd4313-efd5-4624-b569-f0b55b656171",
            blood: {
                name: "AB",
                type: "Positive",
            },
            _event: {
                _id: "6957e9d6-2309-45ed-879e-684c87d9fe43",
                name: "event2",
            },
            amount: 423,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000005",
        name: "Quoc Bao",
        transaction: {
            _id: "80ee995b-d0ac-471c-97ab-9e1223264051",
            blood: {
                name: "B",
                type: "Positive",
            },
            _event: {
                _id: "30ea460a-62c4-4fc0-8a7b-450f63c65af3",
                name: "event2",
            },
            amount: 424,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000006",
        name: "Quoc Bao",
        transaction: {
            _id: "97733c03-99cf-4a7b-a76b-114380d5ee0f",
            blood: {
                name: "AB",
                type: "Negative",
            },
            _event: {
                _id: "371ceacf-0470-4874-ac18-d84330baa688",
                name: "event",
            },
            amount: 425,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000007",
        name: "Quoc Bao",
        transaction: {
            _id: "da3c90a4-ac58-4200-976b-19cd6e61655a",
            blood: {
                name: "B",
                type: "Negative",
            },
            _event: {
                _id: "dbb32e45-6ad3-443b-892d-769362b551f5",
                name: "event2",
            },
            amount: 426,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
];
// *** END of mock data **
const props = defineProps({
    _id: String,
});

let event = $ref();
let catchedData = $ref({});
let showDonorTable = $ref(false);

// Naviagtion settings
const home = $ref({
    icon: "fa-solid fa-calendar-days",
    to: { name: "Events Management" },
});
let items = $ref(null);

onBeforeMount(async () => {
    const { data } = await EventRepo.getById(props._id);

    catchedData = JSON.stringify(data);
    event = { ...data };
    event["startDate"] = new Date(parseInt(event["startDate"]));

    const endDate = dayjs(event.startDate).add(event.duration, "day");
    if (dayjs().isAfter(endDate, "day")) {
        event["status"] = "passed";
    } else if (dayjs().isBefore(event.startDate, "day")) {
        event["status"] = "upcoming";
    } else {
        event["status"] = "ongoing";
    }

    items = [{ label: event ? `${event.name} event` : "Unknown event" }];
});
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

            <!-- Event Content -->
            <div class="card flex">
                <!-- Left content -->
                <div class="card__content flex-center">
                    <img
                        src="../../assets/images/event.png"
                        alt="Event Image"
                    />
                </div>

                <!-- Right content -->
                <div class="card__content">
                    <h2 class="event-title">{{ event?.name }}</h2>

                    <!-- Overview information -->
                    <Divider>
                        <b class="app-highlight" style="padding-inline: 1rem">
                            Overview
                        </b>
                    </Divider>
                    <ul class="event-overview">
                        <!-- Start date -->
                        <li>
                            <b>Start Date: </b>
                            <span class="info">
                                {{ formatDate(event?.startDate) }}
                            </span>
                        </li>
                        <!-- Duration -->
                        <li>
                            <b>Duration: </b>
                            <span class="info">{{ event?.duration }} days</span>
                        </li>
                        <!-- Status -->
                        <li>
                            <b>Status: </b>
                            <span
                                :class="`info event-badge event-${event?.status}`"
                            >
                                {{ event?.status }}
                            </span>
                        </li>
                        <!-- Address -->
                        <li>
                            <b>Address: </b>
                            <span class="info">
                                {{ event?.location.address }},
                                {{ event?.location.city }}
                            </span>
                        </li>
                    </ul>

                    <!-- Event detail -->
                    <Divider>
                        <b class="app-highlight" style="padding-inline: 1rem">
                            Event Description
                        </b>
                    </Divider>
                    <p>{{ event?.detail }}</p>

                    <!-- Edit Button -->
                    <RouterLink
                        :to="{
                            name: 'Event Edit',
                            params: {
                                _id,
                                eventData: catchedData,
                            },
                        }"
                        v-ripple
                        class="p-button p-button-sm p-component mb-2 p-ripple app-router-link-icon edit-btn"
                        v-if="event?.status !== 'passed'"
                    >
                        <i class="fa-solid fa-pen-to-square"></i>
                        Edit
                    </RouterLink>
                </div>
            </div>

            <!-- Event participants -->
            <div class="card">
                <div
                    class="flex-center"
                    style="width: 100%"
                    v-if="!showDonorTable"
                >
                    <PrimeVueButton
                        label="Show Event Participants"
                        @click="showDonorTable = !showDonorTable"
                    />
                </div>

                <template v-else>
                    <h2>Events Participants</h2>
                    <AsyncDonorTable :donorsData="donorsData" />
                </template>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";
.card {
    &__content {
        flex: 1 1 50%;
        padding: 1rem;
        position: relative;

        img {
            width: 25rem;
            border-radius: 20px;
        }

        .event-title {
            color: var(--primary-color);
            font-weight: 900;
        }

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

        .edit-btn {
            position: absolute;
            top: -1rem;
            right: -1rem;
        }
    }
}
</style>
