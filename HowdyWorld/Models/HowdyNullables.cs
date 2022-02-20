using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowdyWorld.Models
{
	[TypeScriptModel]
	public class HowdyNullables
	{
		public int ValInt { get; set; }
		public int? ValNullableInt { get; set; }
		public string ValString { get; set; } = "";
		public string? ValNullableString { get; set; }
		public List<string> ValListOfString { get; set; } = new();
		public List<string>? ValNullableListOfString { get; set; }
		public List<string?>? ValNullableListOfNullableString { get; set; } = new();
	}
}
