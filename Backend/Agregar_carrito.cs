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

namespace ServicioWeb
{
    public static class Agregar_carrito
    {
        [FunctionName("agregar_carrito")]
        public static async Task<IActionResult> Run(
             [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
             ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                if (data.descripcion == null || data.descripcion == "")
                    return new BadRequestObjectResult("Se debe ingresar la descripción");
                if (data.cantidad == null || data.cantidad == 0)
                    return new BadRequestObjectResult("Se debe ingresar la cantidad");

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
                    var cmd = new MySqlCommand("SELECT id_articulo, precio, cantidad FROM articulos WHERE descripcion=@descripcion", conexion);
                    cmd.Parameters.AddWithValue("@descripcion", data.descripcion);

                    MySqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        int cantidad = data.cantidad;
                        int id_articulo = r.GetInt32(0);
                        int precio = r.GetInt32(1);
                        r.Close();

                        if (cantidad > 0)
                        {
                            var cmd_insert = new MySqlCommand("INSERT INTO carrito_compra VALUES (@id,@cantidad)", conexion);
                            cmd_insert.Parameters.AddWithValue("@id", id_articulo);
                            cmd_insert.Parameters.AddWithValue("@cantidad", cantidad);
                            cmd_insert.ExecuteNonQuery();

                            transaccion.Commit();
                            return new OkObjectResult("Articulo agregado al carrito");
                        }
                        return new BadRequestObjectResult("No hay suficientes existencias del articulo");
                    }
                    return new BadRequestObjectResult("El articulo no existe");
                }
                catch (System.Exception e)
                {
                    transaccion.Rollback();
                    return new BadRequestObjectResult(e);
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
