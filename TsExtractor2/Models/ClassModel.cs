using System;
using System.Collections.Generic;

namespace TsExtractor2.Models
{
	public class ClassModel
	{
		public string FilePath { get; set; }
		public string NamespaceName { get; set; }
		public string ClassName { get; set; }
		public bool IsTypescriptModel { get; set; }
		public string BaseTypeName { get; set; }
		public bool IsBaseType { get; set; }
		public List<PropModel> PropertyList { get; set; }

		public bool HasBaseType => !String.IsNullOrEmpty(BaseTypeName) && BaseTypeName != "Object" && BaseTypeName != "Attribute" && BaseTypeName != "Record";
	}
}
