namespace Model.Entities
{
	public class OrderDish
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int DishId { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }

		public virtual Order Order { get; set; }
		public virtual Dish Dish { get; set; }
	}
}
