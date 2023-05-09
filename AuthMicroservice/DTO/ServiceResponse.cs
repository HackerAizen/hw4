using System.Text.Json.Serialization;

namespace AuthMicroservice.DTO
{
	/// <summary>
	/// Ответ слоя сервисов об (не)успешности 
	/// операции
	/// </summary>
	public class ServiceResponse
	{
		[JsonIgnore]
		public ServiceResponseStatuses Status { get; set; }
		[JsonIgnore]
		public string Message { get; set; }
	}
}
