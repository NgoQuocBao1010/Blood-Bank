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

const route = useRoute();
const eventId = route.params.eventId;

const donateDialog = ref(false);
const submitted = ref(false);
const toast = useToast();
const today = new Date();

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

const formatDate = (date) => {
  return moment(date).format("DD/MM/YYYY");
};

const handleSubmitForm = async () => {
  submitted.value = true;
  v$.value.$validate();
  if (!v$.value.$error) {
    donateData.dob = Math.floor(new Date(donateData.dob) / 1000).toString();
    donateData.latestDonationDate = donateData.latestDonationDate
      ? (Math.floor(new Date(donateData.latestDonationDate)) / 1000).toString()
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
    <Toast position="top-right" class="pt-8" />
    <div class="col-12">
      <div class="card">
        <div class="form-donation flex justify-content-center">
          <div class="card py-4 px-4">
            <h2 class="text-center" style="color: var(--DARK_BLUE)">
              Donation Form
            </h2>
            <div class="p-fluid">
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

              <div class="field">
                <label for="idCardNumber">Identify Card Number</label>
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

              <Button
                type="submit"
                label="Submit"
                class="p-button-text w-full"
                @click="handleSubmitForm"
              />
            </div>
          </div>

          <div class="col-2">
            <Divider layout="vertical">
              <b>to</b>
            </Divider>
          </div>
          <div class="col-5 align-items-center justify-content-center">
            <h2 style="color: var(--PRIMARY_COLOR)">Event TITLE</h2>
            <br />
            <Divider layout="horizontal" align="left">
              <span
                class="p-tag flex justify-content-center align-items-center"
              >
                <i class="pi pi-search mr-2"></i>
                Description</span
              >
            </Divider>
            <p class="line-height-3 m-0">
              Event description: Sed ut perspiciatis unde omnis iste natus error
              sit voluptatem accusantium doloremque laudantium, totam rem
              aperiam, eaque ipsa quae ab illo inventore veritatis et quasi
              architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam
              voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed
              quia consequuntur magni dolores eos qui ratione voluptatem sequi
              nesciunt. Consectetur, adipisci velit, sed quia non numquam eius
              modi.
            </p>

            <Divider layout="horizontal" align="left">
              <span
                class="p-tag flex justify-content-center align-items-center"
              >
                <i class="pi pi-calendar-times mr-2"></i>
                Time</span
              >
            </Divider>

            <p class="line-height-3 m-0">
              At vero eos et accusamus et iusto odio dignissimos ducimus qui
              blanditiis praesentium voluptatum deleniti atque corrupti quos
              dolores et quas molestias excepturi sint occaecati cupiditate non
              provident, similique sunt in culpa qui officia deserunt mollitia
              animi, id est laborum et dolorum fuga. Et harum quidem rerum
              facilis est et expedita distinctio. Nam libero tempore, cum soluta
              nobis est eligendi optio cumque nihil impedit quo minus.
            </p>

            <Divider layout="horizontal" align="left">
              <span
                class="p-tag flex justify-content-center align-items-center"
              >
                <i class="pi pi-map-marker mr-2"></i>
                Location</span
              >
            </Divider>

            <p class="line-height-3 m-0">
              Temporibus autem quibusdam et aut officiis debitis aut rerum
              necessitatibus saepe eveniet ut et voluptates repudiandae sint et
              molestiae non recusandae. Itaque earum rerum hic tenetur a
              sapiente delectus, ut aut reiciendis voluptatibus maiores alias
              consequatur aut perferendis doloribus asperiores repellat. Donec
              vel volutpat ipsum. Integer nunc magna, posuere ut tincidunt eget,
              egestas vitae sapien. Morbi dapibus luctus odio.
            </p>
          </div>
        </div>
      </div>
    </div>
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
