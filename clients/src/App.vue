<script setup>
import { watch } from "vue";
import { RouterView, RouterLink } from "vue-router";
import Button from "primevue/button";
import Dialog from "primevue/dialog";
import ConfirmPopup from "primevue/confirmpopup";
import { useConfirm } from "primevue/useconfirm";
import Dropdown from "primevue/dropdown";
import Toast from "primevue/toast";
import { useToast } from "primevue/usetoast";

let name = $ref("Quoc Bao Ngo");
const clickMe = () => {
    if (name.length > 25) name = "Quoc Bao Ngo";
    else name += " Changed";
};

// Dialog handler
let isOpenDialog = $ref(false);

// Confirm popup handler
const confirm = useConfirm();
const popConfirmTest = (event) => {
    confirm.require({
        target: event.currentTarget,
        message: "Football on 6.30pm Thursday?",
        icon: "pi pi-question-circle",
        accept: () => console.log("Accepted"),
        reject: () => console.log("Rejected"),
    });
};

// Dropdown toast noification
let selectedToast = $ref(null);
let toastPos = $ref("top-center");
const toasts = [
    { name: "Toast Top Right Success", code: "tr" },
    { name: "Toast Bottom Left Error", code: "bl" },
];
const toastsHandler = useToast();
watch(
    () => selectedToast,
    (newVal) => {
        if (newVal === "tr") {
            toastPos = "top-right";
            toastsHandler.add({
                severity: "success",
                summary: "Success Message",
                detail: "Success Notification",
                life: 1000,
            });
        } else if (newVal === "bl") {
            toastPos = "bottom-left";
            toastsHandler.add({
                severity: "error",
                summary: "Error Message",
                detail: "Error Notification",
                life: 1000,
            });
        }
    }
);
</script>

<template>
    <router-link :to="{ name: 'Home' }">Home</router-link>
    <router-link :to="{ name: 'About' }">About</router-link>
    <main>
        <p>
            Dynamic Content:
            <span>{{ name }}</span>
        </p>

        <Button
            @click="clickMe"
            label="PrimeVue Button"
            class="p-button-success my-btn"
        />

        <!-- Open Dialog -->
        <Button
            @click="() => (isOpenDialog = true)"
            label="Open Dialog"
            class="my-btn"
        />

        <!-- Open Confirm modal -->
        <Button
            @click="popConfirmTest"
            label="Open Confirm"
            class="my-btn"
            style="background-color: lightcoral; border: none"
            icon="pi pi-apple"
        />

        <!-- Toast selection dropdown -->
        <Dropdown
            placeholder="Choose a toast notification"
            v-model="selectedToast"
            :options="toasts"
            optionLabel="name"
            optionValue="code"
            class="my-btn"
        />
        <RouterView />
    </main>

    <!-- Dialog -->
    <Dialog
        v-model:visible="isOpenDialog"
        :modal="true"
        header="My Dialog"
        position="top"
        style="max-width: 80%"
    >
        Lorem ipsum dolor sit amet, consectetur adipisicing elit. Esse nihil id
        perferendis doloribus dolores deserunt et iste ut quam iure, dolor sint
        culpa facilis dignissimos eos quibusdam modi, facere nesciunt,
        voluptatum impedit enim. Impedit nulla aliquid blanditiis magnam dolorem
        minus?
    </Dialog>

    <!-- Confirm Popup -->
    <ConfirmPopup style="margin-top: 1rem"></ConfirmPopup>

    <!-- Toast -->
    <Toast :position="toastPos" />
</template>

<style lang="scss">
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

html {
    background-color: black;
    color: #fff;
}

a {
    color: white;
    padding: 1rem 2rem;
    transition: all 0.4s ease;
    &.router-link-active {
        color: rgba(73, 255, 88, 0.74);
    }
}

main {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);

    p {
        font-size: 2rem;
        font-weight: bold;
        span {
            color: rgba(73, 255, 88, 0.74);
        }
    }
}

.my-btn {
    margin: 1rem;
    &:focus {
        box-shadow: none !important;
    }
}

.p-dropdown {
    width: 20rem;
}
</style>
