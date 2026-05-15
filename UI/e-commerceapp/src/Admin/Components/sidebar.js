import React from "react";
import "./sidebar.css";

const Sidebar = () => {
  return (
    <div className="sidebar">
      <h2>Admin</h2>

      <ul>
        <li>Dashboard</li>
        <li>Products</li>
        <li>Variants</li>
        <li>Banners</li>
      </ul>
    </div>
  );
};

export default Sidebar;