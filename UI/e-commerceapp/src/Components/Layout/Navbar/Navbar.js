import React, { useEffect, useState } from "react";
import { getParentCategories } from "../../../Services/categoryAPI";
import "./Navbar.css";

import { useNavigate } from "react-router-dom";

const Navbar = () => {
  const [categories, setCategories] = useState([]);
  const navigate = useNavigate();
  useEffect(() => {
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    try {
      const data = await getParentCategories();

      const formatted = data.map((cat) => ({
        ...cat,
        formattedName: formatCategoryName(cat.categoryName),
      }));

      setCategories(formatted);
    } catch (err) {
      console.error("Category API error:", err);
    }
  };

  // 🔧 format function
  const formatCategoryName = (name) => {
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

  return (
    <div className="navbar">
      {categories.map((cat) => (
        <span onClick={() => navigate(`/category/${cat.categoryID}`)}>
          {cat.formattedName}
        </span>
      ))}
    </div>
  );
};

export default Navbar;