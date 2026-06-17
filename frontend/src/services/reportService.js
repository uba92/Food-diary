import axiosClient from "../api/axiosClient";

export const getReport = async (from, to) => {
    const response = await axiosClient.get("/report", {
        params: { from, to },
    });
    return response.data;
};

export const downloadReportPdf = async (from, to) => {
    const response = await axiosClient.get("/report/pdf", {
        params: { from, to },
        responseType: "blob",
    });
    return response.data;
};
