<script setup>
import TabMenu from "primevue/tabmenu";

const items = [
  { label: "HOME", to: "/" },
  { label: "ABOUT DONATION", to: "/about" },
  { label: "WANT TO DONATE", to: "/donate" },
];
</script>

<template>
  <div class="navbar_header">
    <div class="logo">
      <router-link to="/">
        <img src="../assets/images/logo.png" alt="logo" />
      </router-link>
    </div>
    <div class="navbar">
      <TabMenu :model="items" />
      <div class="login-tab">
        <a href="http://localhost:5000/admin/login">LOGIN</a>
      </div>
    </div>
  </div>
</template>

<style lang="scss">
.navbar_header {
  position: fixed;
  z-index: 9999;
  top: 0;
  left: 0;
  width: 100%;
  height: 6rem;
  padding: 0 60px;
  background-color: var(--surface-card);
  display: flex;
  align-items: center;
  box-shadow: 0px 3px 5px rgb(0 0 0 / 2%), 0px 0px 2px rgb(0 0 0 / 5%),
    0px 1px 4px rgb(0 0 0 / 8%);

  .logo {
    display: flex;
    align-items: center;
    color: var(--surface-900);
    font-size: 1.5rem;
    font-weight: 500;
    width: 300px;
    border-radius: 12px;

    img {
      height: 6rem;
    }
  }

  .navbar {
    display: flex;
    margin: 0 0 0 auto;

    .login-tab a {
      align-items: center;
      background-color: #ffffff;
      border-radius: 6px 6px 0px 0px;
      color: #f07279;
      display: flex;
      font-family: -apple-system;
      font-weight: 700;
      padding: 20px;
      text-align: left;
      text-decoration: none;
    }

    .login-tab a:hover {
      background-color: #f07279;
      color: #6c757d;
      transition: background-color linear 0.2s, color linear 0.2s;
    }

    .p-tabmenu .p-tabmenu-nav {
      border: none;

      .p-tabmenuitem {
        .p-menuitem-link:hover {
          background-color: var(--PRIMARY_COLOR);
          transition: background-color linear 0.2s, color linear 0.2s;
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

  .p-tabmenu
    .p-tabmenu-nav
    .p-tabmenuitem
    .p-menuitem-link:not(.p-disabled):focus {
    outline: 0 none;
    outline-offset: 0;
    box-shadow: inset 0 0 0 0.2rem var(--PRIMARY_COLOR);
  }
}
</style>
