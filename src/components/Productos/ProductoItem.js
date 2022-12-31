import React, { useState } from "react";
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';

export const ProductoItem = ({ id, nombre, descripcion, precio, cantidad, foto }) => {

  const [modalShow, setModalShow] = useState(false);

  function ModalCenter(props) {
    return (
      <Modal
        {...props}
        size="lg"
        aria-labelledby="contained-modal-title-vcenter"
        centered
      >
        <Modal.Header closeButton>
          <Modal.Title id="contained-modal-title-vcenter">
            {nombre}
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <h4>Descripción:</h4>
          <p>
            {descripcion}
          </p>
        </Modal.Body>
        <Modal.Footer>
          <Button onClick={props.onHide}>Cerrar</Button>
        </Modal.Footer>
      </Modal>
    );
  }



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
        <Button className="bn" onClick={() => setModalShow(true)}>
        Descripción
        </Button>
        <Button className="bn">
        Compra
        </Button>
      </div>
        <ModalCenter
          show={modalShow}
          onHide={() => setModalShow(false)}
        />
    </div>


    </>
  );
};
