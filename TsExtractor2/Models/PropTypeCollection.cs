using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsExtractor2.Models
{
	public class PropTypeCollection
	{
		private List<string> flatTypeNames;

		public string PropName { get; set; }
		public string TypeName { get; set; }
		public string Category { get; set; }
		public TypeSyntax SubType { get; set; }
		public PropTypeCollection SubSummary { get; set; }

		public List<string> FlattenTypeNames()
		{
			if (flatTypeNames == null)
				flatTypeNames = extractName(new List<string>(), this);

			return flatTypeNames;

			List<string> extractName(List<string> names, PropTypeCollection ts)
			{
				names.Add(ts.TypeName);

				if (ts.SubSummary != null)
					names = extractName(names, ts.SubSummary);

				return names;
			}
		}
	}
}
