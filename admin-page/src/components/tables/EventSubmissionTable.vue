<script setup>
import { onBeforeMount } from "vue";
import InputText from "primevue/inputtext";
import { FilterMatchMode } from "primevue/api";

import EventSubmissionHelper from "../../utils/helpers/EventSubmission.js";
import { formatDate } from "../../utils";

const { donorsData } = defineProps({
    donorsData: {
        type: Array,
        required: true,
    },
});

let data = $ref();

// Filter configurations
let filters = $ref(null);
const initFilter = () => {
    filters = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
        name: { value: null, matchMode: FilterMatchMode.CONTAINS },
        personalId: { value: null, matchMode: FilterMatchMode.CONTAINS },
        email: { value: null, matchMode: FilterMatchMode.CONTAINS },
        phone: { value: null, matchMode: FilterMatchMode.CONTAINS },
        address: { value: null, matchMode: FilterMatchMode.CONTAINS },
    };
};
const clearFilter = () => {
    initFilter();
};

onBeforeMount(() => {
    initFilter();
    data = EventSubmissionHelper.transformData(donorsData);
});
</script>

<template>
    <PrimeVueTable
        :value="data"
        :paginator="true"
        class="p-datatable-gridlines"
        :rows="10"
        dataKey="_id"
        :rowHover="true"
        removableSort
        filterDisplay="row"
        v-model:filters="filters"
        :filters="filters"
        responsiveLayout="scroll"
        :globalFilterFields="[
            'name',
            'personalId',
            'email',
            'phone',
            'address',
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
            <h4 style="text-align: center">No donor found.</h4>
        </template>

        <!-- Columns -->
        <!-- Donor's name -->
        <PrimeVueColumn field="name" header="Name" style="min-width: 300px">
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

        <!-- DOB -->
        <PrimeVueColumn
            field="dob"
            header="Date of Birth"
            :sortable="true"
            style="min-width: 300px"
        >
            <template #body="{ data }">
                {{ formatDate(data.dob) }}
            </template>
        </PrimeVueColumn>

        <!-- Personal Id -->
        <PrimeVueColumn
            field="personalId"
            header="Personal Id"
            style="min-width: 300px"
        >
            <template #body="{ data }">
                {{ data.personalId }}
            </template>
            <template #filter="{ filterModel, filterCallback }">
                <InputText
                    type="text"
                    v-model="filterModel.value"
                    @keydown.enter="filterCallback()"
                    class="p-column-filter"
                    :placeholder="`Search by Id`"
                    v-tooltip.top.focus="'Press enter key to filter'"
                />
            </template>
        </PrimeVueColumn>

        <!-- Email -->
        <PrimeVueColumn field="email" header="Email" style="min-width: 300px">
            <template #body="{ data }">
                {{ data.email }}
            </template>
            <template #filter="{ filterModel, filterCallback }">
                <InputText
                    type="text"
                    v-model="filterModel.value"
                    @keydown.enter="filterCallback()"
                    class="p-column-filter"
                    :placeholder="`Search by email`"
                    v-tooltip.top.focus="'Press enter key to filter'"
                />
            </template>
        </PrimeVueColumn>

        <!-- Phone -->
        <PrimeVueColumn
            field="phone"
            header="Phone Number"
            style="min-width: 300px"
        >
            <template #body="{ data }">
                {{ data.phone }}
            </template>
            <template #filter="{ filterModel, filterCallback }">
                <InputText
                    type="text"
                    v-model="filterModel.value"
                    @keydown.enter="filterCallback()"
                    class="p-column-filter"
                    :placeholder="`Search by email`"
                    v-tooltip.top.focus="'Press enter key to filter'"
                />
            </template>
        </PrimeVueColumn>

        <!-- Address -->
        <PrimeVueColumn
            field="address"
            header="Address"
            style="min-width: 300px"
        >
            <template #body="{ data }">
                {{ data.address }}
            </template>
            <template #filter="{ filterModel, filterCallback }">
                <InputText
                    type="text"
                    v-model="filterModel.value"
                    @keydown.enter="filterCallback()"
                    class="p-column-filter"
                    :placeholder="`Search by email`"
                    v-tooltip.top.focus="'Press enter key to filter'"
                />
            </template>
        </PrimeVueColumn>
    </PrimeVueTable>
</template>

<style lang="scss" scoped></style>
