<script setup>
import { ref, reactive, onBeforeMount } from "vue";
import Button from "primevue/button";
import RadioButton from "primevue/radiobutton";
import InputText from "primevue/inputtext";
import Checkbox from "primevue/checkbox";
import Calendar from "primevue/calendar";
import Divider from "primevue/divider";
import { useToast } from "primevue/usetoast";
import Toast from "primevue/toast";
import {
  email,
  required,
  minLength,
  maxLength,
  numeric,
} from "@vuelidate/validators";
import { useVuelidate } from "@vuelidate/core";
import { useRoute } from "vue-router";

import EventSubmissionRepo from "../../api/EventSubmissionRepo";
import EventRepo from "../../api/EventRepo";
import { determineStatus, formatDate } from "../../utils/index";
import ProgressSpinner from "primevue/progressspinner";

const route = useRoute();
const eventId = route.params.eventId;
const now = route.query.now;

const submitted = ref(false);
const toast = useToast();
const today = new Date();

let loadingData = ref(true);
let eventData = ref(null);

const donateData = reactive({
  eventId: "",
  fullname: "",
  idCardNumber: "",
  dob: "",
  gender: "",
  phone: "",
  email: "",
  address: "",
  latestDonationDate: "",
  medicalHistory: [],
});

const rules = {
  fullname: {
    required,
    minLength: minLength(6),
    maxLength: maxLength(30),
  },
  idCardNumber: {
    required,
    numeric,
  },
  gender: { required },
  phone: {
    required,
    numeric,
    maxLnght: maxLength(10),
    minLength: minLength(10),
  },

  dob: { required },
  email: { required, email },
  address: { required },
};

const v$ = useVuelidate(rules, donateData);

onBeforeMount(async () => {
  const data = await EventRepo.getById({ eventId: eventId, now: now });
  let event = { ...data.data };
  event["startDate"] = new Date(parseInt(event.startDate));
  event["status"] = determineStatus(event);
  event["bgImg"] =
    "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/mobile_banner_center_2500_600.webp";
  eventData.value = event;
  event["participantsValue"] =
    eventData.value.status === "upcoming"
      ? "have registered to join this event"
      : "donated for this event";
  eventData.value = event;
});

const handleSubmitForm = async () => {
  submitted.value = true;
  v$.value.$validate();
  if (!v$.value.$error) {
    // Format data before submit
    donateData.dob = new Date(donateData.dob).getTime().toString();
    donateData.latestDonationDate = donateData.latestDonationDate
      ? new Date(donateData.latestDonationDate).getTime().toString()
      : "";

    const data = {
      eventId: eventId,
      fullname: donateData.fullname,
      idCardNumber: donateData.idCardNumber,
      gender: donateData.gender,
      dob: donateData.dob,
      email: donateData.email,
      phone: donateData.phone,
      address: donateData.address,
      latestDonationDate: donateData.latestDonationDate,
    };

    try {
      await EventSubmissionRepo.post(data);
      toast.add({
        severity: "success",
        summary: "Successful",
        detail: "Your information is saved!!!",
        life: 3000,
      });

      resetForm();
    } catch (error) {
      if (error.response.status == 400) {
        toast.add({
          severity: "error",
          summary: "Form Error",
          detail: error.response.title,
          life: 3000,
        });
      } else {
        throw error;
      }
    }
  }
};

// Reset form after submit
const resetForm = () => {
  donateData.fullname = "";
  donateData.idCardNumber = "";
  donateData.gender = "";
  donateData.dob = "";
  donateData.phone = "";
  donateData.email = "";
  donateData.address = "";
  donateData.latestDonationDate = "";
  donateData.medicalHistory = [];
  submitted.value = false;
};
</script>

