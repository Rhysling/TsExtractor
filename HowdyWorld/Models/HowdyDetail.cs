namespace HowdyWorld.Models
{
	[TypeScriptModel]
	public class HowdyDetail
	{
		public int HowdyId { get; set; }
		public string SayHowdy => "Howdy!";
	}

		namespace SubNamespace
		{
			[TypeScriptModel]
			public class HowdyClassInSubNamespace
			{
				public int HowdyId { get; set; }
				public string SayHowdy => "Howdy!";
			}
		}


}