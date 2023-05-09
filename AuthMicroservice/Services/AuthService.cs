using AuthMicroservice.Configuration;
using AuthMicroservice.DTO;
using AuthMicroservice.Helpers;
using AuthMicroservice.Models.Requests;
using AuthMicroservice.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model;
using Model.Entities;

namespace AuthMicroservice.Services
{
	public class AuthService : IAuthService
	{
		private readonly IDbContextFactory<OrdersProcessingDbContext> _dbContextFactory;
		private readonly JwtHelper _jwtHelper;
		private readonly string[] _availableRoles = new string[] { ApplicationRoles.Customer, ApplicationRoles.Manager, ApplicationRoles.Chef };

		public AuthService(IDbContextFactory<OrdersProcessingDbContext> dbContextFactory,
			IOptions<JwtConfiguration> jwtConfiguration)
		{
			_dbContextFactory = dbContextFactory ??
				throw new ArgumentNullException(nameof(dbContextFactory));

			 if(jwtConfiguration?.Value == null)
				throw new ArgumentNullException(nameof(jwtConfiguration));

			_jwtHelper = new JwtHelper(jwtConfiguration.Value);
		}

		public async Task<AuthResponse> AuthAsync(AuthRequest authRequest)
		{
			AuthResponse response = new AuthResponse()
			{
				Status = ServiceResponseStatuses.Sussess
			};

			User? user;
			try
			{
				using (var context = await _dbContextFactory.CreateDbContextAsync())
				{
					string email = authRequest.Email.ToLower();
					string passwordHash = ShaHelper.Sha256(authRequest.Password);

				    user = await context.Users.FirstOrDefaultAsync(x => x.Email == email &&
					x.PasswordHash == passwordHash);
				}
			}
			catch (Exception)
			{
				response = new AuthResponse()
				{
					Status = ServiceResponseStatuses.ApplicationError,
					Message = "Внутренняя ошибка приложения"
				};

				return response;
			}

			if (user == null)
			{
				response = new AuthResponse()
				{
					Status = ServiceResponseStatuses.ValidationError,
					Message = "Неверные email и/или пароль"
				};

				return response;
			}

			try
			{
				(string token, DateTime expiresAt) = _jwtHelper.CreateJwt(user.Email, user.Id);
				await CreateSession(user.Id, token, expiresAt);

				response.JwtToken = token;
				return response;
			}
			catch (Exception)
			{

				response = new AuthResponse()
				{
					Status = ServiceResponseStatuses.ApplicationError,
					Message = "Внутренняя ошибка приложения"
				};

				return response;
			}
		}

		public async Task<ServiceResponse> RegisterAsync(RegisterRequest registerRequest)
		{

			ServiceResponse response = await ValidateRegister(registerRequest);

			if(response.Status != ServiceResponseStatuses.Sussess)
				return response;


			string passwordHash = ShaHelper.Sha256(registerRequest.Password);

			User newUser = new User
			{
				Email = registerRequest.Email.ToLower(),
				UserName = registerRequest.UserName,
				PasswordHash = passwordHash,
				Role = registerRequest.Role,
			};

			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				try
				{
					await context.Users.AddAsync(newUser);
					await context.SaveChangesAsync();
				}
				catch (Exception)
				{
					return new ServiceResponse()
					{
						Status = ServiceResponseStatuses.ApplicationError,
						Message = "Внутренняя ошибка приложения"
					};
				}
			}

			return response;
		}

		private async Task<ServiceResponse> ValidateRegister(RegisterRequest registerRequest)
		{
			ServiceResponse response = new ServiceResponse()
			{
				Status = ServiceResponseStatuses.Sussess
			};

			if (!_availableRoles.Contains(registerRequest.Role))
			{
				response = new ServiceResponse()
				{
					Status = ServiceResponseStatuses.ValidationError,
					Message = $"Выбрана недопустимая роль. Допустимые: {string.Join(',', _availableRoles)}"
				};
				return response;
			}

			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{

				bool isExistsUserWithSameName = await context.Users.AnyAsync(x => x.UserName == registerRequest.UserName);

				if (isExistsUserWithSameName)
				{
					response = new ServiceResponse()
					{
						Status = ServiceResponseStatuses.ValidationError,
						Message = $"Пользователь с userName: {registerRequest.UserName} уже существует"
					};
					return response;
				}

				bool isExistsUserWithSameEmail = await context.Users.AnyAsync(x => x.Email == registerRequest.Email.ToLower());

				if (isExistsUserWithSameName)
				{
					response = new ServiceResponse()
					{
						Status = ServiceResponseStatuses.ValidationError,
						Message = $"Пользователь с email: {registerRequest.UserName} уже существует"
					};
					return response;
				}

			}

			return response;
		}

		private async Task CreateSession(int userId, string token, DateTime expiresAt)
		{
			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				var session = new Session
				{
					UserId = userId,
					JwtToken = token,
					ExpiresAt = expiresAt,
				};

				await context.Sessions.AddAsync(session);
				await context.SaveChangesAsync();
			}
		}

		public async Task<UserInfoResponse> GetUserInfoAsync(string token)
		{
			var response = new UserInfoResponse
			{
				Status = ServiceResponseStatuses.Sussess
			};
			try
			{
				using (var context = await _dbContextFactory.CreateDbContextAsync())
				{
					var session = context.Sessions.FirstOrDefault(x => x.JwtToken == token);

					if (session == null || session.ExpiresAt < DateTime.UtcNow)
					{
						response = new UserInfoResponse()
						{
							Status = ServiceResponseStatuses.ValidationError,
							Message = "Не найдено активной сессии для переданного токена"
						};

						return response;

					}

					response.UserName = session.User.UserName;
					response.Email = session.User.Email;
					response.Role = session.User.Role;
					response.Id = session.User.Id;

					return response;
				}
			}
			catch (Exception)
			{
				response = new UserInfoResponse
				{
					Status = ServiceResponseStatuses.ApplicationError,
					Message = "Внутренняя ошибка приложения"
				};
				return response;
			}
		}
	}
}
