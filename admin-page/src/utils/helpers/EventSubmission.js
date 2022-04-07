export default {
    transformData(data) {
        const transformData = JSON.parse(JSON.stringify(data)).map((row) => {
            row.name = row.fullName;
            row.personalId = row.idCardNumber;
            row.dob = new Date(parseInt(row.dob));
            row.latestDonationDate = new Date(parseInt(row.latestDonationDate));

            return row;
        });

        return transformData;
    },
};
