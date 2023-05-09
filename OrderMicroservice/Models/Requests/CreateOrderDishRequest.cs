using System.ComponentModel.DataAnnotations;

namespace OrderMicroservice.Models.Requests
{
	public class CreateOrderDishRequest
	{
		public int DishId { get; set; }
		public int Quantity { get; set; }
	}
}
