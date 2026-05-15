import React from "react";
import Sidebar from "./Components/sidebar";
import "./admin.css";
import Dashboard from "./Components/dashboard";

const AdminLayout = ({ children }) => {
  return (
    <div className="admin">
      <Sidebar />
      <div className="main">
        <Dashboard />
        <div className="content">{children}</div>
      </div>
    </div>
  );
};

export default AdminLayout;