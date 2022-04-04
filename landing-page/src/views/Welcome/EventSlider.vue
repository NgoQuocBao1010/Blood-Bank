<script setup>
import { ref, onBeforeMount } from "vue";
import Button from "primevue/button";
import Carousel from "primevue/carousel";
import { useRouter } from "vue-router";
import { useEventStore } from "../../stores/event";
import ProgressSpinner from "primevue/progressspinner";

const eventStore = useEventStore();
const router = useRouter();

const events = ref(null);
const today = new Date();
const now = new Date(today).getTime();

onBeforeMount(async () => {
  if (!eventStore.events) {
    await eventStore.setEvents();
  }
  events.value = eventStore.activeEvents;
});

const handleClick = (eventId) => {
  router.push({
    path: `/donate/${eventId}`,
    query: { now: now },
  });
};
</script>

<template>
  <div class="card">
    <template v-if="!events">
      <div
        class="flex align-items-center justify-content-center"
        style="width 400px; height: 400px; font-size: 50px"
      >
        <ProgressSpinner strokeWidth="4" />
      </div>
    </template>
    <Carousel
      :value="events"
      :numVisible="1"
      :numScroll="1"
      :circular="true"
      :autoplayInterval="2000"
      class="carousel-slider"
    >
      <template #item="slotProps">
        <div
          class="product-item"
          :style="{
            backgroundImage: 'url(' + slotProps.data.bgImg + ')',
          }"
        >
          <div class="product-item-content">
            <Button @click="() => handleClick(slotProps.data._id)"
              >Donate Now</Button
            >
          </div>
        </div>
      </template>
    </Carousel>
  </div>
</template>

<style lang="scss" scoped>
.p-carousel-container {
  button {
    background-color: var(--PRIMARY_COLOR) !important;
  }
}

.product-item {
  display: block;
  width: 100%;
  height: 400px;
  background-size: cover;
  background-repeat: no-repeat;

  &-content {
    height: 100%;
    display: flex;
    justify-content: flex-end;
    align-items: flex-end;

    button {
      background-color: #1e2d50 !important;
      border: 1px solid #fff;
      color: #fff;
      margin-bottom: 20px;
      margin-right: 20px;
    }

    button:hover {
      background-color: var(--PRIMARY_COLOR) !important;
      border: 1px solid #fff;
      color: #fff;
      transition: background-color linear 0.2s, color linear 0.2s,
        border linear 0.2s;
    }
  }

  .product-image {
    width: 100%;
    display: block;
    height: 400px;
  }
}

.p-carousel-indicators.p-reset .p-carousel-indicator.p-highlight {
  .p-link button {
    background-color: var(--DARK_BLUE) !important;
  }
}
</style>
