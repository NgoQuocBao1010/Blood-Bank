<script setup>
import { inject, onBeforeMount, nextTick } from "vue";
import FileUpload from "primevue/fileupload";
import Calendar from "primevue/calendar";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import MultiSelect from "primevue/multiselect";
import DropDown from "primevue/dropdown";
import Dialog from "primevue/dialog";
import { useToast } from "primevue/usetoast";
import { FilterMatchMode } from "primevue/api";

import { useEventStore } from "../../stores/event";
import DonorsHelpers from "../../utils/helpers/Donors";
import { BLOOD_TYPES } from "../../constants";
import { formatDate } from "../../utils";
import { JSONtoExcel, excelToJson } from "../../utils/excel";
import DonorRepo from "../../api/DonorRepo";
import DonorTransactionRepo from "../../api/DonorTransaction";

const { donorsData, participants, isApproveParticipant, isRejectParticipant } =
    defineProps({
        donorsData: {
            type: Array,
            required: true,
        },
        participants: {
            type: Boolean,
            default: false,
        },
        isRejectParticipant: {
            type: Boolean,
            default: false,
        },
        isApproveParticipant: {
            type: Boolean,
            default: false,
        },
    });

let donors = $ref(null);
let selectedParticipants = $ref([]);
const eventStore = useEventStore();
const emits = defineEmits(["updateParticipants"]);
const { showErrorDialog } = inject("errorDialog");

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
        "transaction.eventDonated.name": {
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
        "transaction.rejectReason": {
            value: null,
            matchMode: FilterMatchMode.CONTAINS,
        },
    };
};
const clearFilter = () => {
    initFilter();
};

onBeforeMount(async () => {
    initFilter();

    donors = JSON.parse(JSON.stringify(donorsData));
    donors = donors.map((row) => {
        let donor = { ...row };

        donor["dataKey"] = donor._id + donor.transaction.eventDonated._id;
        donor.transaction.dateDonated = new Date(
            parseInt(donor.transaction.dateDonated)
        );
        return donor;
    });

    if (!eventStore.events) await eventStore.setEvents();
});

// Excel import and export
const toast = useToast();
const downloadExcel = () => {
    if (donorsData.length === 0) {
        toast.add({
            severity: "error",
            summary: "No information",
            detail: "There is no donors data found",
            life: 3000,
        });
        return;
    }
    const excelData = DonorsHelpers.transformRowsForExcelDownload(donorsData);

    let fileName = "";
    if (isApproveParticipant) {
        fileName = "Approved_Donors";
    } else if (isRejectParticipant) {
        fileName = "Rejected_Donors";
    } else {
        fileName = "Pending_Donors";
    }

    JSONtoExcel(excelData, participants ? fileName : "Event_Participants");
};

let newParticipants = $ref({
    eventId: null,
    listParticipants: null,
    files: null,
});
let isRerender = $ref(false);
let selectEventsDialog = $ref(false);
const onSelectExcel = async (event) => {
    newParticipants.files = event.files[0];
    selectEventsDialog = true;

    // Rerender PrimeVue FileUpload Components to remove file name
    isRerender = true;
    await nextTick();
    isRerender = false;
};
const importExcel = async () => {
    if (!newParticipants.eventId) {
        toast.add({
            severity: "error",
            summary: "No Event Information",
            detail: "Please pick the events for these donors",
            life: 3000,
        });

        return;
    }

    const excelData = await excelToJson(newParticipants.files);
    newParticipants.listParticipants =
        DonorsHelpers.reformAfterExcelImport(excelData);
    selectEventsDialog = false;

    try {
        const { data, status } = await DonorRepo.importParticipants(
            newParticipants
        );

        if (data && status === 200) {
            toast.add({
                severity: "success",
                summary: "New Participants",
                detail: `New Participants for ${
                    eventStore.getEventById(newParticipants.eventId)?.name
                } has been added`,
                life: 3000,
            });

            emits("updateParticipants");
        }
    } catch (e) {
        if (e.response && e.response.status === 400) {
            showErrorDialog(
                "Excel has invalid data",
                "There are some participants may already record their data on this event. Please check your data again."
            );
            return;
        }

        throw e;
    }
};

// Approve reject
let isApprove = $ref(null);
let rejectReason = $ref("");
let showConfirmDialog = $ref(false);
const openConfirmDialog = (approve = true) => {
    isApprove = approve;
    showConfirmDialog = true;
};
const handleParticipants = async () => {
    // Show error if there is no rejected reason on rejected
    if (!isApprove && !rejectReason) {
        toast.add({
            severity: "error",
            summary: "Invalid Reject Reason",
            detail: "Please provide a reason for rejecting these participants",
            life: 4000,
        });

        return;
    }

    // Transform data for API calls
    const participants = JSON.parse(JSON.stringify(selectedParticipants)).map(
        (row) => {
            let transformData = {};

            transformData["_id"] = row._id;
            transformData["eventId"] = row.transaction.eventDonated._id;
            transformData["rejectReason"] = rejectReason;
            return transformData;
        }
    );

    const { data, status } = isApprove
        ? await DonorTransactionRepo.approveParticipants(participants)
        : await DonorTransactionRepo.rejectParticipants(participants);

    if (data && status === 200) {
        const action = isApprove ? "approved" : "rejected";
        const toastType = isApprove ? "success" : "warn";

        toast.add({
            severity: toastType,
            summary: isApprove ? "Approved" : "Rejected",
            detail: `${selectedParticipants.length} ${
                selectedParticipants.length > 1
                    ? "participants are"
                    : "participant is"
            } ${action}`,
            life: 3000,
        });
        emits("updateParticipants");
    }
};
</script>

