<script setup>
import { onBeforeMount, onMounted } from "vue";
import { useRouter, RouterLink } from "vue-router";
import dayjs from "dayjs";
import InputText from "primevue/inputtext";
import MultiSelect from "primevue/multiselect";
import Calendar from "primevue/calendar";
import { FilterMatchMode } from "primevue/api";

import EventRepo from "../../api/EventRepo";
import { formatDate } from "../../utils";

const router = useRouter();
let events = $ref();
let fetchingEvent = $ref(true);
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

onBeforeMount(async () => {
    initFilters();

    const { data } = await EventRepo.getAll();
    events = data.map((row) => {
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
    fetchingEvent = false;
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
                    :loading="fetchingEvent"
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
                    <template #empty>
                        <h4 style="text-align: center">No data found.</h4>
                    </template>

                    <!-- Loading fallback -->
                    <template #loading>
                        <h4 style="text-align: center">Fetching data ...</h4>
                    </template>

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
