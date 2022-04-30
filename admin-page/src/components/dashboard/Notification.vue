<script setup>
import { useEventStore } from "../../stores/event";

import AppRepo from "../../api/AppRepo";

const ICONS = [
    {
        name: "Nothing",
        fontAwesomeClass: "fa-solid fa-circle-check text-green-500",
        bgColor: "green",
    },
    {
        name: "Submission",
        fontAwesomeClass: "fa-solid fa-hand-holding-droplet text-orange-500",
        bgColor: "orange",
    },
    {
        name: "Started",
        fontAwesomeClass: "fa-solid fa-circle-play",
        bgColor: "blue",
    },
    {
        name: "Ended",
        fontAwesomeClass: "fa-solid fa-flag-checkered text-purple-500",
        bgColor: "purple",
    },
];

const getMessage = (data) => {
    const type = data.status ? data.status : "Submission";

    return {
        Submission: {
            trigger: data.name,
            action: "has registered for event",
            highlight: eventStore.getEventById(data.id).name,
            color: "orange",
        },
        Started: {
            trigger: data.name,
            action: "",
            highlight: "has begin.",
            color: "blue",
        },
        Ended: {
            trigger: data.name,
            action: "",
            highlight: "has ended.",
            color: "purple",
        },
    }[type];
};

// Retrieve event data
const eventStore = useEventStore();
if (!eventStore.events) await eventStore.setEvents();

const response = await AppRepo.getEventNotifications();
let data = $ref(response.data);

let notifications = {};
for (let timeline in data) {
    // No event
    if (data[timeline].length === 0) {
        notifications[timeline] = [
            {
                icon: ICONS.find((i) => i.name === "Nothing"),
                message: {
                    trigger: "",
                    action: "",
                    highlight: `There is no notification for ${timeline}`,
                    color: "green",
                },
            },
        ];

        continue;
    }

    // Event format
    const formatData = [];
    data[timeline].forEach((event) => {
        formatData.push({
            icon: ICONS.find(
                (i) => i.name === (event.status ? event.status : "Submission")
            ),
            message: {
                ...getMessage(event),
                linkTo: event.id,
            },
        });
    });
    notifications[timeline] = formatData;
}
</script>

<template>
    <div class="card">
        <div class="flex align-items-center justify-content-between mb-4">
            <h5>Event Notifications</h5>
        </div>

        <!-- Notification Pane -->
        <template v-for="timeline in ['today', 'yesterday']">
            <span
                class="block text-600 font-medium mb-3"
                style="text-transform: uppercase"
            >
                {{ timeline }}
            </span>

            <!-- Notifications -->
            <ul class="p-0 mx-0 mt-0 mb-4 list-none">
                <li
                    class="flex align-items-center py-2"
                    :class="{
                        'border-bottom-1 surface-border':
                            index !== notifications[timeline].length - 1,
                    }"
                    v-for="(notification, index) in notifications[timeline]"
                >
                    <div
                        class="w-3rem h-3rem flex align-items-center justify-content-center border-circle mr-3 flex-shrink-0"
                        :class="[`bg-${notification.icon.bgColor}-100`]"
                    >
                        <i
                            class="text-xl text-blue-500"
                            :class="[notification.icon.fontAwesomeClass]"
                        ></i>
                    </div>

                    <!-- Link -->
                    <router-link
                        v-if="notification.message.linkTo"
                        :to="{
                            name: 'Event Detail',
                            params: {
                                _id: notification.message.linkTo,
                                showRegister: true,
                            },
                        }"
                        class="text-900"
                        style="line-height: 1.7"
                    >
                        {{ notification.message.trigger }}
                        <span class="text-700">
                            {{ notification.message.action }}
                            <span
                                class="font-medium"
                                :class="[
                                    `text-${notification.message.color}-500`,
                                ]"
                            >
                                {{ notification.message.highlight }}
                            </span>
                        </span>
                    </router-link>

                    <!-- No event -->
                    <span v-else class="text-900" style="line-height: 1.7">
                        {{ notification.message.trigger }}
                        <span class="text-700">
                            {{ notification.message.action }}
                            <span
                                class="font-medium"
                                :class="[
                                    `text-${notification.message.color}-500`,
                                ]"
                            >
                                {{ notification.message.highlight }}
                            </span>
                        </span>
                    </span>
                </li>
            </ul>
        </template>
    </div>
</template>

<style lang="scss" scoped></style>
