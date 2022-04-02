import Repository from "./Repository";

export default {
    getEndpoints() {
        return Repository.get(``);
    },
    searchByObjectById(id) {
        return Repository.get(`search/${id}`);
    },
};
