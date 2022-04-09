<script setup>
import { inject, watch } from "vue";
import InputText from "primevue/inputtext";
import RadioButton from "primevue/radiobutton";
import Dropdown from "primevue/dropdown";
import Divider from "primevue/divider";
import { useToast } from "primevue/usetoast";
import useVuelidate from "@vuelidate/core";
import { helpers, required, email } from "@vuelidate/validators";

import { useUserStore } from "../../stores/user";
import UserRepo from "../../api/UserRepo";
import HospitalRepo from "../../api/HospitalRepo";

const userStore = useUserStore();
const toast = useToast();
const emits = defineEmits(["onNewAccount"]);
const { showErrorDialog } = inject("errorDialog");

// Formdata and Validalation rules
let formData = $ref({
    email: "",
    isAdmin: true,
    hospitalId: "Fallback Value",
    verifyInfo: {
        email: userStore.email,
        password: "",
    },
});
const formRules = $computed(() => {
    return {
        email: {
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
let badRequestMsg = $ref("");
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

    try {
        // Reform data for API call
        let postData = null;
        if (formData.isAdmin) postData = { email: formData.email };
        else
            postData = {
                email: formData.email,
                hospitalId: formData.hospitalId,
            };
        const { data, status } = await UserRepo.createAccount(postData);

        if (data && status === 200) {
            emits("onNewAccount", data);
        }
    } catch (e) {
        if (e.response && e.response.status === 400) {
            if (e.response.data === "Email existed!")
                badRequestMsg = "Email is invalid! Please try another one!";

            if (e.response.data === "Hospital account existed!") {
                showErrorDialog(
                    "Hospital has existing account",
                    "Every hospital should only has one account. Please cancel the current operation or choose another hospital."
                );
            }
        }
    } finally {
        submitting = false;
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
                    v-model="formData.email"
                    id="new-account"
                    placeholder="Enter the email for new account"
                    type="text"
                    :class="{ 'p-invalid': $v.email.$error || badRequestMsg }"
                />
                <span v-if="$v.email.$error" class="app-form-error">
                    {{ $v.email.$errors[0].$message }}
                </span>
                <span v-if="badRequestMsg" class="app-form-error">
                    {{ badRequestMsg }}
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
    </div>
</template>

<style lang="scss" scoped></style>
