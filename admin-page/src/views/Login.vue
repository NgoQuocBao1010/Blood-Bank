<script setup>
import InputText from "primevue/inputtext";
import Button from "primevue/button";
import Password from "primevue/password";
import Checkbox from "primevue/checkbox";
import { useToast } from "primevue/usetoast";
import useVuelidate from "@vuelidate/core";
import { required, email, helpers } from "@vuelidate/validators";

const formData = $ref({
  email: "",
  password: "",
  checked: false,
});

const formRules = $computed(() => {
  return {
    email: {
      required: helpers.withMessage("This field cannot be empty", required),
      email: helpers.withMessage(
        "This field is not a valid email address",
        email
      ),
    },

    password: {
      required: helpers.withMessage("This field cannot be empty", required),
    },
  };
});

const $v = $(useVuelidate(formRules, formData));
const toast = useToast();
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

  setTimeout(() => {
    submitting = false;

    toast.add({
      severity: "success",
      summary: "Successful",
      detail: "Login successful",
      life: 3000,
    });
  }, 2000);
};
</script>

<template>
  <div
    class="surface-0 flex align-items-center justify-content-center overflow-hidden pt-2 py-7"
  >
    <div class="grid justify-content-center align-items-center lg:p-0 w-full">
      <div class="col-12 xl:col-6 form_wrapper">
        <div class="h-full w-full m-0 py-7 px-4 form_content">
          <div class="text-center mb-5">
            <div class="text-900 text-3xl font-medium mb-3">
              Welcome to Blood Bank!
            </div>
            <span class="text-600 font-medium">Sign in to continue</span>
          </div>

          <!-- Form -->
          <!-- <div class="p-fluid formgrid grid"> -->
          <div class="w-full md:w-10 mx-auto">
            <div class="field col-12">
              <label for="email" class="block text-900 text-xl font-medium mb-2"
                >Email</label
              >
              <InputText
                id="email"
                v-model="formData.email"
                type="text"
                class="w-full mb-3"
                :class="{ 'p-invalid': $v.email.$error }"
                placeholder="Email"
                style="padding: 1rem"
              />
              <span v-if="$v.email.$error" class="app-form-error">
                {{ $v.email.$errors[0].$message }}
              </span>
            </div>

            <div class="field col-12">
              <label
                for="password"
                class="block text-900 font-medium text-xl mb-2"
                >Password</label
              >
              <Password
                id="password"
                v-model="formData.password"
                placeholder="Password"
                :toggleMask="true"
                class="w-full mb-3"
                :class="{ 'p-invalid': $v.password.$error }"
                inputClass="w-full"
                inputStyle="padding:1rem"
              ></Password>
              <span v-if="$v.password.$error" class="app-form-error">
                {{ $v.password.$errors[0].$message }}
              </span>
            </div>

            <div class="flex align-items-center justify-content-between mb-5">
              <div class="flex align-items-center">
                <Checkbox
                  id="rememberme1"
                  v-model="formData.checked"
                  :binary="true"
                  class="mr-2"
                ></Checkbox>
                <label for="rememberme1">Remember me</label>
              </div>
            </div>
            <Button
              label="Sign In"
              class="w-full p-3 text-xl"
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
  width: 100%;
  max-width: 80vh;
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
