<script setup>
import { ref, onMounted } from "vue";
import Button from "primevue/button";
import Dropdown from "primevue/dropdown";
import DataViewLayoutOptions from "primevue/dataviewlayoutoptions";
import DataView from "primevue/dataview";
import Rating from "primevue/rating";

const dataviewValue = ref(null);
const layout = ref("grid");

onMounted(() => {
  dataviewValue.value = [
    {
      id: 1,
      title: "Event 1",
      address: "O Mon",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/mobile_banner_center_2500_600.webp",
      time: "01/01/2022",
    },
    {
      id: 2,
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
  ];
});
</script>

<template>
  <div class="grid px-2">
    <div class="col-12">
      <div class="card">
        <div class="text-center">
          <h1 class="text-900 font-normal mb-4 text-4xl">Blood Events</h1>
        </div>
        <DataView
          :value="dataviewValue"
          :layout="layout"
          :paginator="true"
          :rows="9"
          :sortOrder="sortOrder"
          :sortField="sortField"
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
                  <Button label="Donate now" class="mb-2"></Button>
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
                  <Button label="Donate now"></Button>
                </div>
              </div>
            </div>
          </template>
        </DataView>
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss">
@import "../assets/styles/badges.scss";

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
</style>
