import Repository from "./Repository";
import { useLocalToken } from "./helpers";

const resource = "/user";

export default {
    getAll() {
        useLocalToken();
        return Repository.get(`${resource}/`);
    },
    getToken(email, password) {
        return Repository.post(`${resource}/login/`, { email, password });
    },
};
