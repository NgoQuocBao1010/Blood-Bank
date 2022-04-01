import Repository from "./Repository";

const resource = "/event";

export default {
  getAll() {
    return Repository.get(`${resource}/`);
  },
  getById(id) {
    return Repository.get(`${resource}/${id}`);
  },
};
