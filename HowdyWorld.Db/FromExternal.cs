using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowdyWorld.Db
{
	[TypeScriptModel]
	public class FromExternal
	{
		public int ExternalId { get; set; }
		public NonTsClass ValNonTsClass { get; set; }
	}
}
