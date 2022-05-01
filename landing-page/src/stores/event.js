import { defineStore } from "pinia";

import EventRepo from "../api/EventRepo";
import { determineStatus } from "../utils/index.js";
import { DEFAULT_EVENT_COVER } from "../constant";
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
        event["bgImg"] = event.binaryImage
          ? event.binaryImage
          : DEFAULT_EVENT_COVER;
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

    getEventById: (state) => {
      return (eventID) => state.events?.find((event) => event._id === eventID);
    },
  },
});
