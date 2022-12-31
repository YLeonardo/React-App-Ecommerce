package servicio_json;

import javax.ws.rs.GET;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.Consumes;
import javax.ws.rs.Produces;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.QueryParam;
import javax.ws.rs.FormParam;
import javax.ws.rs.core.Response;

import java.sql.*;
import javax.sql.DataSource;
import javax.naming.Context;
import javax.naming.InitialContext;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import com.google.gson.*;

// la URL del servicio web es http://localhost:8080/Servicio/rest/ws
// donde:
//	"Servicio" es el dominio del servicio web (es decir, el nombre de archivo Servicio.war)
//	"rest" se define en la etiqueta <url-pattern> de <servlet-mapping> en el archivo WEB-INF\web.xml
//	"ws" se define en la siguiente anotacin @Path de la clase Servicio

@Path("ws")
public class Servicio
{
  static DataSource pool = null;
  static
  {		
    try
    {
      Context ctx = new InitialContext();
      pool = (DataSource)ctx.lookup("java:comp/env/jdbc/datasource_Servicio");
    }
    catch(Exception e)
    {
      e.printStackTrace();
    }
  }

  static Gson j = new GsonBuilder().registerTypeAdapter(byte[].class,new AdaptadorGsonBase64()).setDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS").create();

  @POST
  @Path("alta_articulo")
  @Consumes(MediaType.APPLICATION_JSON)
  @Produces(MediaType.APPLICATION_JSON)
  public Response alta(String json) throws Exception
  {
    ParamAltaArticulo p = (ParamAltaArticulo) j.fromJson(json,ParamAltaArticulo.class);
    Articulo articulo = p.articulo;

    Connection conexion = pool.getConnection();

    if (articulo.nombre == null || articulo.nombre.equals(""))
      return Response.status(400).entity(j.toJson(new Error("Se debe ingresar el nombre"))).build();

    if (articulo.descripcion == null || articulo.descripcion.equals(""))
      return Response.status(400).entity(j.toJson(new Error("Se debe ingresar la descripción"))).build();

    if (articulo.precio < 0)
      return Response.status(400).entity(j.toJson(new Error("Se debe ingresar un precio válido"))).build();
    
    if (articulo.cantidad < 0)
      return Response.status(400).entity(j.toJson(new Error("Se debe ingresar una cantidad válida"))).build();
    
    if (articulo.foto == null)
      return Response.status(400).entity(j.toJson(new Error("Se debe ingresar una foto"))).build();
    
    try
    {
      conexion.setAutoCommit(false);

      PreparedStatement stmt_1 = conexion.prepareStatement("INSERT INTO articulos(id_articulo,nombre,descripcion,precio,cantidad,foto) VALUES (0,?,?,?,?,?)");
 
      try
      {
        stmt_1.setString(1,articulo.nombre);
        stmt_1.setString(2,articulo.descripcion);
        stmt_1.setFloat(3,articulo.precio);
        stmt_1.setInt(4,articulo.cantidad);
        stmt_1.setBytes(5,articulo.foto);
        stmt_1.executeUpdate();
      }
      finally
      {
        stmt_1.close();
      }

      conexion.commit();
    }
    catch (Exception e)
    {
      conexion.rollback();
      return Response.status(400).entity(j.toJson(new Error(e.getMessage()))).build();
    }
    finally
    {
      conexion.setAutoCommit(true);
      conexion.close();
    }
    return Response.ok().build();
  }

 
  @POST
  @Path("consulta_articulo")
  @Consumes(MediaType.APPLICATION_JSON)
  @Produces(MediaType.APPLICATION_JSON)
  public Response consulta(String json) throws Exception
  {
    ParamConsultaArticulos p = (ParamConsultaArticulos) j.fromJson(json,ParamConsultaArticulos.class);
    String nombre = p.nombre;
    String descripcion = p.descripcion;

    Connection conexion= pool.getConnection();

    if (p.nombre == null || p.nombre.equals(""))
    return Response.status(400).entity(j.toJson(new Error("Se debe ingresar el nombre"))).build();

    if (p.descripcion == null || p.descripcion.equals(""))
    return Response.status(400).entity(j.toJson(new Error("Se debe ingresar la descripción"))).build();

    try
    {
      PreparedStatement stmt_1 = conexion.prepareStatement("SELECT id_articulo,nombre,descripcion,precio,cantidad,foto FROM articulos WHERE nombre LIKE ? OR descripcion LIKE ?");
      try
      {
        
        stmt_1.setString(1,'%'+nombre+'%');
        stmt_1.setString(2,'%'+descripcion+'%'); 
        ResultSet rs = stmt_1.executeQuery();
        try
        { 
          ArrayList<Articulo> lista = new ArrayList<Articulo>();
          while (rs.next())
          {
            Articulo r = new Articulo();
            r.id_articulo = rs.getInt(1);
            r.nombre = rs.getString(2);
            r.descripcion = rs.getString(3);
            r.precio = rs.getFloat(4);
            r.cantidad = rs.getInt(5);
            r.foto = rs.getBytes(6);
            lista.add(r);

            return Response.ok().entity(j.toJson(lista.toArray())).build();
          }
          return Response.status(400).entity(j.toJson(new Error("El artículo no existe"))).build();
        }
        finally
        {
          rs.close();
        }
      }
      finally
      {
        stmt_1.close();
      }
    }
    catch (Exception e)
    {
      return Response.status(400).entity(j.toJson(new Error(e.getMessage()))).build();
    }
    finally
    {
      conexion.close();
    }
  }

