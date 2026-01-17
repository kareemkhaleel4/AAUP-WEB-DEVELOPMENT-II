import api from "./api";

export const login = async (email, password) => {
  const res = await api.post("/auth/login", { email, password });
  return res.data; // { token }
};

export const register = async (fullName, email, password) => {
  const res = await api.post("/auth/register", {
    fullName,
    email,
    password
  });
  return res.data;
};
