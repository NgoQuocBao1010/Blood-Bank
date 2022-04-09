<script setup>
import { onBeforeMount, defineAsyncComponent } from "vue";
import InputNumber from "primevue/inputnumber";
import Dropdown from "primevue/dropdown";
import RequestRepo from "../api/RequestRepo";
import useVuelidate from "@vuelidate/core";
import { useRoute } from "vue-router";
import { useToast } from "primevue/usetoast";
import { required, numeric } from "@vuelidate/validators";
import { BLOOD_TYPES } from "../constants";
import HospitalRepo from "../api/HospitalRepo";

const AsyncRequestHistory = defineAsyncComponent({
  loader: () => import("../components/tables/RequestHistoryTable.vue"),
});

const route = useRoute();
const hospital_id = route.params._id;

const today = new Date();

let formData = $ref({
  quantity: null,
  blood: {
    name: "",
    type: "",
  },
  hospitalId: hospital_id,
  date: today.getTime(),
});

const formRules = $computed(() => {
  return {
    quantity: { required, numeric },
    blood: {
      name: { required },
      type: { required },
    },
  };
});

const $v = $(useVuelidate(formRules, formData));
const toast = useToast();

let hospitalName = $ref("");
let errorMessage = $ref(null);
let submitting = $ref(false);
let requestHistory = $ref([]);
let showRequestHistory = $ref(false);

// Reset form after submit
const resetForm = () => {
  (formData.quantity = null),
    (formData.blood = {
      name: "",
      type: "",
    }),
    (formData.hospitalId = hospital_id),
    (formData.date = today.getTime());
};

onBeforeMount(async () => {
  const { data } = await HospitalRepo.get(hospital_id);
  hospitalName = data.name;
  requestHistory = data.requestHistory;
});

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
    await RequestRepo.post({
      quantity: formData.quantity,
      blood: {
        name: formData.blood.name,
        type: formData.blood.type,
      },
      hospitalId: hospital_id,
      date: formData.date.toString(),
    });

    errorMessage = null;
    submitting = false;
    resetForm();

    toast.add({
      severity: "success",
      summary: "Successful",
      detail: "Your request is created",
      life: 3000,
    });
  } catch (e) {
    if (e.response) {
      const { status } = e.response;
      if (status === 400) {
        errorMessage = "You have an error";
        toast.add({
          severity: "error",
          summary: "Form Error",
          detail: errorMessage,
          life: 3000,
        });
      }
    } else {
      throw e;
    }
  }
};
</script>

<template>
  <div class="grid">
    <div class="col-12">
      <div class="card">
        <h4 class="hospital-name" :v-model="hospitalName">
          <i class="fa fa-hospital"></i>
          {{ hospitalName }}
        </h4>
        <h3 class="title">Hospital Blood Request Form</h3>

        <!-- Form -->
        <div class="p-fluid formgrid grid">
          <!-- Blood name -->
          <div class="field col-12 md:col-6">
            <label for="city">Blood Name</label>
            <Dropdown
              id="city"
              v-model="formData.blood.name"
              :options="BLOOD_TYPES"
              placeholder="Select One"
              :class="{ 'p-invalid': $v.blood.name.$error }"
            >
              <template #option="slotProps">
                <span :class="'blood-badge type-' + slotProps.option">
                  Type {{ slotProps.option }}
                </span>
              </template>
            </Dropdown>
            <span v-if="$v.blood.name.$error" class="app-form-error">
              This field is required
            </span>
          </div>

          <!-- Blood name -->
          <div class="field col-12 md:col-6">
            <label for="city">Blood Type</label>
            <Dropdown
              id="city"
              v-model="formData.blood.type"
              :options="['Positive', 'Negative']"
              placeholder="Select One"
              :class="{ 'p-invalid': $v.blood.type.$error }"
            >
              <template #option="slotProps">
                <span :class="'blood-badge type-' + slotProps.option">
                  {{ slotProps.option }}
                </span>
              </template>
            </Dropdown>
            <span v-if="$v.blood.type.$error" class="app-form-error">
              This field is required
            </span>
          </div>

          <!-- Event detail -->
          <div class="field col-12 md:col-6">
            <label for="event-detail">Quantity (ml)</label>
            <InputNumber
              v-model="formData.quantity"
              :autoResize="true"
              rows="5"
              cols="30"
              :class="{ 'p-invalid': $v.quantity.$error }"
            />
            <span v-if="$v.quantity.$error" class="app-form-error">
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
      <!-- Request History -->
      <div class="card">
        <div class="flex-center" style="width: 100%" v-if="!showRequestHistory">
          <PrimeVueButton
            label="Show Request History"
            @click="showRequestHistory = !showRequestHistory"
          />
        </div>

        <template v-else>
          <h2>Request History</h2>
          <AsyncRequestHistory
            :requestHistory="requestHistory"
            :v-if="requestHistory"
          />
        </template>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
@import "../assets/styles/badge.scss";

.title {
  font-weight: 900;
  color: var(--primary-color);
  text-align: center;
  margin-bottom: 2rem;
}

.hospital-name {
  color: var(--primary-color);
}

.submit-btn {
  margin-top: 3em;
  width: 8em;
}
</style>
