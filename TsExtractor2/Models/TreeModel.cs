using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsExtractor2.Utilities;

namespace TsExtractor2.Models
{
	public class TreeModel
	{
		private readonly SyntaxTree tree;
		private readonly SyntaxNode rootNode;
		private readonly SemanticModel semModel;
		private readonly string filePath;
		private List<ClassModel> classModels;

		public TreeModel(SyntaxTree tree, Compilation compilation)
		{
			this.tree = tree;
			semModel = compilation.GetSemanticModel(tree);
			filePath = tree.FilePath;
			rootNode = tree.GetRootAsync().Result;
		}

		public SyntaxTree Tree => tree;
		public SyntaxNode RootNode => rootNode;
		public SemanticModel SemModel => semModel;
		public string FilePath => filePath;

		public List<ClassModel> ExtractClassModels()
		{
			if (classModels != null) return classModels;

			classModels = new List<ClassModel>();

			var allClasses = rootNode.DescendantNodes().OfType<ClassDeclarationSyntax>();
			//.Where(x => x.AttributeLists.Any(y => y.Attributes.Any(a => a.Name.ToString().StartsWith("TypeScriptModel"))));

			foreach (var c in allClasses)
			{
				bool isTypeScriptModel = false;
				bool isInterface = false;
				string[] excludedPropList = Array.Empty<string>();

				var atr = c.AttributeLists.SelectMany(a => a.Attributes).FirstOrDefault(b => b.Name.ToString().StartsWith("TypeScriptModel"));

				isTypeScriptModel = atr is not null;

				if (isTypeScriptModel && atr.ArgumentList is not null)
				{
					var args = atr.ArgumentList.Arguments.ToList();

					var exclAtr = args.FirstOrDefault(a => a.NameEquals.Name.ToString() == "ExcludeMembersByName");
					if (exclAtr is not null)
					{
						excludedPropList = exclAtr.Expression.ToString().Replace("\"", "").Split(',');
					}

					var interfaceAtr = args.FirstOrDefault(a => a.NameEquals.Name.ToString() == "IsInterface");
					if (interfaceAtr is not null)
					{
						isInterface = interfaceAtr.Expression.ToString().Replace("\"", "") == "true";
					}
					
				}

				var cl = new ClassModel
				{
					FilePath = filePath,
					ClassName = c.Identifier.ValueText,
					NamespaceName = RosHelpers.GetNamespaceName(c),
					IsTypescriptModel = isTypeScriptModel,
					IsInterface = isInterface,
					// Look for base class -- add properties
					BaseTypeName = ((INamedTypeSymbol)semModel.GetDeclaredSymbol(c)).BaseType?.Name
				};

				var props = c.Members.OfType<PropertyDeclarationSyntax>();
				cl.PropertyList = props.Select(p => new PropModel {
					PropName = p.Identifier.ValueText,
					PropTypes = RosHelpers.GetInfoFromTypeSyntax(semModel, p.Type)
				}).ToList();

				if (excludedPropList.Any())
				{
					cl.PropertyList = cl.PropertyList.Where(a => !excludedPropList.Contains(a.PropName)).ToList();
				}

				classModels.Add(cl);
			}

			return classModels;
		}
	}
}
