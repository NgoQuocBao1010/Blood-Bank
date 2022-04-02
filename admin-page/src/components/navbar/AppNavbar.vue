<script setup>
import InputText from "primevue/inputtext";
import OverlayPanel from "primevue/overlaypanel";
import { useToast } from "primevue/usetoast";

import { useUserStore } from "../../stores/user";
import AppRepo from "../../api/AppRepo";
import router from "../../router";

const userStore = useUserStore();
const emit = defineEmits(["toggleSidebar"]);
const toast = useToast();

let keywordSearch = $ref("");
const searchID = async () => {
    try {
        const { data, status } = await AppRepo.searchByObjectById(
            keywordSearch
        );

        if (data && status === 200) {
            const { type } = data;

            let redirectPage = null;
            switch (type) {
                case "Donor":
                    redirectPage = "Donor Detail";
                case "Event":
                    redirectPage = "Event Detail";
            }

            router.push({
                name: redirectPage,
                params: { _id: keywordSearch },
            });

            keywordSearch = "";
        }
    } catch (e) {
        if (!e.response) throw e;

        const { status } = e.response;
        if (status === 404) {
            toast.add({
                severity: "error",
                summary: "ID not matching",
                detail: "No object from Database is matched with your given ID",
                life: 3000,
            });
        } else {
            throw e;
        }
    }
};

let accountBtn = $ref(null);
const toggleAccountPanel = (event) => {
    // Toggle info button about stock status
    accountBtn.toggle(event);
};
</script>

<template>
    <div class="layout-topbar">
        <!-- Logo -->
        <router-link to="/" class="layout-topbar-logo">
            <img alt="Logo" src="../../assets/images/logo.png" />
            <span>JUDOH</span>
        </router-link>

        <!-- Mobile icon toggle for settings -->
        <button
            class="p-link layout-topbar-menu-button layout-topbar-button"
            v-styleclass="{
                selector: '@next',
                enterClass: 'hidden',
                enterActiveClass: 'scalein',
                leaveToClass: 'hidden',
                leaveActiveClass: 'fadeout',
                hideOnOutsideClick: true,
            }"
        >
            <i class="pi pi-ellipsis-v"></i>
        </button>

        <!-- Top bar Menu -->
        <ul
            class="layout-topbar-menu hidden lg:flex origin-top"
            style="align-items: center"
        >
            <!-- Serach bar -->
            <li>
                <span class="p-input-icon-left">
                    <i class="pi pi-search" />
                    <InputText
                        type="text"
                        style="width: 20rem"
                        v-model="keywordSearch"
                        placeholder="Enter personal ID"
                        @keydown.enter="searchID"
                        v-tooltip.top.focus="'Press enter key to search'"
                    />
                </span>
            </li>

            <!-- Sidebar toggle -->
            <li>
                <button
                    class="p-link layout-topbar-button"
                    @click="emit('toggleSidebar')"
                >
                    <i class="pi pi-bars"></i>
                    <span>Menu</span>
                </button>
            </li>

            <!-- Account toggle -->
            <li>
                <button
                    class="p-link layout-topbar-button"
                    ref="accountBtn"
                    @click="toggleAccountPanel"
                >
                    <i class="pi pi-user"></i>
                    <span>Profile</span>
                </button>
            </li>
        </ul>

        <OverlayPanel ref="accountBtn" id="account_panel">
            <p class="app-highlight">{{ userStore.email || "Guest" }}</p>
            <p v-if="userStore.isLoggedIn">
                <PrimeVueButton
                    icon="fa-solid fa-arrow-right-from-bracket"
                    class="p-button-sm"
                    label="logout"
                    @click="userStore.logout()"
                />
            </p>
            <p v-else>
                <router-link
                    :to="{ name: 'Login' }"
                    class="p-button p-button-sm app-router-link-icon"
                >
                    <i class="fa-solid fa-right-to-bracket"></i>
                    Login
                </router-link>
            </p>
        </OverlayPanel>
    </div>
</template>

<style lang="scss" scoped>
.layout-topbar-logo {
    img {
        height: 5rem;
    }
}

#account_panel {
    p {
        font-size: 1rem;
        text-align: center;
        width: 20em !important;
    }
}
</style>
