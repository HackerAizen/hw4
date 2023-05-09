namespace Model.Entities
{
	public class Dish
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public Decimal Price { get; set; }
		public int Quantity { get; set; }
		public bool IsAvailable { get; set; }

		public virtual ICollection<OrderDish> OrderDishes { get; set; }

		public Dish()
		{
			OrderDishes = new HashSet<OrderDish>();
		}
	}
}
