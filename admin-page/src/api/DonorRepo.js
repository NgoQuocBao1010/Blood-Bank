import Repository from "./Repository";

const resource = "/donor";

export default {
    getAll() {
        return Repository.get(`${resource}/`);
    },
};
