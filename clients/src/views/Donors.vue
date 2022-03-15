<script setup>
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";
import InputText from "primevue/inputtext";
import ProgressBar from "primevue/progressbar";
import { FilterMatchMode, FilterOperator } from "primevue/api";

const donors = [
    {
        id: "3dbf5ab8-4a39-43d9-8fda-2e1b4de09ca2",
        name: "Quoc Bao 1",
        bloodType: "A",
        date: "2022-09-13",
        amount: 40,
        event: "event1",
    },
    {
        id: "37121b27-ddff-4821-b57e-fa9ca36fa131",
        name: "Quoc Bao 2",
        bloodType: "O",
        date: "2022-10-25",
        amount: 41,
        event: "event2",
    },
    {
        id: "3fd4a632-95a2-41f8-b394-61943c034aad",
        name: "Quoc Bao 3",
        bloodType: "AB",
        date: "2022-01-13",
        amount: 42,
        event: "event3",
    },
    {
        id: "a33bca1a-8bbf-4cda-b7e4-868c9c8e9271",
        name: "Quoc Bao 4",
        bloodType: "A",
        date: "2022-03-23",
        amount: 43,
        event: "event4",
    },
    {
        id: "f3809f59-b68c-47ef-8d0a-be86a12c5e8c",
        name: "Quoc Bao 5",
        bloodType: "B",
        date: "2022-04-03",
        amount: 44,
        event: "event5",
    },
    {
        id: "16c6dfc3-f7ab-47c9-92d8-3e351adb986e",
        name: "Quoc Bao 6",
        bloodType: "A",
        date: "2022-09-13",
        amount: 45,
        event: "event1",
    },
    {
        id: "6da5caa2-fbba-451f-addb-e5e301e84af2",
        name: "Quoc Bao 7",
        bloodType: "AB",
        date: "2022-09-13",
        amount: 46,
        event: "event2",
    },
    {
        id: "8d5c1613-815f-4f0a-92b4-ce77ca13755d",
        name: "Quoc Bao 8",
        bloodType: "O",
        date: "2021-09-13",
        amount: 47,
        event: "event3",
    },
    {
        id: "f5c34b63-9512-4c4b-b12e-39962426eb5b",
        name: "Quoc Bao 9",
        bloodType: "AB",
        date: "2021-12-13",
        amount: 48,
        event: "event4",
    },
    {
        id: "ec4b9a70-5fec-4edb-aa40-30e71a9ea46b",
        name: "Quoc Bao 10",
        bloodType: "B",
        date: "2022-06-13",
        amount: 49,
        event: "event5",
    },
    {
        id: "7b1ef3ec-a495-41ec-821d-a7c7ee50c161",
        name: "Quoc Bao 11",
        bloodType: "A",
        date: "2022-09-2",
        amount: 50,
        event: "event2",
    },
];
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <!-- Table header -->
                <h5>Donors Table</h5>

                <!-- Donors table -->
                <DataTable
                    :value="donors"
                    :paginator="true"
                    class="p-datatable-gridlines"
                    :rows="5"
                    dataKey="id"
                    :rowHover="true"
                    removableSort
                    filterDisplay="menu"
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
                            <Button
                                type="button"
                                icon="pi pi-filter-slash"
                                label="Clear"
                                class="p-button-outlined mb-2"
                            />
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
                    <template #empty> No customers found. </template>

                    <!-- Columns -->
                    <!-- Name column -->
                    <Column field="name" header="Name" style="min-width: 12rem">
                        <template #body="{ data }">
                            {{ data.name }}
                        </template>
                    </Column>

                    <!-- Date donated -->
                    <Column
                        header="Date Donated"
                        filterField="date"
                        dataType="date"
                        :sortable="true"
                        style="min-width: 10rem"
                    >
                        <template #body="{ data }">
                            {{ data.date }}
                        </template>
                    </Column>

                    <!-- Event name -->
                    <Column
                        field="event"
                        header="Event"
                        style="min-width: 12rem"
                    >
                        <template #body="{ data }">
                            {{ data.event }}
                        </template>
                    </Column>

                    <!-- Blood Type -->
                    <Column
                        field="bloodType"
                        header="Blood Type"
                        :filterMenuStyle="{ width: '14rem' }"
                        style="min-width: 12rem"
                    >
                        <template #body="{ data }">
                            <span :class="'blood-badge type-' + data.bloodType">
                                Type {{ data.bloodType }}
                            </span>
                        </template>
                    </Column>

                    <!-- Amount -->
                    <Column
                        field="amount"
                        header="Amount"
                        :sortable="true"
                        :showFilterMatchModes="false"
                        style="min-width: 12rem"
                    >
                        <template #body="{ data }">
                            {{ data.amount }} ml
                        </template>
                    </Column>
                </DataTable>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
.blood-badge {
    border-radius: var(--border-radius);
    padding: 0.25em 0.5rem;
    text-transform: uppercase;
    font-weight: 700;
    font-size: 12px;
    letter-spacing: 0.3px;

    &.type-A {
        background: #c8e6c9;
        color: #256029;
    }

    &.type-B {
        background: #ffcdd2;
        color: #c63737;
    }

    &.type-AB {
        background: #feedaf;
        color: #8a5340;
    }

    &.type-O {
        background: #b3e5fc;
        color: #23547b;
    }
}

::v-deep(.p-datatable-frozen-tbody) {
    font-weight: bold;
}

::v-deep(.p-datatable-scrollable .p-frozen-column) {
    font-weight: bold;
}
</style>
