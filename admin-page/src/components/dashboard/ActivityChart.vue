<script setup>
import Chart from "primevue/chart";

import AppRepo from "../../api/AppRepo";
import { THEMES } from "../../constants";

const CHART_OPTIONS = {
    plugins: {
        tooltip: {
            callbacks: {
                label: function (context) {
                    let label = context.dataset.label || "";

                    const unit =
                        parseInt(context.formattedValue) > 1
                            ? "liters"
                            : "liter";

                    label += `: ${context.formattedValue} ${unit}`;
                    return label;
                },
            },
            bodyFont: {
                size: 16,
            },
        },
    },
};

const getChartConfig = (type) => {
    return {
        Received: {
            label: "Blood Imported",
            fill: false,
            backgroundColor: THEMES.primaryColor,
            borderColor: THEMES.primaryColor,
            tension: 0.4,
        },
        Donated: {
            label: "Blood Exported",
            fill: false,
            backgroundColor: THEMES.darkColor,
            borderColor: THEMES.darkColor,
            tension: 0.4,
        },
    }[type];
};

const { data } = await AppRepo.getActivityChartData();
let chartData = data;
chartData.datasets = chartData.datasets.map((type) => {
    return { ...type, ...getChartConfig(type.label) };
});
</script>

<template>
    <div class="card">
        <h5>Blood Activites in Last 6 months</h5>
        <Chart type="line" :data="data" :options="CHART_OPTIONS" />
    </div>
</template>

<style lang="scss" scoped></style>
