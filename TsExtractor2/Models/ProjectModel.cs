using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace TsExtractor2.Models;

public class ProjectModel
{
	private readonly string projectName;
	private readonly Compilation compilation;
	private readonly List<TreeModel> treeList;

	public ProjectModel(string projectName, Compilation compilation)
	{
		this.projectName = projectName;
		this.compilation = compilation;

		treeList = compilation.SyntaxTrees.Select(t => new TreeModel(t, compilation)).ToList();
	}

	public string ProjectName => projectName;
	public Compilation Compilation => compilation;
	public List<TreeModel> TreeList => treeList;

	public List<ClassModel> ClassList
	{ 
		get
		{
			var cl = treeList.SelectMany(t => t.ExtractClassModels()).ToList();

			// Merge any partial classes

			var clp = cl.Where(a => a.IsTypescriptModel && a.IsPartial);

			foreach (var tp in clp)
			{
				tp.PropertyList.AddRange(cl.Where(a => !a.IsTypescriptModel && a.ClassName == tp.ClassName).SelectMany(a => a.PropertyList));
			}

			return cl;
		}
	}
}
