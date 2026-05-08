using System;
using TsExtractor2.Models;
using TsExtractor2.Operations;
using TsExtractor2.Utilities;
using TsExtractor2.Writers;

namespace TsExtractor2
{
	class Program
	{
		static void Main(string[] args)
		{
			RunForProduction(args);
			//RunForTesting();

			Console.WriteLine("Done.");
			Console.ReadKey();
		}


		static void RunForTesting()
		{
			// FOR TESTING:
			//string sourcePath = @"D:\UserData\Documents\Visual Studio 2017\Projects\CrosseratorWebpack\Crosserator.sln";
			//string outPath = $@"D:\yy\tp2\Tsx_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";
			//args = new string[] { $"OutPath={outPath}", $"SourcePath={sourcePath}" };

			//string sourcePath = @"D:\UserData\Documents\AppDev\BotanicaStoreBack\BotanicaStoreBack.sln";
			//string[] excludeProjectNames = { "BotanicaStoreBack.Runner" };

			//string sourcePath = @"D:\UserData\Documents\AppDev\FeederBack\FeederBack.sln";
			//string[] excludeProjectNames = { "CloudantDb", "FeederBack.Tests", "Logger", "UtilitiesMaster" };

			//string sourcePath = @"D:\UserData\Documents\AppDev\NextSemiBack\NextSemiBack.sln";
			//string[] excludeProjectNames = { "CloudantDb", "FeederBack.Tests", "Logger", "UtilitiesMaster" };

			//string sourcePath = @"D:\aa-dev\AppDev\TsExtractor2\HowdyWorld\HowdyWorld.csproj";

			string sourcePath = @"D:\aa-dev\AppDev\BotanicaStoreBack\BotanicaStoreBack.sln";
			string[] excludeProjectNames = ["BotanicaStoreBack.ColorCards", "BotanicaStoreBack.Db", "BotanicaStoreBack.Runner"];

			//string[] excludeProjectNames = [];

			//string sourcePath = @"D:\UserData\Documents\AppDev\TsExtractor2\TsExtractor2.sln";
			//string sourcePath = @"C:\Users\B\Documents\AppDev\TsExtractor2\TsExtractor2.sln";
			//string[] excludeProjectNames = { "TsExtractor2" };


			//string outPath = $@"D:\yy\tp2\Tsx_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
			string outPath = $@"D:\yy-util\tp1\Tsx_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

			// Get compilations from MSB Workspace

			string header = MsbWorkspace.InitWorkspace();
			SolutionModel solutionModel = MsbWorkspace.GetCompilations(sourcePath, excludeProjectNames);

			//var writer = new LogFileWriter(outPath, header, classList);
			var writer = new DtsWriter(outPath, header, solutionModel);
			writer.Write();

			Console.WriteLine("Dts file created.");
			Console.WriteLine(outPath);
			Console.ReadKey();
		}

		static void RunForProduction(string[] args)
		{
			ArgValues.LoadFromFile();
			ArgValues.LoadFromArgs(args);

			ArgumentNullException.ThrowIfNull(ArgValues.SourcePath);
			ArgumentNullException.ThrowIfNull(ArgValues.OutPath);

			// Get compilations from MSB Workspace
			string header = MsbWorkspace.InitWorkspace();
			SolutionModel solutionModel = MsbWorkspace.GetCompilations(ArgValues.SourcePath, ArgValues.ExcludeProjectNames);
			var writer = new DtsWriter(ArgValues.OutPath, header, solutionModel);
			writer.Write();
		}

	}
}
