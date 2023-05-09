namespace Model.Entities
{
	public class Order
	{
		public int Id { get; set; }
		public int UserId { get; set; }	
		public string Status { get; set; }
		public string? SpecialRequests { get; set; }
		public DateTime Created { get; set; } = DateTime.UtcNow;
		public DateTime Updated { get; set; } = DateTime.UtcNow;

		public virtual User User { get; set; }
		public virtual ICollection<OrderDish> OrderDishes { get; set; }

		public Order()
		{
			OrderDishes = new HashSet<OrderDish>();
		}


	}
}
