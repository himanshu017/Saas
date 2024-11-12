using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.MasterAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.Repository.MasterAdmin
{
    public interface IMasterTablesRepository 
    {
        #region Manage Master Types tables
        Task<IEnumerable<CommonTypeBO>> GetAllTypes(byte typeId);
        Task<BaseResponseBO> AddUpdateTypes(CommonTypeBO model);
        Task<BaseResponseBO> DeleteType(CommonTypeBO model);

        Task<IEnumerable<StatusListBO>> GetStatusById(byte id);
        #endregion
    }

}
