import Repository from "./Repository";

const resource = "/Blood";

export default {
    getAll() {
        return Repository.get(`${resource}`);
    },
};
