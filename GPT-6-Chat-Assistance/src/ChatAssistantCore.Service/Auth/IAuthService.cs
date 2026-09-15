using System;
using System.Threading.Tasks;
using ChatAssistantCore.DTO.Auth;

namespace ChatAssistantCore.Service.Auth;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
}
