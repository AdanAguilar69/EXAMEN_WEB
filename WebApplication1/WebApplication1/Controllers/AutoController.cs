using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoController : ControllerBase
    {
        private IConfiguration _Config;

        public AutoController(IConfiguration Config)
        {
            _Config = Config;
        }


        [HttpGet]
        public async Task<ActionResult<List<Auto>>> GetAllAutos()
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection"));
            conexion.Open();
            var autos = conexion.Query<Auto>("ObtenerAutos", commandType: System.Data.CommandType.StoredProcedure);
            return Ok(autos);
        }

        [HttpPost]
        public async Task<ActionResult<CreateAuto>> CreateAuto(CreateAuto CA)
        {
            try
            {

                using (var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection")))
                {
                    await conexion.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Marca", CA.Marca);
                    parameters.Add("@Modelo", CA.Modelo);
                    parameters.Add("@color", CA.color);
                    // Utilizar Execute en lugar de Query ya que estamos insertando y no esperamos un conjunto de resultados
                    await conexion.ExecuteAsync("InsertarAuto", parameters, commandType: CommandType.StoredProcedure);

                    // Retornar un mensaje indicando que el proyecto se ha creado exitosamente
                    return Ok("auto creado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                var message = "Se produjo un error al crear el Auto: " + ex.Message;
                return StatusCode(500, new { message });
            }
        }

        [HttpPut("{ID}")]
        public async Task<ActionResult<List<CreateAuto>>> UpdateAuto(int ID, [FromBody] CreateAuto CA)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection"));
            conexion.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@Marca", CA.Marca);
            parameters.Add("@Modelo", CA.Modelo);
            parameters.Add("@color", CA.color);
            parameters.Add("@ID_Automovil", ID);
            var Auto = conexion.Query<CreateAuto>("ActualizarAuto", parameters, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(Auto);
        }


        [HttpDelete("{ID}")]
        // obteniendo id del Projecto a eliminar (este id es de la clase Project)
        public async Task<ActionResult> DeleteAuto(int ID)
        {
            //manejo de errores con trycach
            try
            {
                using (var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection")))
                {
                    await conexion.OpenAsync();

                    var parametro = new DynamicParameters();
                    parametro.Add("@ID_Automovil", ID);
                    await conexion.ExecuteAsync("EliminarAuto", parametro, commandType: CommandType.StoredProcedure);

                    return Ok("Auto eliminado correctamente.");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error al eliminar el Auto: {ex.Message}");
            }
        }



    }
}
