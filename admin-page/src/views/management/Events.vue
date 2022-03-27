<script setup>
import { onBeforeMount } from "vue";
import { useRouter, RouterLink } from "vue-router";
import dayjs from "dayjs";
import InputText from "primevue/inputtext";
import MultiSelect from "primevue/multiselect";
import Calendar from "primevue/calendar";
import { FilterMatchMode } from "primevue/api";

import { formatDate } from "../../utils";

const router = useRouter();

const eventsData = [
    {
        _id: "f822bdb0-6b7e-4681-8c62-93520c3accfc",
        name: "Health and Wellbeing at work",
        location: {
            city: "Can Tho",
            address: "Can Tho University",
        },
        startDate: new Date("02/11/2021").getTime().toString(),
        duration: 3,
        detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
    },
    {
        _id: "c75f46e3-8726-4a39-bbea-1d11d411ce72",
        name: "Tell me, do you bleed?",
        location: {
            city: "Ho Chi Minh",
            address: "Somewhere",
        },
        startDate: new Date("12/10/2021").getTime().toString(),
        duration: 4,
        detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
    },
    {
        _id: "54a60992-ec21-42c0-807d-7ad4e5b698d5",
        name: "We are donors",
        location: {
            city: "Ho Chi Minh",
            address: "Somewhere",
        },
        startDate: new Date("04/07/2021").getTime().toString(),
        duration: 4,
        detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
    },
    {
        _id: "b641b947-9846-46b3-a3a1-fef365d93bf6",
        name: "Judoh Blood Donations",
        location: {
            city: "Ho Chi Minh",
            address: "Somewhere",
        },
        startDate: new Date("03/01/2022").getTime().toString(),
        duration: 50,
        detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
    },
    {
        _id: "b2c13297-ce8e-4fa2-8a0c-cc7e761cf065",
        name: "Judoh Blood Donations - Summer Edition",
        location: {
            city: "Da Nang",
            address: "Somewhere",
        },
        startDate: new Date("02/06/2022").getTime().toString(),
        duration: 3,
        detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
    },
    {
        _id: "b2c13297-ce8e-4fa2-8a0c-cc7e761cf066",
        name: "Judoh Blood Donations - Chrismas Edition",
        location: {
            city: "Da Nang",
            address: "Somewhere",
        },
        startDate: new Date("12/20/2022").getTime().toString(),
        duration: 3,
        detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
    },
];

let events = $ref(null);
const EVENT_STATUS = ["passed", "ongoing", "upcoming"];
let cities = $ref([]);

// Filter configurations
let filters = $ref(null);
const initFilters = () => {
    filters = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
        name: { value: null, matchMode: FilterMatchMode.CONTAINS },
        status: { value: null, matchMode: FilterMatchMode.IN },
        startDate: { value: null, matchMode: FilterMatchMode.DATE_IS },
        "location.city": { value: null, matchMode: FilterMatchMode.IN },
    };
};
const clearFilter = () => {
    initFilters();
};

onBeforeMount(() => {
    events = eventsData.map((row) => {
        const event = { ...row };

        event["startDate"] = new Date(parseInt(event["startDate"]));

        // Check status of the event
        const endDate = dayjs(event.startDate).add(event.duration, "day");
        if (dayjs().isAfter(endDate, "day")) {
            event["status"] = "passed";
        } else if (dayjs().isBefore(event.startDate, "day")) {
            event["status"] = "upcoming";
        } else {
            event["status"] = "ongoing";
        }

        cities.push(event.location.city);

        return event;
    });

    cities = [...new Set(cities)];

    initFilters();
});

