import dayjs from "dayjs";

const KEYS_TRANSFORM = {};

export default {
    determineStatus(transaction) {
        const { status } = transaction;

        if (status === 1) return "success";
        if (status === -1) return "failed";
        if (status === 0) return "pending";
    },
};
