using System;

namespace HowdyWorld
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class TypeScriptModelAttribute : Attribute
	{
		public string ExcludeMembersByName { get; set; } = "";
		public string OptionalMembersByName { get; set; } = "";
		public bool IsInterface { get; set; } = false;
	}
}