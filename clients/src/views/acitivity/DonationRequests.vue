<script setup>
import { onBeforeMount } from "vue";
import FileUpload from "primevue/fileupload";
import Calendar from "primevue/calendar";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import MultiSelect from "primevue/multiselect";
import DropDown from "primevue/dropdown";
import Dialog from "primevue/dialog";
import Toast from "primevue/toast";
import { useToast } from "primevue/usetoast";
import { FilterMatchMode } from "primevue/api";

import { BLOOD_TYPES } from "../../constants";
import { JSONtoExcel, excelToJson, formatDate } from "../../utils";

// *** Mock data ***
const donorsData = [
    {
        _id: "800000100001",
        name: "Quoc Bao 1",
        transaction: {
            _id: "911fdf65-2913-4705-8df1-7cba4a0a9355",
            blood: {
                name: "O",
                type: "Negative",
            },
            _event: {
                _id: "1440f35b-0db5-484b-9370-872cb3c7f519",
                name: "event",
            },
            amount: 500,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000001",
        name: "Quoc Bao",
        transaction: {
            _id: "6f769818-5060-435e-835b-ab7ab3cfdaec",
            blood: {
                name: "A",
                type: "Positive",
            },
            _event: {
                _id: "de169f18-226d-48e0-9579-b18184e2c260",
                name: "event1",
            },
            amount: 420,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000002",
        name: "Quoc Bao",
        transaction: {
            _id: "6f05348f-5e55-408f-9c70-dddc105e8c7a",
            blood: {
                name: "A",
                type: "Negative",
            },
            _event: {
                _id: "7cae7784-7523-47ae-b1a4-42308f8fb348",
                name: "event",
            },
            amount: 421,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000003",
        name: "Quoc Bao",
        transaction: {
            _id: "f1a2b6d8-cd3b-41ce-b313-d2388e6ed898",
            blood: {
                name: "A",
                type: "Positive",
            },
            _event: {
                _id: "c31675bc-09c6-40d8-8a1f-6a0bc86e5def",
                name: "event1",
            },
            amount: 422,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000004",
        name: "Quoc Bao",
        transaction: {
            _id: "1bbd4313-efd5-4624-b569-f0b55b656171",
            blood: {
                name: "AB",
                type: "Positive",
            },
            _event: {
                _id: "6957e9d6-2309-45ed-879e-684c87d9fe43",
                name: "event2",
            },
            amount: 423,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000005",
        name: "Quoc Bao",
        transaction: {
            _id: "80ee995b-d0ac-471c-97ab-9e1223264051",
            blood: {
                name: "B",
                type: "Positive",
            },
            _event: {
                _id: "30ea460a-62c4-4fc0-8a7b-450f63c65af3",
                name: "event2",
            },
            amount: 424,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000006",
        name: "Quoc Bao",
        transaction: {
            _id: "97733c03-99cf-4a7b-a76b-114380d5ee0f",
            blood: {
                name: "AB",
                type: "Negative",
            },
            _event: {
                _id: "371ceacf-0470-4874-ac18-d84330baa688",
                name: "event",
            },
            amount: 425,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
    {
        _id: "800000000007",
        name: "Quoc Bao",
        transaction: {
            _id: "da3c90a4-ac58-4200-976b-19cd6e61655a",
            blood: {
                name: "B",
                type: "Negative",
            },
            _event: {
                _id: "dbb32e45-6ad3-443b-892d-769362b551f5",
                name: "event2",
            },
            amount: 426,
            dateDonated: new Date("2022-09-13").getTime(),
        },
    },
];
const events = $computed(() => {
    const allEvents = donorsData.map((don) => don.transaction._event.name);
    return [...new Set(allEvents)];
});
// *** END of mock data ***

let donors = $ref(null);
const toast = useToast();

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

// Approve, reject requests
let showApproveDiaglog = $ref(false);
let showRejectDiaglog = $ref(false);
let selectedRequestPayload = $ref({
    id: null,
    approve: null,
    rejectReason: null,
});

const openDiaglog = (donorId, approve = false) => {
    selectedRequestPayload.id = donorId;
    selectedRequestPayload.approve = approve;

    if (approve) {
        showApproveDiaglog = true;
    } else {
        showRejectDiaglog = true;
    }
};

const closeDialogs = () => {
    showApproveDiaglog = false;
    showRejectDiaglog = false;
    selectedRequestPayload = {
        id: null,
        approve: null,
        rejectReason: null,
    };
};

const requestActions = () => {
    const { id, rejectReason } = selectedRequestPayload;

    const donor = donors.find((el) => el._id === id);

    toast.add({
        severity: selectedRequestPayload.approve ? "info" : "error",
        summary: selectedRequestPayload.approve
            ? `Blood Donation Successfully`
            : `Blood Donation Rejected`,
        detail: selectedRequestPayload.approve
            ? `Blood donation from donor ${donor.name} approved!`
            : `Blood donation from donor ${donor.name} rejected because ${rejectReason}`,
        life: 20000,
    });
    closeDialogs();
};

onBeforeMount(() => {
    // Convert int to date
    donors = donorsData.map((row) => {
        let donor = { ...row };
        donor.transaction.dateDonated = new Date(donor.transaction.dateDonated);
        return donor;
    });
    initFilter();
});

// Uploaded excel
const onSelectExcel = (event) => {
    const excelFile = event.files[0];

    excelToJson(excelFile);
};

const downloadExcelFile = () => {
    // Format data before convert to excel
    // const excelData = donorsData.map((el) => {
    //     let row = { ...el };
    //     row["Donor's Name"] = row["name"];
    //     row["Blood Type"] = row["bloodType"];
    //     row["Date Donated (dd/mm/yyyy)"] = formatDate(row["date"]);
    //     row["Amount (ml)"] = row["amount"];
    //     row["Event Name"] = row["event"];
    //     delete row["id"];
    //     delete row["name"];
    //     delete row["bloodType"];
    //     delete row["name"];
    //     delete row["date"];
    //     delete row["amount"];
    //     delete row["event"];
    //     return row;
    // });
    // JSONtoExcel(excelData, "donor_request_data");
};
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <!-- Page headers -->
                <h2>Donation Requests Monitor</h2>

                <!-- Donors table -->
                <PrimeVueTable
                    :value="donors"
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
                        <div
                            class="flex justify-content-between flex-column sm:flex-row"
                        >
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
                                    @click="downloadExcelFile"
                                    class="p-button-outlined mb-2 mr-2"
                                />

                                <FileUpload
                                    mode="basic"
                                    @select="onSelectExcel($event)"
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
                                v-tooltip.top.focus="
                                    'Press enter key to filter'
                                "
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
                        style="min-width: 300px; max-width: 14rem"
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
                                :class="
                                    'blood-badge type-' +
                                    data.transaction.blood.name
                                "
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
                        style="min-width: 200px; max-width: 10rem"
                    >
                        <template #body="{ data }">
                            {{ data.transaction.amount }} ml
                        </template>
                        <template #filter="{ filterModel, filterCallback }">
                            <InputNumber
                                v-model="filterModel.value"
                                @keydown.enter="filterCallback()"
                                class="p-column-filter"
                                v-tooltip.top.focus="
                                    'Press enter key to filter'
                                "
                            />
                        </template>
                    </PrimeVueColumn>
                </PrimeVueTable>
            </div>
        </div>

        <!-- Approve dialog -->
        <Dialog
            header="Approve this request?"
            v-model:visible="showApproveDiaglog"
            :breakpoints="{ '960px': '75vw' }"
            :style="{ width: '50vw' }"
        >
            <p>Approve this request?</p>

            <!-- Footer - Action buttons -->
            <template #footer>
                <PrimeVueButton
                    label="No"
                    icon="pi pi-times"
                    @click="showApproveDiaglog = false"
                    class="p-button-text"
                />
                <PrimeVueButton
                    label="Yes"
                    icon="pi pi-check"
                    @click="requestActions"
                />
            </template>
        </Dialog>

        <!-- Reject dialog -->
        <Dialog
            header="Reject this request?"
            v-model:visible="showRejectDiaglog"
            :breakpoints="{ '960px': '75vw' }"
            :style="{ width: '50vw' }"
        >
            <InputText
                type="text"
                class="p-column-filter"
                style="width: 100%"
                :placeholder="`Why do you reject this request?`"
                v-model="selectedRequestPayload.rejectReason"
            />

            <!-- Footer - Action buttons -->
            <template #footer>
                <PrimeVueButton
                    label="No"
                    icon="pi pi-times"
                    @click="showRejectDiaglog = false"
                    class="p-button-text"
                />
                <PrimeVueButton
                    label="Yes"
                    icon="pi pi-check"
                    @click="requestActions"
                />
            </template>
        </Dialog>

        <!-- Toast -->
        <Toast position="bottom-right" />
    </div>
</template>

<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";

.approve-btn {
    border: none !important;
    background: #00c897 !important;
}

.reject-btn {
    border: none !important;
    background: #ff6363 !important;
}
</style>
