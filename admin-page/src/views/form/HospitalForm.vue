<script setup>
import { onBeforeMount } from "vue";
import { useRouter } from "vue-router";
import InputText from "primevue/inputtext";
import { useToast } from "primevue/usetoast";
import useVuelidate from "@vuelidate/core";
import { required } from "@vuelidate/validators";
import HospitalRepo from "../../api/HospitalRepo";

const router = useRouter();
const { _id, hospitalData } = defineProps({
  _id: String,
  hospitalData: String,
});

const hospitalFetchData = $ref();

let formData = $ref({
  hospital_name: "",
  address: "",
  phone: "",
});

const formRules = $computed(() => {
  return {
    hospital_name: { required },
    address: { required },
    phone: { required },
  };
});

const { name } = router.currentRoute.value;

onBeforeMount(async () => {
  // Check if this is a Event Edit page
  // Fetch data if there is no passed props
  const { data } = await HospitalRepo.get(_id);
  hospitalFetchData = data;

  if (name === "Hospital Edit" && _id) {
    if (hospitalData) {
      fixingVuevalidateBugs(JSON.parse(hospitalData));
    } else {
      fixingVuevalidateBugs(hospitalFetchData);
    }
  }
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
      detail:
        name === "Hospital Edit"
          ? "Hospital is updated"
          : "New hospital is created",
      life: 3000,
    });
  }, 2000);
};

// Helpers
const fixingVuevalidateBugs = (data) => {
  formData.hospital_name = data.hospital_name;
  formData.address = data.address;
  formData.phone = data.phone;
};
</script>

<template>
  <div class="grid">
    <div class="col-12">
      <div class="card">
        <h3 class="title">
          Hospital
          {{ name === "Hospital Edit" ? "Update" : "Creation" }}
          Form
        </h3>

        <!-- Form -->
        <div class="p-fluid formgrid grid">
          <!-- Event Name -->
          <div class="field col-12">
            <label for="hospital-name">Hospital Name</label>
            <InputText
              v-model="formData.hospital_name"
              id="hospital-name"
              type="text"
              :class="{ 'p-invalid': $v.hospital_name.$error }"
            />
            <span v-if="$v.hospital_name.$error" class="app-form-error">
              This field is required
            </span>
          </div>

          <!-- Address -->
          <div class="field col-12 md:col-6">
            <label for="address">Address</label>
            <InputText
              id="address"
              type="text"
              v-model="formData.address"
              :class="{ 'p-invalid': $v.address.$error }"
            />
            <span v-if="$v.address.$error" class="app-form-error">
              This field is required
            </span>
          </div>

          <!-- Phone number -->
          <div class="field col-12 md:col-6">
            <label for="phone">Phone</label>
            <InputText
              id="phone"
              type="text"
              v-model="formData.phone"
              :class="{ 'p-invalid': $v.phone.$error }"
            />
            <span v-if="$v.phone.$error" class="app-form-error">
              This field is required
            </span>
          </div>

          <!-- Submiitting button -->
          <div class="field col-12">
            <PrimeVueButton
              type="button"
              label="Submit"
              class="submit-btn mb-2 mr-2"
              @click="submitData"
              :loading="submitting"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.title {
  font-weight: 900;
  color: var(--primary-color);
  text-align: center;
  margin-bottom: 2rem;
}

.submit-btn {
  margin-top: 3em;
  width: 8em;
}
</style>
