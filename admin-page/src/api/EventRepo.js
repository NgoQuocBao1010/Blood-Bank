import Repository from "./Repository.js";

const resource = "/event";

export default {
    getAll() {
        return Repository.get(`${resource}/`);
    },
    getById(eventId) {
        return Repository.get(`${resource}/${eventId}`);
    },
    getParticipants(eventId) {
        return Repository.get(`${resource}/listParticipants/${eventId}`);
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
