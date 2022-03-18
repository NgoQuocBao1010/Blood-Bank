<script setup>
import { onBeforeMount } from "vue";
import dayjs from "dayjs";
import Calendar from "primevue/calendar";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import MultiSelect from "primevue/multiselect";
import DropDown from "primevue/dropdown";
import { FilterMatchMode } from "primevue/api";

import { BLOOD_TYPES } from "../../constants";
import { JSONtoExcel } from "../../utils";

const donors = [
    {
        id: "3dbf5ab8-4a39-43d9-8fda-2e1b4de09ca2",
        name: "Quoc Bao 1",
        bloodType: "A",
        date: new Date("2022-09-13"),
        amount: 40,
        event: "event1",
    },
    {
        id: "37121b27-ddff-4821-b57e-fa9ca36fa131",
        name: "Quoc Bao 2",
        bloodType: "O",
        date: new Date("2022-10-25"),
        amount: 41,
        event: "event2",
    },
    {
        id: "3fd4a632-95a2-41f8-b394-61943c034aad",
        name: "Quoc Bao 3",
        bloodType: "AB",
        date: new Date("2022-01-13"),
        amount: 42,
        event: "event3",
    },
    {
        id: "a33bca1a-8bbf-4cda-b7e4-868c9c8e9271",
        name: "Quoc Bao 4",
        bloodType: "A",
        date: new Date("2022-03-23"),
        amount: 43,
        event: "event4",
    },
    {
        id: "f3809f59-b68c-47ef-8d0a-be86a12c5e8c",
        name: "Quoc Bao 5",
        bloodType: "B",
        date: new Date("2022-04-03"),
        amount: 44,
        event: "event5",
    },
    {
        id: "16c6dfc3-f7ab-47c9-92d8-3e351adb986e",
        name: "Quoc Bao 6",
        bloodType: "A",
        date: new Date("2022-09-13"),
        amount: 45,
        event: "event1",
    },
    {
        id: "6da5caa2-fbba-451f-addb-e5e301e84af2",
        name: "Quoc Bao 7",
        bloodType: "AB",
        date: new Date("2022-09-13"),
        amount: 46,
        event: "event2",
    },
    {
        id: "8d5c1613-815f-4f0a-92b4-ce77ca13755d",
        name: "Quoc Bao 8",
        bloodType: "O",
        date: new Date("2021-09-13"),
        amount: 47,
        event: "event3",
    },
    {
        id: "f5c34b63-9512-4c4b-b12e-39962426eb5b",
        name: "Quoc Bao 9",
        bloodType: "AB",
        date: new Date("2021-12-13"),
        amount: 48,
        event: "event4",
    },
    {
        id: "ec4b9a70-5fec-4edb-aa40-30e71a9ea46b",
        name: "Quoc Bao 10",
        bloodType: "B",
        date: new Date("2022-06-13"),
        amount: 49,
        event: "event5",
    },
    {
        id: "7b1ef3ec-a495-41ec-821d-a7c7ee50c161",
        name: "Quoc Bao 11",
        bloodType: "A",
        date: new Date("2022-09-02"),
        amount: 50,
        event: "event2",
    },
    {
        id: "7b1ef3ec-a495-41ec-821d-afsdee50c161",
        name: "Quoc Bao 12",
        bloodType: "Rh",
        date: new Date("2022-09-02"),
        amount: 50,
        event: "event2",
    },
];

const downloadExcelFile = () => {
    // Format data before convert to excel
    const excelData = donors.map((el) => {
        let row = { ...el };

        row["Donor's Name"] = row["name"];
        row["Blood Type"] = row["bloodType"];
        row["Date Donated"] = row["date"];
        row["Amount (ml)"] = row["amount"];
        row["Event Name"] = row["event"];

        delete row["id"];
        delete row["name"];
        delete row["bloodType"];
        delete row["name"];
        delete row["date"];
        delete row["amount"];
        delete row["event"];

        return row;
    });

    JSONtoExcel(excelData, "donor_data");
};

const events = $computed(() => {
    const allEvents = donors.map((don) => don.event);
    return [...new Set(allEvents)];
});

const formatDate = (date) => {
    // Format date object
    const day = dayjs(date);
    return day.format("DD/MM/YYYY");
};

