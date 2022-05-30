import Repository from "./Repository.js";

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
    getActivityChartData() {
        return Repository.get(`chart`);
    },
    getEventNotifications() {
        return Repository.get(`notification`);
    },
};
