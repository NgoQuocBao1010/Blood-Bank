import dayjs from "dayjs";
import customParseFormat from "dayjs/plugin/customParseFormat";
dayjs.extend(customParseFormat);

const getKeyByValue = (object, value) => {
    return Object.keys(object).find((key) => object[key] === value);
};

const timeDiffernet = (date) => {
    const date1 = dayjs(date);
    const now = dayjs();

    let timeUnit = "";
    let values = 0;

    const minutes = Math.floor(now.diff(date1) / 60000);

    if (minutes >= 60 * 24) {
        values = Math.floor((minutes / 60) * 24);
        timeUnit = values > 1 ? "days" : "day";
    } else if (minutes >= 60) {
        values = Math.floor(minutes / 60);
        timeUnit = values > 1 ? "hours" : "hour";
    } else {
        values = minutes;
        timeUnit = values > 1 ? "minutes" : "minute";
    }

    return values > 0 ? `${values} ${timeUnit} ago` : "Just Now";
};

const formatDate = (date) => {
    // Format date object
    const day = dayjs(date);
    return day.format("DD/MM/YYYY");
};

const converToDate = (stringDate) => {
    return dayjs(stringDate, "DD/MM/YYYY", true).toDate();
};

export { formatDate, getKeyByValue, converToDate, timeDiffernet };
