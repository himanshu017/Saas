using AdminPanel.DataModel.Models;
using AdminPanel.Services.Repository;
using AdminPanel.Shared.BO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public class GroupService : IGroupService
    {
        private readonly IGenericRepository<Group> _group;

        private readonly IMapper _mapper;

        public GroupService(IMapper mapper, IGenericRepository<Group> group)
        {
            _mapper = mapper;
            _group = group;
        }

        public IEnumerable<GroupsBO> GetAllGroups(GetGroups model)
        {
            var groups = _group.GetWithConditions(x => x.CompanyId == model.CompanyId && x.GroupScopeId == model.ScopeId, "GroupDetails");

            return _mapper.Map<IEnumerable<GroupsBO>>(groups);
        }

        public async Task<BaseResponseBO> AddEditGroups(GroupsBO model)
        {
            var response = new BaseResponseBO() { IsSuccess = true };
            try
            {
                var record = new Group();
                if (model.Id == 0)
                {
                    _mapper.Map(model,record);

                    foreach (var item in model.ReferemceIds)
                    {
                        record.GroupDetails.Add(new GroupDetail() { ReferenceId = item });
                    }

                    await _group.AddAsync(record);
                    response.Message = "Group added successfully";
                }
                else
                {
                    record = await _group.GetSingleWithConditions(x => x.Id == model.Id, "GroupDetails");
                    _mapper.Map(model, record);

                    record.GroupDetails.Clear();

                    foreach (var item in model.ReferemceIds)
                    {
                        record.GroupDetails.Add(new GroupDetail() { ReferenceId = item });
                    }

                    await _group.UpdateAsync(record);
                    response.Message = "Group updated successfully";
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<BaseResponseBO> DeleteGroup(long id)
        {
            try
            {
                var record = await _group.GetSingleWithConditions(x => x.Id == id, "GroupDetails");

                record.GroupDetails.Clear();
                await _group.Delete(id);

                return new BaseResponseBO()
                {
                    IsSuccess = true,
                    Message = "Group deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
