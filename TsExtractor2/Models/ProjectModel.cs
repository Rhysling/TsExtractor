using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsExtractor2.Models
{
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

		public List<ClassModel> ClassList => treeList.SelectMany(t => t.ExtractClassModels()).ToList();
	}
}
