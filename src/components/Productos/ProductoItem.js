import React, { useContext } from "react";
import { Link } from "react-router-dom";
import { DataContext } from "context/DataProvider";

export const ProductoItem = ({ id, nombre, descripcion, precio, cantidad, foto }) => {
  const value = useContext(DataContext);
  const addCarrito = value.addCarrito;

  return (
    <div key={id} className="producto">
      <Link to={`/producto/${id}`}>
        <div className="producto__img">
          <img src={foto} alt={nombre} />
        </div>
      </Link>
      <div className="producto__footer">
        <h1>{nombre}</h1>
        <p className="precio">${precio} </p>
        <p className="precio">Disponibles: {cantidad} </p>
      </div>
      <div className="bottom">
        <button onClick={() => addCarrito(id)} className="btn">
          Añadir al carrito
        </button>
        <div>
          <Link to={`/producto/${id}`} className="btn">
            Descripción
          </Link>
        </div>
      </div>
    </div>
  );
};
