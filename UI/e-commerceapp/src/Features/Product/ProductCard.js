import React from "react";
import { useNavigate } from "react-router-dom";
import "./Product.css";

const ProductCard = ({ product }) => {
  const navigate = useNavigate();

  return (
    <div
      className="card"
      onClick={() => navigate(`/products/${product.productID}`)}
    >
      <img
        src={product.displayImage || "https://via.placeholder.com/150"}
        alt={product.productName}
        className="product-img"
      />
      <h3>{product.productName}</h3>
      <p>{product.brand}</p>
      <p>{product.description}</p>
    </div>
  );
};

export default ProductCard;