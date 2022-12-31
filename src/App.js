import React from "react";
import { Header } from "./components/Header/Header";
import { Footer } from "./components/Footer/Footer";
import { Carrito } from "./components/Carrito/Carrito";
import { DataProvider } from "./context/DataProvider";
import { BrowserRouter as Router } from "react-router-dom";
import Routes from "./components/routes.js";
import "boxicons";

function App() {
  return (
    <DataProvider>
      <div className="App">
        <Router>
          <Header />
          <Carrito />
          <Routes />
        </Router>
      </div>
      <Footer />
    </DataProvider>
  );
}

export default App;
