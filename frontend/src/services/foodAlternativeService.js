import axiosClient from "../api/axiosClient";

export const getFoodAlternatives = async () => {
  const response = await axiosClient.get("/foodalternatives");
  return response.data;
};

export const createFoodAlternative = async (data) => {
  const response = await axiosClient.post("/foodalternatives", data);
  return response.data;
};

export const updateFoodAlternative = async (id, data) => {
  const response = await axiosClient.put(`/foodalternatives/${id}`, data);
  return response.data;
};

export const deleteFoodAlternative = async (id) => {
  await axiosClient.delete(`/foodalternatives/${id}`);
};