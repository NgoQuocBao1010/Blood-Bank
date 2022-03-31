<script setup>
import { onBeforeMount } from "vue";
import Calendar from "primevue/calendar";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import MultiSelect from "primevue/multiselect";
import DropDown from "primevue/dropdown";
import { FilterMatchMode } from "primevue/api";

import { BLOOD_TYPES } from "../../constants";

import { useRoute } from "vue-router";
import { useToast } from "primevue/usetoast";
import RequestsHelpers from "../../utils/helpers/Requests";
import { JSONtoExcel } from "../../utils/excel";
import { formatDate } from "../../utils/index";

import { TRANSACTION_STATUS } from "../../constants";
import DonorTransactionHelper from "../../utils/helpers/DonorTransaction";

const route = useRoute();
const hospital_id = route.params._id;

const { requestHistory, isAcitivy } = defineProps({
  requestHistory: {
    type: Array,
    required: true,
  },
  isAcitivy: {
    type: Boolean,
    default: false,
  },
});

let requests = $ref(null);
let selectedRequests = $ref([]);
// let fetchingData = $ref(true);

// Filter configurations
let filters = $ref(null);
const initFilter = () => {
  filters = {
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    hospitalName: { value: null, matchMode: FilterMatchMode.CONTAINS },
    date: {
      value: null,
      matchMode: FilterMatchMode.DATE_IS,
    },
    "blood.name": {
      value: null,
      matchMode: FilterMatchMode.IN,
    },
    "blood.type": {
      value: null,
      matchMode: FilterMatchMode.EQUALS,
    },
    quantity: {
      value: null,
      matchMode: FilterMatchMode.GREATER_THAN_OR_EQUAL_TO,
    },
    approveStatus: { value: null, matchMode: FilterMatchMode.IN },
  };
};
const clearFilter = () => {
  initFilter();
};

let excelData;

onBeforeMount(() => {
  let requestData;

  //   Check if not in activity, show only request hitory belong to  hospital
  if (isAcitivy === false) {
    if (requestHistory && requestHistory.length != 0) {
      requestData = requestHistory.filter(
        (request) => request.hospitalId === hospital_id
      );
      excelData = RequestsHelpers.transformRowsBeforeExcel(requestData);
    }
  } else {
    requestData = requestHistory;
    excelData = RequestsHelpers.transformRowsBeforeExcel(requestData);
  }

  if (requestData) {
    requests = requestData.map((row) => {
      let request = { ...row };
      request.date = new Date(request.date * 1000);
      request.status = DonorTransactionHelper.determineStatus(request);
      return request;
    });
  }
  initFilter();
});

// Excel import and export
const toast = useToast();
const downloadExcel = () => {
  if (requestHistory.length === 0) {
    toast.add({
      severity: "error",
      summary: "No information",
      detail: "There is no requests data found",
      life: 3000,
    });
    return;
  }

  JSONtoExcel(excelData, "Pending_Requests");
};
</script>

<template>
  <PrimeVueTable
    :value="requests"
    :paginator="true"
    class="p-datatable-gridlines"
    :rows="5"
    dataKey="_id"
    :rowHover="true"
    removableSort
    filterDisplay="row"
    v-model:selection="selectedRequests"
    v-model:filters="filters"
    :filters="filters"
    responsiveLayout="scroll"
    :globalFilterFields="[
      'hospitalName',
      'date',
      'blood.name',
      'blood.type',
      'quantity',
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
            @click="downloadExcel"
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
    <template #empty> No requests found. </template>

    <!-- Columns -->

    <!-- Selection column -->
    <PrimeVueColumn
      selectionMode="multiple"
      headerStyle="width: 2rem"
      v-if="isAcitivy"
    ></PrimeVueColumn>

    <!-- Hospital name -->
    <PrimeVueColumn
      field="hospitalName"
      header="Hospital Name"
      style="min-width: 250px; max-width: 12rem"
      v-if="isAcitivy"
    >
      <template #body="{ data }">
        {{ data && data.hospitalName }}
      </template>
      <template #filter="{ filterModel, filterCallback }">
        <InputText
          type="text"
          v-model="filterModel.value"
          @keydown.enter="filterCallback()"
          class="p-column-filter"
          :placeholder="`Search by hospital's name`"
          v-tooltip.top.focus="'Press enter key to filter'"
        />
      </template>
    </PrimeVueColumn>

    <!-- Date requested -->
    <PrimeVueColumn
      header="Date Requested"
      field="date"
      dataType="date"
      :sortable="true"
      style="min-width: 200px; width: 14rem !important"
    >
      <template #body="{ data }">
        {{ formatDate(data.date) }}
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

    <!-- Blood Name -->
    <PrimeVueColumn field="blood.name" header="Blood Name" style="width: 8rem">
      <template #body="{ data }">
        <span :class="'blood-badge type-' + data.blood.name">
          Type {{ data.blood.name }}
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
      field="blood.type"
      header="Blood Type"
      style="min-width: 200px"
    >
      <template #body="{ data }">
        {{ data.blood.type }}
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

    <!-- quantity -->
    <PrimeVueColumn
      field="quantity"
      header="quantity (ml)"
      dataType="numeric"
      :sortable="true"
      :showFilterMatchModes="false"
      style="min-width: 180px; max-width: 10rem"
    >
      <template #body="{ data }"> {{ data.quantity }} ml </template>
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
      field="approveStatus"
      header="Status"
      style="max-width: 14rem !important"
    >
      <template #body="{ data }">
        <span
          :class="'transaction-badge status-' + data.approveStatus"
          style="cursor: pointer"
          v-tooltip.bottom="{
            value: `Failed Reason: ${data.rejectReason}`,
            class: 'reason-tooltip',
          }"
          v-if="data.approveStatus === 'failed'"
        >
          {{ data.approveStatus }}
        </span>
        <span :class="'transaction-badge status-' + data.approveStatus" v-else>
          {{ data.approveStatus }}
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
            <span :class="'transaction-badge status-' + slotProps.option">
              {{ slotProps.option }}
            </span>
          </template>
        </MultiSelect>
      </template>
    </PrimeVueColumn>

    <!-- Table's footer -->
    <template #footer v-if="selectedRequests.length > 0">
      <PrimeVueButton
        type="button"
        icon="pi pi-check-circle"
        label="Approve"
        class="p-button p-button-sm mr-2 approve-btn"
      />

      <PrimeVueButton
        type="button"
        icon="pi pi-times-circle"
        label="Reject"
        class="p-button p-button-sm reject-btn"
      />
    </template>
  </PrimeVueTable>
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
