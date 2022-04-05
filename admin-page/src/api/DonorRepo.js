import Repository from "./Repository";

const resource = "/donor";

export default {
    getAll() {
        return Repository.get(`${resource}/`);
    },
    getSuccess() {
        return Repository.get(`${resource}/success`);
    },
    getReject() {
        return Repository.get(`${resource}/failure`);
    },
    getById(id) {
        return Repository.get(`${resource}/${id}`);
    },
    importParticipants(data) {
        return Repository.post(`${resource}/`, data);
    },
};
