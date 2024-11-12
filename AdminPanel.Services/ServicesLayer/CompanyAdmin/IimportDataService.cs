using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public interface IimportDataService
    {
        IEnumerable<FieldInfo> GetImportedTableColumns(ImportFor importType);

        IEnumerable<CommonDropdownBO> GetImportedMapping(int companyId, string Importfor);

        Task<BaseResponseBO> ImportData(ImportBaseClass model, MemoryStream stream,string path);

        ImportBaseClass GetMappingById(int MappingId);

        List<string> GetImportedSheetColumns(string path, ImportBaseClass model, MemoryStream stream);
    }
}
