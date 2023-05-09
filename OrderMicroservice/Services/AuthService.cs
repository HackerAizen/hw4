using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderMicroservice.Configuration;
using OrderMicroservice.Models.Responses;
using System.Xml.Linq;

namespace OrderMicroservice.Services
{
	public class AuthService : IAuthService
	{
		private readonly AuthConfiguration _authConfiguration;
		private readonly IHttpClientFactory _httpClientFactory;

		public AuthService(
			IOptions<AuthConfiguration> options, 
			IHttpClientFactory httpClientFactory)
		{
			_authConfiguration = options.Value ??
				throw new ArgumentNullException(nameof(options));

			_httpClientFactory = httpClientFactory ??
				throw new ArgumentNullException(nameof(httpClientFactory));
		}
		public async Task<UserInfoResponse?> GetUserInfoAsync(string token)
		{
			using HttpClient client = _httpClientFactory.CreateClient("AuthClient");

			var response = await client.GetAsync($"{_authConfiguration.GetUserInfoEndpoint}?token={token}");

			UserInfoResponse? userInfoResponse = null;

			if(response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				string responseContent = await response.Content.ReadAsStringAsync();

				userInfoResponse  = JsonConvert.DeserializeObject<UserInfoResponse>(responseContent);
			}

			return userInfoResponse;
		}
	}
}
