# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What This Is

TsExtractor2 is a console application that extracts C# types from a solution or project and generates TypeScript definition files (`.d.ts` format). Only classes marked with `[TypeScriptModel]` are extracted.

## Build & Run

```bash
# Build
dotnet build TsExtractor2.sln

# Run (targets .NET 10)
dotnet run --project TsExtractor2/TsExtractor2.csproj
```

**Testing vs Production mode:** `Program.cs` has two entry points. Currently `RunForTesting()` is active (hardcoded paths). To use production mode with CLI args, swap the comment in `Main()` to call `RunForProduction(args)`.

**Production CLI args:**
- `SourcePath=<path>` — path to `.sln` or `.csproj`
- `OutPath=<path>` — output file path
- `ExcludeProjectNames=<comma-separated>` — projects to skip

## Architecture

```
Program
  └─ MsbWorkspace.InitWorkspace()       → registers MSBuild
  └─ MsbWorkspace.GetCompilations()     → returns SolutionModel
       └─ ProjectModel (per project)
            └─ TreeModel (per syntax tree)
                 └─ ClassModel (per [TypeScriptModel] class)
                      └─ PropModel + PropTypeCollection (per property)
  └─ DtsWriter.Write()                  → generates .d.ts file
```

**Key classes:**

| Class | File | Role |
|---|---|---|
| `MsbWorkspace` | `Operations/MsbWorkspace.cs` | Loads MSBuild, opens solution/project via Roslyn workspaces |
| `SolutionModel` | `Models/SolutionModel.cs` | Top-level container; iterates projects and collects all `ClassModel`s |
| `ProjectModel` | `Models/ProjectModel.cs` | Wraps a Roslyn `Compilation`; walks syntax trees |
| `TreeModel` | `Models/TreeModel.cs` | Wraps a single syntax tree; extracts `ClassModel`s using `SemanticModel` |
| `ClassModel` | `Models/ClassModel.cs` | One extracted C# class: name, namespace, properties, base type, interface flag |
| `PropModel` | `Models/PropModel.cs` | A single property: name + `PropTypeCollection` |
| `PropTypeCollection` | `Models/PropTypeCollection.cs` | Recursive type structure for generics/arrays/nullables |
| `Mappings` | `Operations/Mappings.cs` | Static C# → TypeScript type name dictionary |
| `DtsWriter` | `Writers/DtsWriter.cs` | Renders all `ClassModel`s to TypeScript `type`/`interface` declarations |
| `RosHelpers` | `Utilities/RosHelpers.cs` | Roslyn semantic analysis helpers |

## The `[TypeScriptModel]` Attribute

Defined in `HowdyWorld/TypescriptModelAttribute.cs`. Classes without this attribute are ignored.

```csharp
[TypeScriptModel(
    ExcludeMembersByName = "Prop1,Prop2",  // skip these properties
    IsInterface = true                      // emit as `interface IName` instead of `type Name`
)]
public class MyModel { }
```

## Output Behavior

- `IsInterface = false` (default) → `type ClassName = { prop: type; };`
- `IsInterface = true` → `interface IClassName { prop: type; }` with `extends IBase` if inherited
- Property names are camelCased
- Partial classes are merged before output
- Output is grouped by namespace with comments

## Test Project

`HowdyWorld/` is the sample project used for testing extraction. It contains representative models covering nullables, generics, lists, inheritance, and partial classes. Point `sourcePath` in `RunForTesting()` at `HowdyWorld.csproj` to test against it.
