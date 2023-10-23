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

				var bcl = allClasses.Where(a => a.HasBaseType && a.IsTypescriptModel).Select(a => (a.BaseTypeName, a.IsInterface)).ToList();

				foreach (var c in allClasses)
				{
					c.IsBaseType = bcl.Select(a => a.BaseTypeName).Contains(c.ClassName);
					if (c.IsBaseType)
					{
						c.IsInterface = bcl.FirstOrDefault(a => a.BaseTypeName == c.ClassName).IsInterface;
					}

					c.TsName = c.IsInterface ? "I" + c.ClassName : c.ClassName;
				}

				tsClasses = allClasses.Where(a => a.IsTypescriptModel || a.IsBaseType).ToList();

				return tsClasses;
			}
		}
	}
}
