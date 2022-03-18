<script setup>
import TabView from "primevue/tabview";
import TabPanel from "primevue/tabpanel";
</script>

<template>
  <main>
    <div class="heading">
      <h1>TYPES OF DONATION</h1>
    </div>
    <div class="body">
      <div class="description">
        <p>
          The human body contains five liters of blood, which is made of several
          useful components i.e. <b>Whole blood</b>, <b>Platelet</b>, and
          <b>Plasma</b>.
        </p>
        <p>
          Each type of component has several medical uses and can be used for
          different medical treatments. your blood donation determines the best
          donation for you to make.
        </p>
        <p>
          For <b>plasma</b> and <b>platelet</b> donation you must have donated
          whole blood in past two years.
        </p>
      </div>

      <div class="tab_view-blood">
        <TabView ref="tabview1">
          <TabPanel header="Whole Blood">
            <div class="list-item">
              <div class="item">
                <h3>What is it?</h3>
                <p>
                  Blood Collected straight from the donor after its donation
                  usually separated into red blood cells, platelets, and plasma.
                </p>
                <h3>Who can donate?</h3>
                <p>
                  You need to be 18-65 years old, weigh 45kg or more and be fit
                  and healthy.
                </p>
              </div>
              <div class="item">
                <h3>User For?</h3>
                <p>
                  Stomach disease, kidney disease, childbirth, operations, blood
                  loss, trauma, cancer, blood diseases, haemophilia, anemia,
                  heart disease.
                </p>
                <h3>Lasts For?</h3>
                <p>Red cells can be stored for 42 days.</p>
              </div>
              <div class="item">
                <h3>How long does it take?</h3>
                <p>15 minutes to donate.</p>
                <h3>How often can I donate?</h3>
                <p>Every 12 weeks</p>
              </div>
            </div>
          </TabPanel>
          <TabPanel header="Plasma">
            <div class="list-item">
              <div class="item">
                <h3>What is it?</h3>
                <p>
                  The straw-coloured liquid in which red blood cells, white
                  blood cells, and platelets float in.Contains special nutrients
                  which can be used to create 18 different type of medical
                  products to treat many different medical conditions.
                </p>
                <h3>Who can donate?</h3>
                <p>
                  You need to be 18-70 (men) or 20-70 (women) years old, weigh
                  50kg or more and must have given successful whole blood
                  donation in last two years.
                </p>
              </div>
              <div class="item">
                <h3>User For?</h3>
                <p>
                  Immune system conditions, pregnancy (including anti-D
                  injections), bleeding, shock, burns, muscle and nerve
                  conditions, haemophilia, immunisations.
                </p>
                <h3>Lasts For?</h3>
                <p>Plasma can last up to one year when frozen.</p>
              </div>
              <div class="item">
                <h3>How does it work?</h3>
                <p>
                  We collect your blood, keep plasma and return rest to you by
                  apheresis donation.
                </p>
                <h3>How long does it take?</h3>
                <p>15 minutes to donate.</p>
                <h3>How often can I donate?</h3>
                <p>Every 2-3 weeks.</p>
              </div>
            </div>
          </TabPanel>
          <TabPanel header="Platelet">
            <div class="list-item">
              <div class="item">
                <h3>What is it?</h3>
                <p>
                  The tiny 'plates' in blood that wedge together to help to clot
                  and reduce bleeding. Always in high demand, Vital for people
                  with low platelet count, like malaria and cancer patients.
                </p>
                <h3>Who can donate?</h3>
                <p>
                  You need to be 18-70 years old (men), weigh 50kg or more and
                  have given a successful plasma donation in the past 12 months
                </p>
              </div>
              <div class="item">
                <h3>User For?</h3>
                <p>
                  Cancer, blood diseases, haemophilia, anaemia, heart disease,
                  stomach disease, kidney disease, childbirth, operations, blood
                  loss, trauma, burns.
                </p>
                <h3>Lasts For?</h3>
                <p>Just five days.</p>
              </div>
              <div class="item">
                <h3>How does it work?</h3>
                <p>
                  We collect your blood, keep platelet and return rest to you by
                  apheresis donation.
                </p>
                <h3>How long does it take?</h3>
                <p>45 minutes to donate.</p>
                <h3>How often can I donate?</h3>
                <p>Every 2 weeks</p>
              </div>
            </div>
          </TabPanel>
        </TabView>
      </div>
    </div>
  </main>
</template>

<style lang="scss" scoped>
.donation_type-section {
  main {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    .heading {
      text-align: center;
      h1 {
        color: var(--PRIMARY_COLOR);
        margin-bottom: 20px;
      }
    }

    .body {
      width: 900px;
      background-color: #f3f4f4;
      border: 1px solid #ccc;
      padding: 15px;
      text-align: center;

      p {
        color: #292929;
        line-height: 1.8rem;
        font-size: 16px;
        text-align: center;
      }

      .description {
        p {
          padding-bottom: 10px;
        }
      }

      .tab_view-blood {
        margin-top: 20px;

        .list-item {
          display: flex;
          justify-content: space-between;
          .item {
            max-width: 270px;
            border-right: 1px solid #ccc;
            padding: 0 10px;
          }
          .item:last-child {
            border: none;
          }
        }
      }
    }
  }
}
</style>
