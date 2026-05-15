import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getCategoryDetails } from "../../Services/categoryAPI";
import ProductCard from "../Product/ProductCard";
import "./category.css";

const CategoryPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [data, setData] = useState(null);

  useEffect(() => {
    fetchCategory();
  }, [id]);

  const fetchCategory = async () => {
    try {
      const res = await getCategoryDetails(id);
      setData(res);
    } catch (err) {
      console.error(err);
    }
  };

  // format name
  const formatName = (name) => {
    return name
      .split("&")
      .map((word) =>
        word
          .trim()
          .split(" ")
          .map(
            (w) => w.charAt(0).toUpperCase() + w.slice(1)
          )
          .join(" ")
      )
      .join(" & ");
  };

  if (!data) return <p>Loading...</p>;

  return (
    <div className="category-page">

      <h2>{formatName(data.categoryName)}</h2>

      {/* 🔹 CASE 1: HAS SUBCATEGORIES */}
      {data.hasChildren ? (
        <div className="category-grid">
          {data.subCategories.map((sub) => (
            <div
              key={sub.categoryID}
              className="category-card"
              onClick={() => navigate(`/category/${sub.categoryID}`)}
            >
              {formatName(sub.categoryName)}
            </div>
          ))}
        </div>
      ) : (
        /* 🔹 CASE 2: SHOW PRODUCTS */
        <div className="product-grid">
          {data.products.map((p) => (
            <ProductCard key={p.productID} product={p} />
          ))}
        </div>
      )}
    </div>
  );
};

export default CategoryPage;