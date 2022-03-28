import Repository from "./Repository";

const resource = "/donortransaction";

export default {
    approveParticipants(data) {
        return Repository.put(`${resource}/approve`, data);
    },
    rejectParticipants(data) {
        return Repository.put(`${resource}/reject`, data);
    },
};
