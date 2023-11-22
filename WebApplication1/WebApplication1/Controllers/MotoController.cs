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
    public class MotoController : ControllerBase
    {
        private IConfiguration _Config;

        public MotoController(IConfiguration Config)
        {
            _Config = Config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Moto>>> GetAllMotos()
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection"));
            conexion.Open();
            var autos = conexion.Query<Moto>("ObtenerMotos", commandType: System.Data.CommandType.StoredProcedure);
            return Ok(autos);
        }

        [HttpPost]
        public async Task<ActionResult<CreateMoto>> CreateMoto(CreateMoto CM)
        {
            try
            {

                using (var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection")))
                {
                    await conexion.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Marca", CM.Marca);
                    parameters.Add("@Modelo", CM.Modelo);
                    parameters.Add("@color", CM.color);
                    // Utilizar Execute en lugar de Query ya que estamos insertando y no esperamos un conjunto de resultados
                    await conexion.ExecuteAsync("InsertarMoto", parameters, commandType: CommandType.StoredProcedure);

                    // Retornar un mensaje indicando que el proyecto se ha creado exitosamente
                    return Ok("Moto creado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                var message = "Se produjo un error al crear la Moto: " + ex.Message;
                return StatusCode(500, new { message });
            }
        }

        [HttpPut("{ID}")]
        public async Task<ActionResult<List<CreateMoto>>> UpdateMoto(int ID, [FromBody] CreateMoto CM)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection"));
            conexion.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@Marca", CM.Marca);
            parameters.Add("@Modelo", CM.Modelo);
            parameters.Add("@color", CM.color);
            parameters.Add("@ID", ID);
            var Auto = conexion.Query<CreateMoto>("ActualizarMoto", parameters, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(Auto);
        }


        [HttpDelete("{ID}")]
        // obteniendo id del Projecto a eliminar (este id es de la clase Project)
        public async Task<ActionResult> DeleteMoto(int ID)
        {
            //manejo de errores con trycach
            try
            {
                using (var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection")))
                {
                    await conexion.OpenAsync();

                    var parametro = new DynamicParameters();
                    parametro.Add("@ID", ID);
                    await conexion.ExecuteAsync("EliminarMoto", parametro, commandType: CommandType.StoredProcedure);

                    return Ok("Moto eliminado correctamente.");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error al eliminar la Moto: {ex.Message}");
            }
        }

    }
}
