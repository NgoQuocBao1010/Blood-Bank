<script setup>
import Button from "primevue/button";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import { ref } from "@vue/reactivity";
import { useRouter } from "vue-router";

const router = useRouter();
const onClick = () => {
  router.push({ path: "donate" });
};

const products = ref([
  {
    blood_type: "A+",
    blood_donate: "A+ AB+",
    blood_receive: "A+ A- O+ O-",
  },
  {
    blood_type: "O+",
    blood_donate: "O+ A+ B+ AB+	",
    blood_receive: "O+ O-",
  },
  {
    blood_type: "B+",
    blood_donate: "B+ AB+",
    blood_receive: "B+ B- O+ O-",
  },
  {
    blood_type: "AB+",
    blood_donate: "AB+",
    blood_receive: "Everyone",
  },
  {
    blood_type: "A-",
    blood_donate: "A+ A- AB+ AB-",
    blood_receive: "A- O-",
  },
  {
    blood_type: "O-",
    blood_donate: "Everyone",
    blood_receive: "O-",
  },
  {
    blood_type: "B-",
    blood_donate: "B+ B- AB+ AB-",
    blood_receive: "B- O-",
  },
  {
    blood_type: "AB-",
    blood_donate: "AB+ AB-",
    blood_receive: "AB- A- B- O-",
  },
]);
</script>

<template>
  <main>
    <div class="heading">
      <h1>LEARN ABOUT DONATION</h1>
    </div>
    <div class="body">
      <div class="aside">
        <img
          src="https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/donationFact.webp"
          alt=""
        />
        <p>
          After donating blood, the body works to replenish the blood loss. This
          stimulates the production of new blood cells and in turn, helps in
          maintaining good health.
        </p>
        <Button @click="onClick" label="Donate Now" />
      </div>
      <div class="blood_table">
        <DataTable :value="products" showGridlines responsiveLayout="scroll">
          <template #header>Compatible Blood Type Donors</template>
          <Column field="blood_type" header="Blood Type"></Column>
          <Column field="blood_donate" header="Donate Blood To"></Column>
          <Column field="blood_receive" header="Receive Blood From"></Column>
        </DataTable>
      </div>
    </div>
  </main>
</template>

<style lang="scss" scoped>
.about_donation-section {
  main {
    padding: 0 45px;
    .heading {
      text-align: center;
      h1 {
        color: var(--PRIMARY_COLOR);
        margin-bottom: 40px;
      }
    }

    .body {
      display: flex;
      justify-content: space-between;
      align-items: center;
      .aside {
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;

        p {
          color: #292929;
          line-height: 1.8rem;
          font-size: 16px;
          text-align: center;
          font-weight: 700;
          margin-top: 10px;
        }

        img {
          width: 650px;
        }

        button {
          width: 150px;
          background-color: var(--PRIMARY_COLOR);
          color: #fff;
          border: 1px solid #fff;
          margin-top: 40px;
        }

        button:hover {
          background-color: var(--SECONDARY_COLOR);
          color: #fff;
          transition: background-color linear 0.2s, color linear 0.2s;
        }
      }
    }
  }
}
</style>
