import dayjs from "dayjs";
import customParseFormat from "dayjs/plugin/customParseFormat";
dayjs.extend(customParseFormat);

const determineStockStatus = (value) => {
    if (value === 0) return { status: "out", displayStatus: "out of stock" };

    if (value <= 400) return { status: "low", displayStatus: "low in stock" };

    if (value <= 700) return { status: "good", displayStatus: "good in stock" };

    if (value > 700)
        return { status: "great", displayStatus: "great in stock" };
};

const formatDate = (date) => {
    // Format date object
    const day = dayjs(date);
    return day.format("DD/MM/YYYY");
};

export { determineStockStatus, formatDate };
