using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.Account;

namespace AdminPanel.Web.Server.Services
{
    public interface IAccountService
    {
        Task<GenericResponse<LoginResponse>> ValidateUser(LoginBO model);
        Task<BaseResponseBO> Register(AppUserRegisterBO model);
        Task<BaseResponseBO> ForgotPassword(ForgotPasswordModel forgotPasswordModel);
        Task<ResetPasswordResponse> ResetPassword(ResetPasswordBO model);
        Task<BaseResponseBO> ResendVerificationToken(ResendTokenBO model);
        Task<LoginResponse> GetUserInfo(long userId);
        Task<DomainInfoBO> GetDomainInfo(string domain);
    }
}
