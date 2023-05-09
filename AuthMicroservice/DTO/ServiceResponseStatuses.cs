namespace AuthMicroservice.DTO
{
	public enum ServiceResponseStatuses
	{
		/// <summary>
		/// Успешное действие
		/// </summary>
		Sussess,

		/// <summary>
		/// Невалидные входные данные
		/// </summary>
		ValidationError,

		/// <summary>
		/// Внутренняя ошибка приложения
		/// </summary>
		ApplicationError
	}
}
