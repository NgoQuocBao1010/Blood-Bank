import Repository from "./Repository";
import { useLocalToken } from "./helpers";

const resource = "/Hospital";

export default {
  getAll() {
    useLocalToken();
    return Repository.get(`${resource}/`);
  },

  getById(id) {
    useLocalToken();
    return Repository.get(`${resource}/${id}/`);
  },
};
