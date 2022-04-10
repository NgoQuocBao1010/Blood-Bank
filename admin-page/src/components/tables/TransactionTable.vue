<script setup>
import { onBeforeMount } from "vue";
import Calendar from "primevue/calendar";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import DropDown from "primevue/dropdown";
import MultiSelect from "primevue/multiselect";
import { FilterMatchMode } from "primevue/api";

import DonorTransactionHelper from "../../utils/helpers/DonorTransaction";
import { formatDate } from "../../utils";
import { TRANSACTION_STATUS } from "../../constants";

const { transactionData, filterData } = defineProps({
    transactionData: {
        type: Array,
        required: true,
    },
    filterData: {
        type: String,
        required: false,
    },
});

let transactions = $ref(null);
const events = transactionData
    ? [...new Set(transactionData.map((row) => row.eventDonated.name))]
    : [];

onBeforeMount(() => {
    initFilter();
    if (filterData) {
        filters["global"]["value"] = filterData;
    }

    transactions = transactionData.map((row) => {
        let transaction = { ...row };

        transaction.dateDonated = new Date(parseInt(transaction.dateDonated));
        transaction.status =
            DonorTransactionHelper.determineStatus(transaction);

        return transaction;
    });
});

// Filter configurations
let filters = $ref(null);
const initFilter = () => {
    filters = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
        _id: { value: null, matchMode: FilterMatchMode.CONTAINS },
        dateDonated: {
            value: null,
            matchMode: FilterMatchMode.DATE_IS,
        },
        "eventDonated.name": {
            value: null,
            matchMode: FilterMatchMode.EQUALS,
        },
        amount: {
            value: null,
            matchMode: FilterMatchMode.GREATER_THAN_OR_EQUAL_TO,
        },
        status: { value: null, matchMode: FilterMatchMode.IN },
    };
};
const clearFilter = () => {
    initFilter();
};
</script>

<template>
    <div class="flex justify-content-between align-content-center">
        <h3>Donations Table</h3>
        <p class="app-note">
            ** Hover on the FAILED badge to view the failed reason **
        </p>
    </div>
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
        :globalFilterFields="[
            '_id',
            'dateDonated',
            'eventDonated.name',
            'amount',
            'status',
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
                </div>

                <!-- Search Input -->
                <span class="p-input-icon-left mb-2" style="width: 20em">
                    <i class="pi pi-search" />
                    <InputText
                        placeholder="Keyword Search"
                        v-model="filters['global'].value"
                        style="width: 100%"
                    />
                </span>
            </div>
        </template>

        <!-- Empty data fallback -->

        <template #empty>
            <h4 style="text-align: center">No transaction found.</h4>
        </template>

        <!-- Columns -->
        <!-- Event name -->
        <PrimeVueColumn field="eventDonated.name" header="Event">
            <template #body="{ data }">
                {{ data.eventDonated.name }}
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

        <!-- Status -->
        <PrimeVueColumn
            field="status"
            header="Status"
            style="max-width: 14rem !important"
        >
            <template #body="{ data }">
                <span
                    :class="'transaction-badge status-' + data.status"
                    style="cursor: pointer"
                    v-tooltip.bottom="{
                        value: `Failed Reason: ${data.rejectReason}`,
                        class: 'reason-tooltip',
                    }"
                    v-if="data.status === 'failed'"
                >
                    {{ data.status }}
                </span>
                <span :class="'transaction-badge status-' + data.status" v-else>
                    {{ data.status }}
                </span>
            </template>
            <template #filter="{ filterModel, filterCallback }">
                <MultiSelect
                    v-model="filterModel.value"
                    @change="filterCallback()"
                    :options="TRANSACTION_STATUS"
                    class="p-column-filter"
                >
                    <template #option="slotProps">
                        <span
                            :class="
                                'transaction-badge status-' + slotProps.option
                            "
                        >
                            {{ slotProps.option }}
                        </span>
                    </template>
                </MultiSelect>
            </template>
        </PrimeVueColumn>
    </PrimeVueTable>
</template>
<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";
</style>
