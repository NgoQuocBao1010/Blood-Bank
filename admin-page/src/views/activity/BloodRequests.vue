<script setup>
import { onBeforeMount } from "vue";
import RequestHistoryTable from "../../components/tables/RequestHistoryTable.vue";
import RequestRepo from "../../api/RequestRepo";

let requestHistory = $ref(null);

const updateRequests = async () => {
  requestHistory = null;
  const { data } = await RequestRepo.getAll();
  requestHistory = data;
};

onBeforeMount(async () => {
  await updateRequests();
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
          @updateRequests="updateRequests"
        />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
