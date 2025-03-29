//import logo from './logo.svg';
import './App.css';
import React from "react";
import CryptoTable from "./CryptoTable"; // Bileşeni içe aktardık


function App() {
  return (
    <div>
    <h1>Kripto Para Takip</h1>
    <CryptoTable /> {/* Bileşeni burada kullanıyoruz */}
  </div>
  );
}

export default App;
