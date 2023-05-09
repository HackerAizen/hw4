using AuthMicroservice.DTO;

namespace AuthMicroservice.Models.Responses
{
	public class AuthResponse: ServiceResponse
	{
		/// <summary>
		/// Jwt токен
		/// </summary>
		public string JwtToken { get; set; }
	}
}
