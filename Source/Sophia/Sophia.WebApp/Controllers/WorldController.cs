namespace Sophia.WebApp.Controllers;
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
[ApiController]
public class WorldController(IWorldService service)
    : ControllerBase {
    // GET: api/<WorldController>
    [HttpGet]
    public Task<WorldData> Get()
        => service.GetWorld();

    // PUT api/<WorldController>/5
    [HttpPut]
    public Task Put([FromBody] WorldData value)
        => service.UpdateWorld(value);
}
