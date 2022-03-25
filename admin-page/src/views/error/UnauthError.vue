<script setup>
import { RouterLink } from "vue-router";
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card flex-center">
                <h3>Sorry ⛔, you are not allowed to view this page 🔐</h3>
                <h3>
                    You can login here

                    <RouterLink :to="{ name: 'About' }">
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
