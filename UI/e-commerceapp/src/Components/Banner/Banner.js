import React, { useEffect, useState } from "react";
import { getBannersApi } from "../../Services/bannerAPI";
import "./Banner.css";

const Banner = () => {
  const [banners, setBanners] = useState([]);
  const [current, setCurrent] = useState(0);

  useEffect(() => {
    fetchBanners();
  }, []);

  // ✅ Fetch + Filter + Sort
  const fetchBanners = async () => {
    try {
      const data = await getBannersApi();

      const now = new Date();

      const filtered = data
        .filter(
          (b) =>
            b.isActive &&
            new Date(b.startDate) <= now &&
            new Date(b.endDate) >= now
        )
        .sort((a, b) => a.displayOrder - b.displayOrder);

      setBanners(filtered);
    } catch (err) {
      console.error("Banner API error:", err);
    }
  };

  // ✅ Auto slide
  useEffect(() => {
    if (banners.length === 0) return;

    const interval = setInterval(() => {
      setCurrent((prev) => (prev + 1) % banners.length);
    }, 3000);

    return () => clearInterval(interval);
  }, [banners]);

  const nextSlide = () => {
    setCurrent((prev) => (prev + 1) % banners.length);
  };

  const prevSlide = () => {
    setCurrent((prev) =>
      prev === 0 ? banners.length - 1 : prev - 1
    );
  };

  const handleRedirect = (banner) => {
    if (banner.redirectType === "URL" && banner.redirectUrl) {
      window.open(banner.redirectUrl, "_blank");
    }
  };

  if (!banners.length) return null;

  return (
    <div className="banner-container">
      
      {/* IMAGE */}
      <img
        src={banners[current].imageUrl}
        alt={banners[current].title}
        className="banner-img"
        onClick={() => handleRedirect(banners[current])}
      />

      {/* BUTTONS */}
      <button className="prev" onClick={prevSlide}>❮</button>
      <button className="next" onClick={nextSlide}>❯</button>

      {/* DOTS */}
      <div className="dots">
        {banners.map((_, i) => (
          <span
            key={i}
            className={current === i ? "dot active" : "dot"}
            onClick={() => setCurrent(i)}
          />
        ))}
      </div>
    </div>
  );
};

export default Banner;