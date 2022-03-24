<script setup>
import { onBeforeMount } from "vue";
import Calendar from "primevue/calendar";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import DropDown from "primevue/dropdown";
import { FilterMatchMode } from "primevue/api";

import { formatDate } from "../../utils";

const data = [
    {
        _id: "911fdf65-2913-4705-8df1-7cba4a0a9355",
        _event: {
            _id: "1440f35b-0db5-484b-9370-872cb3c7f519",
            name: "event",
        },
        amount: 500,
        dateDonated: new Date("2021-09-13").getTime().toString(),
    },
    {
        _id: "6f769818-5060-435e-835b-ab7ab3cfdaec",
        _event: {
            _id: "de169f18-226d-48e0-9579-b18184e2c260",
            name: "event1",
        },
        amount: 420,
        dateDonated: new Date("2021-09-13").getTime(),
    },
    {
        _id: "6f05348f-5e55-408f-9c70-dddc105e8c7a",
        _event: {
            _id: "7cae7784-7523-47ae-b1a4-42308f8fb348",
            name: "event",
        },
        amount: 421,
        dateDonated: new Date("2021-09-13").getTime(),
    },
];

let transactions = $ref(null);
const events = data ? [...new Set(data.map((row) => row._event.name))] : [];

// Filter configurations
let filters = $ref(null);
const initFilter = () => {
    filters = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
        dateDonated: {
            value: null,
            matchMode: FilterMatchMode.DATE_IS,
        },
        "_event.name": {
            value: null,
            matchMode: FilterMatchMode.EQUALS,
        },
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
    transactions = data.map((row) => {
        let transaction = { ...row };

        transaction.dateDonated = new Date(parseInt(transaction.dateDonated));
        return transaction;
    });
    initFilter();
});
</script>

<template>
    <PrimeVueTable
        :value="transactions"
        :paginator="true"
        class="p-datatable-gridlines"
        :rows="5"
        dataKey="_id"
        :rowHover="true"
        removableSort
        filterDisplay="row"
        v-model:filters="filters"
        :filters="filters"
        responsiveLayout="scroll"
        :globalFilterFields="['dateDonated', '_event.name', 'amount']"
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
        <template #empty> No transaction found. </template>

        <!-- Columns -->

        <!-- Event name -->
        <PrimeVueColumn
            field="_event.name"
            header="Event"
            style="min-width: 20rem"
        >
            <template #body="{ data }">
                {{ data._event.name }}
            </template>
            <template #filter="{ filterModel, filterCallback }">
                <DropDown
                    v-model="filterModel.value"
                    @change="filterCallback"
                    :options="events"
                    class="p-column-filter"
                    placeholder="Select event"
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

        <!-- Date donated -->
        <PrimeVueColumn
            header="Date Donated"
            field="dateDonated"
            dataType="date"
            :sortable="true"
        >
            <template #body="{ data }">
                {{ formatDate(data.dateDonated) }}
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

        <!-- Amount -->
        <PrimeVueColumn
            field="amount"
            header="Amount (ml)"
            dataType="numeric"
            :sortable="true"
            :showFilterMatchModes="false"
            style="min-width: 180px; max-width: 10rem"
        >
            <template #body="{ data }"> {{ data.amount }} ml </template>
            <template #filter="{ filterModel, filterCallback }">
                <InputNumber
                    v-model="filterModel.value"
                    @keydown.enter="filterCallback()"
                    class="p-column-filter"
                    v-tooltip.top.focus="'Press enter key to filter'"
                />
            </template>
        </PrimeVueColumn>
    </PrimeVueTable>
</template>

<style lang="scss" scoped></style>
