import { defineStore } from "pinia";

export const useAppStore = defineStore("app", {
    state: () => {
        return {
            loading: false,
        };
    },
    actions: {
        triggerLoad() {
            this.loading = true;
            document.body.style.overflowY = "hidden";
        },
        closeLoad() {
            this.loading = false;
            document.body.style.overflowY = "auto";
        },
    },
    getters: {},
});
