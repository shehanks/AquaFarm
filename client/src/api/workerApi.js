import axiosClient from "./axiosClient";

export const createWorker = async (data) => {
  const response = await axiosClient.post("/workers", data);
  return response.data;
};

export const uploadWorkerImage = async (file, workerId) => {
  const formData = new FormData();
  formData.append("file", file);
  formData.append("workerId", workerId);

  const response = await axiosClient.post(`/workers/${workerId}/upload-image`, formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });

  return response.data.url;
};