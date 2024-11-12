using AdminPanel.Services.Repository.MasterAdmin;
using AdminPanel.Shared.BO.MasterAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Server.Controllers.MasterAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class MasterTablesController : ControllerBase
    {
        private readonly IMasterTablesRepository _masterTables;

        public MasterTablesController(IMasterTablesRepository masterTables)
        {
            _masterTables = masterTables;
        }


        #region Manage Master Type tables

        [HttpGet]
        public async Task<IActionResult> GetAll(byte typeId)
        {
            return Ok(await _masterTables.GetAllTypes(typeId));
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdate(CommonTypeBO model)
        {
            return Ok(await _masterTables.AddUpdateTypes(model));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CommonTypeBO model)
        {
            return Ok(await _masterTables.DeleteType(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetStatusById(byte id)
        {
            return Ok(await _masterTables.GetStatusById(id));
        }

        #endregion
    }
}
