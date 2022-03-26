import Repository from "./Repository";
import { useLocalToken } from "./helpers";

const resource = "/user";

export default {
    getAll() {
        useLocalToken();
        return Repository.get(`${resource}/`);
        // return Repository.get(`${resource}/`, {
        //     headers: {
        //         Authorization:
        //             "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySUQiOiI2MjNkNjRhNjQ4MDljZjYwMDRiNTY2ZjciLCJuYmYiOjE2NDgyOTUxOTcsImV4cCI6MTY0ODg5OTk5NywiaWF0IjoxNjQ4Mjk1MTk3fQ.-Px78o0DvPNIP4OrJ0LZn3X63t1qdvepEyEOjk67-V0",
        //     },
        // });
    },
    getToken(email, password) {
        return Repository.post(`${resource}/login/`, { email, password });
    },
};
