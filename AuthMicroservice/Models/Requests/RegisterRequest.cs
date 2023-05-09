using System.ComponentModel.DataAnnotations;

namespace AuthMicroservice.Models.Requests
{
	/// <summary>
	/// Модель регистрации
	/// </summary>
	public class RegisterRequest
	{
		/// <summary>
		/// Юзернейм
		/// </summary>
		[Required(ErrorMessage ="Необходимо заполнить имя пользователя")]
		[StringLength(50, ErrorMessage = "Имя пользователя должно быть от 1 до 50 символов", MinimumLength = 1)]
		public string UserName { get; set; }

		/// <summary>
		/// Email
		/// </summary>
		[Required(ErrorMessage = "Необходимо заполнить email")]
		[StringLength(50, ErrorMessage = "Максимальная длина Email-а 50 символов")]
		[EmailAddress(ErrorMessage = "Невалидное значение email-адреса")]
		public string Email { get; set; }

		/// <summary>
		/// Пароль
		/// </summary>
		[Required(ErrorMessage = "Необходимо заполнить пароль")]
		[StringLength(int.MaxValue, MinimumLength =6,  ErrorMessage = "Минимальная длина пароля - 6 символов")]
		public string Password { get; set; }

		/// <summary>
		/// Роль
		/// </summary>
		[Required(ErrorMessage = "Необходимо заполнить роль")]
		public string Role { get; set; }
	}
}
