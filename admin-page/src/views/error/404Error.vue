<script setup>
import { useUserStore } from "../../stores/user";
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card flex-center">
                <h3>The page you try to reach does not exist 🤔🤨</h3>
                <h3 v-if="useUserStore().isLoggedIn">
                    You can return to the dashboard

                    <RouterLink :to="{ name: 'Dashboard' }">
                        👉 here 👈
                    </RouterLink>
                </h3>
                <h3 v-else>
                    You can return to the login page

                    <RouterLink :to="{ name: 'Login' }">
                        👉 here 👈
                    </RouterLink>
                </h3>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
.card {
    flex-direction: column;
}
</style>
