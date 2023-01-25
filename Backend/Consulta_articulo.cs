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
    public static class Consulta_articulo
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

        [FunctionName("consulta_articulo")]
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
                    var cmd = new MySqlCommand("SELECT id_articulo,nombre,descripcion,precio,cantidad,foto,length(foto) FROM articulos WHERE nombre LIKE @nombre OR descripcion LIKE @descripcion", conexion);
                    cmd.Parameters.AddWithValue("@nombre", "%" + data.nombre + "%");
                    cmd.Parameters.AddWithValue("@descripcion", "%" + data.descripcion + "%");

                    MySqlDataReader r = cmd.ExecuteReader();
                    List<Articulo> lista = new List<Articulo>();
                    while (r.Read())
                    {
                        Articulo articulo = new Articulo();
                        articulo.id_articulo = r.GetInt32(0);
                        articulo.nombre = r.GetString(1);
                        articulo.descripcion = r.GetString(2);
                        articulo.precio = r.GetInt32(3);
                        articulo.cantidad = r.GetInt32(4);
                       if (!r.IsDBNull(5))
                        {
                            var longitud = r.GetInt32(6);
                            byte[] foto = new byte[longitud];
                            r.GetBytes(5, 0, foto, 0, longitud);
                            articulo.foto = Convert.ToBase64String(foto);
                        }
                        lista.Add(articulo);
                    }
                    return new OkObjectResult(JsonConvert.SerializeObject(lista));
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
