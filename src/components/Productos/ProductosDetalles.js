import React, { useContext, useEffect, useState } from "react";
import { DataContext } from "context/DataProvider";
import { useParams } from "react-router-dom";
import { ProductoItem } from "./ProductoItem";

export const ProductosDetalles = () => {
  const value = useContext(DataContext);
  const [productos] = value.productos;
  const addCarrito = value.addCarrito;
  const [detalle, setDetalle] = useState([]);
  const [url, setUrl] = useState(0);
  const [images, setImages] = useState("");
  const params = useParams();
  let item = 0;

  useEffect(() => {
    console.log("re render", params.id);
    item = 0;
    productos.forEach((producto) => {
      if (producto.id === parseInt(params.id)) {
        setDetalle(producto);
        setUrl(0);
      }
    });
  }, [params.id, productos]);

  console.log(url);

  useEffect(() => {
    const values = `${detalle.img1}${url}${detalle.img2}`;
    setImages(values);
  }, [url, params.id]);

  const handleInput = (e) => {
    const number = e.target.value.toString().padStart(2, "01");
    setUrl(number);
  };

  if (detalle.length < 1) return null;

  return (
    <>
      {
        <div className="detalles">
          <h2>{detalle.nombre}</h2>

          {url ? (
            <img src={images} alt={detalle.nombre} />
          ) : (
            <img src={detalle.foto} alt={detalle.nombre} />
          )}
          <input
            type="range"
            min="1"
            max="36"
            step="1"
            value={url}
            onChange={handleInput}
          />

          <div className="description">
            <p className="price">${detalle.precio}</p>
            <div className="grid"></div>

            <p>
              <h3>Descripción: </h3>
              <br></br>
              <b>{detalle.descripcion}</b>
            </p>
            <br></br>
            <br></br>
            <button onClick={() => addCarrito(detalle.id)}>
              Añadir al carrito
            </button>
          </div>
        </div>
      }
      <h2 className="relacionados">Productos relacionados</h2>
      <div className="productos">
        {productos.map((producto) => {
          if (item < 6 && detalle.descripcion === producto.descripcion && producto.cantidad >= 1) {
            item++;
            return (
              <ProductoItem
                key={producto.id}
                id={producto.id}
                nombre={producto.nombre}
                descripcion={producto.descripcion}
                precio={producto.precio}
                cantidad={producto.cantidad}
                foto={producto.foto}
              />
            );
          }
        })}
      </div>
    </>
  );
};
