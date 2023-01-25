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

namespace ServicioWeb
{
    public static class Alta_articulo
    {
        [FunctionName("alta_articulo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                if (data.nombre == null || data.nombre == "")
                    return new BadRequestObjectResult("Se debe ingresar el nombre");

                if (data.descripcion == null || data.descripcion == "")
                    return new BadRequestObjectResult("Se debe ingresar la descripción");

                if (data.precio == null || data.precio < 1)
                    return new BadRequestObjectResult("Se debe ingresar el precio");

                if (data.cantidad == null || data.cantidad < 1)
                    return new BadRequestObjectResult("Se debe ingresar la cantidad");

                if (data.foto == null || data.foto == "")
                    return new BadRequestObjectResult("Se debe ingresar una foto");


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
                    var cmd = new MySqlCommand();
                    cmd.Connection = conexion;
                    cmd.Transaction = transaccion;
                    cmd.CommandText = "INSERT INTO articulos(id_articulo,nombre,descripcion,precio,cantidad,foto) VALUES(0, @nombre, @descripcion, @precio, @cantidad, @foto)";
                    cmd.Parameters.AddWithValue("@nombre", data.nombre);
                    cmd.Parameters.AddWithValue("@descripcion", data.descripcion);
                    cmd.Parameters.AddWithValue("@precio", data.precio);
                    cmd.Parameters.AddWithValue("@cantidad", data.cantidad);
                    cmd.Parameters.AddWithValue("@foto", data.foto);
                    cmd.ExecuteNonQuery();

                    transaccion.Commit();
                    return new OkObjectResult("Artículo creado exitosamente");
                }
                catch (Exception e)
                {
                    transaccion.Rollback();
                    return new BadRequestObjectResult(e.Message);
                }
                finally
                {
                    conexion.Close();
                }

            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
