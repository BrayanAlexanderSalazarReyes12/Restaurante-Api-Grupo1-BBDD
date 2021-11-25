using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante_Api_Grupo1_BBDD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetDemo()
        {
            return Content("Hola desde la versión por defecto redesplegando en heroku");
        }
    }
}
