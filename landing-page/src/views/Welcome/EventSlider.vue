<script setup>
import { ref, onMounted } from "vue";
import Button from "primevue/button";
import Carousel from "primevue/carousel";

const click = (data) => {
    console.log(data);
};

const events = ref(null);

onMounted(() => {
    events.value = [
        {
            id: 1,
            title: "Event 1",
            bgImg: "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/mobile_banner_center_2500_600.webp",
            time: "01/01/2022",
        },
        {
            id: 2,
            title: "Event 2",
            bgImg: "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/updated_NewYear2022.webp",
            time: "15/03/2022",
        },
        {
            id: 3,
            title: "Event 3",
            bgImg: "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/ONE_NATION_2500_600.webp",
            time: "18/04/2022",
        },
        {
            id: 4,
            title: "Event 4",
            bgImg: "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/Blood_EHealthID_2500_600.webp",
            time: "18/04/2022",
        },
        {
            id: 1,
            title: "Event 1",
            bgImg: "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/mobile_banner_center_2500_600.webp",
            time: "01/01/2022",
        },
    ];
});
</script>

<template>
    <div class="card">
        <Carousel
            :value="events"
            :numVisible="1"
            :numScroll="1"
            :circular="true"
            :autoplayInterval="2000"
            class="carousel-slider"
        >
            <template #item="slotProps">
                <div
                    class="product-item"
                    :style="{
                        backgroundImage: 'url(' + slotProps.data.bgImg + ')',
                    }"
                    @click="click(slotProps)"
                >
                    <div class="product-item-content">
                        <Button>Login and Generate</Button>
                    </div>
                </div>
            </template>
        </Carousel>
    </div>
</template>

<style lang="scss" scoped>
.p-carousel-container {
    button {
        background-color: var(--PRIMARY_COLOR) !important;
    }
}

.product-item {
    display: block;
    width: 100%;
    height: 400px;
    background-size: cover;
    background-repeat: no-repeat;

    &-content {
        height: 100%;
        display: flex;
        justify-content: flex-end;
        align-items: flex-end;

        button {
            background-color: #1e2d50 !important;
            border: 1px solid #fff;
            color: #fff;
            margin-bottom: 20px;
            margin-right: 20px;
        }

        button:hover {
            background-color: var(--PRIMARY_COLOR) !important;
            border: 1px solid #fff;
            color: #fff;
        }
    }

    .product-image {
        width: 100%;
        display: block;
        height: 400px;
    }
}

.p-carousel-indicators.p-reset .p-carousel-indicator.p-highlight {
    .p-link button {
        background-color: var(--DARK_BLUE) !important;
    }
}
</style>
