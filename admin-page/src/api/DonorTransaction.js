import Repository from "./Repository.js";

const resource = "/donortransaction";

export default {
    getListTransactionByDonor(donorID) {
        return Repository.get(`${resource}/listDonation/${donorID}`);
    },
    approveParticipants(data) {
        return Repository.put(`${resource}/approve`, data);
    },
    rejectParticipants(data) {
        return Repository.put(`${resource}/reject`, data);
    },
};
