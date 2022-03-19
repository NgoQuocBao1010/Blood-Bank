<script setup>
import { ref, onMounted, reactive } from "vue";
import Button from "primevue/button";
import Dropdown from "primevue/dropdown";
import DataViewLayoutOptions from "primevue/dataviewlayoutoptions";
import DataView from "primevue/dataview";
import Rating from "primevue/rating";
import Dialog from "primevue/dialog";
import RadioButton from "primevue/radiobutton";
import InputText from "primevue/inputtext";
import Checkbox from "primevue/checkbox";
import Calendar from "primevue/calendar";
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
import moment from "moment";

const dataviewValue = ref(null);
const layout = ref("grid");
const donateDialog = ref(false);
const submitted = ref(false);
const toast = useToast();

onMounted(() => {
  dataviewValue.value = [
    {
      id: "6235eacaed0d1dd101c87436",
      title: "Event 1",
      address: "O Mon",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/mobile_banner_center_2500_600.webp",
      time: "01/01/2022",
    },
    {
      id: "6235eaf2ed0d1dd101c87437",
      title: "Event 2",
      address: "Nga 6",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/updated_NewYear2022.webp",
      time: "15/03/2022",
    },
    {
      id: 3,
      title: "Event 3",
      address: "Hung Phu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/ONE_NATION_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 4,
      title: "Event 4",
      address: "Giong Rieng",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/Blood_EHealthID_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 5,
      title: "Event 5",
      address: "Ninh Kieu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/mobile_banner_center_2500_600.webp",
      time: "01/01/2022",
    },
    {
      id: 6,
      title: "Event 5",
      address: "Ninh Kieu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/mobile_banner_center_2500_600.webp",
      time: "01/01/2022",
    },
    {
      id: 7,
      title: "Event 7",
      address: "Giong Rieng",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/Blood_EHealthID_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 8,
      title: "Event 8",
      address: "Hung Phu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/ONE_NATION_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 8,
      title: "Event 8",
      address: "Hung Phu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/ONE_NATION_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 8,
      title: "Event 8",
      address: "Hung Phu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/ONE_NATION_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 8,
      title: "Event 8",
      address: "Hung Phu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/ONE_NATION_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 8,
      title: "Event 8",
      address: "Hung Phu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/ONE_NATION_2500_600.webp",
      time: "18/04/2022",
    },
  ];
});

