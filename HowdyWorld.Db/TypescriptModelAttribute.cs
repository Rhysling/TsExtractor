using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HowdyWorld.Db
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class TypeScriptModelAttribute : Attribute
	{
		public string ExcludeMembersByName { get; set; }
		public string OptionalMembersByName { get; set; }
	}
}