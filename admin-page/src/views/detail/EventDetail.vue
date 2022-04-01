<script setup>
import { defineAsyncComponent, onBeforeMount } from "vue";
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
        blood: {
            name: "O",
            type: "Negative",
        },
        transaction: {
            _id: "911fdf65-2913-4705-8df1-7cba4a0a9355",
            eventDonated: {
                _id: "1440f35b-0db5-484b-9370-872cb3c7f519",
                name: "event",
            },
            blood: {
                name: "O",
                type: "Negative",
            },
            amount: 500,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000001",
        name: "Quoc Bao",
        blood: {
            name: "A",
            type: "Positive",
        },
        transaction: {
            _id: "6f769818-5060-435e-835b-ab7ab3cfdaec",
            eventDonated: {
                _id: "de169f18-226d-48e0-9579-b18184e2c260",
                name: "event1",
            },
            blood: {
                name: "A",
                type: "Positive",
            },
            amount: 420,
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
