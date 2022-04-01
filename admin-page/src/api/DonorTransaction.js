import Repository from "./Repository";
import { useLocalToken } from "./helpers";

const resource = "/donortransaction";

export default {
    getListTransactionByDonor(donorID) {
        useLocalToken();
        return Repository.get(`${resource}/listDonation/${donorID}`);
    },
    approveParticipants(data) {
        useLocalToken();
        return Repository.put(`${resource}/approve`, data);
    },
    rejectParticipants(data) {
        useLocalToken();
        return Repository.put(`${resource}/reject`, data);
    },
};
