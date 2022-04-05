import Repository from "./Repository";

const resource = "/eventSubmission";

export default {
  
  post(data) {
    return Repository.post(`${resource}/`, data);
  },
};
