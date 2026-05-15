import api from "./api";

export const getParentCategories = async () => {
  const res = await api.get("/api/categories/parents");
  return res.data;
};


export const getCategoryDetails = async (categoryId) => {
  const res = await api.get(`/api/categories/${categoryId}`);
  return res.data;
};