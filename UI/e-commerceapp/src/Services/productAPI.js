import api from "./api";

// get all products
export const getProductsApi = async () => {
  const res = await api.get("/api/Product");
  return res.data;
};

// get variants by product id
export const getVariantsApi = async (productId) => {
  const res = await api.get(`/GetProductVariants/${productId}`);
  return res.data;
};

export const getTopDiscountedProducts = async () => {
  const res = await api.get("/GetTopDiscountedProducts");
  return res.data;
};