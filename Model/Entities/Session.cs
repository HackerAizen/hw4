namespace Model.Entities
{
	public class Session
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public virtual User User { get; set; }
		public string JwtToken { get; set; }
		public DateTime ExpiresAt { get; set; }
	}
}
