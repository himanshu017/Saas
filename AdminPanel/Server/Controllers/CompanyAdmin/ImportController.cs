using Microsoft.AspNetCore.Mvc;
using AdminPanel.Services.ServicesLayer;
using Microsoft.AspNetCore.Authorization;
using AdminPanel.Shared.BO;
using AdminPanel.Shared;

namespace AdminPanel.Server.Controllers.CompanyAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ImportController : ControllerBase
    {
        private readonly IimportDataService _impService;

        public ImportController(IimportDataService impService)
        {
            _impService = impService;
        }



        [HttpPost]
        public IActionResult ReadExcelFile([FromBody] ImportBaseClass model)
        {
            ImportFor importFor;
            ImportBaseClass importBaseClass = new ImportBaseClass();
            if (Enum.TryParse<ImportFor>(model.ImportFor, out importFor))
            {
                importBaseClass.TableColumns = _impService.GetImportedTableColumns(importFor);
                MemoryStream stream = new MemoryStream();
                stream.Write(model.bytearray, 0, model.bytearray.Length);

                string rootpath = @"wwwroot/ImportData/";

                string fullpath = Path.Combine(Directory.GetCurrentDirectory(), rootpath);


                if (model.MappingId > 0)
                {
                    var response = _impService.GetMappingById(model.MappingId ?? 0);
                    importBaseClass.MappingFields = response.MappingFields;
                    importBaseClass.MappingTitle = response.MappingTitle;
                }

                importBaseClass.ImportedColumns = _impService.GetImportedSheetColumns(fullpath, model, stream);
                importBaseClass.ImportedFileName = model.ImportedFileName;
            }

            return Ok(importBaseClass);
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcelData([FromBody] ImportBaseClass model)
        {
            //WorkBook workBook = WorkBook.Load(model.bytearray);
            //WorkSheet sheet = workBook.WorkSheets.First();
            MemoryStream stream = new MemoryStream();
            stream.Write(model.bytearray, 0, model.bytearray.Length);

            string rootpath = @"wwwroot/ImportData/";

            string fullpath = Path.Combine(Directory.GetCurrentDirectory(), rootpath);

            var response = await _impService.ImportData(model, stream, fullpath);

            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetImportedMapping([FromQuery] int companyId, string Importfor)
        {
            return Ok(_impService.GetImportedMapping(companyId, Importfor));
        }
    }
}
