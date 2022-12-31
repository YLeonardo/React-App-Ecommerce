import React, { useState } from "react";
import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';


export const ProductoItem = ({ id, nombre, descripcion, precio, cantidad, foto }) => {

  const [datosId, setDatosId] = useState("");

  return (
    <>
    <div key={id} className="producto">
        <div className="producto__img">
          <img src={foto} alt={nombre} />
        </div>
      <div className="producto__footer">
        <h1>{nombre}</h1>
        <p className="precio">${precio} </p>
        <p className="precio">Disponibles: {cantidad} </p>
      </div>
      <div className="bottom">
        <Button className="bn">
        Descripción
        </Button>
        <Button className="bn">
        Compra
        </Button>
      </div>
    </div>
    </>
    
  );
};
