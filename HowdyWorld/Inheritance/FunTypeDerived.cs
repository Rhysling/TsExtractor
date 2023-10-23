namespace HowdyWorld.Inheritance
{
	[TypeScriptModel(IsInterface = false)]
	public class FunTypeDerived : FunTypeBase
	{
		public int IdIntDerived { get; set; }
		public string StringDerived { get; set; }
	}
}
