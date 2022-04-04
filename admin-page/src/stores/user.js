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
        async verifyToken() {
            try {
                useLocalToken();
                const { data, status } = await UserRepo.verifyToken();
                console.log(data);
                if (data && status === 200) {
                    this.isLoggedIn = true;
                    this.email = data.email;
                    this.isAdmin = data.isAdmin;
                    this.hospitalId = data.hospital_id;
                }
            } catch (e) {
                console.log(e.response);
                this.logout();
                throw e;
            }
        },
        async login(email, password) {
            const { data } = await UserRepo.getToken(email, password);

            const authToken = data.token;
            this.setToken(authToken);
            this.email = email;
            this.isLoggedIn = true;

            useLocalToken();
        },
        logout() {
            localStorage.removeItem("userToken");
            this.$reset();
        },
    },
});
