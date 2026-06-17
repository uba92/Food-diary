import axiosClient from "../api/axiosClient";

export const getAllWeeklyPlans = async () => {
    const response = await axiosClient.get("/weeklyplan");
    return response.data;
}

export const createWeeklyPlan = async (data) => {
    const response = await axiosClient.post("/weeklyplan", data);
    return response.data;
}

export const updateWeeklyPlan = async (id, data) => {
    const response = await axiosClient.put(`/weeklyplan/${id}`, data);
    return response.data;
}

export const deleteWeeklyPlan = async (id) => {
    await axiosClient.delete(`/weeklyplan/${id}`);
}

export const getWeeklyPlanByDay = async (id) => {
    const response = await axiosClient.get(`/weeklyplan/${id}/by-day`);
    return response.data;
}

export const getWeeklyPlanUsageStats = async (id) => {
    const response = await axiosClient.get(`/weeklyplan/${id}/usage-stats`);
    return response.data;
}