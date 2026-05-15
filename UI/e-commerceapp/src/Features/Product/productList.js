import React, { useEffect, useState } from "react";
import { getProductsApi } from "../../Services/productAPI";
import ProductCard from "./ProductCard";
import "./Product.css";

const ProductList = () => {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    const data = await getProductsApi();
    setProducts(data);
  };

  return (
    <div className="grid">
      {products.map((p) => (
        <ProductCard key={p.productID} product={p} />
      ))}
    </div>
  );
};

export default ProductList;