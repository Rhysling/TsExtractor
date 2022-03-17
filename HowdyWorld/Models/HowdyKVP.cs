using System.Collections.Generic;

namespace HowdyWorld.Models;

[TypeScriptModel]
public class HowdyKVP
{
	public KeyValuePair<string, string> KvpStringString { get; set; }
	public KeyValuePair<int, string> KvpIntString { get; set; }
}
