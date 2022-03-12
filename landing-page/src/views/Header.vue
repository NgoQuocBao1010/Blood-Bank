<script setup>
import TabMenu from "primevue/tabmenu";

const items = [
  { label: "HOME", to: "/" },
  { label: "ABOUT DONATION", to: "/about" },
  { label: "WANT TO DONATE", to: "/donate" },
  { label: "LOGIN", to: "/login" },
];
</script>

<template>
  <div class="navbar_header">
    <div class="logo">
      <img src="../assets/logo-blood_bank.webp" alt="logo" />
    </div>
    <div class="navbar">
      <TabMenu :model="items" />
    </div>
  </div>
</template>

<style lang="scss">
.navbar_header {
  margin-right: 100px;
  display: flex;
  justify-content: space-between;
  align-items: center;

  .logo img {
    background-color: #fff;
    width: 150px;
  }

  .navbar {
    .p-tabmenu .p-tabmenu-nav {
      border: none;

      .p-tabmenuitem {
        .p-menuitem-link:hover {
          background-color: var(--PRIMARY_COLOR);
        }
      }

      .p-tabmenuitem.p-highlight {
        .p-menuitem-link {
          background: var(--PRIMARY_COLOR);
          color: #fff;
        }
      }

      .p-menuitem-link {
        background: #fff;
        color: var(--PRIMARY_COLOR);
      }
    }
  }
}
</style>
