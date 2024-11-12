using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.Repository.Account
{
    public interface IAccountRepository
    {
        Task<LoginResponse> ValidateUser(LoginBO model);
        Task<LoginResponse> GetUserInfo(long userId);
        Task<BaseResponseBO> Register(RegisterBO model);
        Task<BaseResponseBO> ValidateAccount(string token);
        Task<BaseResponseBO> ResendVerificationToken(string email, string type = "");
        Task<BaseResponseBO> ForgotPassword(ForgotPasswordModel forgotPasswordModel);
        Task<ResetPasswordResponse> ResetPassword(ResetPasswordBO model);
        Task<BaseResponseBO> UserRegister(AppUserRegisterBO model);
        Task<DomainInfoBO> GetDomainInfo(string domain);
    }
}
