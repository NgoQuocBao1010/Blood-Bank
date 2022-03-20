<script setup>
import { onMounted } from "vue";
import dayjs from "dayjs";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";
import InputText from "primevue/inputtext";

import { formatDate } from "../../utils";

const eventsData = [
    {
        id: "f822bdb0-6b7e-4681-8c62-93520c3accfc",
        name: "Health and Wellbeing at work",
        location: {
            city: "Can Tho",
            address: "Can Tho University",
        },
        startDate: new Date("02/11/2021"),
        duration: 3,
        detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
        participants: 50,
    },
    {
        id: "c75f46e3-8726-4a39-bbea-1d11d411ce72",
        name: "Tell me, do you bleed?",
        location: {
            city: "Ho Chi Minh",
            address: "Somewhere",
        },
        startDate: new Date("12/10/2021"),
        duration: 4,
        detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
        participants: 51,
    },
    {
        id: "54a60992-ec21-42c0-807d-7ad4e5b698d5",
        name: "We are donors",
        location: {
            city: "Ho Chi Minh",
            address: "Somewhere",
        },
        startDate: new Date("04/07/2021"),
        duration: 4,
        detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
        participants: 52,
    },
    {
        id: "b641b947-9846-46b3-a3a1-fef365d93bf6",
        name: "Judoh Blood Donations",
        location: {
            city: "Ho Chi Minh",
            address: "Somewhere",
        },
        startDate: new Date("03/01/2022"),
        duration: 50,
        detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
        participants: 53,
    },
    {
        id: "b2c13297-ce8e-4fa2-8a0c-cc7e761cf065",
        name: "Judoh Blood Donations - Summer Edition",
        location: {
            city: "Da Nang",
            address: "Somewhere",
        },
        startDate: new Date("02/06/2022"),
        duration: 3,
        detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
    },
    {
        id: "b2c13297-ce8e-4fa2-8a0c-cc7e761cf066",
        name: "Judoh Blood Donations - Chrismas Edition",
        location: {
            city: "Da Nang",
            address: "Somewhere",
        },
        startDate: new Date("12/20/2022"),
        duration: 3,
        detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
    },
];

const events = $ref(null);
onMounted(() => {
    events = eventsData.map((row) => {
        const event = { ...row };

        if (!event["participants"]) event["participants"] = 0;

        // Check status of the event
        const endDate = dayjs(event.startDate).add(event.duration, "day");
        if (dayjs().isAfter(endDate, "day")) {
            event["status"] = "passed";
        } else if (dayjs().isBefore(event.startDate, "day")) {
            event["status"] = "upcoming";
        } else {
            event["status"] = "ongoing";
        }

        return event;
    });
});
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <!-- Page header -->
                <h2>Events Management</h2>

                <!-- Events table -->
                <DataTable
                    :value="events"
                    data-key="id"
                    class="p-datatable-gridlines"
                    :rows="5"
                    :paginator="true"
                    :row-hover="true"
                    responsive-layout="scroll"
                    removable-sort
                >
                    <!-- Header of the table -->
                    <template #header>
                        <div
                            class="flex justify-content-between flex-column sm:flex-row"
                        >
                            <div>
                                <PrimeVueButton
                                    type="button"
                                    icon="pi pi-filter-slash"
                                    label="Clear"
                                    class="p-button-outlined mb-2 mr-2"
                                />
                            </div>

                            <!-- Search Input -->
                            <span class="p-input-icon-left mb-2">
                                <i class="pi pi-search" />
                                <InputText
                                    placeholder="Keyword Search"
                                    style="width: 100%"
                                />
                            </span>
                        </div>
                    </template>

                    <!-- Empty data fallback -->
                    <template #empty> No events found. </template>

                    <!-- Columns -->
                    <!-- Events's name -->
                    <PrimeVueColumn
                        field="name"
                        header="Name"
                        style="min-width: 12rem"
                    >
                        <template #body="{ data }">
                            {{ data.name }}
                        </template>
                    </PrimeVueColumn>

                    <!-- Events Status -->
                    <PrimeVueColumn field="status" header="Status">
                        <template #body="{ data }">
                            <span :class="'event-badge event-' + data.status">
                                {{ data.status }}
                            </span>
                        </template>
                    </PrimeVueColumn>

                    <!-- Event Start Date -->
                    <PrimeVueColumn
                        header="Start date"
                        field="startDate"
                        dataType="date"
                        :sortable="true"
                        style="min-width: 12rem"
                    >
                        <template #body="{ data }">
                            {{ formatDate(data.startDate) }}
                        </template>
                    </PrimeVueColumn>

                    <!-- Event Duration -->
                    <PrimeVueColumn
                        header="Duration"
                        field="duration"
                        dataType="number"
                        :sortable="true"
                    >
                        <template #body="{ data }">
                            {{ data.duration }} days
                        </template>
                    </PrimeVueColumn>

                    <!-- Events Participants -->
                    <PrimeVueColumn
                        header="Participants"
                        field="participants"
                        dataType="number"
                        :sortable="true"
                    >
                        <template #body="{ data }">
                            {{
                                data.participants
                                    ? `${data.participants} participants`
                                    : "No participants"
                            }}
                        </template>
                    </PrimeVueColumn>
                </DataTable>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";
</style>
