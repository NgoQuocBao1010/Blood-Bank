import Repository from "./Repository";
import { useLocalToken } from "./helpers";

const resource = "/Blood";

export default {
    getAll() {
        useLocalToken();
        return Repository.get(`${resource}`);
    },
};
