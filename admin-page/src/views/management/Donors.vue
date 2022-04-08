<script setup>
import { onBeforeMount } from "vue";
import { useRouter } from "vue-router";
import InputText from "primevue/inputtext";
import MultiSelect from "primevue/multiselect";
import DropDown from "primevue/dropdown";
import { FilterMatchMode } from "primevue/api";

import DonorRepos from "../../api/DonorRepo";
import { BLOOD_TYPES } from "../../constants";

let donors = $ref(null);
let fetchingDonors = $ref(true);
// Filter configuration
let filters = $ref(null);
const initFilter = () => {
    filters = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
        name: { value: null, matchMode: FilterMatchMode.CONTAINS },
        email: { value: null, matchMode: FilterMatchMode.CONTAINS },
        gender: { value: null, matchMode: FilterMatchMode.EQUALS },
        "blood.name": { value: null, matchMode: FilterMatchMode.IN },
        "blood.type": { value: null, matchMode: FilterMatchMode.EQUALS },
    };
};
const clearFilter = () => {
    initFilter();
};

const router = useRouter();
const onRowClick = (payload) => {
    // Go to event detail when click a row in the table
    const { _id: donorId } = payload.data;
    router.push({ name: "Donor Detail", params: { _id: donorId } });
};

onBeforeMount(async () => {
    initFilter();

    const { data } = await DonorRepos.getDonors();
    donors = data;
    fetchingDonors = false;
});
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <!-- Page headers -->
                <div
                    class="flex justify-content-between align-content-center"
                    style="width: 100%"
                >
                    <h2>Donors Management</h2>
                    <p class="app-note">
                        * Left click to any row to see more information about
                        the donor *
                    </p>
                </div>

                <!-- Donors table -->
                <PrimeVueTable
                    :value="donors"
                    :loading="fetchingDonors"
                    dataKey="id"
                    class="p-datatable-gridlines"
                    responsiveLayout="scroll"
                    :rows="5"
                    rowStyle="cursor: pointer"
                    @row-click="onRowClick"
                    :rowHover="true"
                    :paginator="true"
                    removableSort
                    v-model:filters="filters"
                    filterDisplay="row"
                    :filters="filters"
                    :globalFilterFields="[
                        'name',
                        'email',
                        'gender',
                        'blood.name',
                        'blood.type',
                    ]"
                >
                    <!-- Header of the table -->
                    <template #header>
                        <div
                            class="flex justify-content-between flex-column sm:flex-row"
                        >
                            <div>
                                <PrimeVueButton
                                    type="button"
                                    icon="pi pi-filter-slash"
                                    label="Clear"
                                    @click="clearFilter()"
                                    class="p-button-outlined mb-2 mr-2"
                                />
                                <PrimeVueButton
                                    type="button"
                                    icon="pi pi-file-excel"
                                    label="Export to Excel"
                                    class="p-button-outlined mb-2"
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

                    <!-- Loading data fallback -->
                    <template #loading>
                        <h4 style="text-align: center">Loading data ...</h4>
                    </template>

                    <!-- Empty data fallback -->
                    <template #empty>
                        <h4 style="text-align: center">No donors found.</h4>
                    </template>

                    <!-- Columns -->
                    <template v-if="!fetchingDonors">
                        <!-- Name -->
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
                                    v-tooltip.top.focus="
                                        'Press enter key to filter'
                                    "
                                />
                            </template>
                        </PrimeVueColumn>

                        <!-- Email -->
                        <PrimeVueColumn
                            field="email"
                            header="Email"
                            style="min-width: 12rem"
                        >
                            <template #body="{ data }">
                                {{ data.email }}
                            </template>
                            <template #filter="{ filterModel, filterCallback }">
                                <InputText
                                    type="text"
                                    v-model="filterModel.value"
                                    @keydown.enter="filterCallback()"
                                    class="p-column-filter"
                                    v-tooltip.top.focus="
                                        'Press enter key to filter'
                                    "
                                />
                            </template>
                        </PrimeVueColumn>

                        <!-- Gender -->
                        <PrimeVueColumn
                            field="gender"
                            header="Gender"
                            style="min-width: 200px"
                        >
                            <template #body="{ data }">
                                <span style="text-transform: capitalize">
                                    {{ data.gender }}
                                </span>
                            </template>
                            <template #filter="{ filterModel, filterCallback }">
                                <DropDown
                                    v-model="filterModel.value"
                                    @change="filterCallback"
                                    :options="['male', 'female']"
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
                            field="blood.name"
                            header="Blood Name"
                            style="max-width: 14rem !important"
                        >
                            <template #body="{ data }">
                                <span
                                    :class="
                                        'blood-badge type-' + data.blood.name
                                    "
                                >
                                    Type {{ data.blood.name }}
                                </span>
                            </template>
                            <template #filter="{ filterModel, filterCallback }">
                                <MultiSelect
                                    v-model="filterModel.value"
                                    @change="filterCallback()"
                                    :options="BLOOD_TYPES"
                                    optionLabel=""
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
                            field="blood.type"
                            header="Blood Type"
                            style="max-width: 14rem !important"
                        >
                            <template #body="{ data }">
                                {{ data.blood.type }}
                            </template>
                            <template #filter="{ filterModel, filterCallback }">
                                <MultiSelect
                                    v-model="filterModel.value"
                                    @change="filterCallback()"
                                    :options="['Positive', 'Negative']"
                                    optionLabel=""
                                    class="p-column-filter"
                                >
                                    <template #option="slotProps">
                                        <span
                                            :class="
                                                'blood-badge type-' +
                                                slotProps.option
                                            "
                                        >
                                            {{ slotProps.option }}
                                        </span>
                                    </template>
                                </MultiSelect>
                            </template>
                        </PrimeVueColumn>
                    </template>
                </PrimeVueTable>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";
::v-deep(.p-datatable-frozen-tbody) {
    font-weight: bold;
}

::v-deep(.p-datatable-scrollable .p-frozen-column) {
    font-weight: bold;
}
</style>
