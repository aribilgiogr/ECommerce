using Core.Concretes.DTOs;
using Utils.Responses;

namespace Core.Abstracts.IServices
{
    public interface IAuthService
    {
        Task<IResult> LoginAsync(LoginDto model);
        Task<IResult> RegisterAsync(RegisterDto model);
        Task LogoutAsync();
    }
}
