using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsExtractor2.Models;

namespace TsExtractor2.Writers
{
	public class LogFileWriter
	{
		private readonly string outFullPath;
		private readonly string header;
		private readonly List<ClassModel> syntaxClasses;

		public LogFileWriter(string outFullPath, string header, List<ClassModel> syntaxClasses)
		{
			this.outFullPath = outFullPath;
			this.header = header;
			this.syntaxClasses = syntaxClasses;
		}

		public void Write()
		{
			var sb = new StringBuilder();

			sb.AppendLine(header);
			string filePath = "";

			foreach (var c in syntaxClasses)
			{
				if (filePath != c.FilePath)
				{
					sb.AppendLine(c.FilePath);
					filePath = c.FilePath;
				}

				sb.AppendLine($"Namespace: {c.NamespaceName}");
				sb.AppendLine($"ClassName: {c.ClassName}");

				foreach (var p in c.PropertyList)
				{
					sb.AppendLine($"  {p.PropName}");
					sb.AppendLine($"    {String.Join("-", p.PropTypes.FlattenTypeNames())}");
				}

				sb.AppendLine();
			}

			File.WriteAllText(outFullPath, sb.ToString());
		}
	}
}
