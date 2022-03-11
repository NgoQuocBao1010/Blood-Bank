<script setup>
import { RouterView, RouterLink } from "vue-router";
import Header from "./views/Header.vue";
</script>

<template>
  <main>
    <div class="header">
      <Header />
    </div>
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
  font-family: Arial, Helvetica, sans-serif;
}

main {
}

.header {
  padding: 20px 60px;
}
</style>
