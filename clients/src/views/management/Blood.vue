<script setup>
import { onBeforeMount } from "vue";
import OverlayPanel from "primevue/overlaypanel";

import { BLOOD_TYPES } from "../../constants";
import { determineStockStatus, JSONtoExcel } from "../../utils";

// *** Mock data ***
const bloodData = [
    {
        id: "0ab35769-453b-4544-a2c5-759078731b5b",
        name: "A",
        type: "positive",
        quantity: 2000,
    },
    {
        id: "dd6ed89c-178e-4d04-9d5e-00650e639fc0",
        name: "A",
        type: "negative",
        quantity: 700,
    },
    {
        id: "53192215-df3d-409a-83a3-cf6d831e9d6d",
        name: "B",
        type: "positive",
        quantity: 1700,
    },
    {
        id: "d70b0b7b-7e92-4596-9e25-32d31ee6a898",
        name: "B",
        type: "negative",
        quantity: 1500,
    },
    {
        id: "1b0b6c62-2601-4bd4-b563-bfd99c10dd7f",
        name: "AB",
        type: "positive",
        quantity: 1000,
    },
    {
        id: "9fdb6041-e35b-49aa-b47b-326ff9c386f4",
        name: "AB",
        type: "negative",
        quantity: 1600,
    },
    {
        id: "e7abb462-442a-47d0-a724-740e70c3435c",
        name: "O",
        type: "positive",
        quantity: 1700,
    },
    {
        id: "9a59d831-4036-420b-8dce-0f6cf86d16c9",
        name: "O",
        type: "negative",
        quantity: 2200,
    },
    {
        id: "12d3c310-79e9-4451-8a3d-1c4bfde1a949",
        name: "Rh",
        type: "positive",
        quantity: 40,
    },
    {
        id: "297397a3-c51c-4a42-b469-0061e64b1a92",
        name: "Rh",
        type: "negative",
        quantity: 0,
    },
];
// *** END of mock data ***

let bloods = $ref([]);

let expandedRows = $ref([]);
const expandAllRows = () => {
    expandedRows = bloods.filter((el) => el.id);
};

const stockInfo = $ref(null);
const toggleStockInfo = (event) => {
    // Toggle info button about stock status
    stockInfo.toggle(event);
};

onBeforeMount(() => {
    /* 
        Calculate each blood category's total quantity and stock status
        Example of a blood cell
        {
            id: str:id
            name: str
            quantity: int
            inStock: bool
            types: [
                name: 'positive' | 'negative'
                bloodType: str
                status: 'low' | 'out' | 'good' | 'great'
                displayStatus: str
            ]
        }
    */

    BLOOD_TYPES.forEach((name) => {
        const data = bloodData.filter((el) => el.name === name);

        // Evaluate stock status of blood type positive
        const positiveData = data.find((el) => el.type === "positive");
        const {
            status: positiveStockStatus,
            displayStatus: positiveStatusDisplay,
        } = determineStockStatus(positiveData.quantity);

        // Evaluate stock status of blood type negative
        const negativeData = data.find((el) => el.type === "negative");
        const {
            status: negativeStockStatus,
            displayStatus: negativeStatusDisplay,
        } = determineStockStatus(negativeData.quantity);

        bloods.push({
            id: positiveData.id,
            name: positiveData.name,
            quantity: positiveData.quantity + negativeData.quantity,
            inStock:
                !["out", "low"].includes(positiveStockStatus) &&
                !["out", "low"].includes(negativeStatusDisplay),
            types: [
                {
                    name: "positive",
                    bloodType: positiveData.name,
                    quantity: positiveData.quantity,
                    status: positiveStockStatus,
                    displayStatus: positiveStatusDisplay,
                },
                {
                    name: "negative",
                    bloodType: negativeData.name,
                    quantity: negativeData.quantity,
                    status: negativeStockStatus,
                    displayStatus: negativeStatusDisplay,
                },
            ],
        });
    });
});

const downloadExcelFile = () => {
    const excelData = bloodData.map((el) => {
        const row = { ...el };

        row["Blood Group"] = row["name"];
        row["Blood Type"] = row["type"];
        row["Quantity (ml)"] = row["quantity"];

        delete row["id"];
        delete row["name"];
        delete row["type"];
        delete row["quantity"];

        return row;
    });

    JSONtoExcel(excelData, "blood_data");
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
                    :value="bloods"
                    dataKey="id"
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
                                />
                                <PrimeVueButton
                                    icon="pi pi-minus"
                                    label="Collapse All"
                                    class="mb-2 mr-2"
                                    @click="expandedRows = null"
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
                        No data found. Probably a server problem.
                    </template>

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
