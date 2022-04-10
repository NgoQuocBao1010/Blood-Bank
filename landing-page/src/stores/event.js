import { defineStore } from "pinia";

import EventRepo from "../api/EventRepo";
import { determineStatus } from "../utils/index.js";

export const useEventStore = defineStore("event", {
  state: () => {
    return {
      events: null,
    };
  },
  actions: {
    async setEvents() {
      const { data } = await EventRepo.getAll();
      this.events = data.map((row) => {
        let event = { ...row };

        event["startDate"] = new Date(parseInt(event["startDate"]));
        event["status"] = determineStatus(event);
        event["bgImg"] =
          "https://www.eraktkosh.in/BLDAHIMS/bloodbank/transactions/assets/webp/mobile_banner_center_2500_600.webp";

        console.log(event);
        return event;
      });
    },
    filter(value) {
      const filterData = this.events.filter((row) => row.status === value);
      this.events = filterData;
    },
  },
  getters: {
    activeEvents: (state) =>
      state.events.filter(
        (event) => event.status === "ongoing" || event.status === "upcoming"
      ),
  },
});
