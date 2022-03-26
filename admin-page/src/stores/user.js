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
            try {
                const response = await UserRepo.getToken(email, password);
                console.log(response);

                const authToken = "45808de6-ce77-436e-b698-c64d8f3148ae";
                this.setToken(authToken);
                this.email = email;
                this.isLoggedIn = true;
            } catch (e) {}
        },
        logout() {
            localStorage.removeItem("userToken");
            this.$reset();
        },
    },
});
