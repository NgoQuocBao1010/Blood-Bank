import Repository from "./Repository";

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
    delete(userId) {
        return Repository.delete(`${resource}/${userId}`);
    },
};
