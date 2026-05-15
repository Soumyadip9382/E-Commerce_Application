import React, { useEffect, useRef, useState } from "react";
import { getTopDiscountedProducts } from "../../Services/productAPI";
import { useNavigate } from "react-router-dom";
import "./topDiscounted.css";

const TopDiscounted = () => {
  const [products, setProducts] = useState([]);
  const scrollRef = useRef();
  const navigate = useNavigate();

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const data = await getTopDiscountedProducts();
      setProducts(data);
    } catch (err) {
      console.error(err);
    }
  };

  const scroll = (direction) => {
    const container = scrollRef.current;
    const scrollAmount = 300;

    if (direction === "left") {
      container.scrollBy({ left: -scrollAmount, behavior: "smooth" });
    } else {
      container.scrollBy({ left: scrollAmount, behavior: "smooth" });
    }
  };

  return (
    <div className="top-section">

      <h2 className="section-title">Top Deals</h2>

      <div className="slider-container">

        {/* LEFT BUTTON */}
        <button className="nav-btn left" onClick={() => scroll("left")}>
          ❮
        </button>

        {/* PRODUCTS */}
        <div className="card-container" ref={scrollRef}>
          {products.map((p) => (
            <div
              className="deal-card"
              key={p.variantId}
              onClick={() => navigate(`/products/${p.productId}`)}
            >
              <img src={p.imageUrl} alt={p.productName} />

              <h4>{p.productName}</h4>

              <div className="price">
                <span className="discounted">₹{p.discountedPrice}</span>
                <span className="base">₹{p.basePrice}</span>
              </div>

              <p className="off">{Math.round(p.discountPercentage)}% off</p>
            </div>
          ))}
        </div>

        {/* RIGHT BUTTON */}
        <button className="nav-btn right" onClick={() => scroll("right")}>
          ❯
        </button>

      </div>
    </div>
  );
};

export default TopDiscounted;