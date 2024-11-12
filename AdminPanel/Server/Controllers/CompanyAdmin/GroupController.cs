using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Server.Controllers.CompanyAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _service;
        public GroupController(IGroupService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllGroups([FromQuery] GetGroups model)
        {
            return Ok(_service.GetAllGroups(model));
        }

        [HttpPost]
        public async Task<IActionResult> AddEditGroups([FromBody] GroupsBO model)
        {
            return Ok(await _service.AddEditGroups(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGroup([FromBody] long id)
        {
            return Ok(await _service.DeleteGroup(id));
        }
    }
}