<template>
  <div class="event-detail-container">
    <template v-if="!eventData">
      <div
        class="flex align-items-center justify-content-center"
        style="width 400px; height: 400px; font-size: 50px"
      >
        <ProgressSpinner strokeWidth="4" />
      </div>
    </template>
    <template v-if="eventData">
      <Toast position="top-right" class="pt-8" />
      <div class="col-12">
        <div class="card">
          <div class="form-donation flex justify-content-center">
            <div>
              <div class="mb-4">
                <img
                  :src="eventData.bgImg"
                  alt=""
                  class="card"
                  style="width: 450px"
                />
              </div>

              <!-- Form submission -->
              <template v-if="eventData.status !== 'passed'">
                <div class="card py-4 px-4 mb-4">
                  <h2 class="text-center" style="color: var(--DARK_BLUE)">
                    Donation Form
                  </h2>
                  <div class="p-fluid">
                    <div class="field">
                      <label for="fullname" class="text-800">Full name*:</label>
                      <InputText
                        id="fullname"
                        v-model="donateData.fullname"
                        required="true"
                        autofocus
                        :class="{
                          'p-invalid': submitted && !donateData.fullname,
                        }"
                      />
                      <small
                        class="p-error"
                        v-if="v$.fullname.$error && submitted"
                        >{{ v$.fullname.$errors[0].$message }}</small
                      >
                    </div>

                    <div class="field">
                      <label for="idCardNumber" class="text-800"
                        >Identify Card Number*:</label
                      >
                      <InputText
                        id="idCardNumber"
                        v-model="donateData.idCardNumber"
                        required="true"
                        autofocus
                        :class="{
                          'p-invalid': submitted && !donateData.idCardNumber,
                        }"
                      />
                      <small
                        class="p-error"
                        v-if="v$.idCardNumber.$error && submitted"
                        >{{ v$.idCardNumber.$errors[0].$message }}</small
                      >
                    </div>
                    <div class="field">
                      <label for="idCardNumber" class="text-800"
                        >Gender*:</label
                      >
                      <div
                        class="field flex"
                        :class="{
                          'p-invalid': submitted && !donateData.gender,
                        }"
                      >
                        <div class="field-radiobutton mr-3 text-800">
                          <RadioButton
                            id="male"
                            name="gender"
                            value="Male"
                            v-model="donateData.gender"
                          />
                          <label for="male">Male</label>
                        </div>
                        <div class="field-radiobutton mr-3 text-800">
                          <RadioButton
                            id="female"
                            name="gender"
                            value="Female"
                            v-model="donateData.gender"
                          />
                          <label for="female">Female</label>
                        </div>
                        <div class="field-radiobutton mr-3 text-800">
                          <RadioButton
                            id="other"
                            name="gender"
                            value="other"
                            v-model="donateData.gender"
                          />
                          <label for="other">Other</label>
                        </div>
                      </div>
                      <small
                        class="p-error"
                        v-if="v$.gender.$error && submitted"
                        >{{ v$.gender.$errors[0].$message }}</small
                      >
                    </div>
                    <div class="field">
                      <label for="dob" class="text-800">Date of birth*</label>
                      <Calendar
                        id="dob"
                        v-model="donateData.dob"
                        :showIcon="true"
                        dateFormat="dd/mm/yy"
                        :baseZIndex="1000000"
                        :class="{ 'p-invalid': submitted && !donateData.dob }"
                      />
                      <small
                        class="p-error"
                        v-if="v$.dob.$error && submitted"
                        >{{ v$.dob.$errors[0].$message }}</small
                      >
                    </div>
                    <div class="field">
                      <label for="phone" class="text-800">Phone*:</label>
                      <InputText
                        id="phone"
                        v-model.trim="donateData.phone"
                        required="true"
                        autofocus
                        :class="{ 'p-invalid': submitted && !donateData.phone }"
                      />
                      <small
                        class="p-error"
                        v-if="v$.phone.$error && submitted"
                        >{{ v$.phone.$errors[0].$message }}</small
                      >
                    </div>
                    <div class="field">
                      <label for="email" class="text-800">Email*:</label>
                      <InputText
                        id="email"
                        v-model.trim="donateData.email"
                        required="true"
                        autofocus
                        :class="{ 'p-invalid': submitted && !donateData.email }"
                      />
                      <small
                        class="p-error"
                        v-if="v$.email.$error && submitted"
                        >{{ v$.email.$errors[0].$message }}</small
                      >
                    </div>
                    <div class="field">
                      <label for="address" class="text-800">Address*:</label>
                      <InputText
                        id="fullname"
                        v-model.trim="donateData.address"
                        required="true"
                        autofocus
                        :class="{
                          'p-invalid': submitted && !donateData.address,
                        }"
                      />
                      <small
                        class="p-error"
                        v-if="v$.address.$error && submitted"
                        >{{ v$.address.$errors[0].$message }}</small
                      >
                    </div>

                    <div class="field">
                      <label for="lastestDoantionDate" class="text-800"
                        >Lastest donate</label
                      >
                      <Calendar
                        id="lastestDoantionDate"
                        v-model="donateData.latestDonationDate"
                        :showIcon="true"
                        :baseZIndex="1000000"
                        dateFormat="dd/mm/yy"
                      />
                    </div>

                    <div class="field">
                      <label class="mb-2 text-800">Medical history</label>
                      <div class="formgrid grid">
                        <div class="field-radiobutton col-6 text-800">
                          <Checkbox
                            id="high_blood"
                            name="medicalHistory"
                            value="high_blood"
                            v-model="donateData.medicalHistory"
                          />
                          <label for="category1">High blood pressure</label>
                        </div>
                        <div class="field-radiobutton col-6 text-800">
                          <Checkbox
                            id="heart_disease"
                            name="medicalHistory"
                            value="heart_disease"
                            v-model="donateData.medicalHistory"
                          />
                          <label for="category2">Heart disease</label>
                        </div>
                      </div>
                    </div>

                    <Button
                      type="submit"
                      label="Submit"
                      class="p-button-text w-full"
                      @click="handleSubmitForm"
                    />
                  </div>
                </div>
              </template>
            </div>

            <div class="col-2">
              <Divider layout="vertical">
                <b>to</b>
              </Divider>
            </div>

            <!-- Event Information -->
            <div class="col-5 align-items-center justify-content-center">
              <!-- Event Title -->
              <h2 style="color: var(--PRIMARY_COLOR)">
                {{ eventData.name }}
              </h2>
              <br />

              <!-- Event Status -->
              <Divider layout="horizontal" align="left">
                <span
                  class="p-tag flex justify-content-center align-items-center"
                >
                  <i class="pi pi-circle-fill mr-2"></i>
                  Status</span
                >
              </Divider>
              <p class="line-height-3 m-0 text-800">
                <span
                  :class="
                    'event-badge status-' + eventData.status.toLowerCase()
                  "
                  >{{ eventData.status }}</span
                >
              </p>

              <!-- Event Details -->
              <Divider layout="horizontal" align="left">
                <span
                  class="p-tag flex justify-content-center align-items-center"
                >
                  <i class="pi pi-search mr-2"></i>
                  Details</span
                >
              </Divider>
              <p class="line-height-3 m-0 text-800">
                {{ eventData.detail }}
              </p>

              <!-- Event Time -->
              <Divider layout="horizontal" align="left">
                <span
                  class="p-tag flex justify-content-center align-items-center"
                >
                  <i class="pi pi-calendar-times mr-2"></i>
                  Time</span
                >
              </Divider>

              <p class="line-height-3 m-0 text-800">
                <span>Start Date: {{ formatDate(eventData.startDate) }}</span>
                <br />
                <span>Duration: {{ eventData.duration }} days</span>
              </p>

              <!-- Event Location -->
              <Divider layout="horizontal" align="left">
                <span
                  class="p-tag flex justify-content-center align-items-center"
                >
                  <i class="pi pi-map-marker mr-2"></i>
                  Location</span
                >
              </Divider>

              <p class="line-height-3 m-0 text-800">
                <span>Address: {{ eventData.location.address }}</span>
                <br />
                <span>City: {{ eventData.location.city }}</span>
              </p>

              <!-- Event Participants -->
              <Divider layout="horizontal" align="left">
                <span
                  class="p-tag flex justify-content-center align-items-center"
                >
                  <i class="pi pi-users mr-2"></i>
                  Participants</span
                >
              </Divider>
              <p class="line-height-3 m-0 text-800">
                <span>{{
                  `${eventData.participants} people ${eventData.participantsValue}`
                }}</span>
              </p>
            </div>
          </div>
        </div>
      </div>
    </template>
  </div>
</template>

<style lang="scss" scoped>
.event-detail-container {
  .form-donation {
    .card {
      min-width: 450px;
      background-color: #ebf0f6;

      border: 1px solid var(--DARK_BLUE);
      border-radius: 20px;

      .field {
        margin-bottom: 0.5rem;
      }

      .p-inputtext:enabled:hover {
        border-color: var(--DARK_BLUE) !important;
      }

      .p-inputtext.p-component {
        .p-inputtext:enabled:hover {
          border-color: var(--DARK_BLUE);
        }
      }
    }
  }
}
</style>
