using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TsExtractor2.Models;
using TsExtractor2.Operations;
using TsExtractor2.Utilities;

namespace TsExtractor2.Writers
{
	public class DtsWriter
	{
		private readonly string outFullPath;
		private readonly string header;
		private readonly List<ClassModel> classModels;
		private readonly List<string> projectNames;
		private readonly string solutionName;

		public DtsWriter(string outFullPath, string header, SolutionModel solutionModel)
		{
			this.outFullPath = outFullPath;
			this.header = header;
			classModels = solutionModel.TsClasses;
			projectNames = solutionModel.ProjectNames;
			solutionName = solutionModel.SolutionName;
		}

		public void Write()
		{
			var sb = new StringBuilder();

			sb.AppendLine(header);
			sb.AppendLine();

			sb.AppendLine($"// SOLUTION: {solutionName}");
			sb.AppendLine();

			sb.AppendLine($"// PROJECT{(projectNames.Count != 1 ? "S" : "" )} SEARCHED:");
			foreach (string p in projectNames)
				sb.AppendLine($"//\t{p}");
			sb.AppendLine();

			sb.AppendLine("type INullable<T> = T | null;");
			sb.AppendLine();

			string ns = "";
			var sc = classModels.OrderBy(a => a.NamespaceName).ThenBy(a => a.ClassName).ToList();
			var tsClassNames = sc.Select(a => a.ClassName).ToList();

			foreach (var c in sc)
			{
				if (ns != c.NamespaceName)
				{
					sb.AppendLine($"// NAMESPACE: {c.NamespaceName}");
					sb.AppendLine();
					ns = c.NamespaceName;
				}

				sb.AppendLine($"interface I{c.ClassName}{(c.HasBaseType ? " extends I" + c.BaseTypeName : "")} {{");

				foreach (var p in c.PropertyList)
					sb.AppendLine($"\t{p.PropName.CamelCase()}: {Mappings.MapPropTypeNamesToTsType(p.PropTypes.FlattenTypeNames(), tsClassNames)};");

				sb.AppendLine("}");
				sb.AppendLine();
			}

			File.WriteAllText(outFullPath, sb.ToString());
		}
	}
}
