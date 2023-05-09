namespace OrderMicroservice.Models.Requests
{
	/// <summary>
	/// Модель обновления блюда
	/// </summary>
	public class UpdateDishRequest
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
