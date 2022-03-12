<script setup>
import { RouterView, RouterLink } from "vue-router";
import Header from "./views/Header.vue";
import Footer from "./views/Footer.vue";
</script>

<template>
  <main>
    <div class="header">
      <Header />
    </div>
    <div class="content">
      <RouterView />
    </div>
    <div class="footer">
      <Footer />
    </div>
  </main>
</template>

<style lang="scss">
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

html {
  background-color: #fff !important;
  color: #ccc;
  font-family: Arial, Helvetica, sans-serif;
}

main {
}

.header {
  padding: 20px 60px;
}

.footer {
  text-align: center;
  padding: 20px;
  background-color: var(--DARK_BLUE);
}
</style>
