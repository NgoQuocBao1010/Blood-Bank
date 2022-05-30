import Repository from "./Repository.js";

const resource = "/user";

export default {
    getAll() {
        return Repository.get(`${resource}/`);
    },
    getToken(email, password) {
        return Repository.post(`${resource}/login/`, { email, password });
    },
    verifyToken() {
        return Repository.get(`${resource}/verify`);
    },
    createAccount(data) {
        return Repository.post(`${resource}/`, data);
    },
    delete(userId) {
        return Repository.delete(`${resource}/${userId}`);
    },
};
