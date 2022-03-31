import Repository from "./Repository";

const resource = "/event";

export default {
    getAll() {
        return Repository.get(`${resource}/`);
    },
    getById(eventId) {
        return Repository.get(`${resource}/${eventId}`);
    },
    create(data) {
        return Repository.post(`${resource}/`, data);
    },
    edit(eventId, data) {
        return Repository.put(`${resource}/${eventId}`, data);
    },
    delete(eventId) {
        return Repository.delete(`${resource}/${eventId}`);
    },
};
