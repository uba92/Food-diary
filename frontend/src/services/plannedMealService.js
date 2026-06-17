import axiosClient from "../api/axiosClient";

export const createPlannedMeal = async (data) => {
    const response = await axiosClient.post("/plannedmeal", data);
    return response.data;
};

export const updatePlannedMeal = async (id, data) => {
    const response = await axiosClient.put(`/plannedmeal/${id}`, data);
    return response.data;
};

export const deletePlannedMeal = async (id) => {
    await axiosClient.delete(`/plannedmeal/${id}`);
};
