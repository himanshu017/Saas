using AdminPanel.DataModel.Context;
using AdminPanel.DataModel.Models;
using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.MasterAdmin;
using AutoMapper;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.Repository.MasterAdmin
{

    public class MasterTablesRepository : IMasterTablesRepository
    {
        private readonly OrderflowContext _context;
        private readonly IMapper _mapper;

        public MasterTablesRepository(OrderflowContext context,
                                IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        #region Manage master type tables
        public async Task<IEnumerable<CommonTypeBO>> GetAllTypes(byte typeId)
        {
            try
            {
                if (typeId == (byte)CommonTypes.AddressType)
                {
                    var allTypes = await _context.AddressTypes.ToListAsync();
                    return _mapper.Map<IEnumerable<CommonTypeBO>>(allTypes);
                }
                else if (typeId == (byte)CommonTypes.CommentType)
                {
                    var allTypes = await _context.CommentTypes.ToListAsync();
                    return _mapper.Map<IEnumerable<CommonTypeBO>>(allTypes);
                }
                else if (typeId == (byte)CommonTypes.DiscountType)
                {
                    var allTypes = await _context.DiscountTypes.ToListAsync();
                    return _mapper.Map<IEnumerable<CommonTypeBO>>(allTypes);
                }
                else if (typeId == (byte)CommonTypes.DiscountLimitationType)
                {
                    var allTypes = await _context.DiscountLimitationTypes.ToListAsync();
                    return _mapper.Map<IEnumerable<CommonTypeBO>>(allTypes);
                }
                else
                {
                    var allTypes = await _context.StatusTypes.ToListAsync();
                    return _mapper.Map<IEnumerable<CommonTypeBO>>(allTypes);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BaseResponseBO> AddUpdateTypes(CommonTypeBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };

                if(model.IsStatus)
                {
                    var status = new Status();
                    if(model.Id == 0)
                    {
                        status.TypeId = model.TypeId;
                        status.StatusDesc = model.TypeDesc;
                        _context.Statuses.Add(status);
                    }
                    else
                    {
                        status = _context.Statuses.Find(model.Id);
                        status.StatusDesc = model.TypeDesc;
                        status.TypeId = model.TypeId;
                    }

                    await _context.SaveChangesAsync();

                    return response;
                }

                if (model.TypeId == (byte)CommonTypes.AddressType)
                {
                    var type = new AddressType();
                    if (model.Id == 0)
                    {
                        // insert code
                        type.TypeDesc = model.TypeDesc;
                        _context.AddressTypes.Add(type);
                        response.Message = "Address Type added.";
                    }
                    else
                    {
                        type = _context.AddressTypes.Find(model.Id);
                        type.TypeDesc = model.TypeDesc;
                        response.Message = "Address Type updated.";
                    }
                }
                else if (model.TypeId == (byte)CommonTypes.CommentType)
                {
                    var type = new CommentType();
                    if (model.Id == 0)
                    {
                        // insert code
                        type.TypeDesc = model.TypeDesc;
                        _context.CommentTypes.Add(type);
                        response.Message = "Address Type added.";
                    }
                    else
                    {
                        type = _context.CommentTypes.Find(model.Id);
                        type.TypeDesc = model.TypeDesc;
                        response.Message = "Address Type updated.";
                    }
                }
                else if (model.TypeId == (byte)CommonTypes.DiscountType)
                {
                    var type = new DiscountType();
                    if (model.Id == 0)
                    {
                        // insert code
                        type.TypeDesc = model.TypeDesc;
                        _context.DiscountTypes.Add(type);
                        response.Message = "Address Type added.";
                    }
                    else
                    {
                        type = _context.DiscountTypes.Find(model.Id);
                        type.TypeDesc = model.TypeDesc;
                        response.Message = "Address Type updated.";
                    }
                }
                else if (model.TypeId == (byte)CommonTypes.DiscountLimitationType)
                {
                    var type = new DiscountLimitationType();
                    if (model.Id == 0)
                    {
                        // insert code
                        type.TypeDesc = model.TypeDesc;
                        _context.DiscountLimitationTypes.Add(type);
                        response.Message = "Address Type added.";
                    }
                    else
                    {
                        type = _context.DiscountLimitationTypes.Find(model.Id);
                        type.TypeDesc = model.TypeDesc;
                        response.Message = "Address Type updated.";
                    }
                }
                else
                {
                    var type = new StatusType();
                    if (model.Id == 0)
                    {
                        // insert code
                        type.TypeDesc = model.TypeDesc;
                        _context.StatusTypes.Add(type);
                        response.Message = "Address Type added.";
                    }
                    else
                    {
                        type = _context.StatusTypes.Find(model.Id);
                        type.TypeDesc = model.TypeDesc;
                        response.Message = "Address Type updated.";
                    }
                }

                await _context.SaveChangesAsync();

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

        public async Task<BaseResponseBO> DeleteType(CommonTypeBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };

                if (model.TypeId == (byte)CommonTypes.AddressType)
                {
                    var type = await _context.AddressTypes.FindAsync(model.Id);
                    if (type == null)
                    {
                        response.IsSuccess = false;
                    }
                    else
                    {
                        _context.AddressTypes.Remove(type);
                    }
                }
                else if (model.TypeId == (byte)CommonTypes.CommentType)
                {
                    var type = await _context.CommentTypes.FindAsync(model.Id);
                    if (type == null)
                    {
                        response.IsSuccess = false;
                    }
                    else
                    {
                        _context.CommentTypes.Remove(type);
                    }
                }
                else if (model.TypeId == (byte)CommonTypes.DiscountType)
                {
                    var type = await _context.DiscountTypes.FindAsync(model.Id);
                    if (type == null)
                    {
                        response.IsSuccess = false;
                    }
                    else
                    {
                        _context.DiscountTypes.Remove(type);
                    }
                }
                else if (model.TypeId == (byte)CommonTypes.DiscountLimitationType)
                {
                    var type = await _context.DiscountLimitationTypes.FindAsync(model.Id);
                    if (type == null)
                    {
                        response.IsSuccess = false;
                    }
                    else
                    {
                        _context.DiscountLimitationTypes.Remove(type);
                    }
                }
                else if (model.TypeId == (byte)CommonTypes.StatusType)
                {
                    var type = await _context.StatusTypes.FindAsync(model.Id);
                    if (type == null)
                    {
                        response.IsSuccess = false;
                    }
                    else
                    {
                        _context.StatusTypes.Remove(type);
                    }
                }
                else 
                {
                    var type = await _context.Statuses.FindAsync(model.Id);
                    if (type == null)
                    {
                        response.IsSuccess = false;
                    }
                    else
                    {
                        _context.Statuses.Remove(type);
                    }
                }

                if (response.IsSuccess)
                {
                    await _context.SaveChangesAsync();
                    response.Message = "Delete successfull.";
                }
                else
                {
                    response.Message = "Selected type not found in the database.";
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

        public async Task<IEnumerable<StatusListBO>> GetStatusById(byte id)
        {
            var allTypes = await (from a in _context.Statuses
                            join b in _context.StatusTypes on a.TypeId equals b.Id
                            where a.TypeId == id
                            select new StatusListBO()
                            {
                                Id = a.Id,
                                StatusDesc = a.StatusDesc,
                                StatusType = b.TypeDesc,
                                TypeId = b.Id
                            }).ToListAsync();

            return allTypes;
        }
        #endregion
    }
}
