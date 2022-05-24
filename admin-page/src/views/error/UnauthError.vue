<script setup>
import { useUserStore } from "../../stores/user.js";
const userStore = useUserStore();
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card flex-center">
                <h3>
                    Sorry ✋, it seems that you are currently not authorized to
                    view this page ⛔️
                </h3>

                <!-- Auth user naviagate to other roles -->
                <h3 v-if="userStore.isLoggedIn">
                    Click
                    <router-link :to="userStore.defaultPage">here</router-link>
                    to return
                </h3>

                <!-- Unauth user try to naviagate to auth page -->
                <h3 v-else>
                    Please try
                    <router-link :to="{ name: 'Login' }">logged in</router-link>
                    to your account again 🙏
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
