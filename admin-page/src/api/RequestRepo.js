import Repository from "./Repository";
import { useLocalToken } from "./helpers";

const resource = "/Request";

export default {
  getAll() {
    useLocalToken();
    return Repository.get(`${resource}/`);
  },

  get(id) {
    useLocalToken();
    return Repository.get(`${resource}/${id}/`);
  },

  post(params) {
    useLocalToken();
    return Repository.post(`${resource}/`, {
      quantity: params.quantity,
      blood: {
        name: params.blood.name,
        type: params.blood.type,
      },
      date: params.date,
      hospital: {
        _id: params.hospital._id,
      },
    });
  },
};
