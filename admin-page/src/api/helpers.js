import Repository from "./Repository.js";

const useLocalToken = () => {
    /* Use local token to authorized */
    Repository.interceptors.request.use((config) => {
        const token = localStorage.getItem("userToken");
        config.headers.Authorization = token ? `Bearer ${token}` : "";
        return config;
    });
};

export { useLocalToken };
