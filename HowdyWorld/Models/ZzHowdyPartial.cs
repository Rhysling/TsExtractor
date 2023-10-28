using System;
using System.Collections.Generic;

namespace HowdyWorld.Models;

public partial class HowdyPartial
{
	public double ThirdValue { get; set; }
	public byte[] FourthByteArray { get; set; } = Array.Empty<byte>();
	public List<string> FifthStringList { get; set; } = new();
}
