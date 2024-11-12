using AdminPanel.DataModel.Models;
using AdminPanel.Services.Helpers;
using AdminPanel.Services.Repository;
using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using AutoMapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> _customer;
        private readonly IGenericRepository<CustomerUserFeaturesMaster> _userfeatures;
        private readonly IGenericRepository<User> _user;
        private readonly IGenericRepository<Address> _address;
        private readonly IGenericRepository<MainCategory> _categories;
        private readonly IGenericRepository<UserSalesrepCode> _repCodes;
        private readonly IGenericRepository<DeliveryRun> _runs;
        private readonly IMapper _mapper;

        public CustomerService(IGenericRepository<Customer> customer,
                               IGenericRepository<CustomerUserFeaturesMaster> userFeatures,
                               IGenericRepository<User> user,
                               IGenericRepository<Address> address,
                               IGenericRepository<MainCategory> categories,
                               IGenericRepository<UserSalesrepCode> repCodes,
                               IGenericRepository<DeliveryRun> runs,
                               IMapper mapper)
        {
            _customer = customer;
            _userfeatures = userFeatures;
            _user = user;
            _address = address;
            _mapper = mapper;
            _categories = categories;
            _repCodes = repCodes;
            _runs = runs;
        }

        public IEnumerable<CustomerBO> GetAllCustomers(GetCustomerModel model)
        {
            var customerCount = _customer.GetAll().Where(x => x.CompanyId == model.CompanyId && (x.IsSalesRep == false || x.IsSalesRep == null)).Count();
            var customers = _customer.GetAll("Categories,ChildCustomers,DeliveryRuns")
                                     .Where(x => x.CompanyId == model.CompanyId && (x.IsSalesRep == false || x.IsSalesRep == null)
                                              && x.IsActive == model.Active && (x.IsDebtorOnHold == null || x.IsDebtorOnHold == model.OnHold)
                                              && (string.IsNullOrEmpty(model.SearchString) || x.CustomerName.Contains(model.SearchString) || x.AlphaCode.Contains(model.SearchString) || x.Email.Contains(model.SearchString)))
                                     .OrderBy(o => o.CustomerId)
                                     .Skip(model.PageSize * model.PageNumber)
                                     .Take(model.PageSize == 0 ? customerCount : model.PageSize)
                                     .ToList();

            var res = _mapper.Map<IEnumerable<CustomerBO>>(customers);

            if (res.Count() > 0)
            {
                res.ToList()[0].TotalRecords = customerCount;
            }
            return res;
        }

        public async Task<BaseResponseBO> AddEditCustomer(CustomerBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };
                var customer = new Customer();
                var checkEmail = await _customer.GetSingleWithConditions(x => x.Email == model.Email);

                if (model.CustomerId == 0)
                {
                    if (checkEmail.CustomerId > 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Email already exists. Please choose another.";

                        return response;
                    }

                    _mapper.Map(model, customer);
                    customer.ContactName = $"{model.FirstName} {model.LastName}";

                    AddEditChildCustomers(model, customer);
                    AddEditCustomerSpecialCategories(model, customer);

                    AddDefaultCustomerUser(model, customer);
                    AddEditCustomerRuns(model, customer);

                    await _customer.AddAsync(customer);
                    response.Message = "Customer record created!";
                }
                else
                {
                    if (checkEmail.CustomerId != model.CustomerId)
                    {
                        response.IsSuccess = false;
                        response.Message = "Email already exists. Please choose another.";

                        return response;
                    }

                    customer = await _customer.GetSingleWithConditions(x => x.CustomerId == model.CustomerId, "Categories,ChildCustomers,DeliveryRuns");
                    _mapper.Map(model, customer);
                    customer.ContactName = $"{model.FirstName} {model.LastName}";

                    AddEditChildCustomers(model, customer);
                    AddEditCustomerSpecialCategories(model, customer);
                    AddEditCustomerRuns(model, customer);

                    await _customer.UpdateAsync(customer);
                    response.Message = "Customer record updated!";
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = $"{ex.Message} :: Inner Exception:: {ex.InnerException?.Message} ",
                };
            }
        }
        private void AddEditCustomerRuns(CustomerBO model, Customer customer)
        {
            if (model.CustomerId > 0)
            {
                customer.DeliveryRuns.Clear();
            }
            if (model.CustomerRuns != null && model.CustomerRuns.Count() > 0)
            {
                var runs = _runs.GetWithConditions(x => model.CustomerRuns.Contains(x.Id));

                foreach (var item in runs)
                {
                    customer.DeliveryRuns.Add(item);
                }
            }
        }

        private void AddEditChildCustomers(CustomerBO model, Customer customer)
        {
            if (model.IsParent)
            {
                if (model.ChildCustomerIds != null && model.ChildCustomerIds.Count() > 0)
                {
                    // remove the existing entries from the table in case of update
                    if (model.CustomerId > 0)
                    {
                        customer.ChildCustomers.Clear();
                    }

                    var childCustomers = _customer.GetAll().Where(x => model.ChildCustomerIds.Contains(x.CustomerId)).ToList();

                    foreach (var cust in childCustomers)
                    {
                        customer.ChildCustomers.Add(cust);
                    }
                }
            }
            else
            {
                // remove the existing entries from the table
                customer.ChildCustomers.Clear();
            }
        }

        private void AddEditCustomerSpecialCategories(CustomerBO model, Customer customer)
        {
            if (model.IsSpecialCategory)
            {
                if (model.SpecialCategories != null && model.SpecialCategories.Count() > 0)
                {
                    // remove the existing entries from the table in case of update
                    if (model.CustomerId > 0)
                    {
                        customer.Categories.Clear();
                    }

                    var categories = _categories.GetAll().Where(x => model.SpecialCategories.Contains(x.Id)).ToList();

                    foreach (var cat in categories)
                    {
                        customer.Categories.Add(cat);
                    }
                }
            }
            else
            {
                // remove the existing entries from the table
                customer.Categories.Clear();
            }
        }

        private void AddDefaultCustomerUser(CustomerBO model, Customer customer)
        {
            var user = new User()
            {
                CompanyId = model.CompanyId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserTypeId = (byte)UserTypes.AppUser,
                IsActive = true,
                IsEmailVerified = true,
                CreatedBy = model.CreatedBy,
                CreatedOn = model.CreatedOn,
            };

            user.UserPasswords.Add(new UserPassword()
            {
                PasswordSalt = PasswordHelper.GetSalt(),
                PasswordHash = PasswordHelper.GetHash("Test@123"),
                IsActive = true,
                CreatedOn = DateTime.UtcNow
            });

            customer.Users.Add(user);
        }

        public async Task<IEnumerable<CustomerUserBO>> GetCustomerUsers(long customerId)
        {
            var customerUsers = await _customer.GetSingleWithConditions(x => x.CustomerId == customerId, "Users");

            return _mapper.Map<IEnumerable<User>, IEnumerable<CustomerUserBO>>(customerUsers.Users);
        }

        public async Task<UserFeaturesBO> GetUserFeaturesByUserId(long userId)
        {
            var userFeatures = await _userfeatures.GetSingleWithConditions(x => x.UserId == userId);

            return _mapper.Map<CustomerUserFeaturesMaster, UserFeaturesBO>(userFeatures);
        }

        public async Task<BaseResponseBO> SaveUserFeatures(UserFeaturesBO model)
        {
            try
            {
                var userFeatures = new CustomerUserFeaturesMaster();
                userFeatures = _mapper.Map<UserFeaturesBO, CustomerUserFeaturesMaster>(model);
                await _userfeatures.UpdateAsync(userFeatures);

                return new BaseResponseBO()
                {
                    IsSuccess = true,
                    Message = "Features updated successfully!"
                };
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

        public async Task<BaseResponseBO> AddEditCustomerUser(CustomerUserBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };

                var user = new User();
                var checkEmail = await _user.GetSingleWithConditions(x => x.Email == model.Email);

                if (model.UserId == 0)
                {
                    if (checkEmail != null)
                    {
                        response.IsSuccess = false;
                        response.Message = "Email already exists. Please choose another.";

                        return response;
                    }

                    user = _mapper.Map<User>(model);
                    user.UserTypeId = (byte)UserTypes.AppUser;

                    var userPassword = new UserPassword();
                    userPassword.PasswordHash = PasswordHelper.GetHash(model.Password);
                    userPassword.PasswordSalt = PasswordHelper.GetSalt();
                    userPassword.IsActive = true;

                    user.UserPasswords.Add(userPassword);

                    await _user.AddAsync(user);
                    response.Message = "User created successfully!";
                    // add code to send out an email to the created user so that they can login to the system and reset their password as per need.
                    // add code to force creating a new password??
                }
                else
                {
                    if (checkEmail != null &&  checkEmail.UserId != model.UserId)
                    {
                        response.IsSuccess = false;
                        response.Message = "Email already exists. Please choose another.";

                        return response;
                    }

                    user = await _user.Get(model.UserId);
                    _mapper.Map(model, user);
                    await _user.UpdateAsync(user);
                    response.Message = "User updated successfully!";
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

        public async Task<BaseResponseBO> UpdateUserPassword(CustomerUserResetPassword model)
        {
            try
            {
                var user = await _user.GetSingleWithConditions(x => x.UserId == model.UserId, "UserPasswords");

                if (model.CheckExisting ?? false)
                {
                    string password = "";
                    var currentPassword = user.UserPasswords.Where(p => p.IsActive == true).FirstOrDefault();
                    if (currentPassword != null)
                    {
                        password = PasswordHelper.GetHash(model.OldPassword) + currentPassword.PasswordSalt;
                    }

                    var exists = user.UserPasswords.Where(p => (p.PasswordHash + p.PasswordSalt) == password && p.IsActive == true).FirstOrDefault();
                    if (exists == null)
                    {
                        return new BaseResponseBO()
                        {
                            IsSuccess = false,
                            Message = "Old password does not match with the one in the system. Please logout and use reset password functionality to reset your password."
                        };
                    }
                }

                // make all existing user passwords as inactive
                foreach (var userPass in user.UserPasswords)
                {
                    userPass.IsActive = false;
                }

                var userPassword = new UserPassword();
                userPassword.PasswordHash = PasswordHelper.GetHash(model.Password);
                userPassword.PasswordSalt = PasswordHelper.GetSalt();
                userPassword.IsActive = true;

                user.UserPasswords.Add(userPassword);

                await _user.UpdateAsync(user);

                return new BaseResponseBO() { IsSuccess = true, Message = "Password updated successfully!" };
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

        public async Task<BaseResponseBO> DeleteCustomerUser(long userId)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };

                var user = await _user.GetSingleWithConditions(x => x.UserId == userId, "UserPasswords,CustomerUserFeaturesMasters,UserDeviceLocationAndTokens");

                if (user != null)
                {
                    await _user.Delete(user);

                    response.Message = "User deleted successfully!";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "User does not exist!";
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

        public async Task<IEnumerable<CustomerAddressBO>> GetCustomerAddresses(long customerId)
        {
            var customerAddresses = await _customer.GetSingleWithConditions(x => x.CustomerId == customerId, "Addresses,Addresses.AddressType");

            return _mapper.Map<IEnumerable<Address>, IEnumerable<CustomerAddressBO>>(customerAddresses.Addresses);
        }

        public async Task<BaseResponseBO> AddEditCustomerAddress(CustomerAddressBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };
                var address = new Address();
                var customer = await _customer.GetSingleWithConditions(x => x.CustomerId == model.CustomerId, "Addresses");

                if (model.Id == 0)
                {
                    _mapper.Map(model, address);

                    customer.Addresses.Add(address);
                    response.Message = "Customer address added!";
                }
                else
                {
                    address = customer.Addresses.FirstOrDefault(x => x.Id == model.Id);
                    _mapper.Map(model, address);

                    response.Message = "Customer address updated!";
                }

                await _customer.UpdateAsync(customer);
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

        public async Task<BaseResponseBO> DeleteCustomerAddress(DeleteCustomerAddressBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };

                var customer = await _customer.GetSingleWithConditions(x => x.CustomerId == model.CustomerId, "Addresses");

                if (customer != null)
                {
                    var address = await _address.GetSingleWithConditions(x => x.Id == model.AddressId, "Orders");

                    customer.Addresses.Remove(address);

                    // check if the addressid is being used in any of ther orders. 
                    // If yes then mark the address as in active else delete the address from the table
                    if (address.Orders.Count() > 0)
                    {
                        address.IsActive = false;
                        address.ModifiedOn = DateTime.UtcNow;
                        address.ModifiedBy = model.UserId;
                        await _address.UpdateAsync(address);
                    }
                    else
                    {
                        await _address.Delete(address);
                    }
                    // update customer address relational table
                    await _customer.UpdateAsync(customer);

                    response.Message = "Address deleted successfully!";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Address does not exist!";
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

        public async Task<BaseResponseBO> AddEditSalesrep(SalesrepBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };
                var user = new User();
                var checkEmail = await _user.GetSingleWithConditions(x => x.Email == model.Email);

                if (model.UserId == 0)
                {
                    if (checkEmail.UserId > 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Email already exists. Please choose another.";

                        return response;
                    }

                    _mapper.Map(model, user);

                    user.UserTypeId = (byte)UserTypes.Sales;

                    // check and add salesman codes in sepecate table
                    if (model.SalesrepCode.Count > 0)
                    {
                        foreach (var item in model.SalesrepCode)
                        {
                            var userRepCodes = new UserSalesrepCode()
                            {
                                SalesrepCode = item
                            };
                            user.UserSalesrepCodes.Add(userRepCodes);
                        }
                    }

                    // add user password
                    user.UserPasswords.Add(new UserPassword()
                    {
                        PasswordSalt = PasswordHelper.GetSalt(),
                        PasswordHash = PasswordHelper.GetHash(model.Password),
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow
                    });

                    await _user.AddAsync(user);
                    response.Message = "Salesrep record added.";
                }
                else
                {
                    if (checkEmail.UserId != model.UserId)
                    {
                        response.IsSuccess = false;
                        response.Message = "Email already exists. Please choose another.";

                        return response;
                    }

                    user = await _user.GetSingleWithConditions(x => x.UserId == model.UserId, "UserSalesrepCodes");
                    _mapper.Map(model, user);

                    user.UserSalesrepCodes.Clear();

                    if (model.SalesrepCode.Count > 0)
                    {
                        foreach (var item in model.SalesrepCode)
                        {
                            var userRepCodes = new UserSalesrepCode()
                            {
                                SalesrepCode = item
                            };
                            user.UserSalesrepCodes.Add(userRepCodes);
                        }
                    }
                    await _user.UpdateAsync(user);
                    response.Message = "Salesrep record updated.";
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

        public IEnumerable<SalesrepBO> GetAllSalesreps(long companyId)
        {
            try
            {
                var salesReps = _user.GetAll("UserSalesrepCodes")
                                     .Where(x => x.UserTypeId == (byte)UserTypes.Sales && x.CompanyId == companyId)
                                     .ToList();

                return _mapper.Map<IEnumerable<SalesrepBO>>(salesReps);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<BaseResponseBO> DeleteSalesrep(long userId)
        {
            try
            {
                var user = await _user.GetSingleWithConditions(x => x.UserId == userId, "UserSalesrepCodes");
                user.UserSalesrepCodes.Clear();
                await _user.Delete(user);

                return new BaseResponseBO()
                {
                    IsSuccess = true,
                    Message = "Salesrep record deleted."
                };
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

        public IEnumerable<string> AllSalesmanCodes(long companyId)
        {
            return _repCodes.GetAll()
                            .Where(x => x.User.CompanyId == companyId && x.User.IsActive == true)
                            .Select(s => s.SalesrepCode).ToList();
        }

        public async Task<bool> UpdateBitField(UpdateBitBO model)
        {
            try
            {
                await _customer.UpdateBitField(model.Id, model.ColName, model.Value, model.ModifiedBy);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
