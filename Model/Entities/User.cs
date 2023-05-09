namespace Model.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public DateTime Created { get; set; } = DateTime.UtcNow;
		public DateTime Updated { get; set; } = DateTime.UtcNow;
		public string Role { get; set; }

		public virtual ICollection<Session> Sessions { get; set; }
		public virtual ICollection<Order> Orders { get; set; }

		public User()
		{
			Sessions = new HashSet<Session>();
			Orders = new HashSet<Order>();
		}
	}
}
