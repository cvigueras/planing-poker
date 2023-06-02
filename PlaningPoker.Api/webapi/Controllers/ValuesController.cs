using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Hola");
        }
    }
}
