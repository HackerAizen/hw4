namespace AuthMicroservice.Configuration
{
	public class JwtConfiguration
	{
		public string Key { get; set; }
		public string Issuer { get; set; }
		public TimeSpan LifeTime { get; set; }
	}
}
