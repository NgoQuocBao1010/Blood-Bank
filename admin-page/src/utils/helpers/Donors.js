import { formatDate } from "..";

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
    transformObject(data) {
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
    transformRows(data) {
        const rows = data.map((row) => this.transformObject(row));

        return rows;
    },
};
