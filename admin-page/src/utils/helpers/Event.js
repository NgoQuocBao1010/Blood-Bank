import dayjs from "dayjs";

export default {
    determineStatus(event) {
        const endDate = dayjs(event.startDate).add(event.duration, "day");
        if (dayjs().isAfter(endDate, "day") || dayjs().isSame(endDate, "day")) {
            return "passed";
        } else if (dayjs().isBefore(event.startDate, "day")) {
            return "upcoming";
        }

        return "ongoing";
    },
};
