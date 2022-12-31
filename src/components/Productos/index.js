import React, { useContext } from "react";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import { DataContext } from "context/DataProvider";
import { ProductoItem } from "./ProductoItem";
import ProductoCaja from "../../images/agregar_producto.png";
import { WSClient } from "../../WSClient";


export const ProductosList = () => {
  const value = useContext(DataContext);
  const [productos] = value.productos;

  const URL = "../../Servicio/rest/ws";
  const foto = null; // por default la foto es nula

  function get(id) {
    return document.getElementById(id);
  }

  function consulta() {
    const producto = new WSClient(URL);
    producto.postJson(
      "consulta_articulo",
      {
        // se debe pasar como parametro el nombre y descripción del articulo a consultar
        // si el articulo no existe regresa un error
        nombre: get("consulta_nombre").value,
        descripcion: get("consulta_descripcion").value,
      },
      function (code, result) {
        if (code == 200) {
          //Lugar donde se pone la respuesta de consulta

          const descripcionProducto = result.descripcion;
          get("consulta_nombre").value = result.nombre;
          get("consulta_precio").value = result.apellido_paterno;
          foto = result.foto;
          get("consulta_imagen").src =
            foto != null ? "data:image/jpeg;base64," + foto : { ProductoCaja };

          get("consulta_nombre").readOnly = true;
        }
        // el objeto "result" es de tipo Error
        else alert(JSON.stringify(result));
      }
    );
  }

  return (
    <>
    <h1 className="produ">CONSULTA DE ARTÍCULO</h1>
      <Form className="form-captura">
        <Form.Group className="mb-3">
          <Form.Label>Nombre *</Form.Label>
          <Form.Control type="text" placeholder="" id="consulta_nombre" />
          <Form.Label>Descripción *</Form.Label>
          <Form.Control type="text" placeholder="" id="consulta_descripcion" />
        </Form.Group>
        <div className="d-grid gap-2 mb-auto">
        <Button onclick={() => consulta()} variant="primary" type="button" size="lg">
          Buscar artículo
        </Button>
        </div>
      </Form>
      <h1 className="hi-alta">PRODUCTOS</h1>
      <div className="productos">
        {productos.map((producto) => (
          <ProductoItem
            key={producto.id}
            title={producto.title}
            image={producto.image}
            category={producto.category}
            price={producto.price}
            id={producto.id}
          />
        ))}
      </div>
      <br></br><br></br><br></br><br></br><br></br><br></br>
    </>
  );
};
