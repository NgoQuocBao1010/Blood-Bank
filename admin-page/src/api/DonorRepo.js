import Repository from "./Repository";
import { useLocalToken } from "./helpers";

const resource = "/donor";

export default {
    getAll() {
        useLocalToken();
        return Repository.get(`${resource}/`);
    },
    getSuccess() {
        useLocalToken();
        return Repository.get(`${resource}/success`);
    },
    getById(id) {
        useLocalToken();
        return Repository.get(`${resource}/${id}`);
    },
    importParticipants(data) {
        useLocalToken();
        return Repository.post(`${resource}/`, data);
    },
};
