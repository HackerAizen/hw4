using AuthMicroservice.DTO;
using AuthMicroservice.Models.Requests;
using AuthMicroservice.Models.Responses;

namespace AuthMicroservice.Services
{
	public interface IAuthService
	{
		Task<ServiceResponse> RegisterAsync(RegisterRequest registerRequest);
		Task<AuthResponse> AuthAsync(AuthRequest authRequest);
		Task<UserInfoResponse> GetUserInfoAsync(string token);
	}
}
