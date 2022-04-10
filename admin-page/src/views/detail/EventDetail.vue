<script setup>
import { defineAsyncComponent, onBeforeMount } from "vue";
import Breadcrumb from "primevue/breadcrumb";
import Divider from "primevue/divider";

import EventRepo from "../../api/EventRepo";
import EventHelper from "../../utils/helpers/Event";
import EventSubmissionRepo from "../../api/EventSubmissionRepo";
import { formatDate } from "../../utils";

const AsyncDonorTable = defineAsyncComponent({
    loader: () => import("../../components/tables/DonorTable.vue"),
});

const AsyncEventSubmissonTable = defineAsyncComponent({
    loader: () => import("../../components/tables/EventSubmissionTable.vue"),
});

const props = defineProps({
    _id: String,
});

let event = $ref();
let isUpcomingEvent = $ref(false);
let catchedData = $ref({});

let showDonorTable = $ref(false);
let donorsData = $ref(null);
const getParticipants = async () => {
    const { data } = !isUpcomingEvent
        ? await EventRepo.getParticipants(props._id)
        : await EventSubmissionRepo.getByEventId(props._id);
    donorsData = data ? data : [];
    showDonorTable = true;
};

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
    event["status"] = EventHelper.determineStatus(event);

    isUpcomingEvent = event["status"] === "upcoming";

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
            <template v-if="event">
                <div class="card flex">
                    <!-- Left content -->
                    <div class="card__content flex-center">
                        <img :src="event.binaryImage" alt="Event Image" />
                    </div>

                    <!-- Right content -->
                    <div class="card__content">
                        <h2 class="event-title">{{ event.name }}</h2>

                        <Divider>
                            <b
                                class="app-highlight"
                                style="padding-inline: 1rem"
                            >
                                Overview
                            </b>
                        </Divider>

                        <!-- Overview information -->
                        <ul class="event-overview">
                            <!-- Start date -->
                            <li>
                                <b>Start Date: </b>
                                <span class="info">
                                    {{ formatDate(event.startDate) }}
                                </span>
                            </li>

                            <!-- Duration -->
                            <li>
                                <b>Duration: </b>
                                <span class="info"
                                    >{{ event.duration }} days</span
                                >
                            </li>

                            <!-- Status -->
                            <li>
                                <b>Status: </b>
                                <span
                                    :class="`info event-badge event-${event.status}`"
                                >
                                    {{ event.status }}
                                </span>
                            </li>

                            <!-- Address -->
                            <li>
                                <b>Address: </b>
                                <span class="info">
                                    {{ event.location.address }},
                                    {{ event.location.city }}
                                </span>
                            </li>

                            <!-- Participants -->
                            <b>Participants: </b>
                            <span class="info">
                                {{ event.participants }}
                                <span v-if="event.status === 'upcoming'">
                                    participants *
                                </span>
                                <span v-else> donors</span>
                            </span>
                        </ul>

                        <Divider>
                            <b
                                class="app-highlight"
                                style="padding-inline: 1rem"
                            >
                                Event Description
                            </b>
                        </Divider>

                        <!-- Event detail -->
                        <p>{{ event.detail }}</p>

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
                            v-if="event.status !== 'passed'"
                        >
                            <i class="fa-solid fa-pen-to-square"></i>
                            Edit
                        </RouterLink>
                    </div>
                </div>
            </template>

            <!-- Event participants -->
            <div class="card">
                <div
                    class="flex-center"
                    style="width: 100%"
                    v-if="!showDonorTable"
                >
                    <PrimeVueButton
                        :label="
                            !isUpcomingEvent
                                ? 'Show Event Participants'
                                : 'Show Event Registers'
                        "
                        @click="getParticipants"
                    />
                </div>

                <template v-else>
                    <h2>
                        {{
                            !isUpcomingEvent
                                ? "Event Participants"
                                : "Event Registers"
                        }}
                    </h2>
                    <Component
                        :is="
                            !isUpcomingEvent
                                ? AsyncDonorTable
                                : AsyncEventSubmissonTable
                        "
                        :donorsData="donorsData"
                    />
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
