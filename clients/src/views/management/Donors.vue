<script setup>
import { onBeforeMount } from "vue";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import MultiSelect from "primevue/multiselect";
import DropDown from "primevue/dropdown";
import { FilterMatchMode } from "primevue/api";

import { BLOOD_TYPES } from "../../constants";

const donors = [
    {
        _id: "3dbf5ab8-4a39-43d9-8fda-2e1b4de09ca2",
        name: "Quoc Bao 1",
        phone: "0945127866",
        address: "1st street, Can Tho city, VietNam",
        gender: "male",
        email: "baobao@gmail.com",
        dob: new Date("05/11/2000").getTime().toString(),
        blood: {
            name: "A",
            type: "Negative",
        },
    },
];

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
onBeforeMount(() => {
    initFilter();
});
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <!-- Page headers -->
                <h2>Donors Management</h2>

                <!-- Donors table -->
                <PrimeVueTable
                    :value="donors"
                    :paginator="true"
                    class="p-datatable-gridlines"
                    :rows="5"
                    dataKey="id"
                    :rowHover="true"
                    removableSort
                    filterDisplay="row"
                    v-model:filters="filters"
                    :filters="filters"
                    responsiveLayout="scroll"
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
                                :class="'blood-badge type-' + data.blood.name"
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
