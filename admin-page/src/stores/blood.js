import { defineStore } from "pinia";

import BloodRepo from "../api/BloodRepo.js";
import BloodHelper from "../utils/helpers/Blood.js";

export const useBloodStore = defineStore("blood", {
    state: () => {
        return {
            bloodData: null,
        };
    },
    actions: {
        async getData() {
            const { data } = await BloodRepo.getAll();
            this.bloodData = data;

            return this.bloodData;
        },
    },
    getters: {
        summaryData: (state) =>
            BloodHelper.transformDataForTable(state.bloodData),
    },
});