  @POST
  @Path("consulta_cantidad")
  @Consumes(MediaType.APPLICATION_JSON)
  @Produces(MediaType.APPLICATION_JSON)
  public Response consulta_cantidad(String json) throws Exception
  {
    ParamConsultaCantidad p = (ParamConsultaCantidad) j.fromJson(json,ParamConsultaCantidad.class);
    int id_articulo = p.id_articulo;

    Connection conexion= pool.getConnection();

    try
    {
      PreparedStatement stmt_1 = conexion.prepareStatement("SELECT cantidad FROM articulos WHERE id_articulo=?");
      try
      {
        stmt_1.setInt(1,id_articulo);

        ResultSet rs = stmt_1.executeQuery();
        try
        {
          if (rs.next())
          {
            int cantidad = rs.getInt(1);

            return Response.ok().entity(j.toJson(cantidad)).build();
          }
          return Response.status(400).entity(j.toJson(new Error("El artículo no existe"))).build();
        }
        finally
        {
          rs.close();
        }
      }
      finally
      {
        stmt_1.close();
      }
    }
    catch (Exception e)
    {
      return Response.status(400).entity(j.toJson(new Error(e.getMessage()))).build();
    }
    finally
    {
      conexion.close();
    }
  }

  @POST
  @Path("comprar")
  @Consumes(MediaType.APPLICATION_JSON)
  @Produces(MediaType.APPLICATION_JSON)
  public Response comprar(String json) throws Exception
  {
    ParamComprar p = (ParamComprar) j.fromJson(json,ParamComprar.class);
    int id_articulo = p.id_articulo;
    int cantidad = p.cantidad;

    Connection conexion= pool.getConnection();

    conexion.setAutoCommit(false);

    try
    {
      PreparedStatement stmt_1 = conexion.prepareStatement("UPDATE articulos SET cantidad=cantidad=? WHERE id_articulo=?");
      try
      {
        stmt_1.setInt(1,cantidad);
        stmt_1.setInt(2,id_articulo);
        stmt_1.executeUpdate();
      }
      finally
      {
        stmt_1.close();
      }

      PreparedStatement stmt_2 = conexion.prepareStatement("INSERT INTO carrito_compra (id_articulo,cantidad) VALUES (?,?)");
      try
      {
        stmt_2.setInt(1,id_articulo);
        stmt_2.setInt(2,cantidad);
        stmt_2.executeUpdate();
      }
      finally
      {
        stmt_2.close();
      }
      conexion.commit();
    }
    catch (Exception e)
    {
      conexion.rollback();
      return Response.status(400).entity(j.toJson(new Error(e.getMessage()))).build();
    }
    finally
    {
      conexion.setAutoCommit(true);
      conexion.close();
    }
    return Response.ok().build();
  }

