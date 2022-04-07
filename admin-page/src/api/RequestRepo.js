import Repository from "./Repository";

const resource = "/Request";

export default {
  getAll() {
    return Repository.get(`${resource}/`);
  },

  get(id) {
    return Repository.get(`${resource}/${id}/`);
  },

  getPending() {
    return Repository.get(`${resource}/pendingRequest`);
  },
  getApproved() {
    return Repository.get(`${resource}/approvedRequest`);
  },
  getRejected() {
    return Repository.get(`${resource}/rejectedRequest`);
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
      hospitalId: params.hospitalId,
    });
  },

  approveRequests(data) {
    return Repository.put(`${resource}/approve`, data);
  },
  rejectRequests(data) {
    return Repository.put(`${resource}/reject`, data);
  },
};
