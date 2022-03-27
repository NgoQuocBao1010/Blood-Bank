import { utils, writeFile, read as readExcel } from "xlsx";
import dayjs from "dayjs";
import customParseFormat from "dayjs/plugin/customParseFormat";
dayjs.extend(customParseFormat);

/* Convert javascript object to excel file and download */
const JSONtoExcel = (jsonData, name = "test") => {
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

        result = result.map((row) => {
            row["Date Donated (dd/mm/yyyy)"] = dayjs(
                row["Date Donated (dd/mm/yyyy)"],
                "DD/MM/YYYY"
            )
                .toDate()
                .getTime();
            return row;
        });

        console.log(result);
    };

    reader.readAsArrayBuffer(exelFile);
};

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

export { JSONtoExcel, excelToJson, determineStockStatus, formatDate };
