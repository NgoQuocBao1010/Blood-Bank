<template>
    <div class="col-12 lg:col-6 xl:col-3 skeleton" v-for="_ in 4">
        <div class="card mb-0">
            <div class="flex justify-content-between mb-3" style="width: 100%">
                <div style="width: 100%">
                    <span class="block mb-3 skeleton__title"> </span>
                    <div
                        class="text-900 font-medium text-xl skeleton__value"
                    ></div>
                </div>
                <div
                    class="flex align-items-center justify-content-center border-round skeleton__icon"
                ></div>
            </div>
            <p class="skeleton__detail"></p>
        </div>
    </div>
</template>

<style lang="scss" scoped>
.skeleton {
    &__title {
        width: 50% !important;
        height: 1.3rem;
        animation: 1.5s loading_skeleton linear infinite;
    }

    &__value {
        width: 40%;
        height: 2rem;
        animation: 1.5s loading_skeleton linear infinite;
    }

    &__detail {
        width: 70%;
        height: 1.2rem;
        animation: 1.5s loading_skeleton linear infinite;
    }

    &__icon {
        width: 2.5rem;
        height: 2.5rem;
        animation: 1.5s loading_skeleton linear infinite;
    }
}
</style>