const onRowClick = (payload) => {
    // Go to event detail when click a row in the table
    const { _id: eventId } = payload.data;
    router.push({ name: "Event Detail", params: { _id: eventId } });
};
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <!-- Page header -->
                <div
                    class="flex justify-content-between align-content-center"
                    style="width: 100%"
                >
                    <h2>Events Management</h2>
                    <p class="app-note">
                        * Left click to any row to see more information about
                        the event *
                    </p>
                </div>

                <!-- Events table -->
                <PrimeVueTable
                    :value="events"
                    data-key="_id"
                    class="p-datatable-gridlines"
                    :rows="5"
                    :row-hover="true"
                    rowStyle="cursor: pointer"
                    @row-click="onRowClick"
                    responsive-layout="scroll"
                    :paginator="true"
                    removable-sort
                    filterDisplay="row"
                    v-model:filters="filters"
                    :filters="filters"
                    :globalFilterFields="[
                        'name',
                        'status',
                        'startDate',
                        'location.city',
                    ]"
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
                                    @click="clearFilter"
                                />

                                <RouterLink
                                    :to="{ name: 'Event Create' }"
                                    v-ripple
                                    class="p-button p-component mb-2 p-ripple app-router-link-icon"
                                >
                                    <i class="fa-solid fa-circle-plus"></i>
                                    New Events
                                </RouterLink>
                            </div>

                            <!-- Search Input -->
                            <span class="p-input-icon-left mb-2">
                                <i class="pi pi-search" />
                                <InputText
                                    placeholder="Keyword Search"
                                    style="width: 100%"
                                    v-model="filters['global'].value"
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
                        style="min-width: 20rem"
                    >
                        <template #body="{ data }">
                            {{ data.name }}
                        </template>
                        <template #filter="{ filterModel, filterCallback }">
                            <InputText
                                type="text"
                                v-model="filterModel.value"
                                @keydown.enter="filterCallback()"
                                class="p-column-filter"
                                :placeholder="`Search by name`"
                                v-tooltip.top.focus="
                                    'Press enter key to filter'
                                "
                            />
                        </template>
                    </PrimeVueColumn>

                    <!-- Events Status -->
                    <PrimeVueColumn
                        field="status"
                        header="Status"
                        style="max-width: 12rem"
                    >
                        <template #body="{ data }">
                            <span :class="'event-badge event-' + data.status">
                                {{ data.status }}
                            </span>
                        </template>
                        <template #filter="{ filterModel, filterCallback }">
                            <MultiSelect
                                v-model="filterModel.value"
                                @change="filterCallback()"
                                :options="EVENT_STATUS"
                                optionLabel=""
                                placeholder="Select event status"
                                class="p-column-filter"
                            >
                                <template #option="slotProps">
                                    <span
                                        :class="
                                            'event-badge event-' +
                                            slotProps.option
                                        "
                                    >
                                        {{ slotProps.option }}
                                    </span>
                                </template>
                            </MultiSelect>
                        </template>
                    </PrimeVueColumn>

                    <!-- Events Location -->
                    <PrimeVueColumn
                        field="location.city"
                        header="City"
                        style="max-width: 12rem"
                    >
                        <template #body="{ data }">
                            {{ data.location.city }}
                        </template>
                        <template #filter="{ filterModel, filterCallback }">
                            <MultiSelect
                                v-model="filterModel.value"
                                @change="filterCallback()"
                                :options="cities"
                                optionLabel=""
                                placeholder="Select location"
                                class="p-column-filter"
                            >
                                {{ slotProps.option }}
                            </MultiSelect>
                        </template>
                    </PrimeVueColumn>

                    <!-- Event Start Date -->
                    <PrimeVueColumn
                        header="Start date"
                        field="startDate"
                        dataType="date"
                        :sortable="true"
                        style="min-width: 14rem"
                    >
                        <template #body="{ data }">
                            {{ formatDate(data.startDate) }}
                        </template>
                        <template #filter="{ filterModel, filterCallback }">
                            <Calendar
                                v-model="filterModel.value"
                                dateFormat="mm/dd/yy"
                                placeholder="mm/dd/yyyy"
                                @date-select="filterCallback()"
                            />
                        </template>
                    </PrimeVueColumn>

                    <!-- Event Duration -->
                    <PrimeVueColumn
                        header="Duration"
                        field="duration"
                        dataType="number"
                        :sortable="true"
                        style="min-width: 10rem"
                    >
                        <template #body="{ data }">
                            {{ data.duration }} days
                        </template>
                    </PrimeVueColumn>
                </PrimeVueTable>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";
</style>
