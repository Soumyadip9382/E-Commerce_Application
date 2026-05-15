import React from "react";
import "./Header.css";

const Header = () => {
  return (
    <div className="header">
      <div className="logo">Flipkart</div>

      <input
        className="search"
        type="text"
        placeholder="Search for products, brands and more"
      />

      <div className="actions">
        <button className="login">Login</button>
        <span className="cart">Cart</span>
      </div>
    </div>
  );
};

export default Header;