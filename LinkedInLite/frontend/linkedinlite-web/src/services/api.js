import axios from "axios";
import { authStore } from "../store/auth.store";
import { API_BASE_URL } from "../config";

const api = axios.create({
  baseURL: `${API_BASE_URL}/api` // match backend port
});

api.interceptors.request.use((config) => {
  const user = authStore.getUser();
  if (user?.token) {
    config.headers.Authorization = `Bearer ${user.token}`;
  }
  return config;
});

export default api;
