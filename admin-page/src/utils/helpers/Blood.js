import dayjs from "dayjs";
import { BLOOD_TYPES } from "../../constants";

const KEYS_TRANSFORM = {
    quantity: "Quantity (ml)",
};

export default {
    determineStockStatus(value) {
        if (value === 0)
            return { status: "out", displayStatus: "out of stock" };

        if (value <= 400)
            return { status: "low", displayStatus: "low in stock" };

        if (value <= 700)
            return { status: "good", displayStatus: "good in stock" };
        // Greater than 700
        else return { status: "great", displayStatus: "great in stock" };
    },
    transformDataForTable(bloodData) {
        let transformData = [];

        BLOOD_TYPES.forEach((name) => {
            const data = bloodData.filter((el) => el.name === name);

            // Evaluate stock status of blood type positive
            const positiveData = data.find((el) => el.type === "Positive");
            const {
                status: positiveStockStatus,
                displayStatus: positiveStatusDisplay,
            } = this.determineStockStatus(positiveData.quantity);

            // Evaluate stock status of blood type negative
            const negativeData = data.find((el) => el.type === "Negative");
            const {
                status: negativeStockStatus,
                displayStatus: negativeStatusDisplay,
            } = this.determineStockStatus(negativeData.quantity);

            transformData.push({
                _id: positiveData._id,
                name: positiveData.name,
                quantity: positiveData.quantity + negativeData.quantity,
                inStock:
                    !["out", "low"].includes(positiveStockStatus) &&
                    !["out", "low"].includes(negativeStatusDisplay),
                types: [
                    {
                        name: "positive",
                        bloodType: positiveData.name,
                        quantity: positiveData.quantity,
                        status: positiveStockStatus,
                        displayStatus: positiveStatusDisplay,
                    },
                    {
                        name: "negative",
                        bloodType: negativeData.name,
                        quantity: negativeData.quantity,
                        status: negativeStockStatus,
                        displayStatus: negativeStatusDisplay,
                    },
                ],
            });
        });

        return transformData;
    },
    transformARowForExcel(row) {
        let transformData = {};
        transformData["Blood Type"] = `${row.name} ${row.type}`;

        for (let key in row) {
            if (KEYS_TRANSFORM[key])
                transformData[KEYS_TRANSFORM[key]] = row[key];
        }
        return transformData;
    },
    transformRowsForExcelDownload(data) {
        const rows = JSON.parse(JSON.stringify(data)).map((row) =>
            this.transformARowForExcel(row)
        );

        return rows;
    },
};
