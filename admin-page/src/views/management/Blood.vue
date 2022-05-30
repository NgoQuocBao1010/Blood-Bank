<script setup>
import { onBeforeMount } from "vue";
import OverlayPanel from "primevue/overlaypanel";

import { useBloodStore } from "../../stores/blood.js";
import BloodHelper from "../../utils/helpers/Blood.js";
import { JSONtoExcel } from "../../utils/excel";

const bloodStore = useBloodStore();
let bloodData = null;
let tableData = $ref([]);
let fetchingData = $ref(true);

let expandedRows = $ref([]);
const expandAllRows = () => {
    expandedRows = tableData.filter((el) => el._id);
};

onBeforeMount(async () => {
    fetchingData = false;
    bloodData = await bloodStore.getData();
    tableData = bloodStore.summaryData;
});

let stockInfo = $ref(null);
const toggleStockInfo = (event) => {
    // Toggle info button about stock status
    stockInfo.toggle(event);
};

const downloadExcelFile = () => {
    const excelData = BloodHelper.transformRowsForExcelDownload(bloodData);
    JSONtoExcel(excelData, "Blood_data");
};
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <!-- Page headers -->
                <h2>Blood Management</h2>

                <!-- Blood Table -->
                <PrimeVueTable
                    :value="tableData"
                    :loading="fetchingData"
                    dataKey="_id"
                    v-model:expandedRows="expandedRows"
                    responsiveLayout="scroll"
                    removableSort
                    class="p-datatable-gridlines"
                >
                    <!-- Table Header -->
                    <template #header>
                        <div class="header-container">
                            <div>
                                <!-- Expand and Collapse all buttons -->
                                <PrimeVueButton
                                    icon="pi pi-plus"
                                    label="Expand All"
                                    class="mr-2 mb-2"
                                    @click="expandAllRows()"
                                    :disabled="expandedRows.length === 5"
                                />
                                <PrimeVueButton
                                    icon="pi pi-minus"
                                    label="Collapse All"
                                    class="mb-2 mr-2"
                                    @click="expandedRows = []"
                                    :disabled="expandedRows.length === 0"
                                />
                                <!-- Export to JSON button -->
                                <PrimeVueButton
                                    type="button"
                                    icon="pi pi-file-excel"
                                    label="Export to Excel"
                                    @click="downloadExcelFile"
                                    class="p-button-outlined mb-2"
                                />
                            </div>

                            <!-- Information Button -->
                            <i
                                class="information pi pi-info-circle mr-2 mb-2"
                                ref="stockInfo"
                                @click="toggleStockInfo"
                            ></i>

                            <!-- Stock Status Information -->
                            <OverlayPanel ref="stockInfo" id="overlay_panel">
                                <h5>Stock Status:</h5>

                                <p>Out of stock: <span>0ml</span></p>
                                <p>Low in stock: <span>&lt; 400ml</span></p>
                                <p>
                                    Good in stock:
                                    <span>400ml &lt;= 700ml &lt; 1000ml</span>
                                </p>
                                <p>Great in stock: <span>&gt;= 1000ml</span></p>
                            </OverlayPanel>
                        </div>
                    </template>

                    <!-- Empty data fallback -->
                    <template #empty>
                        <h4 style="text-align: center">
                            No data found. Probably a server problem.
                        </h4>
                    </template>

                    <!-- Loading fallback -->
                    <template #loading>
                        <h4 style="text-align: center">Fetching data ...</h4>
                    </template>

                    <template v-if="tableData"> </template>
                    <!-- Expand Icon -->
                    <PrimeVueColumn
                        :expander="true"
                        headerStyle="width: 3rem"
                    />

                    <!-- Blood Type Name -->
                    <PrimeVueColumn
                        field="name"
                        header="Name"
                        style="min-width: 8rem !important"
                    >
                        <template #body="slotProps">
                            {{ slotProps.data.name }}
                        </template>
                    </PrimeVueColumn>

                    <!-- Total Quantity Column -->
                    <PrimeVueColumn
                        field="quantity"
                        header="Total Quantity"
                        :sortable="true"
                        style="text-align: center"
                    >
                        <template #body="slotProps">
                            <p style="text-align: center">
                                {{ slotProps.data.quantity }} ml
                            </p>
                        </template>
                    </PrimeVueColumn>

                    <!-- Stock status Column -->
                    <PrimeVueColumn
                        field="inStock"
                        header="Stock status"
                        header-style="width: 10rem"
                        :sortable="true"
                    >
                        <template #body="slotProps">
                            <p style="text-align: center">
                                <i
                                    class="fa-solid fa-circle-exclamation"
                                    style="font-size: 1.5rem; color: #ff1818"
                                    v-if="!slotProps.data.inStock"
                                ></i>
                                <i
                                    class="fa-solid fa-circle-check"
                                    style="font-size: 1.5rem; color: #00c897"
                                    v-else
                                ></i>
                            </p>
                        </template>
                    </PrimeVueColumn>

                    <!-- Expand Rows to a Datatable -->
                    <template #expansion="slotProps">
                        <div class="p-3">
                            <!-- Row Expand Table Headers -->
                            <p>Blood {{ slotProps.data.name }} details</p>

                            <!-- Expand Table -->
                            <PrimeVueTable
                                :value="slotProps.data.types"
                                responsiveLayout="scroll"
                            >
                                <!-- Type -->
                                <PrimeVueColumn field="name" header="Type">
                                    <template #body="slotProps">
                                        <span
                                            style="text-transform: capitalize"
                                        >
                                            {{ slotProps.data.bloodType }}
                                            {{ slotProps.data.name }}
                                        </span>
                                    </template>
                                </PrimeVueColumn>

                                <!-- Quantity -->
                                <PrimeVueColumn
                                    field="quantity"
                                    header="Quantity"
                                    :sortable="true"
                                >
                                    <template #body="slotProps">
                                        <p style="text-align: center">
                                            {{ slotProps.data.quantity }} ml
                                        </p>
                                    </template>
                                </PrimeVueColumn>

                                <!-- Stock Status -->
                                <PrimeVueColumn
                                    field="status"
                                    header="Stock Status"
                                    header-style="width:15rem"
                                >
                                    <template #body="slotProps">
                                        <span
                                            :class="
                                                'stock-badge status-' +
                                                slotProps.data.status
                                            "
                                        >
                                            {{ slotProps.data.displayStatus }}
                                        </span>
                                    </template>
                                </PrimeVueColumn>
                            </PrimeVueTable>
                        </div>
                    </template>
                </PrimeVueTable>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";

.header-container {
    display: flex;
    justify-content: space-between;
    align-items: center;
    i.information {
        font-size: 1.5rem;
        font-weight: 900;
        cursor: pointer;
    }
}

#overlay_panel {
    h5 {
        color: var(--primary-color);
    }
    p {
        font-weight: bold;
        span {
            font-style: italic;
            color: var(--primary-color);
        }
    }
}
</style>
