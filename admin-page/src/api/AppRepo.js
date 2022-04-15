import Repository from "./Repository";

export default {
    getEndpoints() {
        return Repository.get(``);
    },
    searchByObjectById(id) {
        return Repository.get(`search/${id}`);
    },
    getOverview() {
        return Repository.get(`dashboardInfo`);
    },
    getRecentActivities() {
        return Repository.get(`recentActivities`);
    },
    getEventNotifications() {
        return Repository.get(`notification`);
    },
};
