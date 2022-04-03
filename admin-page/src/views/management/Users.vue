<script setup>
import { onBeforeMount } from "vue";
import InputText from "primevue/inputtext";
import DropDown from "primevue/dropdown";
import { FilterMatchMode } from "primevue/api";

import UserRepo from "../../api/UserRepo";
import UserCreation from "../../components/form/UserCreation.vue";
import { useUserStore } from "../../stores/user";

let users = $ref(null);
let fetchingData = $ref(true);

// Filter configuration
let filters = $ref(null);
const initFilter = () => {
    filters = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
        email: { value: null, matchMode: FilterMatchMode.CONTAINS },
        role: { value: null, matchMode: FilterMatchMode.EQUALS },
    };
};
const clearFilter = () => {
    initFilter();
};

onBeforeMount(async () => {
    const { data } = await UserRepo.getAll();

    users = data.map((row) => ({
        ...row,
        role: row.isAdmin ? "admin" : "hospital",
    }));

    fetchingData = false;

    initFilter();
});

const getRowClass = (data) => {
    return data.email === useUserStore().email ? "active-user" : "";
};

const deleteUser = (userId) => {
    console.log(userId);
};
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <!-- Page header -->
                <h2>Active Users</h2>

                <!-- User table -->
                <PrimeVueTable
                    :value="users"
                    :loading="fetchingData"
                    dataKey="_id"
                    class="p-datatable-gridlines"
                    responsiveLayout="scroll"
                    :rows="5"
                    :rowHover="true"
                    :rowClass="getRowClass"
                    :paginator="true"
                    removableSort
                    v-model:filters="filters"
                    filterDisplay="row"
                    :filters="filters"
                    :globalFilterFields="['email', 'role']"
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
                            </div>

                            <!-- Search Input -->
                        </div>
                    </template>

                    <!-- Empty data fallback -->
                    <template #empty>
                        <h4 style="text-align: center">No users found</h4>
                    </template>

                    <!-- Loading fallback -->
                    <template #loading>
                        <h4 style="text-align: center">Fetching data ...</h4>
                    </template>

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
                                v-if="users"
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
                        field="role"
                        header="Role"
                        style="width: 30rem"
                    >
                        <template #body="{ data }">
                            <span :class="`account-badge ${data.role}`">
                                {{ data.role }} account
                            </span>
                        </template>
                        <template #filter="{ filterModel, filterCallback }">
                            <DropDown
                                v-if="users"
                                v-model="filterModel.value"
                                @change="filterCallback"
                                :options="['admin', 'hospital']"
                                class="p-column-filter"
                                placeholder="Select type"
                                :showClear="true"
                            >
                                <template #value="slotProps">
                                    <span
                                        :class="`account-badge ${slotProps.value}`"
                                        v-if="slotProps.value"
                                    >
                                        {{ slotProps.value }} account
                                    </span>
                                    <span v-else>
                                        {{ slotProps.placeholder }}
                                    </span>
                                </template>
                                <template #option="slotProps">
                                    <span
                                        :class="`account-badge ${slotProps.option}`"
                                    >
                                        {{ slotProps.option }} account
                                    </span>
                                </template>
                            </DropDown>
                        </template>
                    </PrimeVueColumn>

                    <!-- Delete Icon -->
                    <PrimeVueColumn>
                        <template #body="{ data }">
                            <PrimeVueButton
                                icon="pi pi-trash"
                                type="button"
                                class="p-button-text"
                                @click="deleteUser(data._id)"
                            ></PrimeVueButton>
                        </template>
                    </PrimeVueColumn>
                </PrimeVueTable>
            </div>

            <!-- User creation Form -->
            <UserCreation />
        </div>
    </div>
</template>

<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";
</style>
