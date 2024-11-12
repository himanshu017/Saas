using AdminPanel.Shared.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public interface IGroupService
    {
        IEnumerable<GroupsBO> GetAllGroups(GetGroups model);
        Task<BaseResponseBO> AddEditGroups(GroupsBO model);
        Task<BaseResponseBO> DeleteGroup(long id);
    }
}