let filters = $ref(null);
const initFilter = () => {
    filters = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
        name: { value: null, matchMode: FilterMatchMode.CONTAINS },
        date: { value: null, matchMode: FilterMatchMode.DATE_IS },
        event: { value: null, matchMode: FilterMatchMode.EQUALS },
        bloodType: { value: null, matchMode: FilterMatchMode.IN },
        amount: {
            value: null,
            matchMode: FilterMatchMode.GREATER_THAN_OR_EQUAL_TO,
        },
    };
};
const clearFilter = () => {
    initFilter();
};
onBeforeMount(() => {
    initFilter();
});
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <!-- Page headers -->
                <h2>Donors Management</h2>

                <!-- Donors table -->
                <PrimeVueTable
                    :value="donors"
                    :paginator="true"
                    class="p-datatable-gridlines"
                    :rows="5"
                    dataKey="id"
                    :rowHover="true"
                    removableSort
                    filterDisplay="row"
                    v-model:filters="filters"
                    :filters="filters"
                    responsiveLayout="scroll"
                    :globalFilterFields="[
                        'name',
                        'date',
                        'event',
                        'bloodType',
                        'amount',
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
                                    @click="clearFilter()"
                                    class="p-button-outlined mb-2 mr-2"
                                />
                                <PrimeVueButton
                                    type="button"
                                    icon="pi pi-file-excel"
                                    label="Export to Excel"
                                    @click="downloadExcelFile"
                                    class="p-button-outlined mb-2"
                                />
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
                    <template #empty> No donors found. </template>

                    <!-- Columns -->
                    <!-- Donor's name -->
                    <PrimeVueColumn
                        field="name"
                        header="Name"
                        style="min-width: 12rem"
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

                    <!-- Date donated -->
                    <PrimeVueColumn
                        header="Date Donated"
                        field="date"
                        dataType="date"
                        :sortable="true"
                        style="min-width: 12rem"
                    >
                        <template #body="{ data }">
                            {{ formatDate(data.date) }}
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

                    <!-- Event name -->
                    <PrimeVueColumn
                        field="event"
                        header="Event"
                        style="min-width: 12rem"
                    >
                        <template #body="{ data }">
                            {{ data.event }}
                        </template>
                        <template #filter="{ filterModel, filterCallback }">
                            <DropDown
                                v-model="filterModel.value"
                                @change="filterCallback"
                                :options="events"
                                placeholder="Choose Event"
                                class="p-column-filter"
                                :showClear="true"
                            >
                                <template #value="slotProps">
                                    <span v-if="slotProps.value">
                                        {{ slotProps.value }}
                                    </span>
                                    <span v-else>
                                        {{ slotProps.placeholder }}
                                    </span>
                                </template>
                                <template #option="slotProps">
                                    <span>{{ slotProps.option }}</span>
                                </template>
                            </DropDown>
                        </template>
                    </PrimeVueColumn>

                    <!-- Blood Type -->
                    <PrimeVueColumn
                        field="bloodType"
                        header="Blood Type"
                        style="max-width: 14rem !important"
                    >
                        <template #body="{ data }">
                            <span :class="'blood-badge type-' + data.bloodType">
                                Type {{ data.bloodType }}
                            </span>
                        </template>
                        <template #filter="{ filterModel, filterCallback }">
                            <MultiSelect
                                v-model="filterModel.value"
                                @change="filterCallback()"
                                :options="BLOOD_TYPES"
                                optionLabel=""
                                placeholder="Select blood type"
                                class="p-column-filter"
                            >
                                <template #option="slotProps">
                                    <span
                                        :class="
                                            'blood-badge type-' +
                                            slotProps.option
                                        "
                                    >
                                        Type {{ slotProps.option }}
                                    </span>
                                </template>
                            </MultiSelect>
                        </template>
                    </PrimeVueColumn>

                    <!-- Amount -->
                    <PrimeVueColumn
                        field="amount"
                        header="Amount"
                        dataType="numeric"
                        :sortable="true"
                        :showFilterMatchModes="false"
                        style="min-width: 12rem"
                    >
                        <template #body="{ data }">
                            {{ data.amount }} ml
                        </template>
                        <template #filter="{ filterModel, filterCallback }">
                            <InputNumber
                                v-model="filterModel.value"
                                @keydown.enter="filterCallback()"
                                class="p-column-filter"
                                :placeholder="`Filter by amount (ml)`"
                                v-tooltip.top.focus="
                                    'Press enter key to filter'
                                "
                            />
                        </template>
                    </PrimeVueColumn>
                </PrimeVueTable>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";
::v-deep(.p-datatable-frozen-tbody) {
    font-weight: bold;
}

::v-deep(.p-datatable-scrollable .p-frozen-column) {
    font-weight: bold;
}
</style>
