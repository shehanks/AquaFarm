import axios from "axios";
import { config } from "../config";

const axiosClient = axios.create({
  baseURL: `${config.backendUrl}/api`,
  headers: {
    "Content-Type": "application/json",
  },
});

export default axiosClient;
