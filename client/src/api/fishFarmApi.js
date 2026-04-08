import axiosClient from "./axiosClient";

export const createFishFarm = async (data) => {
  const response = await axiosClient.post("/fishfarms", data);
  return response.data;
};

export const fetchFishFarms = async (skip = 0, take = 10) => {
  const response = await axiosClient.get(`/fishfarms?skip=${skip}&take=${take}`);
  console.log("Fetched fish farms:", response.data);
  return response.data;
};

export const fetchWorkersByFishFarm = async (fishFarmId, skip = 0, take = 10) => {
  const response = await axiosClient.get(`/fishfarms/${fishFarmId}/workers?skip=${skip}&take=${take}`);
  console.log(`Fetched workers for fish farm ${fishFarmId}:`, response.data);
  return response.data;
};

export const uploadFishFarmImage = async (file, fishFarmId) => {
  const formData = new FormData();
  formData.append("file", file);
  formData.append("fishFarmId", fishFarmId);

  const response = await axiosClient.post(`/fishfarms/${fishFarmId}/upload-image`, formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });

  return response.data.url;
};
