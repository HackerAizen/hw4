using OrderMicroservice.DTO;

namespace OrderMicroservice.Models.Responses
{
	public class DishResponse: ServiceResponse
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public bool IsAvailable { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
