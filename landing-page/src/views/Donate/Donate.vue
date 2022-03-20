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
import { useRouter } from "vue-router";

const dataviewValue = ref(null);
const layout = ref("grid");
const router = useRouter();
console.log(router);
onMounted(() => {
  dataviewValue.value = [
    {
      id: "6235eacaed0d1dd101c87436",
      title: "Event 1",
      location: "O Mon",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/mobile_banner_center_2500_600.webp",
      time: "01/01/2022",
    },
    {
      id: "6235eaf2ed0d1dd101c87437",
      title: "Event 2",
      location: "Nga 6",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/updated_NewYear2022.webp",
      time: "15/03/2022",
    },
    {
      id: 3,
      title: "Event 3",
      location: "Hung Phu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/ONE_NATION_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 4,
      title: "Event 4",
      location: "Giong Rieng",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/Blood_EHealthID_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 5,
      title: "Event 5",
      location: "Ninh Kieu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/mobile_banner_center_2500_600.webp",
      time: "01/01/2022",
    },
    {
      id: 6,
      title: "Event 5",
      location: "Ninh Kieu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/mobile_banner_center_2500_600.webp",
      time: "01/01/2022",
    },
    {
      id: 7,
      title: "Event 7",
      location: "Giong Rieng",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/Blood_EHealthID_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 8,
      title: "Event 8",
      location: "Hung Phu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/ONE_NATION_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 8,
      title: "Event 8",
      location: "Hung Phu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/ONE_NATION_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 8,
      title: "Event 8",
      location: "Hung Phu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/ONE_NATION_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 8,
      title: "Event 8",
      location: "Hung Phu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/ONE_NATION_2500_600.webp",
      time: "18/04/2022",
    },
    {
      id: 8,
      title: "Event 8",
      location: "Hung Phu",
      description: "Description abcdefqweqweqweqweqweqweqweqwe",
      bgImg:
        "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/ONE_NATION_2500_600.webp",
      time: "18/04/2022",
    },
  ];
});

const handleClick = (eventId) => {
  console.log("event ID", { eventId: eventId });
  router.push({
    path: `/donate/${eventId}`,
  });
};
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
        >
          <template #header>
            <div class="flex align-items-right">
              <DataViewLayoutOptions v-model="layout" />
            </div>
          </template>
          <template #list="slotProps">
            <div class="col-12" @click="() => handleClick(slotProps.data.id)">
              <div
                class="flex flex-column md:flex-row align-items-center p-3 w-full event-item"
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
              </div>
            </div>
          </template>

          <template #grid="slotProps">
            <div
              class="col-12 md:col-4"
              @click="() => handleClick(slotProps.data.id)"
            >
              <div class="card m-3 border-1 surface-border event-item">
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
                <div class="flex justify-content-between">
                  <div class="flex align-items-center justify-content-between">
                    <span class="text-xl font-semibold">
                      <i class="pi pi-calendar-times text-xl"></i>
                      {{ slotProps.data.time }}</span
                    >
                  </div>
                  <div>
                    <span class="text-xl font-semibold">
                      <i class="pi pi-map-marker text-xl"></i>
                      {{ slotProps.data.location }}</span
                    >
                  </div>
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
@import "../../assets/styles/badges.scss";
.card {
  background-color: var(--surface-card);
  padding: 1.5rem;
  color: var(--surface-900);
  margin-bottom: 1rem;
  border-radius: 12px;
  box-shadow: 0 3px 5px rgba(0, 0, 0, 0.02), 0 0 2px rgba(0, 0, 0, 0.05),
    0 1px 4px rgba(0, 0, 0, 0.08) !important;

  .event-item:hover {
    cursor: pointer;
    opacity: 0.75;
    transition: opacity 0.2s;
  }
}
</style>
