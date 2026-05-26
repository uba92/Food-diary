import axios from "axios";

const axiosClient = axios.create({
    baseURL: "https://localhost:7288/api"
});

export default axiosClient;