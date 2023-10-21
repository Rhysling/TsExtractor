namespace HowdyWorld.Models
{

	[TypeScriptModel(ExcludeMembersByName="ExcludedInt,ExcludedString", IsInterface=true)]
	public class ContactMessage
	{
		public required string RequiredName { get; set; }
		public required string RequiredEmail { get; set; }
		public required string RequiredCompany { get; set; }
		public required string RequiredPhone { get; set; }
		public required string RequiredMessage { get; set; }
		public string? SentAt { get; set; }
		public int StatusCode { get; set; }

		public int ExcludedInt { get; set; }

		public required string ExcludedString { get; set; }
	}


}
