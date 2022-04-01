import Repository from "./Repository";
import { useLocalToken } from "./helpers";

export default {
    getEndpoints() {
        return Repository.get(``);
    },
    searchByObjectById(id) {
        useLocalToken();
        return Repository.get(`search/${id}`);
    },
};
