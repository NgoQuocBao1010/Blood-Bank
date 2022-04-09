<script setup>
import { onBeforeMount } from "vue";
import RequestHistoryTable from "../../components/tables/RequestHistoryTable.vue";
import RequestRepo from "../../api/RequestRepo";
import Dropdown from "primevue/dropdown";
import ProgressBar from "primevue/progressbar";

let requestHistory = $ref(null);
let showSelection = $ref(true);

// Filter requests
let sortKey = $ref();
let sortOptions = $ref([
  { label: "Pending", value: "0" },
  { label: "Approved", value: "1" },
  { label: "Rejected", value: "-1" },
]);

const onSortChange = async (event) => {
  const value = event?.value.value;
  requestHistory = null;

  if (!value || value === "0") {
    const { data } = await RequestRepo.getPending();
    requestHistory = data;
    showSelection = true;
  }
  if (value === "1") {
    const { data } = await RequestRepo.getApproved();
    requestHistory = data;
    showSelection = false;
  } else if (value === "-1") {
    const { data } = await RequestRepo.getRejected();
    requestHistory = data;
    showSelection = false;
  }
};

const updateRequests = async () => {
  requestHistory = null;
  const { data } = await RequestRepo.getPending();
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
        <div class="flex justify-content-between">
          <h2>Blood Requests Monitor</h2>
          <div class="switch">
            <span class="pr-4">Change this status to filter</span>
            <Dropdown
              v-model="sortKey"
              :options="sortOptions"
              optionLabel="label"
              placeholder="Filter By Status"
              @change="onSortChange($event)"
            />
          </div>
        </div>

        <!-- Blood Request table -->
        <RequestHistoryTable
          v-if="requestHistory"
          :requestHistory="requestHistory"
          :isActivity="true"
          :showSelection="showSelection"
          @updateRequests="updateRequests"
        />

        <!-- Progress bar -->
        <div
          class="flex flex-center"
          style="flex-direction: column; margin-block: 3rem"
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

<style lang="scss"></style>
