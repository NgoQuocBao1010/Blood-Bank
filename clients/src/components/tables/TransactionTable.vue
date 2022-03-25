<script setup>
import { onBeforeMount } from "vue";
import Calendar from "primevue/calendar";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import DropDown from "primevue/dropdown";
import { FilterMatchMode } from "primevue/api";

import { formatDate } from "../../utils";

const { transactionData, participants } = defineProps({
    transactionData: {
        type: Array,
        required: true,
    },
});

let transactions = $ref(null);
const events = transactionData
    ? [...new Set(transactionData.map((row) => row._event.name))]
    : [];

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
    transactions = transactionData.map((row) => {
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
        <PrimeVueColumn field="_event.name" header="Event">
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
