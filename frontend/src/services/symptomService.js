import axiosClient from "../api/axiosClient";

export const getSymptoms = async (from, to) => {
    const params = from && to ? { from, to } : undefined;
    const response = await axiosClient.get("/symptomentry", { params });
    return response.data;
};

export const createSymptom = async (data) => {
    const response = await axiosClient.post("/symptomentry", data);
    return response.data;
};

export const updateSymptom = async (id, data) => {
    const response = await axiosClient.put(`/symptomentry/${id}`, data);
    return response.data;
};

export const deleteSymptom = async (id) => {
    await axiosClient.delete(`/symptomentry/${id}`);
};
