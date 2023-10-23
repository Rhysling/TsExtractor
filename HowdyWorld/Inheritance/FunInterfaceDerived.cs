namespace HowdyWorld.Inheritance
{
	[TypeScriptModel(IsInterface = true)]
	public class FunInterfaceDerived : FunInterfaceBase
	{
		public int IdIntDerived { get; set; }
		public string StringDerived { get; set; }
	}
}
