import React, { useState, useEffect } from "react";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import Figure from "react-bootstrap/Figure";
import ProductoCaja from "../../images/agregar_producto.png";
import Swal from "sweetalert2";
import { WSClient } from "../../WSClient";

export const CapturaArticulo = () => {

  const [nombre, setNombre] = useState("");
  const [descripcion, setDescripcion] = useState("");
  const [precio, setPrecio] = useState("");
  const [cantidad, setCantidad] = useState("");
  const [foto, setFoto] = useState(null);

  /* useEffect(() => {
   
  });*/

  const handleLimpiar = () => {
    setNombre("");
    setDescripcion("");
    setPrecio("");
    setCantidad("");
    setFoto(null);
    const imagen = document.getElementById('alta_imagen');
    imagen.src=ProductoCaja;
  };

  const readSingleFile = async (e) => {
    e.preventDefault();
    var file = e.target.files[0];
    if (!file) return;
    const reader = new FileReader();
    const imagen = document.getElementById('alta_imagen');
    reader.onload = async (e) => { 
      imagen.src = reader.result;
      setFoto = reader.result.split(",")[1];
    };
    reader.readAsDataURL(file);
  }

  function alta(e) {
    e.preventDefault();
    const URL = "../../Servicio/rest/ws";
    var producto = new WSClient(URL);
    var articulo = {
      nombre,
      descripcion,
      precio,
      cantidad,
      foto,
    };
    producto.postJson(
      "alta_articulo",
      {
        articulo: articulo,
      },
      function (code, result) {
        if (code == 200) {
          Swal.fire("El artículo se capturó correctamente", "", "success");
          handleLimpiar();
        } else var error = JSON.parse(JSON.stringify(result));
        Swal.fire(error.message, "El artículo no se creó", "error");
      }
    );
  }

  return (
    <>
      <h1 className="produ">CAPTURA DE ARTÍCULO</h1>
      <Form className="form-captura">
        <Form.Group className="mb-3">
          <Form.Label>Nombre *</Form.Label>
          <Form.Control
            type="text"
            id="alta_nombre"
            name="nombre"
            value={nombre}
            onChange={(e) => setNombre(e.target.value)}
          />
          <Form.Label>Descripción *</Form.Label>
          <Form.Control
            type="text"
            placeholder=""
            id="alta_descripcion"
            name="descripcion"
            value={descripcion}
            onChange={(e) => setDescripcion(e.target.value)}
          />
          <Form.Label>Precio *</Form.Label>
          <Form.Control
            type="text"
            placeholder=""
            id="alta_precio"
            name="precio"
            value={precio}
            onChange={(e) => setPrecio(e.target.value)}
          />
          <Form.Label>Cantidad *</Form.Label>
          <Form.Control
            type="text"
            placeholder=""
            id="alta_cantidad"
            name="cantidad"
            value={cantidad}
            onChange={(e) => setCantidad(e.target.value)}
          />
          <Figure className="espacio">
            <Figure.Image
              id="alta_imagen"
              width={171}
              height={180}
              alt="Agregar Producto"
              src={ProductoCaja}
            />
          </Figure>
          <Form.Control
            type="file"
            multiple={false}
            accept="image/*"
            name="foto"
            onChange={(e) => readSingleFile(e)}
          />
        </Form.Group>
        <div className="d-grid gap-2 mb-auto">
          <Button onClick={alta} variant="primary" type="button" size="lg">
            Crear Artículo
          </Button>
          <Button onClick={() => handleLimpiar()} type="button" size="lg">
            Limpiar
          </Button>
        </div>
        <br></br>
        <br></br>
        <br></br>
        <br></br>
        <br></br>
        <br></br>
        <br></br>
      </Form>
    </>
  );
};

export default CapturaArticulo;