  @POST
  @Path("borrar_articulo")
  @Consumes(MediaType.APPLICATION_JSON)
  @Produces(MediaType.APPLICATION_JSON)
  public Response borrar_articulo(String json) throws Exception
  {
    ParamBorrarArticulo p = (ParamBorrarArticulo) j.fromJson(json,ParamBorrarArticulo.class);
    int id_articulo = p.id_articulo;

    Connection conexion= pool.getConnection();

    conexion.setAutoCommit(false);

    try
    {
      PreparedStatement stmt_1 = conexion.prepareStatement("UPDATE articulos SET cantidad+(SELECT cantidad FROM carrito_compra WHERE id_articulo=?) WHERE id_articulo=?");
      try
      {
        stmt_1.setInt(1,id_articulo);
        stmt_1.setInt(2,id_articulo);

        stmt_1.executeUpdate();
        
      }
      finally
      {
        stmt_1.close();
      }

      PreparedStatement stmt_2 = conexion.prepareStatement("DELETE FROM carrito_compra WHERE id_articulo=?");
      try
      {
        stmt_2.setInt(1,id_articulo);
	      stmt_2.executeUpdate();
      }
      finally
      {
        stmt_2.close();
      }

      
      conexion.commit();
    }
    catch (Exception e)
    {
      conexion.rollback();
      return Response.status(400).entity(j.toJson(new Error(e.getMessage()))).build();
    }
    finally
    {
      conexion.setAutoCommit(true);
      conexion.close();
    }
    return Response.ok().build();
  }

  @POST
  @Path("borrar_carrito")
  @Consumes(MediaType.APPLICATION_JSON)
  @Produces(MediaType.APPLICATION_JSON)
  public Response borrar_carrito(String json) throws Exception
  {
    ParamBorrarArticulo p = (ParamBorrarArticulo) j.fromJson(json,ParamBorrarArticulo.class);
    int id_articulo = p.id_articulo;

    Connection conexion= pool.getConnection();

    PreparedStatement stmt_1 = conexion.prepareStatement("SELECT id_articulo FROM carrito_compra WHERE id_articulo=?");
      try
      {
        stmt_1.setInt(1,id_articulo);

        ResultSet rs = stmt_1.executeQuery();
      }
      finally
      {
        stmt_1.close();
      }

    conexion.setAutoCommit(false);

    try
    {
      PreparedStatement stmt_2 = conexion.prepareStatement("UPDATE articulos SET cantidad+(SELECT cantidad FROM carrito_compra WHERE id_articulo=?) WHERE id_articulo=?");
      try
      {
        stmt_2.setInt(1,id_articulo);
        stmt_2.setInt(2,id_articulo);

        stmt_2.executeUpdate();
        
      }
      finally
      {
        stmt_2.close();
      }

      PreparedStatement stmt_3 = conexion.prepareStatement("DELETE FROM carrito_compra WHERE id_articulo=?");
      try
      {
        stmt_3.setInt(1,id_articulo);
	      stmt_3.executeUpdate();
      }
      finally
      {
        stmt_3.close();
      }

      
      conexion.commit();
    }
    catch (Exception e)
    {
      conexion.rollback();
      return Response.status(400).entity(j.toJson(new Error(e.getMessage()))).build();
    }
    finally
    {
      conexion.setAutoCommit(true);
      conexion.close();
    }
    return Response.ok().build();
  }

  
}
