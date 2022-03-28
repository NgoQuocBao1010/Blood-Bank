import axios from "axios";

// Base domain and base url
const BASE_DOMAIN = "http://localhost:5000/api";
const baseURL = BASE_DOMAIN;

export default axios.create({ baseURL });
