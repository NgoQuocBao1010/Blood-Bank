import dayjs from "dayjs";
import customParseFormat from "dayjs/plugin/customParseFormat";
dayjs.extend(customParseFormat);

const getKeyByValue = (object, value) => {
    return Object.keys(object).find((key) => object[key] === value);
};

const formatDate = (date) => {
    // Format date object
    const day = dayjs(date);
    return day.format("DD/MM/YYYY");
};

const converToDate = (stringDate) => {
    return dayjs(stringDate, "DD/MM/YYYY", true).toDate();
};

export { formatDate, getKeyByValue, converToDate };
