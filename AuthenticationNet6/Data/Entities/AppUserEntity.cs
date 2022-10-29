namespace Data.Entities
{
	public class AppUserEntity
	{
		public string Id { get; set; } = string.Empty;
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		public override string ToString()
		{
			return $"User: {FirstName} {LastName}";
		}
	}
}
