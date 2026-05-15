import React from "react";

const Products = () => {
  return (
    <div>
      <h2>Products</h2>

      <button>Add Product</button>

      <table border="1" width="100%" cellPadding="10">
        <thead>
          <tr>
            <th>Name</th>
            <th>Price</th>
            <th>Category</th>
            <th>Actions</th>
          </tr>
        </thead>

        <tbody>
          <tr>
            <td>iPhone 14</td>
            <td>70000</td>
            <td>Electronics</td>
            <td>Edit | Delete</td>
          </tr>
        </tbody>
      </table>
    </div>
  );
};

export default Products;