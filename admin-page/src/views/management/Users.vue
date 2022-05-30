<script setup>
import { nextTick, onBeforeMount } from "vue";
import InputText from "primevue/inputtext";
import DropDown from "primevue/dropdown";
import Dialog from "primevue/dialog";
import { useToast } from "primevue/usetoast";
import { FilterMatchMode } from "primevue/api";

import UserRepo from "../../api/UserRepo.js";
import UserCreation from "../../components/form/UserCreation.vue";
import { useUserStore } from "../../stores/user.js";

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

const fetchUsersData = async () => {
    const { data } = await UserRepo.getAll();

    users = data.map((row) => ({
        ...row,
        role: row.isAdmin ? "admin" : "hospital",
    }));

    fetchingData = false;
};

onBeforeMount(async () => {
    initFilter();
    await fetchUsersData();
});

const getRowClass = (data) => {
    return data.email === useUserStore().email ? "active-user" : "";
};

// Rerender Form
let isRerender = $ref(false);
const forceReRenderForm = async () => {
    isRerender = true;
    await nextTick();
    isRerender = false;
};

// Delete user
const toast = useToast();
const resultData = $ref({
    show: false,
    password: "",
});
const onNewAccount = async (newAccount) => {
    resultData.show = true;
    resultData.email = newAccount.email;
    resultData.password = newAccount.password;

    await fetchUsersData();
    await forceReRenderForm();
};
const deleteUser = async (userId) => {
    if (!window.confirm("Delete this user?")) return;

    fetchingData = true;
    const { data, status } = await UserRepo.delete(userId);

    if (data && status === 200) {
        users = users.filter((el) => el._id !== userId);
        fetchingData = false;
    }
};

const copyToClipboard = (value) => {
    navigator.clipboard.writeText(value);
    toast.add({
        severity: "info",
        summary: "Copied to clipboard",
        life: 3000,
    });
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
                    :rows="10"
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
                                v-if="data.email !== useUserStore().email"
                            ></PrimeVueButton>
                        </template>
                    </PrimeVueColumn>
                </PrimeVueTable>
            </div>

            <!-- User creation Form -->
            <UserCreation v-if="!isRerender" @onNewAccount="onNewAccount" />
        </div>
    </div>

    <!-- Pop up dialog to reveal information -->
    <Dialog
        header="New Account Has Been Created"
        v-model:visible="resultData.show"
        :style="{ width: '50vw' }"
        :modal="true"
    >
        <p class="info">
            <i class="fa-solid fa-envelope"></i>
            {{ resultData.email }}

            <button
                v-ripple
                class="p-ripple copy-btn"
                v-tooltip.top="'Copy email to clipboard'"
                @click="copyToClipboard(resultData.email)"
            >
                <i class="fa-solid fa-clone"></i>
            </button>
        </p>
        <p class="info password">
            <i class="fa-solid fa-lock"></i>
            {{ resultData.password }}

            <button
                v-ripple
                class="p-ripple copy-btn"
                v-tooltip.top="'Copy password to clipboard'"
                @click="copyToClipboard(resultData.password)"
            >
                <i class="fa-solid fa-clone"></i>
            </button>
        </p>

        <template #footer>
            <PrimeVueButton
                label="Close"
                icon="pi pi-times"
                @click="resultData.show = false"
            />
        </template>
    </Dialog>
</template>

<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";

.info {
    margin-top: 1rem;
    padding: 1rem;
    color: #fff;
    background: rgba(var(--primary-color-rbg), 0.8);
    border-radius: 20px;
    display: flex;
    align-items: center;

    &.password {
        background-color: rgb(124, 178, 196);
    }

    > i {
        font-size: 1.2rem;
        margin-right: 1rem;
    }

    .copy-btn {
        margin-left: auto;
        color: lightgray;
        background: #fff;
        border: none;
        border-radius: 1000px;
        aspect-ratio: 1 / 1;
        display: flex;
        justify-content: center;
        align-items: center;
        cursor: pointer;

        i {
            font-size: 1.2rem;
            padding: 0.3rem;
        }
    }
}
</style>
