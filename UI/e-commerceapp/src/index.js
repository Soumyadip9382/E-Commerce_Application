import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App/App";
import { AuthProvider } from "./Features/Auth/authContext";
import "./Styles/globalStyle.css";

const root = ReactDOM.createRoot(document.getElementById("root"));

root.render(
  <AuthProvider>
    <App />
  </AuthProvider>
);