import { defineStore } from "pinia";

import UserRepo from "../api/UserRepo";
import { useLocalToken } from "../api/helpers";

export const useUserStore = defineStore("user", {
    state: () => {
        return {
            token: localStorage.getItem("userToken")
                ? localStorage.getItem("userToken")
                : null,
            email: null,
            isAdmin: null,
            hospitalId: null,
            isLoggedIn: false,
        };
    },
    actions: {
        setToken(authToken) {
            this.token = authToken;
            localStorage.setItem("userToken", this.token);
        },
        setState(userInfo) {
            this.isLoggedIn = true;
            this.email = userInfo.email;
            this.isAdmin = userInfo.isAdmin;
            this.hospitalId = userInfo.hospitalId;
        },
        async verifyToken() {
            try {
                useLocalToken();
                const { data, status } = await UserRepo.verifyToken();
                if (data && status === 200) {
                    this.setState(data);
                }
            } catch (e) {
                console.log(e.response);
                this.logout();
                throw e;
            }
        },
        async login(email, password) {
            const { data } = await UserRepo.getToken(email, password);

            const { token, user } = data;
            this.setToken(token);
            this.setState(user);

            useLocalToken();
        },
        logout() {
            localStorage.removeItem("userToken");
            this.$reset();
        },
    },
    getters: {
        defaultPage: (state) => {
            return state.isAdmin
                ? { name: "Dashboard" }
                : {
                      name: "Hospital Page",
                      params: { _id: state.hospitalId },
                  };
        },
        profilePage: (state) =>
            state.isAdmin
                ? { name: "Dashboard" }
                : {
                      name: "Hospital Profile",
                      params: { _id: state.hospitalId },
                  },
    },
});
