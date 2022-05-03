<script setup>
import InputText from "primevue/inputtext";
import OverlayPanel from "primevue/overlaypanel";
import { useToast } from "primevue/usetoast";
import { useRouter } from "vue-router";

import { useUserStore } from "../../stores/user";
import AppRepo from "../../api/AppRepo";

const router = useRouter();
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

            router.push({
                name: `${type} Detail`,
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

const logout = () => {
    router.push({ name: "Login" });
    userStore.logout();
};
</script>

<template>
    <div class="layout-topbar">
        <!-- Logo -->
        <router-link
            v-if="userStore.defaultPage"
            :to="userStore.defaultPage"
            class="layout-topbar-logo"
        >
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
            <!-- Search bar -->
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
            <p class="app-highlight">
                <template v-if="userStore.isLoggedIn">
                    <i
                        v-if="userStore.isAdmin"
                        class="fa-solid fa-user-lock"
                    ></i>
                    <i v-else class="fa-solid fa-user-nurse"></i>
                </template>
                {{ userStore.email || "Guest" }}
            </p>
            <p v-if="userStore.isLoggedIn">
                <PrimeVueButton
                    icon="fa-solid fa-arrow-right-from-bracket"
                    class="p-button-sm"
                    label="logout"
                    @click="logout()"
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

    .app-highlight {
        i {
            font-size: 1.2rem;
            padding: 10px;
        }
    }
}
</style>
