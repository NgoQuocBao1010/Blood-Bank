import { defineStore } from "pinia";

export const useAppStore = defineStore("app", {
    state: () => {
        return {
            loading: false,
        };
    },
    actions: {
        openLoadingScreen() {
            this.loading = true;
            document.body.style.overflowY = "hidden";
        },
        closeLoadingScreen() {
            this.loading = false;
            document.body.style.overflowY = "auto";
        },
    },
    getters: {},
});
