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
    public class BicicletaController : ControllerBase
    {
        private IConfiguration _Config;

        public BicicletaController(IConfiguration Config)
        {
            _Config = Config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Bicicleta>>> GetAllBicicletas()
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection"));
            conexion.Open();
            var autos = conexion.Query<Bicicleta>("ObtenerBicicletas", commandType: System.Data.CommandType.StoredProcedure);
            return Ok(autos);
        }

        [HttpPost]
        public async Task<ActionResult<CreateBicicleta>> CreateCreateBicicleta(CreateBicicleta CB)
        {
            try
            {

                using (var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection")))
                {
                    await conexion.OpenAsync();

                    var parameters = new DynamicParameters();
                    parameters.Add("@Marca", CB.Marca);
                    parameters.Add("@Modelo", CB.Modelo);
                    parameters.Add("@Tipo", CB.Tipo);
                    parameters.Add("@color", CB.color);
                    // Utilizar Execute en lugar de Query ya que estamos insertando y no esperamos un conjunto de resultados
                    await conexion.ExecuteAsync("InsertarBicicleta", parameters, commandType: CommandType.StoredProcedure);

                    // Retornar un mensaje indicando que el proyecto se ha creado exitosamente
                    return Ok("bicicleta creado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                var message = "Se produjo un error al crear la bicicleta: " + ex.Message;
                return StatusCode(500, new { message });
            }
        }

        [HttpPut("{ID}")]
        public async Task<ActionResult<List<CreateBicicleta>>> UpdateBicicleta(int ID, [FromBody] CreateBicicleta CB)
        {
            using var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection"));
            conexion.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@Marca", CB.Marca);
            parameters.Add("@Modelo", CB.Modelo);
            parameters.Add("@Tipo", CB.Tipo);
            parameters.Add("@color", CB.color);
            parameters.Add("@ID", ID);
            var Auto = conexion.Query<CreateBicicleta>("ActualizarBicicleta", parameters, commandType: System.Data.CommandType.StoredProcedure);
            return Ok(Auto);
        }


        [HttpDelete("{ID}")]
        // obteniendo id del Projecto a eliminar (este id es de la clase Project)
        public async Task<ActionResult> DeleteBicicleta(int ID)
        {
            //manejo de errores con trycach
            try
            {
                using (var conexion = new SqlConnection(_Config.GetConnectionString("DefaultConnection")))
                {
                    await conexion.OpenAsync();

                    var parametro = new DynamicParameters();
                    parametro.Add("@ID", ID);
                    await conexion.ExecuteAsync("EliminarBicicleta", parametro, commandType: CommandType.StoredProcedure);

                    return Ok("bicicleta eliminada correctamente.");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error al eliminar la bicicleta: {ex.Message}");
            }
        }

    }
}
