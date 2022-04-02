<script setup></script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card flex-center">
                <h3>
                    Sorry ✋, it seems that you are currently not authorized to
                    view this page ⛔️
                </h3>
                <h3>
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
