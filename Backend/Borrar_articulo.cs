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
using System.Collections.Generic;

namespace ServicioWeb
{
    public static class Borrar_articulo
    {

        [FunctionName("borrar_articulo")]
        public static async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                if (data.id_articulo == null)
                    return new BadRequestObjectResult("Se debe ingresar el id del articulo");

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
                    var cmd = new MySqlCommand("SELECT id_articulo, cantidad FROM carrito_compra WHERE id_articulo=@id", conexion);
                    cmd.Parameters.AddWithValue("@id", data.id_articulo);

                    MySqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        int cantidad = r.GetInt32(1);
                        r.Close();

                        var cmd_delete = new MySqlCommand("DELETE FROM carrito_compra where id_articulo=@id", conexion);
                        cmd_delete.Parameters.AddWithValue("@id", data.id_articulo);
                        cmd_delete.ExecuteNonQuery();

                        var cmd_update = new MySqlCommand("UPDATE articulos SET `cantidad`=`cantidad` + @cantidad WHERE id_articulo=@id", conexion);
                        cmd_update.Parameters.AddWithValue("@cantidad", cantidad);
                        cmd_update.Parameters.AddWithValue("@id", data.id_articulo);
                        cmd_update.ExecuteNonQuery();
                        transaccion.Commit();
                        return new OkObjectResult("Artículo eliminado del carrito");
                    }
                    return new BadRequestObjectResult("El articulo no existe");
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
