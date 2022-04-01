import Repository from "./Repository";
import { useLocalToken } from "./helpers";

const resource = "/event";

export default {
    getAll() {
        return Repository.get(`${resource}/`);
    },
    getById(id) {
        return Repository.get(`${resource}/${id}`);
    },
    createNewEvent(data) {
        useLocalToken();
        return Repository.post(`${resource}/`, data);
    },
};
