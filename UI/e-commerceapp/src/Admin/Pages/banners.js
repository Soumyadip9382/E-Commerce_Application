import React from "react";

const Banners = () => {
  return (
    <div>
      <h2>Banners</h2>

      <button>Add Banner</button>

      <div style={{ display: "flex", gap: "20px" }}>
        <div>
          <img src="https://via.placeholder.com/300x150" alt="banner" />
          <p>Edit | Delete</p>
        </div>
      </div>
    </div>
  );
};

export default Banners;