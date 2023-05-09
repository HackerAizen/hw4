using System.Text;
using System.Security.Cryptography;

namespace AuthMicroservice.Helpers
{
	public static class ShaHelper
	{
		public static string Sha256(string data)
		{
			using (SHA256 crypt = SHA256.Create())
			{
				string hash = String.Empty;
				byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(data));
				foreach (byte theByte in crypto)
				{
					hash += theByte.ToString("x2");
				}
				return hash;
			}
		}
	}
}
