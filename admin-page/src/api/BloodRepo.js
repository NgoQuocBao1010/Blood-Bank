import Repository from "./Repository.js";

const resource = "/Blood";

export default {
    getAll() {
        return Repository.get(`${resource}`);
    },
};
