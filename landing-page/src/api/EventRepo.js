import Repository from "./Repository";

const resource = "/event";

export default {
  getAll() {
    return Repository.get(`${resource}/`);
  },
  getById(params) {
    return Repository.get(`${resource}/${params.eventId}?now=${params.now}`);
  },
};
