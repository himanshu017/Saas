using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Server.Controllers.CompanyAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _service;

        public MessageController(IMessageService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageBO model)
        {
            return Ok(await _service.SendMessage(model));
        }

    }
}
