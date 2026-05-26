import axiosClient from "../api/axiosClient";

export const getAllWeeklyPlans = async () => {
    const response = await axiosClient.get("/weeklyplan");
    return response.data;
}

export const createWeeklyPlan = async (data) => {
    const response = await axiosClient.post("/weeklyplan", data);
    return response.data;
}