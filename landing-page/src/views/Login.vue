<script setup>
import { ref, onMounted } from "vue";
import InputText from "primevue/inputtext";
import Button from "primevue/button";
import Password from "primevue/password";
import Checkbox from "primevue/checkbox";

const data = ref({
  email: "",
  password: "",
  checked: false,
});

const handleOnSubmit = () => {
  console.log("Form data: ", data.value);
};
</script>

<template>
  <div
    class="surface-0 flex align-items-center justify-content-center overflow-hidden pt-2"
  >
    <div class="grid justify-content-center p-2 lg:p-0" style="min-width: 80%">
      <div class="col-12 xl:col-6 form_wrapper">
        <div class="h-full w-full m-0 py-7 px-4 form_content">
          <div class="text-center mb-5">
            <!-- <img
              src="../assets/images/logo.png"
              alt="Image"
              height="50"
              class="mb-3"
            /> -->
            <div class="text-900 text-3xl font-medium mb-3">
              Welcome to Blood Bank!
            </div>
            <span class="text-600 font-medium">Sign in to continue</span>
          </div>

          <div class="w-full md:w-10 mx-auto">
            <label for="email1" class="block text-900 text-xl font-medium mb-2"
              >Email</label
            >
            <InputText
              id="email1"
              v-model="data.email"
              type="text"
              class="w-full mb-3"
              placeholder="Email"
              style="padding: 1rem"
            />

            <label
              for="password1"
              class="block text-900 font-medium text-xl mb-2"
              >Password</label
            >
            <Password
              id="password1"
              v-model="data.password"
              placeholder="Password"
              :toggleMask="true"
              class="w-full mb-3"
              inputClass="w-full"
              inputStyle="padding:1rem"
            ></Password>

            <div class="flex align-items-center justify-content-between mb-5">
              <div class="flex align-items-center">
                <Checkbox
                  id="rememberme1"
                  v-model="data.checked"
                  :binary="true"
                  class="mr-2"
                ></Checkbox>
                <label for="rememberme1">Remember me</label>
              </div>
              <a class="font-medium no-underline ml-2 text-right cursor-pointer"
                >Forgot password?</a
              >
            </div>
            <Button
              label="Sign In"
              class="w-full p-3 text-xl"
              @click="handleOnSubmit"
            ></Button>
          </div>
        </div>
      </div>
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
  border-radius: 56px;
  padding: 0.3rem;
  background: linear-gradient(
    180deg,
    var(--PRIMARY_COLOR),
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
    background-color: var(--PRIMARY_COLOR);
    border-color: var(--PRIMARY_COLOR);
  }

  .p-button {
    background-color: var(--SECONDARY_COLOR);
    border-color: var(--SECONDARY_COLOR);
  }

  .p-button:focus {
    box-shadow: 0 0 0 2px #ffffff, 0 0 0 4px var(--SECONDARY_COLOR),
      0 1px 2px 0 black;
  }

  a {
    color: var(--PRIMARY_COLOR) !important;
  }
}
</style>
