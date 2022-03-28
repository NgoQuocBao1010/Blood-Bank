import { defineStore } from "pinia";

import EventRepo from "../api/EventRepo";
import EventHelper from "../utils/helpers/Event";

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
                event["status"] = EventHelper.determineStatus(event);

                return event;
            });
        },
    },
    getters: {
        activeEvents: (state) =>
            state.events.filter(
                (event) =>
                    event.status === "ongoing" || event.status === "passed"
            ),
        names: (state) => state.events?.map((event) => event.name),
        getEventById: (state) => {
            return (eventID) =>
                state.events.find((event) => event._id === eventID);
        },
    },
});
