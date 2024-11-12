using AdminPanel.DataModel.Context;
using AdminPanel.Shared.BO;
using AutoMapper;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.Services.Helpers;
using AdminPanel.DataModel.Models;
using AdminPanel.Shared;
using System.Collections;
using Microsoft.Extensions.Options;
using AdminPanel.Shared.BO.Account;
using NLog;

namespace AdminPanel.Services.Repository.Account
{
    public class AccountRepository : BaseServiceBO, IAccountRepository
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger(); // iimplement logging in services
        private readonly OrderflowContext _context;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly AppSettings _appSettings;

        public AccountRepository(OrderflowContext context,
                                IMapper mapper,
                                IMailService mailService,
                                IOptions<AppSettings> appSettings)
        {
            _context = context;
            _mapper = mapper;
            _mailService = mailService;
            _appSettings = appSettings.Value;
        }

        public async Task<LoginResponse> ValidateUser(LoginBO model)
        {
            try
            {
                LoginResponse loginResponse = new();
                var company = await _context.Companies.Where(x => x.DomainName == model.DomainName).FirstOrDefaultAsync();

                var user = await _context.Users
                                    .Include(x => x.UserPasswords)
                                    .Where(u => (u.UserName == model.UserName || u.Email == model.UserName)
                                       && ((model.DeviceType == "Admin" || string.IsNullOrEmpty(model.DeviceType))
                                            ?
                                            (u.UserTypeId == (byte)UserTypes.CompanyAdmin || u.UserTypeId == (byte)UserTypes.SuperAdmin)
                                            :
                                            (u.UserTypeId == (byte)UserTypes.AppUser)))
                                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    loginResponse.IsSuccess = false;
                    loginResponse.Message = "Username / password entered is incorrect";
                }
                else
                {
                    // check to verify if the user belongs to the correct company in case of app users.
                    if (user.UserTypeId != (byte)UserTypes.CompanyAdmin && user.UserTypeId != (byte)UserTypes.SuperAdmin)
                    {
                        if (user.CompanyId != company.CompanyId)
                        {
                            loginResponse.IsSuccess = false;
                            loginResponse.Message = "Username / password entered is incorrect";
                            return loginResponse;
                        }
                    }

                    // check if the user is in active state and email has been verified
                    if ((user.IsActive ?? false) && (user.IsEmailVerified ?? false))
                    {
                        string password = "";
                        var currentPassword = user.UserPasswords.Where(p => p.IsActive == true).FirstOrDefault();
                        if (currentPassword != null)
                        {
                            password = PasswordHelper.GetHash(model.Password) + currentPassword.PasswordSalt;
                        }

                        var userPassword = user.UserPasswords.Where(p => (p.PasswordHash + p.PasswordSalt) == password && p.IsActive == true).FirstOrDefault();
                        if (userPassword == null)
                        {
                            loginResponse.IsSuccess = false;
                            loginResponse.Message = "Username / password entered is incorrect";
                        }
                        else
                        {
                            loginResponse = await GetUserInfo(user.UserId);

                            // Add user location and login datetime
                            await AddUserLocationAndDeviceInfo(user, model);

                            loginResponse.IsSuccess = true;
                            loginResponse.Message = "Success";
                        }
                    }
                    else
                    {
                        loginResponse.IsSuccess = false;
                        loginResponse.Message = "Please verify your account in order to proceed.";
                    }
                }

                return loginResponse;
            }
            catch (Exception ex)
            {
                return new LoginResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        private async Task AddUserLocationAndDeviceInfo(User? user, LoginBO model)
        {
            if (_appSettings.RecordLoginHistory)
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

                var logLocationDeiceiInfo = new UserDeviceLocationAndToken()
                {
                    UserId = user.UserId,
                    DeviceType = $"{model.DeviceType}: {user.UserType.Role}",
                    DeviceToken = model.DeviceToken ?? "",
                    IpAddress = host.AddressList.First(addr => addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString(),
                    LastLogin = DateTime.UtcNow
                };

                user.UserDeviceLocationAndTokens.Add(logLocationDeiceiInfo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<LoginResponse> GetUserInfo(long userId)
        {
            try
            {
                var user = await _context.Users
                                         .Include(x => x.UserPasswords)
                                         .Include(x => x.Company)
                                         .ThenInclude(x => x.CompanyConfigurationSetting)
                                         .Include(x => x.UserType)
                                         .Include(x => x.CustomerUserFeaturesMasters)
                                         .Where(u => u.UserId == userId)
                                         .FirstOrDefaultAsync();

                if (user != null)
                {
                    var res = _mapper.Map<User, LoginResponse>(user);

                    if (user.UserTypeId == (byte)UserTypes.CompanyAdmin)
                    {
                        res.ManagedFeatures = (from c in _context.CompanyPackageDetails
                                               join p in _context.PackageFeatures on c.PackageId equals p.PackageId
                                               where c.CompanyId == user.CompanyId
                                               select new
                                               {
                                                   p.SelectedFeatures
                                               }).FirstOrDefault()?.SelectedFeatures;
                    }

                    res.IsSuccess = true;
                    return res;
                }
                else
                {
                    return new LoginResponse()
                    {
                        IsSuccess = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new LoginResponse()
                {
                    IsSuccess = false,
                    Message = $"ERROR: {ex.Message} :: INNER: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<BaseResponseBO> Register(RegisterBO model)
        {
            try
            {
                // check if email already exists or not.
                var verify = _context.Companies.Where(c => c.Email == model.Email).FirstOrDefault();
                if (verify != null)
                {
                    return new BaseResponseBO()
                    {
                        IsSuccess = false,
                        Message = "Email already exists. Please choose a different email and try again."
                    };
                }

                // check if company name already exists or not
                verify = _context.Companies.Where(c => c.CompanyName == model.CompanyName).FirstOrDefault();
                if (verify != null)
                {
                    return new BaseResponseBO()
                    {
                        IsSuccess = false,
                        Message = "Company name is already registered. Please choose a different company name and try again."
                    };
                }

                // check if Domain name already exists or not
                verify = _context.Companies.Where(c => c.DomainName == model.DomainName).FirstOrDefault();
                if (verify != null)
                {
                    return new BaseResponseBO()
                    {
                        IsSuccess = false,
                        Message = "Domain name is already registered. Please choose a different domain name and try again."
                    };
                }

                Company comp = new();

                _mapper.Map(model, comp);
                comp.CreatedOn = DateTime.UtcNow;
                _context.Companies.Add(comp);
                await _context.SaveChangesAsync();

                User user = new();
                string verificationToken = $"{Guid.NewGuid().ToString().Replace("-", "")}_{DateTime.UtcNow.Ticks}";

                _mapper.Map(model, user);
                user.CompanyId = comp.CompanyId;
                AddUserPassword(model.Password, user);
                user.UserTypeId = (byte)UserTypes.CompanyAdmin;
                return await SaveUser(user, verificationToken);

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

        private async Task<BaseResponseBO> SaveUser(User user, string verificationToken)
        {
            string message = string.Empty;
            user.CreatedOn = DateTime.UtcNow;
            user.VerificationCode = verificationToken;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var company = await _context.Companies.FindAsync(user.CompanyId);

            if ((UserTypes)user.UserTypeId == UserTypes.CompanyAdmin)
            {
                Hashtable ht = new Hashtable();
                ht["@Name"] = $"{user.FirstName} {user.LastName}";
                ht["@VerificationLink"] = _appSettings.EmailVerificationUrl + verificationToken;
                var mail = _mailService.SendEmailAsync(user.Email, "", "Orderflow - Email verification", EmailTemplates.VerifyEmail.ToString(), ht, "");
                message = $"Welcome to OrderFlow Family! <br/>Thanks you for registering at our website.A verification email has been sent to <b>{user.Email}</b>. " +
                          $"<br/>Click on the verification link in the email to verify your account and enable login. Once you activate your account, you will be able to access our website. ";

            }
            else
            {
                Hashtable ht = new Hashtable();
                ht["@Name"] = $"{user.FirstName} {user.LastName}";
                ht["@Company"] = company?.CompanyName;
                var mail = _mailService.SendEmailAsync(user.Email, "", "OrderFlow - Welcome", EmailTemplates.WelcomeRegister.ToString(), ht, "");
                message = $"Thank you for choosing OrderFlow! A verification email has been sent to the company admin and upon approval of your request " +
                    $"a confirmation email will be sent to you. Meanwhile, please verify your email address using the link that was sent to {user.Email}.";
            }

            return new BaseResponseBO()
            {
                IsSuccess = true,
                Message = message
            };
        }

        private static void AddUserPassword(string password, User user)
        {
            var salt = PasswordHelper.GetSalt();
            var hash = PasswordHelper.GetHash(password);

            UserPassword usePwd = new();

            usePwd.PasswordHash = hash;
            usePwd.PasswordSalt = salt;
            usePwd.IsActive = true;
            usePwd.UserId = user.UserId;
            usePwd.CreatedOn = DateTime.UtcNow;

            user.UserPasswords.Add(usePwd);
        }

        public async Task<BaseResponseBO> ValidateAccount(string token)
        {
            try
            {
                string error = "The verification token is invalid or has expired. Please use the form below to receive a new verification token to your email and try again.";
                var user = await _context.Users.Where(user => user.VerificationCode == token).FirstOrDefaultAsync();

                if (user == null || string.IsNullOrEmpty(token) || !VerifyToken(token))
                {
                    return new BaseResponseBO()
                    {
                        IsSuccess = false,
                        Message = error
                    };
                }
                else
                {
                    user.IsActive = true;
                    user.IsEmailVerified = true;
                    user.VerificationCode = "";

                    // set the company to active state
                    var company = _context.Companies.Where(c => c.CompanyId == user.CompanyId).FirstOrDefault();
                    company.IsActie = true;

                    // add record in company package table for default package only if the base package is choosen
                    // if any other package is chosen then this will be added after payment is done and the user is subscribed
                    // to the plan
                    AddDefaultPackageForCompany(company);

                    // add a default deliery run and sales rep entry for the new company after verification
                    AddDefaultRunAndSalesRepForCompany(company);

                    await _context.SaveChangesAsync();

                    try
                    {
                        // call the procedure to add seed data to the company tabled for easy initial access
                        await _context.GetProcedures().InsertSeedDataAsync(company.CompanyId);
                    }
                    catch
                    {}

                    return new BaseResponseBO()
                    {
                        IsSuccess = true,
                        Message = "Congratulations! Your account has been verified. You can start using your company admin portal now! An email will be sent"
                                + " to you shortly, with the details about your personalized web portal access, when that has been configured."
                    };
                }
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

        private void AddDefaultPackageForCompany(Company? company)
        {
            var defaultpackage = _context.Packages.FirstOrDefault(x => x.IsDefault == true);
            var packageDetails = new CompanyPackageDetail()
            {
                CompanyId = company.CompanyId,
                PackageId = defaultpackage?.PackageId ?? 0,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
            };

            company.IsMobileOrdering = defaultpackage.IsMobileOrdering;
            company.IsProofOfDelivery = defaultpackage.IsProofOfDelivery;
            company.IsWebOrdering = defaultpackage.IsWebOrdering;

            _context.CompanyPackageDetails.Add(packageDetails);
        }

        private void AddDefaultRunAndSalesRepForCompany(Company? company)
        {
            var salesRep = new User()
            {
                CompanyId = company.CompanyId,
                FirstName = "Test",
                LastName = "Rep",
                Email = $"sales@{company.DomainName}.com",
                IsActive = true,
                IsEmailVerified = true,
                CreatedOn = DateTime.UtcNow,
                UserTypeId = (byte)UserTypes.Sales
            };

            AddUserPassword("Rep@123", salesRep);

            var userRepCodes = new UserSalesrepCode()
            {
                SalesrepCode = "Default"
            };

            salesRep.UserSalesrepCodes.Add(userRepCodes);

            var deliveryRun = new DeliveryRun()
            {
                CompanyId = company.CompanyId,
                RunNo = "Default",
                DaysOfWeek = "M,T,W,R,F",
                CreatedOn = DateTime.UtcNow
            };

            _context.Users.Add(salesRep);
            _context.DeliveryRuns.Add(deliveryRun);

        }

        private bool VerifyToken(string token)
        {
            long ticks = Convert.ToInt64(token.Split('_')[1]);
            DateTime tokenDate = new DateTime(ticks);
            DateTime currentDate = DateTime.UtcNow;

            TimeSpan ts = currentDate - tokenDate;

            if (ts.TotalMinutes > _appSettings.TokenExpirationTimeout)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<BaseResponseBO> ResendVerificationToken(string email, string type = "")
        {
            try
            {
                var response = new BaseResponseBO();

                User user = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();

                if (user != null)
                {
                    var company = await _context.Companies.Where(x => x.CompanyId == user.CompanyId).FirstOrDefaultAsync();

                    string verificationToken = $"{Guid.NewGuid().ToString().Replace("-", "")}_{DateTime.UtcNow.Ticks}";
                    user.VerificationCode = verificationToken;
                    user.ModifiedOn = DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Message = "A new verification link has been send to the email you provided. Please check your email and click on that link to vefiry your account.";

                    // send email with verification link
                    Hashtable ht = new Hashtable();
                    ht["@Name"] = $"{user.FirstName} {user.LastName}";

                    if (string.IsNullOrEmpty(type))
                    {
                        ht["@VerificationLink"] = _appSettings.EmailVerificationUrl + verificationToken;
                    }
                    else
                    {
                        ht["@VerificationLink"] = string.Format(_appSettings.EmailVerificationUrl, company.DomainName) + verificationToken;
                    }

                    var mail = _mailService.SendEmailAsync(email, "", "Orderflow - Email verification", EmailTemplates.VerifyEmail.ToString(), ht, "");
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "There is no company record corresponding to the email provided. Please check the email and try again.";
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

        public async Task<BaseResponseBO> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                var response = new BaseResponseBO();
                User? user = new User();
                string verificationToken = $"{Guid.NewGuid().ToString().Replace("-", "")}_{DateTime.UtcNow.Ticks}";
                string resetUrl = _appSettings.PasswordResetUrl + verificationToken;

                if ((UserTypes)forgotPasswordModel.UserTypeId == UserTypes.CompanyAdmin)
                {
                    user = await _context.Users.Where(x => x.Email == forgotPasswordModel.Email).FirstOrDefaultAsync();
                }
                else if ((UserTypes)forgotPasswordModel.UserTypeId == UserTypes.AppUser)
                {
                    user = await _context.Users.Where(x => x.Email == forgotPasswordModel.Email
                                                   && x.CompanyId == forgotPasswordModel.CompanyId)
                                               .FirstOrDefaultAsync();
                }


                if (user != null)
                {
                    var company = await _context.Companies.FindAsync(user.CompanyId);

                    if ((UserTypes)forgotPasswordModel.UserTypeId == UserTypes.AppUser)
                    {
                        resetUrl = string.Format(_appSettings.PasswordResetUrl, company.DomainName) + verificationToken;
                    }

                    user.ResetCode = verificationToken;
                    user.ModifiedOn = DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Message = "We have sent an email with instructions to reset your password. Please check your inbox and follow the steps to reset your password.";

                    // send email with verification link
                    Hashtable ht = new Hashtable();
                    ht["@Name"] = $"{user.FirstName} {user.LastName}";
                    ht["@VerificationLink"] = resetUrl;
                    var mail = await _mailService.SendEmailAsync(forgotPasswordModel.Email, "", "Orderflow - Reset Password", EmailTemplates.ForgotPassword.ToString(), ht, "");
                    _logger.Info($"Email response :{mail}");
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "We did not find an account matching the email you provided. Please check your email and try again.";
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

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordBO model)
        {
            try
            {
                string error = "The verification token is invalid or has expired.";
                var response = new BaseResponseBO();
                User? user = await _context
                                    .Users
                                    .Include(p => p.UserPasswords)
                                    .Where(x => x.ResetCode == model.ResetToken && x.ResetCode != null).FirstOrDefaultAsync();

                if (user == null)
                {
                    return new ResetPasswordResponse()
                    {

                        IsSuccess = false,
                        Message = error,
                        IsRecent = false
                    };
                }

                var isValidToken = VerifyToken(model.ResetToken);

                if (!isValidToken)
                {
                    return new ResetPasswordResponse()
                    {
                        IsSuccess = false,
                        Message = error,
                        IsRecent = false
                    };
                }
                // add logic to check if the password is most recently used one or not
                var isRecent = IsRecentlyUsedPassword(user.UserPasswords.ToList(), model.Password);

                if (isRecent)
                {
                    return new ResetPasswordResponse()
                    {
                        IsSuccess = false,
                        Message = "Your new password can not be same as any of your recent passwords. Please choose a different password.",
                        IsRecent = true
                    };
                }

                // make all existing user passwords as inactive
                foreach (var userPass in user.UserPasswords)
                {
                    userPass.IsActive = false;
                }

                _context.UserPasswords.UpdateRange(user.UserPasswords);

                user.ResetCode = "";
                user.ModifiedOn = DateTime.UtcNow;

                AddUserPassword(model.Password, user);

                await _context.SaveChangesAsync();

                return new ResetPasswordResponse()
                {
                    IsSuccess = true,
                    Message = "Your password has been reset successfully.",
                    IsRecent = false
                };
            }
            catch (Exception ex)
            {
                return new ResetPasswordResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        private bool IsRecentlyUsedPassword(List<UserPassword> passwords, string newPasword)
        {
            bool isRecent = false;
            var last3 = passwords.OrderByDescending(p => p.Id).Take(3);

            foreach (var pwd in last3)
            {
                string hash = PasswordHelper.GetHash(newPasword);

                if (pwd.PasswordHash == hash)
                {
                    isRecent = true;
                    break;
                }
            }

            return isRecent;
        }


        #region API Account Methods
        public async Task<BaseResponseBO> UserRegister(AppUserRegisterBO model)
        {
            var verifyMail = await _context.Users.Where(u => u.Email == model.Email && u.CompanyId == model.CompanyId).FirstOrDefaultAsync();

            if (verifyMail != null)
            {
                return ReturnMethod(false, "Email already exists for another customer.");
            }
            User user = new();
            string verificationToken = $"{Guid.NewGuid().ToString().Replace("-", "")}_{DateTime.UtcNow.Ticks}";

            _mapper.Map(model, user);
            AddUserPassword(model.Password, user);
            return await SaveUser(user, verificationToken);
        }

        public async Task<DomainInfoBO> GetDomainInfo(string domain)
        {
            try
            {
                var domainInfo = await _context.Companies
                                        .Where(x => x.DomainName == domain)
                                        .FirstOrDefaultAsync();

                return _mapper.Map<DomainInfoBO>(domainInfo);
            }
            catch (Exception ex)
            {
                return new DomainInfoBO() { IsSuccess = false, Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}" };
            }
        }

        #endregion

    }
}
