<script setup>
import Overview from "../components/dashboard/Overview.vue";
import OverviewSkeleton from "../components/dashboard/OverviewSkeleton.vue";
import RecentActivities from "../components/dashboard/Recent.vue";
import BloodStorage from "../components/dashboard/BloodStorage.vue";
import BloodStorageSkeleton from "../components/dashboard/BloodStorageSkeleton.vue";
import ActivityChart from "../components/dashboard/ActivityChart.vue";
import Notification from "../components/dashboard/Notification.vue";
import AppProgressBar from "../components/AppProgressBar.vue";
</script>

<template>
    <div class="grid">
        <!-- General Information -->
        <Suspense>
            <template #default>
                <Overview />
            </template>
            <template #fallback>
                <OverviewSkeleton />
            </template>
        </Suspense>

        <!-- Recent Activities && Blood Type Storage -->
        <div class="col-12 xl:col-6">
            <!-- Activity table -->
            <RecentActivities />

            <!-- Blood Storage -->
            <Suspense>
                <template #default>
                    <BloodStorage />
                </template>
                <template #fallback>
                    <BloodStorageSkeleton />
                </template>
            </Suspense>
        </div>

        <!-- Blood Activities && Notification Panel -->
        <div class="col-12 xl:col-6">
            <!-- Blood Chart Activities -->
            <Suspense>
                <template #default>
                    <ActivityChart />
                </template>
                <template #fallback>
                    <AppProgressBar />
                </template>
            </Suspense>

            <!-- Notification Panel -->
            <Suspense>
                <template #default>
                    <Notification />
                </template>
                <template #fallback>
                    <AppProgressBar />
                </template>
            </Suspense>
        </div>
    </div>
</template>

<style lang="scss" scoped>
.grid {
    .card > span {
        @media screen and (max-width: 1300px) {
            font-size: 0.9rem;
        }
    }
}
</style>
