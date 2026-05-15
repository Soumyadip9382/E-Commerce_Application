import api from "./api";

export const loginApi = async (data) => {
  const res = await api.post("/login", data);
  return res.data;
};

export const registerApi = async (data) => {
  const res = await api.post("/register", data);
  return res.data;
};