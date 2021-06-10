using System;
using System.Collections.Generic;
using System.Linq;

namespace TsExtractor2.Models
{
	public class SolutionModel
	{
		private List<ClassModel> tsClasses;

		public string SolutionName { get; set; }
		public List<ProjectModel> Projects { get; set; }

		public List<string> ProjectNames => Projects.Select(a => a.ProjectName).ToList();

		public List<ClassModel> TsClasses
		{
			get
			{
				if (tsClasses != null) return tsClasses;

				var allClasses = Projects.SelectMany(a => a.ClassList).ToList();

				var baseClassNames = allClasses.Where(a => a.HasBaseType && a.IsTypescriptModel).Select(a => a.BaseTypeName).ToList();

				foreach (var c in allClasses)
					c.IsBaseType = baseClassNames.Contains(c.ClassName);

				tsClasses = allClasses.Where(a => a.IsTypescriptModel || a.IsBaseType).ToList();

				return tsClasses;
			}
		}
	}
}
