<script setup>
import { RouterLink } from "vue-router";
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card flex-center">
                <h3>The page you try to reach does not exist 🤔🤨</h3>
                <h3>
                    You can return to the dashboard

                    <RouterLink :to="{ name: 'Dashboard' }">
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
