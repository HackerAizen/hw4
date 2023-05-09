using AuthMicroservice.DTO;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace AuthMicroservice.Models.Responses
{
	/// <summary>
	/// Модель информации о пользователе
	/// </summary>
	public class UserInfoResponse: ServiceResponse
	{
		/// <summary>
		/// Id пользователя
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Email
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Роль
		/// </summary>
		public string Role { get; set; }
	}
}
