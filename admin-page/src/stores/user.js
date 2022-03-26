import { defineStore } from "pinia";

import UserRepo from "../api/UserRepo";

export const useUserStore = defineStore("user", {
    state: () => {
        return {
            token: localStorage.getItem("userToken")
                ? localStorage.getItem("userToken")
                : null,
            email: null,
            role: null,
            isLoggedIn: false,
        };
    },
    actions: {
        setToken(authToken) {
            this.token = authToken;
            localStorage.setItem("userToken", this.token);
        },
        verifyToken() {
            console.log("Make API call to verify token");

            if (this.token) {
                this.email = "admin@gmail.com";
                this.isLoggedIn = true;
            }

            return this.token ? false : true;
        },
        async login(email, password) {
            const { data } = await UserRepo.getToken(email, password);

            const authToken = data.token;
            this.setToken(authToken);
            this.email = email;
            this.isLoggedIn = true;
        },
        logout() {
            localStorage.removeItem("userToken");
            this.$reset();
        },
    },
});
