using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using static ServicioWeb.Consulta_articulo;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace ServicioWeb
{
    public static class Borrar_carrito
    {

        public class Articulo
        {
            public int id_articulo;
            public String nombre;
            public String descripcion;
            public int precio;
            public int cantidad;
            public string foto;
        }

        [FunctionName("borrar_carrito")]
        public static async Task<IActionResult> Run(
             [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
             ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                string Server = Environment.GetEnvironmentVariable("Server");
                string UserID = Environment.GetEnvironmentVariable("UserID");
                string Password = Environment.GetEnvironmentVariable("Password");
                string Database = Environment.GetEnvironmentVariable("Database");

                string sc = "Server=" + Server + ";UserID=" + UserID + ";Password=" + Password + ";" + "Database=" + Database + ";SslMode=Preferred;";
                var conexion = new MySqlConnection(sc);
                conexion.Open();

                MySqlTransaction transaccion = conexion.BeginTransaction();

                try
                {
                    var cmd = new MySqlCommand("SELECT a.nombre, a.descripcion, a.precio, length(b.foto), b.foto, c.cantidad, a.id_articulo FROM carrito_compra c INNER JOIN articulos a ON c.id_articulo=a.id_articulo", conexion);

                    MySqlDataReader r = cmd.ExecuteReader();
                    ArrayList articulos = new ArrayList();

                    while (r.Read())
                    {
                        Articulo articulo = new Articulo();
                        articulo.descripcion = r.GetString(0);
                        articulo.precio = r.GetInt32(1);
                    if (!r.IsDBNull(3))
                    {
                        var longitud = r.GetInt32(2);
                        byte[] foto = new byte[longitud];
                        r.GetBytes(3, 0, foto, 0, longitud);
                        articulo.foto = Convert.ToBase64String(foto);
                    }
                    articulo.cantidad = r.GetInt32(4);
                        articulo.id_articulo = r.GetInt32(5);
                        articulos.Add(articulo);
                    }
                    r.Close();

                    var cmd_delete = new MySqlCommand("DELETE FROM carrito_compra", conexion);
                    cmd_delete.ExecuteNonQuery();

                    foreach (Articulo articulo in articulos)
                    {
                        var cmd_update = new MySqlCommand("UPDATE articulos SET `cantidad`=`cantidad`+@cantidad WHERE id_articulo=@id", conexion);
                        cmd_update.Parameters.AddWithValue("@cantidad", articulo.cantidad);
                        cmd_update.Parameters.AddWithValue("@id", articulo.id_articulo);
                        cmd_update.ExecuteNonQuery();
                    }
                    transaccion.Commit();

                    return new OkObjectResult("Carrito eliminado");
                }
                catch (System.Exception e)
                {
                    transaccion.Rollback();
                    return new BadRequestObjectResult(e.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }
            catch (System.Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}