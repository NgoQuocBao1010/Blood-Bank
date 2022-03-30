import { utils, writeFile, read as readExcel } from "xlsx";

/* Convert javascript object to excel file and download */
const JSONtoExcel = (jsonData, name = "unknown") => {
    const workbook = utils.book_new();
    const worksheet = utils.json_to_sheet(jsonData);

    // Set column width of each header
    worksheet["!cols"] = new Array(Object.keys(jsonData[0]).length).fill({
        wch: 30,
    });

    utils.book_append_sheet(workbook, worksheet, "sample");
    writeFile(workbook, `${name}.xlsx`);
};

/* convert excel file to javascript object */
const excelToJson = (exelFile) => {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        let result = {};

        reader.onload = (e) => {
            let data = e.target.result;
            data = new Uint8Array(data);
            let array = new Array();

            for (let i = 0; i != data.length; ++i)
                array[i] = String.fromCharCode(data[i]);

            const bstr = array.join("");
            const workbook = readExcel(bstr, {
                type: "binary",
            });
            const first_sheet_name = workbook.SheetNames[0];
            const worksheet = workbook.Sheets[first_sheet_name];

            result = utils.sheet_to_json(worksheet, {
                raw: true,
            });
            resolve(result);
        };

        reader.readAsArrayBuffer(exelFile);
    });
};

export { excelToJson, JSONtoExcel };
