using Leoni.Utils;
using static Leoni.Dtos.AuthDto;

namespace Leoni.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoggedInDto> signIN(RegistrationDto input);
        Task SignOut(string refreshToken, string? employeeId, bool alldevices = false);
        Task<LoggedInDto> SignUP(RegistrationDto input);

        Task<LoggedInDto> RotateTokens(string refreshToken);
    }
}
