using AuthMicroservice.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthMicroservice.Helpers
{
	public  class JwtHelper
	{

		private readonly JwtConfiguration _jwtConfiguration;

		public JwtHelper(JwtConfiguration jwtConfiguration)
		{
			_jwtConfiguration = jwtConfiguration;
		}

		public  (string, DateTime) CreateJwt(string email, int userId)
		{
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, email)
			};

			DateTime expiresAt = DateTime.UtcNow.Add(_jwtConfiguration.LifeTime);

			var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key));
			var token = new JwtSecurityToken(
				issuer: _jwtConfiguration.Issuer,
				audience: email,
				claims: claims,
				expires: expiresAt,
				signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
			);
			return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
		}
	}
}
