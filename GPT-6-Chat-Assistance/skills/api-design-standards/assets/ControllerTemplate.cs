using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace <ProjectName>Core.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly IExampleService _service;

        public ExampleController(IExampleService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            
            return Ok(result);
        }
    }
}
