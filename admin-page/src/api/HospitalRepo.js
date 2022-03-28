import Repository from "./Repository";
import { useLocalToken } from "./helpers";

const resource = "/Hospital";

export default {
  getAll() {
    useLocalToken();
    return Repository.get(`${resource}`);
  },
  get(id) {
    useLocalToken();
    return Repository.get(`${resource}/${id}`);
  },
  post(params) {
    useLocalToken();
    return Repository.post(`${resource}/`, {
      name: params.name,
      address: params.address,
      phone: params.phone,
    });
  },
  put(params) {
    useLocalToken();
    return Repository.put(`${resource}/`, {
      name: params.name,
      address: params.address,
      phone: params.phone,
    });
  },
};
