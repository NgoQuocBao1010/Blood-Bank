import Repository from "./Repository.js";

const resource = "/donor";

export default {
    getDonors() {
        return Repository.get(`${resource}/info`);
    },
    getAllDonations() {
        return Repository.get(`${resource}/`);
    },
    getSuccessDonations() {
        return Repository.get(`${resource}/success`);
    },
    getRejectDonations() {
        return Repository.get(`${resource}/failure`);
    },
    getById(id) {
        return Repository.get(`${resource}/${id}`);
    },
    importParticipants(data) {
        return Repository.post(`${resource}/`, data);
    },
};
