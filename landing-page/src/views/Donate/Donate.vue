<script setup>
import { ref, onBeforeMount, reactive } from "vue";
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
import { formatDate } from "../../utils/index";
import { useEventStore } from "../../stores/event";

// const dataviewValue = ref(null);
const layout = ref("grid");
const router = useRouter();
const eventStore = useEventStore();
let sortKey = ref();
let sortOrder = ref();
let sortField = ref();
let sortOptions = ref([
  { label: "On Coming", value: "status" },
  { label: "Up Coming", value: "!status" },
  { label: "Passed", value: "!!status" },
]);

onBeforeMount(async () => {
  if (!eventStore.events) {
    await eventStore.setEvents();
  }
});

const onSortChange = (event) => {
  console.log(event);
  const value = event.value.value;
  const sortValue = event.value;

  if (value.indexOf("!") === 0) {
    sortOrder.value = -1;
    sortField.value = value.substring(1, value.length);
    sortKey.value = sortValue;
  } else {
    sortOrder.value = 1;
    sortField.value = value;
    sortKey.value = sortValue;
  }
};

const handleClick = (eventId) => {
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
          :value="eventStore.events"
          :layout="layout"
          :paginator="true"
          :rows="9"
          :sortOrder="sortOrder"
          :sortField="sortField"
        >
          <template #header>
            <div class="grid grid-nogutter">
              <div class="col-6" style="text-align: left">
                <Dropdown
                  v-model="sortKey"
                  :options="sortOptions"
                  optionLabel="label"
                  placeholder="Sort By Status"
                  @change="onSortChange($event)"
                />
              </div>
              <div class="col-6" style="text-align: right">
                <DataViewLayoutOptions v-model="layout" />
              </div>
            </div>
          </template>
          <!-- Rows layout -->
          <template #list="slotProps">
            <div class="col-12" @click="() => handleClick(slotProps.data._id)">
              <div
                class="flex flex-column md:flex-row align-items-center p-3 w-full event-item"
              >
                <img
                  :src="slotProps.data.bgImg"
                  :alt="slotProps.data.name"
                  class="my-4 md:my-0 w-9 md:w-10rem shadow-2 mr-5"
                  style="height: 70px"
                />
                <div class="flex-1 text-center md:text-left">
                  <div class="font-bold text-2xl">
                    {{ slotProps.data.name }}
                  </div>
                  <div class="mb-3">{{ slotProps.data.title }}</div>
                  <span
                    class="text-xl font-semibold mb-2 align-self-center md:align-self-end"
                  >
                    <i class="pi pi-calendar-times text-xl"></i>
                    Start Date: {{ formatDate(slotProps.data.startDate) }}</span
                  >
                </div>
              </div>
            </div>
          </template>

          <!-- Grid layout -->
          <template #grid="slotProps">
            <div
              class="col-12 md:col-4"
              @click="() => handleClick(slotProps.data._id)"
            >
              <div class="card m-3 border-1 surface-border event-item">
                <div class="event-grid-item-top">
                  <span
                    :class="
                      'event-badge status-' +
                      slotProps.data.status.toLowerCase()
                    "
                    >{{ slotProps.data.status }}</span
                  >
                </div>
                <div class="text-center">
                  <img
                    :src="slotProps.data.bgImg"
                    :alt="slotProps.data.name"
                    class="w-9 shadow-2 my-3 mx-0"
                    style="height: 150px"
                  />
                  <div
                    class="text-2xl font-bold"
                    style="color: var(--PRIMARY_COLOR)"
                  >
                    {{ slotProps.data.name }}
                  </div>
                  <div class="mb-3">{{ slotProps.data.title }}</div>
                </div>
                <div class="flex justify-content-between">
                  <div class="flex align-items-center justify-content-between">
                    <span class="text-xl font-semibold">
                      <i class="pi pi-calendar-times text-xl"></i>
                      {{ formatDate(slotProps.data.startDate) }}</span
                    >
                  </div>
                  <div>
                    <span class="text-xl font-semibold">
                      <i class="pi pi-map-marker text-xl"></i>
                      {{ slotProps.data.location.city }}</span
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
