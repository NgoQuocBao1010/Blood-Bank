<script setup>
import { useRouter } from "vue-router";
import InputText from "primevue/inputtext";
import Button from "primevue/button";
import Password from "primevue/password";
import { useToast } from "primevue/usetoast";
import useVuelidate from "@vuelidate/core";
import { required, email, helpers } from "@vuelidate/validators";

import { useUserStore } from "../stores/user";

// Form data and validation rules
const formData = $ref({
    email: "",
    password: "",
});
const formRules = $computed(() => {
    return {
        email: {
            required: helpers.withMessage(
                "This field cannot be empty",
                required
            ),
            email: helpers.withMessage(
                "This field is not a valid email address",
                email
            ),
        },

        password: {
            required: helpers.withMessage(
                "This field cannot be empty",
                required
            ),
        },
    };
});

const router = useRouter();
const userStore = useUserStore();
const $v = $(useVuelidate(formRules, formData));
const toast = useToast();
let errorMessage = $ref(null);
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
    try {
        await userStore.login(formData.email, formData.password);
        errorMessage = null;

        toast.add({
            severity: "success",
            summary: "Login Success",
            detail: `Welcom, ${userStore.email} 👋`,
            life: 3000,
        });

        router.push(userStore.defaultPage);
    } catch (e) {
        if (e.response) {
            const { status } = e.response;
            if (status === 400) {
                errorMessage = "Email Or Password is invalid";
                toast.add({
                    severity: "error",
                    summary: "Login Failed",
                    detail: "Please ensure your email and password are correct",
                    life: 3000,
                });
            } else throw e;
        } else {
            throw e;
        }
    } finally {
        submitting = false;
    }
};
</script>

<template>
    <div
        class="surface-0 flex align-items-center justify-content-center overflow-hidden pt-2 py-5"
    >
        <div class="grid justify-content-center align-items-center lg:p-0">
            <div class="col-12 xl:col-6 form_wrapper">
                <div class="h-full w-full m-0 py-7 px-4 form_content">
                    <!-- Header -->
                    <div class="text-center mb-5">
                        <div class="text-900 text-3xl font-medium mb-3">
                            You are reaching Judoh Admin Page
                        </div>
                        <span class="text-600 font-medium">
                            Please sign in to continue or
                            <router-link to="#">
                                👉 go back to main page 👈
                            </router-link>
                        </span>
                    </div>

                    <!-- Form -->
                    <div class="w-full md:w-10 mx-auto">
                        <!-- Email -->
                        <div class="field col-12">
                            <label
                                for="email"
                                class="block text-900 text-xl font-medium mb-2"
                            >
                                Email
                            </label>
                            <InputText
                                id="email"
                                v-model="formData.email"
                                @keydown.enter="submitData"
                                type="text"
                                class="w-full mb-3"
                                :class="{
                                    'p-invalid':
                                        $v.email.$error || errorMessage,
                                }"
                                placeholder="Email"
                                style="padding: 1rem"
                            />
                            <span v-if="$v.email.$error" class="app-form-error">
                                {{ $v.email.$errors[0].$message }}
                            </span>
                            <span v-if="errorMessage" class="app-form-error">
                                {{ errorMessage }}
                            </span>
                        </div>

                        <!-- Password -->
                        <div class="field col-12">
                            <label
                                for="password"
                                class="block text-900 font-medium text-xl mb-2"
                            >
                                Password
                            </label>
                            <Password
                                id="password"
                                v-model="formData.password"
                                @keydown.enter="submitData"
                                placeholder="Password"
                                :toggleMask="true"
                                class="w-full mb-3"
                                :class="{
                                    'p-invalid':
                                        $v.password.$error || errorMessage,
                                }"
                                inputClass="w-full"
                                inputStyle="padding:1rem"
                                panelClass="app-hide-class"
                            ></Password>
                            <span
                                v-if="$v.password.$error"
                                class="app-form-error"
                            >
                                {{ $v.password.$errors[0].$message }}
                            </span>
                            <span v-if="errorMessage" class="app-form-error">
                                {{ errorMessage }}
                            </span>
                        </div>

                        <!-- Submit Button -->
                        <Button
                            label="Sign In"
                            class="w-full mt-3 p-3 text-xl"
                            :loading="submitting"
                            @click="submitData"
                        ></Button>
                    </div>
                </div>
            </div>
            <!-- </div> -->
        </div>
    </div>
</template>

<style lang="scss" scoped>
.pi-eye {
    transform: scale(1.6);
    margin-right: 1rem;
}

.pi-eye-slash {
    transform: scale(1.6);
    margin-right: 1rem;
}

.form_wrapper {
    width: 600px;
    border-radius: 56px;
    padding: 0.3rem;
    background: linear-gradient(
        180deg,
        var(--primary-color),
        rgba(33, 150, 243, 0) 30%
    );
}

.form_content {
    border-radius: 53px;
    background: linear-gradient(
        180deg,
        var(--surface-50) 38.9%,
        var(--surface-0)
    );

    .p-button:hover {
        background-color: var(--primary-color);
        border-color: var(--primary-color);
    }

    .p-button {
        background-color: var(--secondary-color);
        border-color: var(--secondary-color);
    }

    .p-button:focus {
        box-shadow: 0 0 0 2px #ffffff, 0 0 0 4px var(--secondary-color),
            0 1px 2px 0 black;
    }

    a {
        color: var(--primary-color) !important;
    }
}
</style>
