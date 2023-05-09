namespace OrderMicroservice.Models.Responses
{
	public class OrderDishResponse
	{
		public int DishId { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}
}
