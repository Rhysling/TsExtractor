using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsExtractor2.Operations
{
	public static class Mappings
	{
		private static readonly Dictionary<string, string> sysToTsTypes = new Dictionary<string, string> {
			{ "Int32", "number"},
			{ "Int64", "error-long not supported in js"},
			{ "Single", "number"},
			{ "Double", "number"},
			{ "Decimal", "number"},
			{ "Byte", "string"},
			{ "String", "string"},
			{ "Char", "string"},
			{ "Guid", "string"},
			{ "Boolean", "boolean"}
		};

		public static string MapPropTypeNamesToTsType(List<string> typeList, List<string> TsClassList)
		{
			typeList.Reverse();
			var tq = new Queue<string>(typeList);

			string tsType, sysType;

			sysType = tq.Dequeue();

			if (sysToTsTypes.ContainsKey(sysType))
				tsType = sysToTsTypes[sysType];
			else if (TsClassList.Contains(sysType))
				tsType = "I" + sysType;
			else
				return "any";

			while (tq.Any())
			{
				string mod = tq.Dequeue();

				if (mod == "Nullable/")
					tsType = $"INullable<{tsType}>";
				else if (mod == "List/" || mod == "Array/")
					tsType = $"Array<{tsType}>";
				else
					return "any";
			}

			return tsType;
		}

	}
}
