<script setup>
import { onBeforeMount } from "vue";
import { useRoute } from "vue-router";
import HospitalRepo from "../../api/HospitalRepo";

const route = useRoute();
const hospitalId = route.params._id;
let hospital = $ref(null);

onBeforeMount(async () => {
  const hospitalData = await HospitalRepo.get(hospitalId);
  hospital = hospitalData.data;
});
</script>

<template>
  <div class="grid">
    <div class="col-12">
      <!-- Hospital Content -->
      <div class="card">
        <div class="card__content">
          <div class="card__header">
            <h2 class="card-title">{{ hospital && hospital.name }} Hospital</h2>
            <!-- Edit Button -->
            <RouterLink
              :to="{
                name: 'Hospital Edit Profile',
                params: {
                  hospitalId,
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
    </div>
  </div>
</template>
<style lang="scss" scoped>
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
