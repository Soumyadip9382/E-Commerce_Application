import React, { useState } from "react";
import { registerApi } from "../../Services/authAPI";
import { useNavigate, Link } from "react-router-dom";

const RegisterPage = () => {
  const [form, setForm] = useState({
    name: "",
    email: "",
    password: "",
  });

  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await registerApi(form);
      alert("Registered successfully");
      navigate("/login");
    } catch (err) {
      alert(err.response?.data?.message || "Error");
    }
  };

  return (
    <div className="auth">
      <h2>Register</h2>

      <form onSubmit={handleSubmit}>
        <input placeholder="Name"
          onChange={(e) => setForm({ ...form, name: e.target.value })} />

        <input type="email" placeholder="Email"
          onChange={(e) => setForm({ ...form, email: e.target.value })} />

        <input type="password" placeholder="Password"
          onChange={(e) => setForm({ ...form, password: e.target.value })} />

        <button>Register</button>
      </form>

      <p>
        Already user? <Link to="/login">Login</Link>
      </p>
    </div>
  );
};

export default RegisterPage;