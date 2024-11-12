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
    public class CutoffService : ICutoffService
    {
        private readonly IGenericRepository<DeliveryDateCutoff> _cutoff;
        private readonly IMapper _mapper;

        public CutoffService(IMapper mapper,
                             IGenericRepository<DeliveryDateCutoff> cutoff)
        {
            _cutoff = cutoff;
            _mapper = mapper;
        }

        public IEnumerable<DeliveryCutoffBO> GetCutoffDetails(long companyId)
        {
            var cutoffs = _cutoff.GetAll().Where(x => x.CompanyId == companyId);
            return _mapper.Map<IEnumerable<DeliveryCutoffBO>>(cutoffs);
        }

        public async Task<BaseResponseBO> UpdateCutoffDetails(DeliveryCutoffBO model)
        {
            var response = new BaseResponseBO() { IsSuccess = true, Message = "Record updated successfully" };
            try
            {
                var record = new DeliveryDateCutoff();
                if (model.Id == 0)
                {
                    _mapper.Map(model, record);

                    await _cutoff.AddAsync(record);
                }
                else
                {
                    record = await _cutoff.GetSingleWithConditions(x => x.Id == model.Id && x.CutoffTypeId == model.CutoffTypeId);

                    // check if record with cutofftype doesn't exist then add record
                    if(record == null)
                    {
                        record = new DeliveryDateCutoff();
                        _mapper.Map(model, record);

                        await _cutoff.AddAsync(record);
                    }
                    else // update the existing record
                    {
                        _mapper.Map(model, record);

                        await _cutoff.UpdateAsync(record);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public Task<DateTime> CalculateNextDeliveryDate(long companyId)
        {
            throw new NotImplementedException();
        }
    }
}
