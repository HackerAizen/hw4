using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using OrderMicroservice.Models.Responses;
using OrderMicroservice.Services;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace OrderMicroservice.Middleware
{
	public class AuthMiddleware : AuthenticationHandler<AuthenticationSchemeOptions>
	{

		private readonly IAuthService _authService;
		public AuthMiddleware(IOptionsMonitor<AuthenticationSchemeOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock,
			IAuthService authService)
			: base(options, logger, encoder, clock)
		{
			_authService = authService ??
				throw new ArgumentNullException(nameof(authService));
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.ContainsKey("Authorization"))
			{
				return AuthenticateResult.NoResult();
			}
			if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out AuthenticationHeaderValue? headerValue))
			{
				return AuthenticateResult.NoResult();
			}


			try
			{
				UserInfoResponse? userInfo = await _authService.GetUserInfoAsync(headerValue.ToString());

				if (userInfo == null)
					return AuthenticateResult.NoResult();


				var claims = new[] { 
					new Claim("Email", userInfo.Email),
					new Claim(ClaimTypes.Role, userInfo.Role),
					new Claim("UserId", userInfo.Id.ToString())
				};

				var identity = new ClaimsIdentity(claims, Scheme.Name);
				var principal = new ClaimsPrincipal(identity);
				var ticket = new AuthenticationTicket(principal, Scheme.Name);

				return AuthenticateResult.Success(ticket);
			}
			catch (Exception)
			{
				return AuthenticateResult.NoResult();
			}
		}
	}
}
