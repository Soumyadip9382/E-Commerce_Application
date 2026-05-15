import React from "react";

const Variants = () => {
  return (
    <div>
      <h2>Variants</h2>

      <button>Add Variant</button>

      <table border="1" width="100%" cellPadding="10">
        <thead>
          <tr>
            <th>Product</th>
            <th>Size</th>
            <th>Color</th>
            <th>Stock</th>
            <th>Price</th>
          </tr>
        </thead>

        <tbody>
          <tr>
            <td>Nike Shoes</td>
            <td>10</td>
            <td>Black</td>
            <td>50</td>
            <td>5000</td>
          </tr>
        </tbody>
      </table>
    </div>
  );
};

export default Variants;