const donateData = reactive({
  eventId: "",
  fullname: "",
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

const onClickDonate = (eventId) => {
  donateData.eventId = eventId;
  donateDialog.value = true;
  submitted.value = false;
};

const formatDate = (date) => {
  return moment(date).format("DD/MM/YYYY");
};

const handleSubmitForm = () => {
  submitted.value = true;
  v$.value.$validate();
  if (!v$.value.$error) {
    donateData.dob = formatDate(donateData.dob);
    donateData.latestDonationDate = formatDate(donateData.latestDonationDate);
    toast.add({
      severity: "success",
      summary: "Successful",
      detail: "Your information is saved!!!",
      life: 3000,
    });
    console.log("donateData :", {
      eventId: donateData.eventId,
      fullname: donateData.fullname,
      gender: donateData.gender,
      dob: donateData.dob,
      email: donateData.email,
      phone: donateData.phone,
      address: donateData.address,
      lastestDonationDate: donateData.latestDonationDate,
      medicalHistory: donateData.medicalHistory,
    });
    resetForm();
    donateDialog.value = false;
  }
};

const resetForm = () => {
  donateData.fullname = "";
  donateData.gender = "";
  donateData.dob = "";
  donateData.phone = "";
  donateData.email = "";
  donateData.address = "";
  donateData.latestDonationDate = "";
  donateData.medicalHistory = [];
  submitted.value = false;
};

const hideDialog = () => {
  resetForm();
  donateDialog.value = false;
};
</script>

<template>
  <div class="grid px-2">
    <div class="col-12">
      <Toast position="top-right" class="pt-8" />

      <div class="card">
        <div class="text-center">
          <h1 class="text-900 font-normal mb-4 text-4xl">Blood Events</h1>
        </div>
        <DataView
          :value="dataviewValue"
          :layout="layout"
          :paginator="true"
          :rows="9"
        >
          <template #header>
            <div class="flex align-items-right">
              <DataViewLayoutOptions v-model="layout" />
            </div>
          </template>
          <template #list="slotProps">
            <div class="col-12">
              <div
                class="flex flex-column md:flex-row align-items-center p-3 w-full"
              >
                <img
                  :src="slotProps.data.bgImg"
                  :alt="slotProps.data.title"
                  class="my-4 md:my-0 w-9 md:w-10rem shadow-2 mr-5"
                  style="height: 70px"
                />
                <div class="flex-1 text-center md:text-left">
                  <div class="font-bold text-2xl">
                    {{ slotProps.data.title }}
                  </div>
                  <div class="mb-3">{{ slotProps.data.description }}</div>
                  <span
                    class="text-xl font-semibold mb-2 align-self-center md:align-self-end"
                  >
                    <i class="pi pi-calendar-times text-xl"></i>
                    Time: {{ slotProps.data.time }}</span
                  >
                </div>
                <div
                  class="flex flex-row md:flex-column justify-content-between w-full md:w-auto align-items-center md:align-items-end mt-5 md:mt-0"
                >
                  <Button
                    @click="() => onClickDonate(slotProps.data.id)"
                    label="Donate now"
                    class="mb-2"
                  ></Button>
                </div>
              </div>
            </div>
          </template>

          <template #grid="slotProps">
            <div class="col-12 md:col-4">
              <div class="card m-3 border-1 surface-border">
                <div class="text-center">
                  <img
                    :src="slotProps.data.bgImg"
                    :alt="slotProps.data.title"
                    class="w-9 shadow-2 my-3 mx-0"
                    style="height: 150px"
                  />
                  <div class="text-2xl font-bold">
                    {{ slotProps.data.title }}
                  </div>
                  <div class="mb-3">{{ slotProps.data.description }}</div>
                </div>
                <div class="flex align-items-center justify-content-between">
                  <span class="text-xl font-semibold">
                    <i class="pi pi-calendar-times text-xl"></i>
                    {{ slotProps.data.time }}</span
                  >
                  <Button
                    @click="() => onClickDonate(slotProps.data.id)"
                    label="Donate now"
                  ></Button>
                </div>
              </div>
            </div>
          </template>
        </DataView>
      </div>
    </div>

    <Dialog
      :visible="donateDialog"
      :style="{ width: '450px', paddingTop: '100px' }"
      :modal="true"
      class="form-donation p-fluid"
    >
      <template #header>
        <h1 style="color: var(--DARK_BLUE)">Donation Form</h1>
      </template>
      <div class="field">
        <label for="fullname">Full name</label>
        <InputText
          id="fullname"
          v-model="donateData.fullname"
          required="true"
          autofocus
          :class="{ 'p-invalid': submitted && !donateData.fullname }"
        />
        <small class="p-error" v-if="v$.fullname.$error && submitted">{{
          v$.fullname.$errors[0].$message
        }}</small>
      </div>
      <div
        class="field flex"
        :class="{ 'p-invalid': submitted && !donateData.gender }"
      >
        <div class="field-radiobutton mr-3">
          <RadioButton
            id="male"
            name="gender"
            value="Male"
            v-model="donateData.gender"
          />
          <label for="male">Male</label>
        </div>
        <div class="field-radiobutton mr-3">
          <RadioButton
            id="female"
            name="gender"
            value="Female"
            v-model="donateData.gender"
          />
          <label for="female">Female</label>
        </div>
        <div class="field-radiobutton mr-3">
          <RadioButton
            id="other"
            name="gender"
            value="other"
            v-model="donateData.gender"
          />
          <label for="other">Other</label>
        </div>
        <small class="p-error" v-if="v$.gender.$error && submitted">{{
          v$.gender.$errors[0].$message
        }}</small>
      </div>
      <div class="field">
        <label for="dob">Birthday</label>
        <Calendar
          id="dob"
          v-model="donateData.dob"
          :showIcon="true"
          dateFormat="dd/mm/yy"
          :baseZIndex="1000000"
          :class="{ 'p-invalid': submitted && !donateData.dob }"
        />
        <small class="p-error" v-if="v$.dob.$error && submitted">{{
          v$.dob.$errors[0].$message
        }}</small>
      </div>
      <div class="field">
        <label for="phone">Phone</label>
        <InputText
          id="phone"
          v-model.trim="donateData.phone"
          required="true"
          autofocus
          :class="{ 'p-invalid': submitted && !donateData.phone }"
        />
        <small class="p-error" v-if="v$.phone.$error && submitted">{{
          v$.phone.$errors[0].$message
        }}</small>
      </div>
      <div class="field">
        <label for="email">Email</label>
        <InputText
          id="email"
          v-model.trim="donateData.email"
          required="true"
          autofocus
          :class="{ 'p-invalid': submitted && !donateData.email }"
        />
        <small class="p-error" v-if="v$.email.$error && submitted">{{
          v$.email.$errors[0].$message
        }}</small>
      </div>
      <div class="field">
        <label for="address">Address</label>
        <InputText
          id="fullname"
          v-model.trim="donateData.address"
          required="true"
          autofocus
          :class="{ 'p-invalid': submitted && !donateData.address }"
        />
        <small class="p-error" v-if="v$.address.$error && submitted">{{
          v$.address.$errors[0].$message
        }}</small>
      </div>

      <div class="field">
        <label for="lastestDoantionDate">Lastest donate</label>
        <Calendar
          id="lastestDoantionDate"
          v-model="donateData.latestDonationDate"
          :showIcon="true"
          :baseZIndex="1000000"
          dateFormat="dd/mm/yy"
        />
      </div>

      <div class="field">
        <label class="mb-3">Medical history</label>
        <div class="formgrid grid">
          <div class="field-radiobutton col-6">
            <Checkbox
              id="high_blood"
              name="medicalHistory"
              value="high_blood"
              v-model="donateData.medicalHistory"
            />
            <label for="category1">High blood pressure</label>
          </div>
          <div class="field-radiobutton col-6">
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
      <template #footer>
        <Button
          label="Cancel"
          icon="pi pi-times"
          class="p-button-text"
          @click="hideDialog"
        />
        <Button
          type="submit"
          label="Submit"
          icon="pi pi-check"
          class="p-button-text"
          @click="handleSubmitForm"
        />
      </template>
    </Dialog>
  </div>
</template>

<style scoped lang="scss">
@import "../../assets/styles/badges.scss";
.card {
  background-color: var(--surface-card);
  padding: 1.5rem;
  color: var(--surface-900);
  margin-bottom: 1rem;
  border-radius: 12px;
  box-shadow: 0 3px 5px rgba(0, 0, 0, 0.02), 0 0 2px rgba(0, 0, 0, 0.05),
    0 1px 4px rgba(0, 0, 0, 0.08) !important;

  button {
    background-color: var(--PRIMARY_COLOR);
  }

  button:focus {
    box-shadow: 0 0 0 2px #ffffff, 0 0 0 4px var(--PRIMARY_COLOR),
      0 1px 2px 0 black;
  }

  button:hover {
    background-color: var(--SECONDARY_COLOR);
  }
}

.form-donation {
  .p-inputtext:enabled:hover {
    border-color: var(--DARK_BLUE) !important;
  }

  .p-inputtext.p-component {
    .p-inputtext:enabled:hover {
      border-color: var(--DARK_BLUE);
    }
  }

  .p-button.p-button-text {
    background-color: transparent;
    color: var(--DARK_BLUE);
    border-color: transparent;
  }
}
</style>
