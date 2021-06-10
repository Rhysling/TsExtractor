using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowdyWorld.Models
{
	[TypeScriptModel]
	public class HowdyDerived : HowdyBase
	{
		public int IdInDerived { get; set; }
		public string StringInDerived { get; set; }
	}
}
