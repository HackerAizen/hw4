using AuthMicroservice.DTO;
using AuthMicroservice.Models.Requests;
using AuthMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace AuthMicroservice.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController: Controller
	{
		private readonly IAuthService _authService;
		
		public AuthController(IAuthService authService)
		{
			_authService = authService ??
				throw new ArgumentNullException(nameof(authService));
		}

		/// <summary>
		/// Регистрация
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequest request)
		{
			if (!ModelState.IsValid)
			{
				return UnprocessableEntity(ModelState);
			}

			try
			{
				ServiceResponse response = await _authService.RegisterAsync(request);

				if(response.Status ==  ServiceResponseStatuses.Sussess)
					return Ok("Пользователь успешно зарегистрирован");
				
				if(response.Status == ServiceResponseStatuses.ValidationError)
				{
					return UnprocessableEntity(response.Message);
				}

				return BadRequest(response.Message);
			}
			catch (Exception)
			{
				return BadRequest("Внутренняя ошибка приложения");
			}
		}

		/// <summary>
		/// Авторизация
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("Auth")]
		public async Task<IActionResult> Auth([FromBody] AuthRequest request)
		{
			try
			{
				var response = await _authService.AuthAsync(request);

				if (response.Status == ServiceResponseStatuses.Sussess)
					return Ok(response.JwtToken);

				if (response.Status == ServiceResponseStatuses.ValidationError)
				{
					return Unauthorized(response.Message);
				}

				return BadRequest(response.Message);
			}
			catch (Exception)
			{
				return BadRequest("Внутренняя ошибка приложения");
			}
		}

		
		/// <summary>
		/// Информация о пользователе
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		[HttpGet("UserInfo")]
		public async Task<IActionResult> GetUserInfo(string token)
		{
			if (string.IsNullOrWhiteSpace(token))
			{
				return BadRequest("Не передан токен");
			}

			try
			{
				var response = await _authService.GetUserInfoAsync(token);

				if (response.Status == ServiceResponseStatuses.Sussess)
					return Ok(response);

				if (response.Status == ServiceResponseStatuses.ValidationError)
				{
					return Unauthorized(response.Message);
				}

				return BadRequest(response.Message);
			}
			catch (Exception)
			{
				return BadRequest("Внутренняя ошибка приложения");
			}
		}
		
	}
}
