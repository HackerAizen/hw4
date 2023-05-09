using OrderMicroservice.Models.Responses;

namespace OrderMicroservice.Services
{
	public interface IAuthService
	{
		Task<UserInfoResponse> GetUserInfoAsync(string token);
	}
}
