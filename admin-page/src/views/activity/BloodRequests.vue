<script setup>
import { onBeforeMount } from "vue";
import RequestHistoryTable from "../../components/tables/RequestHistoryTable.vue";
import RequestRepo from "../../api/RequestRepo";

let requestHistory = $ref(null);

onBeforeMount(async () => {
  const { data } = await RequestRepo.getAll();
  requestHistory = data;
});
</script>

<template>
  <div class="grid">
    <div class="col-12">
      <div class="card">
        <!-- Page headers -->
        <h2>Blood Requests Monitor</h2>

        <!-- Blood Request table -->
        <RequestHistoryTable
          v-if="requestHistory"
          :requestHistory="requestHistory"
          :isAcitivy="true"
        />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
