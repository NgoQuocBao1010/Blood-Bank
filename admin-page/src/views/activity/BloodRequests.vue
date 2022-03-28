<script setup>
import RequestHistoryTable from "../../components/tables/RequestHistoryTable.vue";

// *** Mock data ***
const requestHistory = [
  {
    _id: "800000100001",
    hospital_id: "62272d644a3850baa259a81b",
    hospital_name: "Benh vien Hoan My",
    dateRequested: new Date("2022-09-13").getTime(),
    blood: {
      name: "O",
      type: "Negative",
    },
    amount: 500,
  },
  {
    _id: "800000100002",
    hospital_id: "62272d644a3850baa259a81c",
    hospital_name: "Nga 6",
    dateRequested: new Date("2022-09-27").getTime(),
    blood: {
      name: "A",
      type: "Positive",
    },
    amount: 350,
  },
  {
    _id: "800000100003",
    hospital_id: "62272db04a3850baa259a81d",
    hospital_name: "O Mon",
    dateRequested: new Date("2022-10-05").getTime(),
    blood: {
      name: "AB",
      type: "Positive",
    },
    amount: 350,
  },
];
// *** END of mock data ***
</script>

<template>
  <div class="grid">
    <div class="col-12">
      <div class="card">
        <!-- Page headers -->
        <h2>Blood Requests Monitor</h2>

        <!-- Blood Request table -->
        <RequestHistoryTable
          :requestHistory="requestHistory"
          :isAcitivy="true"
        />
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped></style>
