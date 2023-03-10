import React from "react";
import { Switch, Route } from "react-router-dom";
import Home from "./Home/index";
import { ProductosList } from "./Productos";
import { CapturaArticulo } from "./CapturaArticulo/CapturaArticulo";

export default function Routes() {
  return (
    <section>
      <Switch>
        <Route path="/" exact component={Home} />
        <Route path="/compra" exact component={ProductosList} />
        <Route path="/captura_articulo" exact component={CapturaArticulo} />
        <Route path="*" element={<h1>404</h1>} />
      </Switch>
    </section>
  );
}
