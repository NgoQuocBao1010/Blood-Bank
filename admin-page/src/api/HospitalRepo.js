import Repository from "./Repository.js";

const resource = "/Hospital";

export default {
    getAll() {
        return Repository.get(`${resource}`);
    },
    get(id) {
        return Repository.get(`${resource}/${id}`);
    },
    post(params) {
        return Repository.post(`${resource}/`, {
            name: params.name,
            address: params.address,
            phone: params.phone,
        });
    },
    put(params) {
        return Repository.put(`${resource}/${params._id}`, {
            name: params.name,
            address: params.address,
            phone: params.phone,
        });
    },
};
