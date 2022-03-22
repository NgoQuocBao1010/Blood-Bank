<script setup>
import { onBeforeMount } from "vue";
import dayjs from "dayjs";
import Breadcrumb from "primevue/breadcrumb";
import Divider from "primevue/divider";

import { formatDate } from "../../utils";

// *** Mock data ***
const data = {
    _id: "f822bdb0-6b7e-4681-8c62-93520c3accfc",
    name: "Health and Wellbeing at work",
    location: {
        city: "Can Tho",
        address: "Can Tho University",
    },
    startDate: new Date("02/11/2021").getTime().toString(),
    duration: 3,
    detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
};
// *** END of mock data **
const props = defineProps({
    _id: String,
});

const event = $ref(null);

// Naviagtion settings
const home = $ref({
    icon: "fa-solid fa-calendar-days",
    to: { name: "Events Management" },
});
const items = $ref(null);

onBeforeMount(() => {
    event = { ...data };
    event["startDate"] = new Date();

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

            <!-- Main Content -->
            <div class="card">
                <!-- Left content -->
                <div class="card__content flex-center">
                    <img
                        src="../../assets/images/event.png"
                        alt="Event Image"
                    />
                </div>

                <!-- Right content -->
                <div class="card__content">
                    <h2 class="event-title">{{ event.name }}</h2>

                    <!-- Overview information -->
                    <Divider>
                        <b class="app-highlight">Overview</b>
                    </Divider>
                    <ul class="event-overview">
                        <!-- Start date -->
                        <li>
                            <b>Start Date: </b>
                            <span>{{ formatDate(event.startDate) }}</span>
                        </li>
                        <!-- Duration -->
                        <li>
                            <b>Duration: </b>
                            <span>{{ event.duration }} days</span>
                        </li>
                        <!-- Status -->
                        <li>
                            <b>Status: </b>
                            <span :class="`event-badge event-${event.status}`">
                                {{ event.status }}
                            </span>
                        </li>
                        <!-- Address -->
                        <li>
                            <b>Address: </b>
                            <span>
                                {{ event.location.address }},
                                {{ event.location.city }}
                            </span>
                        </li>
                    </ul>

                    <!-- Event detail -->
                    <Divider>
                        <b class="app-highlight">Event Description</b>
                    </Divider>
                    <p>{{ event.detail }}</p>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";

.flex-center {
    display: flex;
    align-items: center;
    justify-content: center;
}
.card {
    display: flex;

    &__content {
        flex: 1 1 50%;
        padding: 1rem;

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
            }
        }
    }
}
</style>
