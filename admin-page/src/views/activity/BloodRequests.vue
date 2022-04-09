<script setup>
import { onBeforeMount, watch } from "vue";
import ProgressBar from "primevue/progressbar";

import RequestRepo from "../../api/RequestRepo";
import RequestHistoryTable from "../../components/tables/RequestHistoryTable.vue";

let requestHistory = $ref(null);
let fetchingRequests = $ref(false);
const fetchData = (type) => {
  return {
    pending: RequestRepo.getPending(),
    rejected: RequestRepo.getRejected(),
    approved: RequestRepo.getApproved(),
  }[type];
};

const tabs = ["approved", "rejected", "pending"];
let requestType = $ref("pending");
watch(
  () => requestType,
  async () => {
    await fetchrequestHistory();
  }
);

const fetchrequestHistory = async () => {
  fetchingRequests = true;
  try {
    requestHistory = null;
    const { data } = await fetchData(requestType);
    requestHistory = data;
  } catch (e) {
    throw e;
  } finally {
    fetchingRequests = false;
  }
};

const updateRequests = async () => {
  requestType = "pending";
  await fetchrequestHistory();
};

onBeforeMount(async () => {
  await fetchrequestHistory();
});
</script>

<template>
  <div class="grid">
    <div class="col-12">
      <div class="card">
        <!-- Page headers -->
        <div class="flex header">
          <h2>Blood Requests Monitor</h2>
        </div>

        <!-- Donor Type Selections -->
        <ul class="tab-wrapper">
          <li
            v-for="tab in tabs"
            v-ripple
            class="p-ripple tab"
            :class="{ active: tab === requestType }"
            @click="requestType = tab"
          >
            <p class="content">{{ tab }}</p>
          </li>
        </ul>

        <!-- Donor Table -->
        <div id="donor-table" v-if="requestHistory">
          <RequestHistoryTable
            :requestHistory="requestHistory"
            :isActivity="true"
            :isRejectParticipant="requestType === 'rejected'"
            :isApproveParticipant="requestType === 'approved'"
            @updateRequests="updateRequests"
          />
        </div>

        <!-- Progress bar -->
        <div
          class="flex flex-center"
          style="
            flex-direction: column;
            padding: 3rem 1rem;
            background-color: #f8f9fa;
          "
          v-else
        >
          <h5>Loading ...</h5>
          <ProgressBar
            mode="indeterminate"
            style="height: 0.5em; width: 100%"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.tab-wrapper {
  padding-top: 1rem;
  margin: 0;
  border-top: 1px solid rgb(236, 236, 236);
  display: flex;
  justify-content: flex-end;
  gap: 0.2rem;
  .tab {
    padding: 0.75rem 2rem;
    cursor: pointer;
    border-top-left-radius: 30px;
    border-top-right-radius: 30px;
    text-transform: capitalize;
    font-weight: 700;
    transition: all 0.3s ease;
    color: lightgray;
    position: relative;

    &:hover {
      background-color: #f8f9fa;
    }

    &.active {
      background-color: #f8f9fa;
      color: var(--primary-color);
    }

    .content {
      margin: 0;
      padding: 0;
    }
  }
}
</style>
