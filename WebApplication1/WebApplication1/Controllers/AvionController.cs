using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvionController : ControllerBase
    {
        private IConfiguration _Config;

        public AvionController(IConfiguration Config)
        {
            _Config = Config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Avion>>> GetAllAviones()
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection"));
            conexion.Open();
            var autos = conexion.Query<Avion>("ObtenerAviones", commandType: System.Data.CommandType.StoredProcedure);
            return Ok(autos);
        }

        [HttpPost]
        public async Task<ActionResult<CreateAvion>> CreateCreateAvion(CreateAvion CA)
        {
            try
            {

                using (var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection")))
                {
                    await conexion.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Modelo", CA.Modelo);
                    parameters.Add("@color", CA.color);
                    // Utilizar Execute en lugar de Query ya que estamos insertando y no esperamos un conjunto de resultados
                    await conexion.ExecuteAsync("InsertarAvion", parameters, commandType: CommandType.StoredProcedure);

                    // Retornar un mensaje indicando que el proyecto se ha creado exitosamente
                    return Ok("Avion creado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                var message = "Se produjo un error al crear el Avion: " + ex.Message;
                return StatusCode(500, new { message });
            }
        }

        [HttpPut("{ID}")]
        public async Task<ActionResult<List<CreateAvion>>> UpdateAvion(int ID, [FromBody] CreateAvion CA)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection"));
            conexion.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@Modelo", CA.Modelo);
            parameters.Add("@color", CA.color);
            parameters.Add("@ID", ID);
            var Auto = conexion.Query<CreateAvion>("ActualizarAvion", parameters, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(Auto);
        }


        [HttpDelete("{ID}")]
        // obteniendo id del Projecto a eliminar (este id es de la clase Project)
        public async Task<ActionResult> DeleteAvion(int ID)
        {
            //manejo de errores con trycach
            try
            {
                using (var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection")))
                {
                    await conexion.OpenAsync();

                    var parametro = new DynamicParameters();
                    parametro.Add("@ID", ID);
                    await conexion.ExecuteAsync("EliminarAvion", parametro, commandType: CommandType.StoredProcedure);

                    return Ok("Avion eliminada correctamente.");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error al eliminar el avion : {ex.Message}");
            }
        }

    }
}
