<script setup>
import EventSlider from "./EventSlider.vue";
import Content from "./Content.vue";
</script>

<template>
  <div class="welcome">
    <div class="section slider-section">
      <EventSlider />
    </div>
    <div class="section content-section">
      <Content />
    </div>
  </div>
</template>

<style lang="scss" scoped>
.section {
}
</style>
