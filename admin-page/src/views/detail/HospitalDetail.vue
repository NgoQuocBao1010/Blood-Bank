<script setup>
import { defineAsyncComponent, onBeforeMount } from "vue";
import { RouterLink } from "vue-router";
import Breadcrumb from "primevue/breadcrumb";

import HospitalRepo from "../../api/HospitalRepo";
import RequestRepo from "../../api/RequestRepo";

const AsyncRequestHistory = defineAsyncComponent({
  loader: () => import("../../components/tables/RequestHistoryTable.vue"),
});

// const requestHistory = [
//   {
//     _id: "800000100001",
//     hospital_id: "62272d644a3850baa259a81b",
//     name: "Hoan My",
//     dateRequested: new Date("2022-09-13").getTime(),
//     blood: {
//       name: "O",
//       type: "Negative",
//     },
//     amount: 500,
//   },
//   {
//     _id: "800000100002",
//     hospital_id: "62272d9d4a3850baa259a81c",
//     name: "Nga 6",
//     dateRequested: new Date("2022-09-27").getTime(),
//     blood: {
//       name: "A",
//       type: "Positive",
//     },
//     amount: 350,
//   },
//   {
//     _id: "800000100003",
//     hospital_id: "62272db04a3850baa259a81d",
//     name: "O Mon",
//     dateRequested: new Date("2022-10-05").getTime(),
//     blood: {
//       name: "AB",
//       type: "Positive",
//     },
//     amount: 350,
//   },
// ];
// *** END of mock data **
const { _id } = defineProps({
  _id: String,
});

let requestHistory = $ref(null);
let hospital = $ref(null);
let showRequestHistory = $ref(false);

// Naviagtion settings
const home = $ref({
  icon: "fa-solid fa-hospital",
  to: { name: "Hospitals Management" },
});
let items = $ref(null);

onBeforeMount(async () => {
  const hospitalData = await HospitalRepo.get(_id);
  const requestData = await RequestRepo.getAll();

  requestHistory = requestData.data;

  console.log(requestHistory);
  hospital = hospitalData.data;

  items = [
    {
      label: hospital ? `${hospital.name}` : "Unknown hospital",
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
            <h2 class="card-title">
              {{ hospital && hospital.name }}
            </h2>
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
