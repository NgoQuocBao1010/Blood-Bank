import { formatDate } from "..";

const KEYS_TRANSFORM = {
  _id: "Hospital ID",
  name: "Hospital Name",
  address: "Address",
  phone: "Phone Number",
};

export default {
  transformObject(data) {
    let transformData = {};

    for (let key in data) {
      if (KEYS_TRANSFORM[key]) transformData[KEYS_TRANSFORM[key]] = data[key];
    }

    return transformData;
  },
  transformRowsBeforeExcel(data) {
    const rows = data.map((row) => this.transformObject(row));

    return rows;
  },
};
