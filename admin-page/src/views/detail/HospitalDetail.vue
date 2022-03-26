<script setup>
import { defineAsyncComponent, onBeforeMount } from "vue";
import { RouterLink } from "vue-router";
import dayjs from "dayjs";
import Breadcrumb from "primevue/breadcrumb";
import Divider from "primevue/divider";

import { formatDate } from "../../utils";

import HospitalRepo from "../../api/HospitalRepo";

const AsyncRequestHistory = defineAsyncComponent({
  loader: () => import("../../components/tables/RequestHistoryTable.vue"),
});

const requestHistory = [
  {
    _id: "800000100001",
    hospital_id: "62272d644a3850baa259a81b",
    hospital_name: "Hoan My",
    dateRequested: new Date("2022-09-13").getTime(),
    blood: {
      name: "O",
      type: "Negative",
    },
    amount: 500,
  },
  {
    _id: "800000100002",
    hospital_id: "62272d9d4a3850baa259a81c",
    hospital_name: "Nga 6",
    dateRequested: new Date("2022-09-27").getTime(),
    blood: {
      name: "A",
      type: "Positive",
    },
    amount: 350,
  },
  {
    _id: "800000100003",
    hospital_id: "62272db04a3850baa259a81d",
    hospital_name: "O Mon",
    dateRequested: new Date("2022-10-05").getTime(),
    blood: {
      name: "AB",
      type: "Positive",
    },
    amount: 350,
  },
];
// *** END of mock data **
const { _id } = defineProps({
  _id: String,
});

let hospital = $ref(null);
let showRequestHistory = $ref(false);

// Naviagtion settings
const home = $ref({
  icon: "fa-solid fa-hospital",
  to: { name: "Hospitals Management" },
});
let items = $ref(null);

onBeforeMount(async () => {
  const { data } = await HospitalRepo.get(_id);
  hospital = data;
  items = [
    {
      label: hospital ? `${hospital.hospital_name}` : "Unknown hospital",
    },
  ];
});
</script>

<template>
  <div class="grid">
    <div class="col-12">
      <!-- Navigation -->
      <Breadcrumb
        :home="home"
        :model="items"
        style="margin-bottom: 1rem; border-radius: 15px"
      />

      <!-- Hospital Content -->
      <div class="card">
        <div class="card__content">
          <div class="card__header">
            <h2 class="card-title">{{ hospital && hospital.hospital_name }}</h2>
            <!-- Edit Button -->
            <RouterLink
              :to="{
                name: 'Hospital Edit',
                params: {
                  _id,
                  hospitalData: JSON.stringify(hospital),
                },
              }"
              v-ripple
              class="p-button p-button-sm p-component mb-2 p-ripple app-router-link-icon edit-btn"
            >
              <i class="fa-solid fa-pen-to-square"></i>
              Edit
            </RouterLink>
          </div>

          <!-- Details -->
          <div class="card__information">
            <div class="section">
              <p>
                <i class="fa-solid fa-passport"></i>
                {{ hospital && hospital._id }}
              </p>
              <p>
                <i class="fa-solid fa-location-pin"></i>
                {{ hospital && hospital.address }}
              </p>
              <p>
                <i class="fa-solid fa-phone"></i>
                {{ hospital && hospital.phone }}
              </p>
            </div>
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
          <AsyncRequestHistory :requestHistory="requestHistory" />
        </template>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
@import "../../assets/styles/badge.scss";
.card {
  &__content {
    flex: 1 1 50%;

    p {
      padding-left: 1rem;
      i {
        color: var(--primary-color);
        font-size: 1.2rem;
        padding-right: 1rem;
      }
    }
  }

  &__header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1rem;

    .card-title {
      color: var(--primary-color);
      font-weight: 900;
    }
  }
}
</style>
