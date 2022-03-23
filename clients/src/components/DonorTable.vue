<script setup>
import { onBeforeMount } from "vue";
import FileUpload from "primevue/fileupload";
import Calendar from "primevue/calendar";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import MultiSelect from "primevue/multiselect";
import DropDown from "primevue/dropdown";
import { FilterMatchMode } from "primevue/api";

import { BLOOD_TYPES } from "../constants";
import { formatDate } from "../utils";

const { donorsData, participants } = defineProps({
    donorsData: {
        type: Array,
        required: true,
    },
    participants: {
        type: Boolean,
        default: false,
    },
});

let donors = $ref(null);
const events = participants
    ? [...new Set(donorsData.map((don) => don.transaction._event.name))]
    : null;

let selectedDonors = $ref([]);

// Filter configurations
let filters = $ref(null);
const initFilter = () => {
    filters = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
        name: { value: null, matchMode: FilterMatchMode.CONTAINS },
        _id: { value: null, matchMode: FilterMatchMode.CONTAINS },
        "transaction.dateDonated": {
            value: null,
            matchMode: FilterMatchMode.DATE_IS,
        },
        "transaction._event.name": {
            value: null,
            matchMode: FilterMatchMode.EQUALS,
        },
        "transaction.blood.name": {
            value: null,
            matchMode: FilterMatchMode.IN,
        },
        "transaction.blood.type": {
            value: null,
            matchMode: FilterMatchMode.EQUALS,
        },
        "transaction.amount": {
            value: null,
            matchMode: FilterMatchMode.GREATER_THAN_OR_EQUAL_TO,
        },
    };
};
const clearFilter = () => {
    initFilter();
};

onBeforeMount(() => {
    donors = donorsData.map((row) => {
        let donor = { ...row };
        donor.transaction.dateDonated = new Date(donor.transaction.dateDonated);
        return donor;
    });
    initFilter();
});
</script>

<template>
    <PrimeVueTable
        :value="donors"
        :paginator="true"
        class="p-datatable-gridlines"
        :rows="5"
        dataKey="_id"
        :rowHover="true"
        removableSort
        filterDisplay="row"
        v-model:selection="selectedDonors"
        v-model:filters="filters"
        :filters="filters"
        responsiveLayout="scroll"
        :globalFilterFields="[
            'name',
            '_id',
            'transaction.dateDonated',
            'transaction._event.name',
            'transaction.blood.name',
            'transaction.blood.type',
            'transaction.amount',
        ]"
    >
        <!-- Header of the table -->
        <template #header>
            <div class="flex justify-content-between flex-column sm:flex-row">
                <div class="flex">
                    <PrimeVueButton
                        type="button"
                        icon="pi pi-filter-slash"
                        label="Clear"
                        @click="clearFilter"
                        class="p-button-outlined mb-2 mr-2"
                    />

                    <PrimeVueButton
                        type="button"
                        icon="pi pi-file-excel"
                        label="Export to Excel"
                        class="p-button-outlined mb-2 mr-2"
                    />

                    <FileUpload
                        mode="basic"
                        name="requestFiles"
                        choose-label="Upload Excel File"
                        accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
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

        <!-- Selection column -->
        <PrimeVueColumn
            selectionMode="multiple"
            headerStyle="width: 2rem"
            v-if="participants"
        ></PrimeVueColumn>

        <!-- Donor's name -->
        <PrimeVueColumn field="name" header="Name" style="min-width: 12rem">
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
                    v-tooltip.top.focus="'Press enter key to filter'"
                />
            </template>
        </PrimeVueColumn>

        <!-- Personal ID -->
        <PrimeVueColumn
            field="_id"
            header="Personal ID"
            style="min-width: 200px; max-width: 14rem"
        >
            <template #body="{ data }">
                {{ data._id }}
            </template>
            <template #filter="{ filterModel, filterCallback }">
                <InputText
                    type="text"
                    v-model="filterModel.value"
                    @keydown.enter="filterCallback()"
                    class="p-column-filter"
                    :placeholder="`Search by ID`"
                    v-tooltip.top.focus="'Press enter key to filter'"
                />
            </template>
        </PrimeVueColumn>

        <!-- Date donated -->
        <PrimeVueColumn
            header="Date Donated"
            field="transaction.dateDonated"
            dataType="date"
            :sortable="true"
            style="min-width: 200px; width: 14rem !important"
        >
            <template #body="{ data }">
                {{ formatDate(data.transaction.dateDonated) }}
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
            field="transaction._event.name"
            header="Event"
            style="min-width: 250px; max-width: 12rem"
            v-if="participants"
        >
            <template #body="{ data }">
                {{ data.transaction._event.name }}
            </template>
            <template #filter="{ filterModel, filterCallback }">
                <DropDown
                    v-model="filterModel.value"
                    @change="filterCallback"
                    :options="events"
                    class="p-column-filter"
                    style="height: 2.2rem"
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

        <!-- Blood Name -->
        <PrimeVueColumn
            field="transaction.blood.name"
            header="Blood Name"
            style="width: 8rem"
        >
            <template #body="{ data }">
                <span
                    :class="'blood-badge type-' + data.transaction.blood.name"
                >
                    Type {{ data.transaction.blood.name }}
                </span>
            </template>
            <template #filter="{ filterModel, filterCallback }">
                <MultiSelect
                    v-model="filterModel.value"
                    @change="filterCallback()"
                    :options="BLOOD_TYPES"
                    class="p-column-filter"
                >
                    <template #option="slotProps">
                        <span :class="'blood-badge type-' + slotProps.option">
                            Type {{ slotProps.option }}
                        </span>
                    </template>
                </MultiSelect>
            </template>
        </PrimeVueColumn>

        <!-- Blood Type -->
        <PrimeVueColumn
            field="transaction.blood.type"
            header="Blood Type"
            style="min-width: 200px"
        >
            <template #body="{ data }">
                {{ data.transaction.blood.type }}
            </template>
            <template #filter="{ filterModel, filterCallback }">
                <DropDown
                    v-model="filterModel.value"
                    @change="filterCallback"
                    :options="['Positive', 'Negative']"
                    class="p-column-filter"
                    style="height: 2.2rem"
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

        <!-- Amount -->
        <PrimeVueColumn
            field="transaction.amount"
            header="Amount (ml)"
            dataType="numeric"
            :sortable="true"
            :showFilterMatchModes="false"
            style="min-width: 180px; max-width: 10rem"
        >
            <template #body="{ data }">
                {{ data.transaction.amount }} ml
            </template>
            <template #filter="{ filterModel, filterCallback }">
                <InputNumber
                    v-model="filterModel.value"
                    @keydown.enter="filterCallback()"
                    class="p-column-filter"
                    v-tooltip.top.focus="'Press enter key to filter'"
                />
            </template>
        </PrimeVueColumn>

        <!-- Table's footer -->
        <template #footer v-if="selectedDonors.length > 0">
            <PrimeVueButton
                type="button"
                icon="pi pi-check-circle"
                label="Approve"
                class="p-button p-button-sm mr-2 approve-btn"
            />

            <PrimeVueButton
                type="button"
                icon="pi pi-times-circle"
                label="Reject"
                class="p-button p-button-sm reject-btn"
            />
        </template>
    </PrimeVueTable>
</template>

<style lang="scss" scoped>
@import "../assets/styles/badge.scss";

.approve-btn {
    border: none !important;
    background: #00c897 !important;
}

.reject-btn {
    border: none !important;
    background: #ff6363 !important;
}
</style>