<template>
    <PrimeVueTable
        :value="donors"
        :paginator="true"
        class="p-datatable-gridlines"
        :rows="5"
        dataKey="dataKey"
        :rowHover="true"
        removableSort
        filterDisplay="row"
        v-model:selection="selectedParticipants"
        v-model:filters="filters"
        :filters="filters"
        responsiveLayout="scroll"
        :globalFilterFields="[
            'name',
            '_id',
            'transaction.dateDonated',
            'transaction.eventDonated.name',
            'transaction.blood.name',
            'transaction.blood.type',
            'transaction.amount',
            'transaction.rejectReason',
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
                        @click="downloadExcel"
                        class="p-button-outlined mb-2 mr-2"
                    />

                    <FileUpload
                        mode="basic"
                        @select="onSelectExcel($event)"
                        name="requestFiles"
                        choose-label="Upload Excel File"
                        :showUploadButton="false"
                        accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
                        v-if="participants && !isRerender"
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
            <h4 style="text-align: center">No participants found.</h4>
        </template>

        <!-- Columns -->

        <!-- Selection column -->
        <PrimeVueColumn
            selectionMode="multiple"
            headerStyle="width: 2rem"
            v-if="participants"
        ></PrimeVueColumn>

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

        <!-- Reject Reason -->
        <PrimeVueColumn
            field="transaction.rejectReason"
            header="Reject Reason"
            style="min-width: 300px"
            v-if="isRejectParticipant"
        >
            <template #body="{ data }">
                {{ data.transaction.rejectReason }}
            </template>
            <template #filter="{ filterModel, filterCallback }">
                <InputText
                    type="text"
                    v-model="filterModel.value"
                    @keydown.enter="filterCallback()"
                    class="p-column-filter"
                    :placeholder="`Search keyword`"
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
            style="min-width: 250px; width: 14rem !important"
            v-if="participants"
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
            field="transaction.eventDonated.name"
            header="Event"
            style="min-width: 250px; max-width: 12rem"
            v-if="participants"
        >
            <template #body="{ data }">
                {{ data.transaction.eventDonated.name }}
            </template>
            <template #filter="{ filterModel, filterCallback }">
                <DropDown
                    v-model="filterModel.value"
                    @change="filterCallback"
                    :options="eventStore.names"
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
        <template #footer v-if="selectedParticipants.length > 0">
            <PrimeVueButton
                type="button"
                icon="pi pi-check-circle"
                label="Approve"
                class="p-button p-button-sm mr-2 approve-btn"
                @click="openConfirmDialog"
                v-if="!isApproveParticipant"
            />

            <PrimeVueButton
                type="button"
                icon="pi pi-times-circle"
                label="Reject"
                class="p-button p-button-sm reject-btn"
                @click="openConfirmDialog(false)"
                v-if="!isRejectParticipant"
            />
        </template>
    </PrimeVueTable>

    <!-- Event for import excel dialog -->
    <Dialog
        header="Which event that these participants from?"
        v-model:visible="selectEventsDialog"
        :style="{ width: '50vw' }"
        :modal="true"
    >
        <DropDown
            v-model="newParticipants.eventId"
            :options="eventStore.activeEvents"
            optionValue="_id"
            class="p-column-filter"
            placeholder="Choose event"
            style="width: 100%"
            :showClear="true"
        >
            <template #value="slotProps">
                <span v-if="slotProps.value">
                    {{ eventStore.getEventById(slotProps.value)?.name }}
                </span>
                <span v-else>
                    {{ slotProps.placeholder }}
                </span>
            </template>
            <template #option="slotProps">
                <span>{{ slotProps.option.name }}</span>
            </template>
        </DropDown>

        <template #footer>
            <PrimeVueButton
                label="Import"
                icon="pi pi-plus-circle"
                @click="importExcel"
            />
            <PrimeVueButton
                label="Close"
                icon="pi pi-times"
                @click="selectEventsDialog = false"
            />
        </template>
    </Dialog>

    <!-- Approve Reject dialog -->
    <Dialog
        :header="isApprove ? 'Approving participants' : 'Reject participants'"
        v-model:visible="showConfirmDialog"
        :style="{ width: '50vw' }"
        position="bottom"
        :modal="true"
    >
        <p class="m-0" v-if="isApprove">
            You are approving
            <span class="app-highlight">
                {{ selectedParticipants.length }} participants
            </span>
            . Are you sure to proceed?
        </p>
        <template v-else>
            <p class="m-0">
                You are rejecting
                <span class="app-highlight">
                    {{ selectedParticipants.length }} participants
                </span>
                . Please provide a reason below.
            </p>
            <InputText
                class="reject-input"
                placeholder="Type in the reject reason"
                v-model="rejectReason"
            />
        </template>

        <template #footer>
            <PrimeVueButton
                label="Cancel"
                icon="pi pi-times"
                @click="showConfirmDialog = false"
                class="p-button-text"
            />
            <PrimeVueButton
                label="Proceed"
                icon="pi pi-check"
                @click="handleParticipants"
            />
        </template>
    </Dialog>
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

.reject-input {
    margin-top: 1rem;
    width: 80%;
}
</style>
