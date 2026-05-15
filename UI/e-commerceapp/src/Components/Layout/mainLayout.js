import React from "react";
import Header from "../Layout/Header/Header";
import Navbar from "../Layout/Navbar/Navbar";
import { Outlet } from "react-router-dom";

const MainLayout = () => {
  return (
    <>
      <Header />
      <Navbar />
      <Outlet />
    </>
  );
};

export default MainLayout;