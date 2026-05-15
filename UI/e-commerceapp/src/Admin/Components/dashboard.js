import React from "react";
import {
  BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer,
  LineChart, Line, PieChart, Pie, Cell, Legend
} from "recharts";

const Dashboard = () => {

  // 📊 Top 5 Products (Bar)
  const topProducts = [
    { name: "iPhone 14", sales: 120 },
    { name: "Samsung TV", sales: 98 },
    { name: "Nike Shoes", sales: 85 },
    { name: "Laptop", sales: 70 },
    { name: "Headphones", sales: 65 },
  ];

  // 📈 Monthly Orders (Line)
  const monthlyOrders = [
    { month: "Jan", orders: 200 },
    { month: "Feb", orders: 300 },
    { month: "Mar", orders: 250 },
    { month: "Apr", orders: 400 },
    { month: "May", orders: 350 },
  ];

  // 🥧 Top Categories (Pie)
  const categories = [
    { name: "Electronics", value: 400 },
    { name: "Fashion", value: 300 },
    { name: "Home", value: 200 },
    { name: "Beauty", value: 150 },
    { name: "Sports", value: 100 },
  ];

  // 💰 Monthly Revenue (Line)
  const revenue = [
    { month: "Jan", revenue: 50000 },
    { month: "Feb", revenue: 80000 },
    { month: "Mar", revenue: 65000 },
    { month: "Apr", revenue: 90000 },
    { month: "May", revenue: 75000 },
  ];

  const COLORS = ["#0088FE", "#00C49F", "#FFBB28", "#FF8042", "#845EC2"];

  return (
    <div style={{ padding: "20px" }}>
      
      <div style={{ display: "flex", gap: "20px", marginBottom: "20px" }}>
        
        {/* Top Products */}
        <div style={{ flex: 1, background: "#fff", padding: "20px" }}>
          <h3>Top 5 Products</h3>
          <ResponsiveContainer width="100%" height={250}>
            <BarChart data={topProducts}>
              <XAxis dataKey="name" />
              <YAxis />
              <Tooltip />
              <Bar dataKey="sales" fill="#2874f0" />
            </BarChart>
          </ResponsiveContainer>
        </div>

        {/* Monthly Orders */}
        <div style={{ flex: 1, background: "#fff", padding: "20px" }}>
          <h3>Monthly Orders</h3>
          <ResponsiveContainer width="100%" height={250}>
            <LineChart data={monthlyOrders}>
              <XAxis dataKey="month" />
              <YAxis />
              <Tooltip />
              <Line type="monotone" dataKey="orders" stroke="#ff7300" />
            </LineChart>
          </ResponsiveContainer>
        </div>
      </div>

      {/* ROW 2 */}
      <div style={{ display: "flex", gap: "20px", marginBottom: "20px" }}>

        {/* Categories */}
        <div style={{ flex: 1, background: "#fff", padding: "20px" }}>
          <h3>Top Categories</h3>
          <ResponsiveContainer width="100%" height={250}>
            <PieChart>
              <Pie data={categories} dataKey="value" outerRadius={80}>
                {categories.map((entry, index) => (
                  <Cell key={index} fill={COLORS[index % COLORS.length]} />
                ))}
              </Pie>
              <Legend />
            </PieChart>
          </ResponsiveContainer>
        </div>

        {/* Revenue */}
        <div style={{ flex: 1, background: "#fff", padding: "20px" }}>
          <h3>Monthly Revenue</h3>
          <ResponsiveContainer width="100%" height={250}>
            <LineChart data={revenue}>
              <XAxis dataKey="month" />
              <YAxis />
              <Tooltip />
              <Line type="monotone" dataKey="revenue" stroke="#28a745" />
            </LineChart>
          </ResponsiveContainer>
        </div>
      </div>

      {/* INSIGHTS / SUGGESTIONS */}
      <div style={{ background: "#fff", padding: "20px" }}>
        <h3>Insights & Suggestions</h3>
        <ul>
          <li>🔥 Electronics category is dominating sales</li>
          <li>📈 Orders increased significantly in April</li>
          <li>⚠️ Some products have declining sales — consider discounts</li>
          <li>💡 Promote low-performing categories with banners</li>
          <li>📦 Check inventory for top-selling items to avoid stock-out</li>
        </ul>
      </div>

    </div>
  );
};

export default Dashboard;