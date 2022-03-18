import * as XLSX from "xlsx";

const JSONtoExcel = (jsonData, name = "test") => {
    const workbook = XLSX.utils.book_new();
    workbook["!cols"] = [{ wch: 100 }];

    XLSX.utils.book_append_sheet(
        workbook,
        XLSX.utils.json_to_sheet(jsonData, {}),
        "sample"
    );

    XLSX.writeFile(workbook, `${name}.xlsx`);
};

export { JSONtoExcel };
