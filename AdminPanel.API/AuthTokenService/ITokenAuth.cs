using AdminPanel.Shared.BO;

namespace AdminPanel.API.AuthTokenService
{
    public interface ITokenAuth
    {
        Task<LoginResponse> GenerateToken(LoginResponse res);
    }
}
