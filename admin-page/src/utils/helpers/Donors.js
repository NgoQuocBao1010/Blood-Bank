import { formatDate, stringToDate, getKeyByValue } from "..";

const KEYS_TRANSFORM = {
    _id: "Personal ID",
    name: "Name",
    dob: "Date of Birth",
    gender: "Gender",
    address: "Address",
    phone: "Phone Number",
    email: "Email",
};

export default {
    transformARowForExcel(data) {
        let transformData = {};

        for (let key in data) {
            if (KEYS_TRANSFORM[key])
                transformData[KEYS_TRANSFORM[key]] =
                    key !== "dob" ? data[key] : formatDate(parseInt(data[key]));
        }

        if (data.transaction.eventDonated)
            transformData["Event"] = data.transaction.eventDonated.name;

        transformData["Date Donated"] = formatDate(
            parseInt(data.transaction.dateDonated)
        );

        transformData["Blood Type"] = `${data.blood.name} ${data.blood.type}`;
        transformData["Donation Amount (ml)"] = data.transaction.amount;

        return transformData;
    },
    transformRowsForExcelDownload(data) {
        const rows = data.map((row) => this.transformARowForExcel(row));

        return rows;
    },
    reformExcelRow(data) {
        let reformData = {
            transaction: {},
        };

        for (let key in data) {
            const reformKey = getKeyByValue(KEYS_TRANSFORM, key);
            if (reformKey) {
                reformData[reformKey] =
                    reformKey !== "dob"
                        ? data[key]
                        : stringToDate(data[key]).getTime().toString();
            } else {
                if (key === "Blood Type") {
                    const [name, type] = [...data[key].split(" ")];
                    reformData.transaction["blood"] = { name, type };
                } else if (key === "Donation Amount (ml)") {
                    reformData.transaction["amount"] = data[key];
                } else if (key === "Date Donated") {
                    reformData.transaction["dateDonated"] = stringToDate(
                        data[key]
                    )
                        .getTime()
                        .toString();
                }
            }
        }

        return reformData;
    },
    reformAfterExcelImport(data) {
        const rows = data.map((row) => this.reformExcelRow(row));

        return rows;
    },
};
