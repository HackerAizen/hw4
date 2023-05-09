using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models.Requests
{
	/// <summary>
	/// Модель авторизации
	/// </summary>
	public class AuthRequest
	{
		/// <summary>
		/// Email
		/// </summary>
		[Required( ErrorMessage = "Необходимо передать email")]
		public string Email { get; set; }

		/// <summary>
		/// Пароль
		/// </summary>
		[Required(ErrorMessage = "Необходимо передать пароль")]
		public string Password { get; set; }
	}
}
