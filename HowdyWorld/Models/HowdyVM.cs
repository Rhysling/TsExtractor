using HowdyWorld.Db;
using System;
using System.Collections.Generic;

namespace HowdyWorld.Models
{
	[TypeScriptModel]
	public class HowdyVM
	{
		public int Id { get; set; }
		public string NameStr { get; set; }
		public float ValFloat { get; set; }
		public double ValDouble { get; set; }
		public decimal ValDecimal { get; set; }
		public byte ValByte { get; set; }
		public char ValChar { get; set; }
		public bool ValBool { get; set; }
		public Guid ValGuid { get; set; }

		public long? ValNullableLong { get; set; }

		public int[] ValArrayInt { get; set; }
		public string[] ValArrayString { get; set; }
		public char[] ValArrayChar { get; set; }
		public int?[] ValArrayNullableInt { get; set; }
		public NameValueItem[] ValArrayNVI { get; set; }


		public HowdyDetail ValHowdyDetail { get; set; }
		public List<NameValueItem> ValListNVI { get; set; }
		public List<string> ValListString { get; set; }
		public List<int?> ValListNullableInt { get; set; }
		public List<int?[]> ValListArrayNullableInt { get; set; }
		public List<List<NameValueItem>> ValListListNVI { get; set; }

		public int ValIntFromArrow => 23;

		public FromExternal ValFromExternal { get; set; }
		public NonTsClass ValNonTsClass { get; set; }
	}
}