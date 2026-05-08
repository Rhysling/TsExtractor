using System;
using System.Collections.Generic;

namespace TsExtractor2.Models
{
	public class ClassModel
	{
		public required string FilePath { get; set; }
		public required string NamespaceName { get; set; }
		public required string ClassName { get; set; }
		public bool IsTypescriptModel { get; set; }
		public string? BaseTypeName { get; set; }
		public bool IsBaseType { get; set; }
		public bool IsInterface { get; set; } = false;
		public bool IsPartial { get; set; } = false;
		public string? TsName { get; set; }
		public List<PropModel> PropertyList { get; set; } = [];

		public bool HasBaseType => !String.IsNullOrEmpty(BaseTypeName) && BaseTypeName != "Object" && BaseTypeName != "Attribute" && BaseTypeName != "Record";
	}
}
