import React from "react";
import { Link } from "react-router-dom";
import Portada from "images/Jordan3.png";

export default function Inicio() {
  return (
    <>
    <div className="inicio">
      <h1  className="titulo-bienvenido">BIENVENIDO!</h1>
      <img src={Portada} alt="Jordan 3 Tinker" />
    </div>
    <br></br><br></br><br></br><br></br><br></br><br></br><br></br>
    </>
  );
}
