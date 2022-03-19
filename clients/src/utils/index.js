import { utils, writeFile } from "xlsx";

const JSONtoExcel = (jsonData, name = "test") => {
    const workbook = utils.book_new();
    const worksheet = utils.json_to_sheet(jsonData);

    // Set column width of each header
    worksheet["!cols"] = new Array(Object.keys(jsonData[0]).length).fill({
        wch: 20,
    });

    utils.book_append_sheet(workbook, worksheet, "sample");
    writeFile(workbook, `${name}.xlsx`);
};

const determineStockStatus = (value) => {
    if (value === 0) return { status: "out", displayStatus: "out of stock" };

    if (value <= 400) return { status: "low", displayStatus: "low in stock" };

    if (value <= 700) return { status: "good", displayStatus: "good in stock" };

    if (value > 700)
        return { status: "great", displayStatus: "great in stock" };
};

export { JSONtoExcel, determineStockStatus };
