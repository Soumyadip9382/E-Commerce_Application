import React from "react";
import Banner from "../../Components/Banner/Banner";
import ProductList from "../Product/productList";
import TopDiscounted from "./topDiscounted";

const HomePage = () => 
{
    return (
        <div>
        <Banner />
        <TopDiscounted />
        {/* <ProductList/> */}
        </div>
    );
}
export default HomePage;