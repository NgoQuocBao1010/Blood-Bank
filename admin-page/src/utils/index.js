import dayjs from "dayjs";
import customParseFormat from "dayjs/plugin/customParseFormat";
dayjs.extend(customParseFormat);

const getKeyByValue = (object, value) => {
    return Object.keys(object).find((key) => object[key] === value);
};

const timeDifference = (date) => {
    // Calculate the time different between a datetime object and now
    // Return int
    const date1 = dayjs(date);
    const now = dayjs();

    let timeUnit = "";
    let values = 0;

    const minutes = Math.floor(now.diff(date1) / 60000);

    if (minutes >= 60 * 24) {
        values = Math.floor(minutes / (60 * 24));
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
    // Return a string DD/MM/YYYY
    const day = dayjs(date);
    return day.format("DD/MM/YYYY");
};

const stringToDate = (stringDate) => {
    // Covert a string DD/MM/YYYY to a date object
    // Return a date type
    return dayjs(stringDate, "DD/MM/YYYY", true).toDate();
};

const fileToBase64 = (file) =>
    new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = (e) => reject(e);
    });

export {
    formatDate,
    getKeyByValue,
    stringToDate,
    timeDifference,
    fileToBase64,
};
