<script setup>
import EventSlider from "./EventSlider.vue";
import AboutDonationSection from "./AboutDonationSection.vue";
import DonationTypesSection from "./DonationTypesSection.vue";
</script>

<template>
  <div class="welcome">
    <div class="section slider-section">
      <EventSlider />
    </div>
    <div class="section about_donation-section">
      <AboutDonationSection />
    </div>

    <div class="section donation_type-section">
      <DonationTypesSection />
    </div>
  </div>
</template>

<style lang="scss" scoped>
.welcome {
  .section {
    border-bottom: 1px solid #ccc;
    padding: 20px 0;
  }
}
</style>
