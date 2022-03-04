<script setup>
import { RouterView, RouterLink } from "vue-router";
import Header from "./views/Header.vue";
</script>

<template>
  <main>
    <Header />
    <div class="content">
      <RouterView />
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
  background-color: #fff;
  color: #ccc;
}

main {
  padding: 20px 60px;
}
</style>
