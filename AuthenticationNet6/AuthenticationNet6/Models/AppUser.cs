using System.Text.Json.Serialization;

namespace AuthenticationNet6.Models
{
	public class AppUser
	{
		public string? Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Username { get; set; }

		[JsonIgnore]
		public string? Password { get; set; }

		public override string ToString()
		{
			return $"User: {FirstName} {LastName}";
		}
	}
}
