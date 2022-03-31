<script setup>
import InputText from "primevue/inputtext";
import RadioButton from "primevue/radiobutton";
import Dropdown from "primevue/dropdown";
import Divider from "primevue/divider";
import Dialog from "primevue/dialog";
import { useToast } from "primevue/usetoast";
import useVuelidate from "@vuelidate/core";
import { helpers, required, email } from "@vuelidate/validators";

import { useUserStore } from "../../stores/user";
import UserRepo from "../../api/UserRepo";
import HospitalRepo from "../../api/HospitalRepo";
import { watch } from "vue";

const userStore = useUserStore();
const toast = useToast();

// Formdata and Validalation rules
let formData = $ref({
    newEmail: "",
    isAdmin: true,
    hospitalId: "Fallback Value",
    verifyInfo: {
        email: userStore.email,
        password: "",
    },
});
const formRules = $computed(() => {
    return {
        newEmail: {
            required: helpers.withMessage("This field is required", required),
            email: helpers.withMessage("Please provide a valid email", email),
        },
        verifyInfo: {
            password: {
                required: helpers.withMessage(
                    "This field is required",
                    required
                ),
            },
        },
    };
});
const resultData = $ref({
    show: false,
    password: "GadCVV",
});

let hosiptals = $ref(null);
watch(
    () => formData.isAdmin,
    async (newValue) => {
        if (newValue) return;
        if (hosiptals) return;

        const { data } = await HospitalRepo.getAll();
        hosiptals = data;
        formData.hospitalId = hosiptals[0]._id;
    }
);

const $v = $(useVuelidate(formRules, formData));
let submitting = $ref(false);
const submitData = async () => {
    // Form validation
    const isCorrect = await $v.$validate();
    if (!isCorrect) {
        toast.add({
            severity: "error",
            summary: "Form Error",
            detail: "Please fix your form 🙏",
            life: 3000,
        });

        return;
    }
    // Make API call to server
    submitting = true;
    const isVerified = await verifyAccount(
        formData.verifyInfo.email,
        formData.verifyInfo.password
    );

    if (!isVerified) {
        submitting = false;
        return;
    }
};

const verifyAccount = async (email, password) => {
    try {
        await userStore.login(email, password);
        return true;
    } catch (e) {
        if (e.response?.status === 400) {
            toast.add({
                severity: "error",
                summary: "Account Verify Failed",
                detail: "Your password is invalid.",
                life: 3000,
            });

            return false;
        }

        throw e;
    }
};

const copy = (value) => {
    navigator.clipboard.writeText(value);
    toast.add({
        severity: "info",
        summary: "Copied to clipboard",
        life: 3000,
    });
};
</script>

<template>
    <div class="card">
        <h4 class="app-highlight" style="text-align: center">
            User Creation Form
        </h4>

        <div class="p-fluid formgrid grid">
            <!-- New Account -->
            <div class="field col-12 md:col-6">
                <label for="new-account">New Account</label>
                <InputText
                    v-model="formData.newEmail"
                    id="new-account"
                    placeholder="Enter the email for new account"
                    type="text"
                    :class="{ 'p-invalid': $v.newEmail.$error }"
                />
                <span v-if="$v.newEmail.$error" class="app-form-error">
                    {{ $v.newEmail.$errors[0].$message }}
                </span>
            </div>

            <!-- Account type -->
            <div class="field-radiobutton col-12 md:col-3 flex-center">
                <RadioButton
                    name="account_type"
                    :value="true"
                    v-model="formData.isAdmin"
                />
                <label for="account_type">Admin Account</label>
            </div>

            <div class="field-radiobutton col-12 md:col-3 flex-center">
                <RadioButton
                    name="account_type"
                    :value="false"
                    v-model="formData.isAdmin"
                />
                <label for="account_type">Hospital Account</label>
            </div>

            <!-- Hospital -->
            <div class="field col-12" v-if="hosiptals && !formData.isAdmin">
                <Dropdown
                    v-model="formData.hospitalId"
                    :options="hosiptals"
                    optionValue="_id"
                    class="p-column-filter"
                    placeholder="Choose hospital"
                    :showClear="true"
                >
                    <template #value="slotProps">
                        <span v-if="slotProps.value">
                            {{
                                hosiptals.find(
                                    (el) => el._id === slotProps.value
                                )?.name
                            }}
                        </span>
                        <span v-else>
                            {{ slotProps.placeholder }}
                        </span>
                    </template>
                    <template #option="slotProps">
                        <span>{{ slotProps.option.name }}</span>
                    </template>
                </Dropdown>
            </div>

            <!-- Divider -->
            <Divider>
                <span class="app-note">
                    ** Verify your account before submit **
                </span>
            </Divider>

            <!-- Email -->
            <div class="field col-12 md:col-6">
                <label>Email</label>
                <InputText
                    v-model="formData.verifyInfo.email"
                    id="email"
                    type="text"
                    disabled="disabled"
                />
            </div>

            <!-- Password -->
            <div class="field col-12 md:col-6">
                <label>Password</label>
                <InputText
                    v-model="formData.verifyInfo.password"
                    id="password"
                    type="password"
                    :class="{ 'p-invalid': $v.verifyInfo.password.$error }"
                />
                <span
                    v-if="$v.verifyInfo.password.$error"
                    class="app-form-error"
                >
                    {{ $v.verifyInfo.password.$errors[0].$message }}
                </span>
            </div>

            <!-- Submit -->
            <div class="field col-12 md:col-6">
                <PrimeVueButton
                    label="Submit"
                    :loading="submitting"
                    @click="submitData"
                    style="width: 8em; margin-top: 1em"
                />
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
                {{ formData.newEmail }}

                <button
                    v-ripple
                    class="p-ripple copy-btn"
                    v-tooltip.top="'Copy email to clipboard'"
                    @click="copy(formData.newEmail)"
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
                    @click="copy(resultData.password)"
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
    </div>
</template>

<style lang="scss" scoped>
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
