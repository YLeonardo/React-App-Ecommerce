import React, { useState } from "react";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import { ProductoItem } from "./ProductoItem";
import Swal from "sweetalert2";
import { WSClient } from "../../WSClient";

export const ProductosList = () => {
  const [nombre, setNombre] = useState("");
  const [descripcion, setDescripcion] = useState("");
  const [productos, setProductos] = useState([]);

  const handleLimpiar = () => {
    setNombre("");
    setDescripcion("");
  };

  function consulta() {
    const URL = "../../api";
    const producto = new WSClient(URL);
    producto.postJson(
      "consulta_articulo",
      {
        nombre,
        descripcion,
      },
      function (code, result) {
        if (code == 200) {
          setProductos(result);

          Swal.fire("El artículo se capturó correctamente", "", "success");
          handleLimpiar();
        }
        var error = JSON.parse(JSON.stringify(result));
        Swal.fire(error.message, "No se encontró alguna coincidencia", "error");
      }
    );
  }

  return (
    <>
      <h1 className="produ">CONSULTA DE ARTÍCULO</h1>
      <Form className="form-captura">
        <Form.Group className="mb-3">
          <Form.Label>Nombre *</Form.Label>
          <Form.Control
            type="text"
            id="consulta_nombre"
            name="nombre"
            value={nombre}
            onChange={(e) => setNombre(e.target.value)}
          />
          <Form.Label>Descripción *</Form.Label>
          <Form.Control
            type="text"
            placeholder=""
            id="consulta_descripcion"
            name="descripcion"
            value={descripcion}
            onChange={(e) => setDescripcion(e.target.value)}
          />
        </Form.Group>
        <div className="d-grid gap-2 mb-auto">
          <Button
            onClick={() => consulta()}
            variant="primary"
            type="button"
            size="lg"
          >
            Buscar artículo
          </Button>
        </div>
      </Form>
      <h1 className="hi-alta">PRODUCTOS</h1>
      <div className="productos">
        {productos.map((producto) => (
          <ProductoItem
            key={producto.id}
            id={producto.id}
            nombre={producto.nombre}
            descripcion={producto.descripcion}
            precio={producto.precio}
            cantidad={producto.cantidad}
            foto={producto.foto}
          />
        ))}
      </div>
      <br></br>
      <br></br>
      <br></br>
      <br></br>
      <br></br>
      <br></br>
    </>
  );
};
