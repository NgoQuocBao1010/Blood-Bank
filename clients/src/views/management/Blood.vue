<script setup>
import { onBeforeMount } from "vue";
import Datatable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";
import OverlayPanel from "primevue/overlaypanel";

let bloods = [
    {
        id: "f665977b-5a21-4f7b-8875-65cf341c1e58",
        name: "A",
        types: [
            { name: "negative", quantity: 2000 },
            { name: "positive", quantity: 700 },
        ],
    },
    {
        id: "f77dec5e-1b4d-4d8e-9db8-82c50219bfc8",
        name: "B",
        types: [
            { name: "negative", quantity: 1500 },
            { name: "positive", quantity: 1700 },
        ],
    },
    {
        id: "ea82a6b0-85f0-4701-a9f6-d8f0de14c7c8",
        name: "AB",
        types: [
            { name: "negative", quantity: 1600 },
            { name: "positive", quantity: 1000 },
        ],
    },
    {
        id: "1b544f9f-30ba-4a6a-97e1-b66a429da14e",
        name: "O",
        types: [
            { name: "negative", quantity: 2200 },
            { name: "positive", quantity: 1700 },
        ],
    },
    {
        id: "d25b5c52-f51b-4f31-b948-23a798d8d45e",
        name: "Rh",
        types: [
            { name: "negative", quantity: 0 },
            { name: "positive", quantity: 40 },
        ],
    },
];
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
    // Calculate each blood category's total quantity and stock status
    bloods = bloods.map((bloodType) => {
        const positive = bloodType.types.find((el) => el.name === "positive");
        const negative = bloodType.types.find((el) => el.name === "negative");
        const totalQuantity = positive.quantity + negative.quantity;

        Object.assign(positive, {
            ...determineStockStatus(positive.quantity),
            bloodType: bloodType.name,
        });
        Object.assign(negative, {
            ...determineStockStatus(negative.quantity),
            bloodType: bloodType.name,
        });

        bloodType["inStock"] =
            !["out", "low"].includes(positive["status"]) &&
            !["out", "low"].includes(negative["status"]);
        bloodType["quantity"] = totalQuantity;
        bloodType["types"] = [positive, negative];

        return bloodType;
    });
});

const determineStockStatus = (value) => {
    if (value === 0) return { status: "out", displayStatus: "out of stock" };

    if (value <= 400) return { status: "low", displayStatus: "low in stock" };

    if (value <= 700) return { status: "good", displayStatus: "good in stock" };

    if (value > 700)
        return { status: "great", displayStatus: "great in stock" };
};
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <!-- Page headers -->
                <h2>Blood Management</h2>

                <!-- Blood Table -->
                <Datatable
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
                                <Button
                                    icon="pi pi-plus"
                                    label="Expand All"
                                    class="mr-2 mb-2"
                                    @click="expandAllRows()"
                                />
                                <Button
                                    icon="pi pi-minus"
                                    label="Collapse All"
                                    class="mb-2 mr-2"
                                    @click="expandedRows = null"
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
                    <Column :expander="true" headerStyle="width: 3rem" />

                    <!-- Blood Type Name -->
                    <Column
                        field="name"
                        header="Name"
                        style="min-width: 8rem !important"
                    >
                        <template #body="slotProps">
                            {{ slotProps.data.name }}
                        </template>
                    </Column>

                    <!-- Total Quantity Column -->
                    <Column
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
                    </Column>

                    <!-- Stock status Column -->
                    <Column
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
                    </Column>

                    <!-- Expand Rows to a Datatable -->
                    <template #expansion="slotProps">
                        <div class="p-3">
                            <!-- Row Expand Table Headers -->
                            <p>Blood {{ slotProps.data.name }} details</p>

                            <!-- Expand Table -->
                            <Datatable
                                :value="slotProps.data.types"
                                responsiveLayout="scroll"
                            >
                                <!-- Type -->
                                <Column field="name" header="Type">
                                    <template #body="slotProps">
                                        <span
                                            style="text-transform: capitalize"
                                        >
                                            {{ slotProps.data.bloodType }}
                                            {{ slotProps.data.name }}
                                        </span>
                                    </template>
                                </Column>

                                <!-- Quantity -->
                                <Column
                                    field="quantity"
                                    header="Quantity"
                                    :sortable="true"
                                >
                                    <template #body="slotProps">
                                        <p style="text-align: center">
                                            {{ slotProps.data.quantity }} ml
                                        </p>
                                    </template>
                                </Column>

                                <!-- Stock Status -->
                                <Column
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
                                </Column>
                            </Datatable>
                        </div>
                    </template>
                </Datatable>
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
