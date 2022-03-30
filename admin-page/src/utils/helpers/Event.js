import dayjs from "dayjs";

const KEYS_TRANSFORM = {};

export default {
    determineStatus(event) {
        const endDate = dayjs(event.startDate).add(event.duration, "day");
        if (dayjs().isAfter(endDate, "day")) {
            return "passed";
        } else if (dayjs().isBefore(event.startDate, "day")) {
            return "upcoming";
        }

        return "ongoing";
    },
};
