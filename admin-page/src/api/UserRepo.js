import Repository from "./Repository";

const resource = "/user";

export default {
    getAll() {
        return Repository.get(`${resource}/`);
    },
    getToken(email, password) {
        return Repository.post(`${resource}/login/`, { email, password });
    },
};
