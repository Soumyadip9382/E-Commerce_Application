import api from "./api";

export const getBannersApi = async () => {
  const res = await api.get("/banners");
  return res.data;
};