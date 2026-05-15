import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getVariantsApi } from "../../Services/productAPI";
import "./productDetails.css";

const ProductDetails = () => {
  const { id } = useParams();
  const [variants, setVariants] = useState([]);

  useEffect(() => {
    fetchVariants();
  }, [id]);

  const fetchVariants = async () => {
    const data = await getVariantsApi(id);
    setVariants(data);
  };

  const getPrimaryImage = (images) => {
    return images?.find((img) => img.isPrimary)?.imageURL || images?.[0]?.imageURL;
  };

  return (
    <div className="variant-grid">
      {variants.map((item) => {
        const v = item.variants;

        return (
          <div className="variant-card" key={v.variantID}>
            
            {/* IMAGE */}
            <img
              src={getPrimaryImage(item.images)}
              alt={v.variantName}
              className="variant-img"
            />

            {/* TITLE */}
            <h4 className="title">{v.variantName}</h4>

            {/* COLOR + STORAGE */}
            <p className="meta">
              {v.color} | {v.size}
            </p>

            {/* PRICE */}
            <div className="price-section">
              <span className="discounted">
                ₹{v.discountedPrice}
              </span>

              <span className="base">
                ₹{v.basePrice}
              </span>
            </div>

            {/* STOCK */}
            <p className={v.stockQuantity > 0 ? "in-stock" : "out-stock"}>
              {v.stockQuantity > 0 ? "In Stock" : "Out of Stock"}
            </p>

          </div>
        );
      })}
    </div>
  );
};

export default ProductDetails;