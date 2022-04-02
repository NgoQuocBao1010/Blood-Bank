import Repository from "./Repository";

const resource = "/Request";

export default {
    getAll() {
        return Repository.get(`${resource}/`);
    },

    get(id) {
        return Repository.get(`${resource}/${id}/`);
    },

    post(params) {
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
};
