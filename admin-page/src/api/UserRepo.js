import Repository from "./Repository";

const resource = "/user";

export default {
    getToken(email, password) {
        return Repository.get(`${resource}/login/`);
    },
};
