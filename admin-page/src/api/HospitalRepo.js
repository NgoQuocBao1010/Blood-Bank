import Repository from "./Repository";

const resource = "/Hospital";

export default {
  getAll() {
    return Repository.get(`${resource}`);
  },
  get(id) {
    return Repository.get(`${resource}/${id}`);
  },
};
