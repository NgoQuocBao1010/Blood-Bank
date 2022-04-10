import { formatDate } from "..";

const KEYS_TRANSFORM = {
  hospitalName: "Hospital Name",
  date: "Date Requested",
  quantity: "Quantity (ml)",
};

export default {
  transformObject(data) {
    let transformData = {};

    for (let key in data) {
      if (KEYS_TRANSFORM[key])
        transformData[KEYS_TRANSFORM[key]] =
          key !== "date" ? data[key] : formatDate(parseInt(data[key]));
    }

    if (data.blood.name) transformData["Blood Name"] = data.blood.name;
    if (data.blood.type) transformData["Blood Type"] = data.blood.type;

    return transformData;
  },
  transformRowsBeforeExcel(data) {
    const rows = data.map((row) => this.transformObject(row));

    return rows;
  },
};
