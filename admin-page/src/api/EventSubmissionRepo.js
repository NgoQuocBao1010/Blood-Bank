import Repository from "./Repository";

const resource = "/eventSubmission";

export default {
    getByEventId(eventId) {
        return Repository.get(`${resource}/event/${eventId}`);
    },
    post(data) {
        return Repository.post(`${resource}/`, data);
    },
};
