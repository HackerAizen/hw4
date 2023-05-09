using OrderMicroservice.DTO;

namespace OrderMicroservice.Models.Responses
{
	public class OrderInfoResponse: ServiceResponse
	{
		public int Id { get; set; }

		public int UserId { get; set; }
		public string Status { get; set; }
		public string? SpecialRequests { get; set; }
		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }
		public OrderDishResponse[] Dishes { get; set; }
	}
}
