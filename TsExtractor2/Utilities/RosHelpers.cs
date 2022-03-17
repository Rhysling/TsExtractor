using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsExtractor2.Models;

namespace TsExtractor2.Utilities
{
	public static class RosHelpers
	{
		public static PropTypeCollection GetInfoFromTypeSyntax(SemanticModel semanticModel, TypeSyntax typeSyntax)
		{
			var tsum = new PropTypeCollection();

			//SymbolInfo si;
			ISymbol sym = semanticModel.GetSymbolInfo(typeSyntax).Symbol;

			switch (typeSyntax)
			{
				case PredefinedTypeSyntax t:
					tsum.Category = "Predef";
					tsum.TypeName = sym.Name;
					tsum.SubType = null;
					break;

				case IdentifierNameSyntax t:
					tsum.Category = "Named";
					tsum.TypeName = sym?.Name ?? t.Identifier.Text;
					tsum.SubType = null;
					break;

				case ArrayTypeSyntax t:
					tsum.Category = "Array";
					tsum.TypeName = "Array/";
					tsum.SubType = t.ElementType;
					break;

				case NullableTypeSyntax t:
					tsum.Category = "Nullable";
					tsum.TypeName = "Nullable/"; //(sym is not null) ? sym.Name + "/" : "";
					tsum.SubType = t.ElementType;
					break;

				case GenericNameSyntax t:
					//var a = t.TypeArgumentList.Arguments[0] as IdentifierNameSyntax;

					tsum.Category = "Generic";
					tsum.TypeName = (sym is not null) ? sym.Name + "/" : "";
					tsum.SubType = t.TypeArgumentList.Arguments[0];
					break;

				default:
					tsum.Category = "Not Found";
					tsum.TypeName = "";
					tsum.SubType = null;
					break;
			}

			if (tsum.SubType != null)
				tsum.SubSummary = GetInfoFromTypeSyntax(semanticModel, tsum.SubType);

			return tsum;
		}


		public static string GetNamespaceName(SyntaxNode syntaxNode)
		{
			// set defaults
			string result = "";
			SyntaxNode currentNode = syntaxNode;
			if (currentNode == null) return "Refernce node missing";

			while (true)
			{
				currentNode = currentNode.Parent;
				if (currentNode == null) break;

				if (currentNode is NamespaceDeclarationSyntax ns)
				{
					result = ns.Name.ToFullString().TrimEnd('\r', '\n') + "." + result;
					break;
				}

				if (currentNode is FileScopedNamespaceDeclarationSyntax fns)
				{
					result = fns.Name.ToFullString().TrimEnd('\r', '\n') + "." + result;
					break;
				}

			}

			if (result.Length > 0)
				return result.Substring(0, result.Length - 1);
			else
				return "No namespace found";

		}

	}
}
