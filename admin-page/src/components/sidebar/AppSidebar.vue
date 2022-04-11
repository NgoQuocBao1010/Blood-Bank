<script setup>
import { useUserStore } from "../../stores/user";
import SidebarMenu from "./SidebarMenu.vue";

const userStore = useUserStore();

const adminMenu = [
  {
    label: "Overview",
    items: [
      {
        label: "Dashboard",
        icon: "pi pi-fw pi-home",
        to: userStore.defaultPage,
      },
      {
        label: "Users",
        icon: "fa-solid fa-user pi-fw",
        to: { name: "Users Management" },
      },
    ],
  },
  // Management
  {
    label: "Management",
    items: [
      {
        label: "Blood",
        icon: "fa-solid fa-droplet pi-fw",
        to: { name: "Blood Management" },
      },
      {
        label: "Donors",
        icon: "fa-solid fa-user-group pi-fw",
        to: { name: "Donors Management" },
      },
      {
        label: "Events",
        icon: "fa-solid fa-calendar-days pi-fw",
        to: { name: "Events Management" },
      },
      {
        label: "Hospitals",
        icon: "fa-solid fa-hospital pi-fw",
        to: { name: "Hospitals Management" },
      },
    ],
  },
  // Activity
  {
    label: "Activity",
    items: [
      {
        label: "Donation",
        icon: "fa-solid fa-hand-holding-droplet fa-droplet pi-fw",
        to: { name: "Donation Requests" },
      },
      {
        label: "Blood Requests",
        icon: "fa-solid fa-hospital pi-fw",
        to: { name: "Blood Requests" },
      },
    ],
  },
  // Information
  {
    label: "Information",
    items: [
      {
        label: "About",
        icon: "pi pi-fw pi-info-circle",
        to: { name: "About" },
      },
      {
        label: "View Source",
        icon: "pi pi-fw pi-search",
        newTab: true,
        url: "https://github.com/NgoQuocBao1010/Blood-Bank",
      },
    ],
  },
];

const hospitalMenu = [
  {
    label: "Overview",
    items: [
      {
        label: "Dashboard",
        icon: "pi pi-fw pi-home",
        to: userStore.defaultPage,
      },
    ],
  },
  {
    label: "Overview",
    items: [
      {
        label: "Profile",
        icon: "fa-solid fa-hospital-user pi-fw",
        to: userStore.profilePage,
      },
    ],
  },
  // Information
  {
    label: "Information",
    items: [
      {
        label: "About",
        icon: "pi pi-fw pi-info-circle",
        to: { name: "About" },
      },
      {
        label: "View Source",
        icon: "pi pi-fw pi-search",
        newTab: true,
        url: "https://github.com/NgoQuocBao1010/Blood-Bank",
      },
    ],
  },
];
</script>

<template>
  <div class="layout-menu-container" v-if="userStore.isLoggedIn">
    <SidebarMenu
      class="layout-menu"
      :items="userStore.isAdmin ? adminMenu : hospitalMenu"
      :root="true"
    />
  </div>
</template>

<style lang="scss" scoped></style>
