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
		private SyntaxTree tree;
		private SyntaxNode rootNode;
		private SemanticModel semModel;
		private string filePath;
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
				var cl = new ClassModel
				{
					FilePath = filePath,
					ClassName = c.Identifier.ValueText,
					NamespaceName = RosHelpers.GetNamespaceName(c),
					IsTypescriptModel = c.AttributeLists.Any(a => a.Attributes.Any(b => b.Name.ToString().StartsWith("TypeScriptModel")))
				};

				// Look for base class -- add properties
				cl.BaseTypeName = ((INamedTypeSymbol)semModel.GetDeclaredSymbol(c)).BaseType?.Name;

				var props = c.Members.OfType<PropertyDeclarationSyntax>();
				cl.PropertyList = props.Select(p => new PropModel {
					PropName = p.Identifier.ValueText,
					PropTypes = RosHelpers.GetInfoFromTypeSyntax(semModel, p.Type)
				}).ToList();

				classModels.Add(cl);
			}

			return classModels;
		}
	}
}
