using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OrderMicroservice.Models.Requests
{
	public class CreateOrderRequest
	{
		[JsonIgnore]
		public int UserId { get; set; }

		[Required(ErrorMessage = "Необходимо заполнить блюда")]
		public CreateOrderDishRequest[] Dishes { get; set; }
		[Required(ErrorMessage = "Необходимо заполнить статус")]
		public string Status { get; set; }
		public string SpecialRequests { get;set; }
	}
}
