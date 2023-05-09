using System.Text.Json.Serialization;

namespace OrderMicroservice.DTO
{
	/// <summary>
	/// Ответ слоя сервисов об (не)успешности 
	/// операции
	/// </summary>
	public class ServiceResponse
	{
		[JsonIgnore]
		public ServiceResponseStatuses ReponseStatus { get; set; }
		[JsonIgnore]
		public string Message { get; set; }
	}
}
