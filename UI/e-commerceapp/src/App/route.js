import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";

import MainLayout from "../Components/Layout/mainLayout";
import AuthLayout from "../Components/Layout/authLayout";

import HomePage from "../Features/Home/home";
import ProductList from "../Features/Product/productList";
import CartPage from "../Features/Cart/cart";

import LoginPage from "../Features/Auth/loginPage";
import RegisterPage from "../Features/Auth/registerPage";

import ProtectedRoute from "../Components/Common/protectedRoute";

import ProductDetails from "../Features/Product/productDetails";

import CategoryPage from "../Features/Categories/categoryPage";

import AdminLayout from "../Admin/adminLayout";
import Products from "../Admin/Pages/products";
import Variants from "../Admin/Pages/variants";
import Banners from "../Admin/Pages/banners";

const AppRoutes = () => {
  return (
    <BrowserRouter>
      <Routes>

        {/* 🔹 MAIN APP (with header/navbar) */}
        <Route element={<MainLayout />}>
          <Route path="/" element={<HomePage />} />
          <Route path="/products" element={<ProductList />} />
          <Route path="/products/:id" element={<ProductDetails />} />

          <Route path="/category/:id" element={<CategoryPage />} />
          <Route path="/admin" element={<AdminLayout />}>
            <Route path="admin/products" element={<Products />} />
            <Route path="admin/variants" element={<Variants />} />
            <Route path="admin/banners" element={<Banners />} />
          </Route>

          <Route
            path="/cart"
            element={
              <ProtectedRoute>
                <CartPage />
              </ProtectedRoute>
            }
          />
        </Route>

        {/* 🔹 AUTH PAGES (no header/navbar) */}
        <Route element={<AuthLayout />}>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
        </Route>

      </Routes>
    </BrowserRouter>
  );
};

export default AppRoutes;