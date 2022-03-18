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

export { JSONtoExcel };
