<script setup>
import { onBeforeMount } from "vue";
import { useRouter, RouterLink } from "vue-router";
import InputText from "primevue/inputtext";
import { FilterMatchMode } from "primevue/api";

import HospitalRepo from "../../api/HospitalRepo";

const router = useRouter();

let hospitals = $ref([]);
let fetchingData = $ref(true);

// Filter configurations
let filters = $ref(null);
const initFilters = () => {
  filters = {
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    name: { value: null, matchMode: FilterMatchMode.CONTAINS },
    address: { value: null, matchMode: FilterMatchMode.CONTAINS },
    phone: { value: null, matchMode: FilterMatchMode.CONTAINS },
  };
};
const clearFilter = () => {
  initFilters();
};

onBeforeMount(async () => {
  const data = await HospitalRepo.getAll();
  fetchingData = false;
  hospitals = data.data;

  // console.log("hospital data: ", hospitals);

  // hospitals && hospitals.length !== 0 && initFilters();
  initFilters();
});

const onRowClick = (payload) => {
  // Go to event detail when click a row in the table
  const { _id: hospitalId } = payload.data;
  router.push({ name: "Hospital Detail", params: { _id: hospitalId } });
};
</script>

<template>
  <div class="grid">
    <div class="col-12">
      <div class="card">
        <!-- Page header -->
        <div
          class="flex justify-content-between align-content-center"
          style="width: 100%"
        >
          <h2>Hospital Management</h2>
          <p class="app-note">
            * Left click to any row to see more information about the hospital *
          </p>
        </div>

        <!-- Hospital table -->
        <PrimeVueTable
          :value="hospitals"
          :loading="fetchingData"
          data-key="_id"
          class="p-datatable-gridlines"
          :rows="5"
          :row-hover="true"
          rowStyle="cursor: pointer"
          @row-click="onRowClick"
          responsive-layout="scroll"
          :paginator="true"
          removable-sort
          filterDisplay="row"
          v-model:filters="filters"
          :filters="filters"
          :globalFilterFields="['name', 'address', 'phone']"
        >
          <!-- Header of the table -->
          <template #header>
            <div class="flex justify-content-between flex-column sm:flex-row">
              <div>
                <PrimeVueButton
                  type="button"
                  icon="pi pi-filter-slash"
                  label="Clear"
                  class="p-button-outlined mb-2 mr-2"
                  @click="clearFilter"
                />

                <RouterLink
                  :to="{ name: 'Hospital Create' }"
                  v-ripple
                  class="p-button p-component mb-2 p-ripple app-router-link-icon"
                >
                  <i class="fa-solid fa-circle-plus"></i>
                  New Hospitals
                </RouterLink>
              </div>

              <!-- Search Input -->
              <span class="p-input-icon-left mb-2">
                <i class="pi pi-search" />
                <InputText
                  placeholder="Keyword Search"
                  style="width: 100%"
                  v-if="!fetchingData"
                  v-model="filters['global'].value"
                />
              </span>
            </div>
          </template>

          <!-- Empty data fallback -->
          <template #empty> No events found. </template>

          <!-- Columns -->
          <!-- Hospital's name -->
          <PrimeVueColumn field="name" header="Name" style="min-width: 20rem">
            <template #body="{ data }">
              {{ data.name }}
            </template>
            <template #filter="{ filterModel, filterCallback }">
              <InputText
                type="text"
                v-if="!fetchingData"
                v-model="filterModel.value"
                @keydown.enter="filterCallback()"
                class="p-column-filter"
                :placeholder="`Search by name`"
                v-tooltip.top.focus="'Press enter key to filter'"
              />
            </template>
          </PrimeVueColumn>

          <!-- Address -->
          <PrimeVueColumn
            field="address"
            header="Address"
            style="max-width: 18rem"
          >
            <template #body="{ data }">
              {{ data.address }}
            </template>
            <template #filter="{ filterModel, filterCallback }">
              <InputText
                type="text"
                v-if="!fetchingData"
                v-model="filterModel.value"
                @keydown.enter="filterCallback()"
                class="p-column-filter"
                :placeholder="`Search by address`"
                v-tooltip.top.focus="'Press enter key to filter'"
              />
            </template>
          </PrimeVueColumn>

          <!-- Phone Number -->
          <PrimeVueColumn
            field="phone"
            header="Phone Number"
            style="max-width: 6rem"
          >
            <template #body="{ data }">
              {{ data.phone }}
            </template>
            <template #filter="{ filterModel, filterCallback }">
              <InputText
                type="text"
                v-if="!fetchingData"
                v-model="filterModel.value"
                @keydown.enter="filterCallback()"
                class="p-column-filter"
                :placeholder="`Search by phone number`"
                v-tooltip.top.focus="'Press enter key to filter'"
              />
            </template>
          </PrimeVueColumn>
        </PrimeVueTable>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";
</style>
