# TsExtractor2

A console application that extracts C# types from a Visual Studio solution or project and generates TypeScript definition files (`.d.ts`). It uses the Roslyn compiler API and MSBuild Locator to semantically analyze C# source code without requiring a separate build step.

---

## How It Works

Only classes explicitly marked with the `[TypeScriptModel]` attribute are extracted. The tool:

1. Locates a Visual Studio instance to initialize MSBuild.
2. Opens the target solution or project file via the Roslyn Workspace API.
3. Walks every syntax tree in each project, using the semantic model to resolve types.
4. Merges partial class declarations.
5. Maps C# types to TypeScript equivalents.
6. Writes a single `.d.ts` output file grouped by namespace.

---

## Solution Structure

```
TsExtractor2.sln
├── TsExtractor2/          Main console application (.NET 10)
├── HowdyWorld/            Test fixture — sample C# models (.NET 10)
└── HowdyWorld.Db/         Test fixture — secondary namespace (.NET 7)
```

### TsExtractor2 — Main Project

```
TsExtractor2/
├── Program.cs                     Entry point; RunForTesting / RunForProduction modes
├── settings.txt                   Tab-delimited config (auto-loaded from app directory)
├── Models/
│   ├── SolutionModel.cs           Aggregates all ProjectModels; computes final TsClasses list
│   ├── ProjectModel.cs            Wraps a Roslyn Compilation; merges partial classes
│   ├── TreeModel.cs               Extracts ClassModels from one SyntaxTree via SemanticModel
│   ├── ClassModel.cs              Represents one extracted C# class or interface
│   ├── PropModel.cs               Represents one property (name + type)
│   └── PropTypeCollection.cs      Recursive structure describing composed types
├── Operations/
│   ├── MsbWorkspace.cs            Registers MSBuild; opens solution/project via Roslyn
│   └── Mappings.cs                Dictionary: C# type names → TypeScript type names
├── Utilities/
│   ├── ArgValues.cs               Loads config from settings.txt and CLI arguments
│   ├── RosHelpers.cs              Roslyn semantic analysis helpers
│   ├── StringExt.cs               ProperCase() / CamelCase() string extensions
│   └── Utils.cs                   FindSlnPath() — searches parent directories for .sln
└── Writers/
    ├── DtsWriter.cs               Renders ClassModels to .d.ts output
    └── LogFileWriter.cs           Debug log writer (not used in production flow)
```

---

## The `[TypeScriptModel]` Attribute

This attribute must be present on any C# class you want extracted. It lives in the **target solution** (not in TsExtractor2 itself) — you add it to the project being analyzed.

```csharp
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class TypeScriptModelAttribute : Attribute
{
    // Comma-separated property names to omit from output
    public string ExcludeMembersByName { get; set; } = "";

    // Comma-separated property names to mark as optional (?) in output
    public string OptionalMembersByName { get; set; } = "";

    // Emit as TypeScript interface instead of type alias
    public bool IsInterface { get; set; } = false;
}
```

**Example usage:**

```csharp
[TypeScriptModel(IsInterface = true, ExcludeMembersByName = "InternalToken,AuditLog")]
public class ContactMessage
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? InternalToken { get; set; }   // excluded
    public string? AuditLog { get; set; }         // excluded
}
```

---

## Configuration

Configuration is read from two sources, in priority order (CLI arguments override `settings.txt`):

### `settings.txt`

A tab-delimited file in the TsExtractor2 application directory:

```
sourcePath	D:\path\to\YourSolution.sln
outPath	D:\output\your-types.d.ts
excludeProjectNames	ProjectA,ProjectB,ProjectC.Tests
```

| Key | Description |
|-----|-------------|
| `sourcePath` | Path to a `.sln` or `.csproj` file |
| `outPath` | Path for the generated `.d.ts` output file |
| `excludeProjectNames` | Comma-separated project names to skip during extraction |

### CLI Arguments

Arguments can be passed on the command line using `Key=Value` syntax:

```
TsExtractor2.exe SourcePath=C:\MySolution.sln OutPath=D:\out\types.d.ts ExcludeProjectNames=Foo,Bar
```

---

## Running the Tool

### Production Mode

```bash
dotnet run --project TsExtractor2/TsExtractor2.csproj
```

Reads `settings.txt` (and any CLI args) to determine `sourcePath`, `outPath`, and exclusions.

### Testing Mode

`Program.cs` contains a `RunForTesting()` method with hardcoded paths pointing at the HowdyWorld test fixtures. To activate it, comment out `RunForProduction(args)` and uncomment `RunForTesting()` in `Main()`:

```csharp
static async Task Main(string[] args)
{
    // RunForTesting();
    await RunForProduction(args);
}
```

---

## Type Mapping

| C# Type | TypeScript Type | Notes |
|---------|-----------------|-------|
| `int`, `float`, `double`, `decimal` | `number` | |
| `long` | `error-long not supported in js` | Intentional error string — unsupported |
| `string`, `char`, `byte`, `Guid`, `DateTime` | `string` | |
| `bool` | `boolean` | |
| `List<T>`, `T[]` | `Array<T>` | Both map to Array |
| `Nullable<T>` / `T?` | `INullable<T>` | Custom wrapper emitted in output header |
| `[TypeScriptModel]` class | `ClassName` or `IClassName` | Interface prefix applied when `IsInterface = true` |
| Everything else | `any` | |

`long` is deliberately mapped to an error string to make unsafe conversions visible in TypeScript tooling.

The output file always includes this header type to support nullables:

```typescript
type INullable<T> = T | null | undefined;
```

---

## Output Format

The generated file groups declarations by namespace. Property names are camelCased.

```typescript
// Using MSBuild ver. 17.x.x
// Generated - 2026/05/18-10:45:00

// SOLUTION: MySolution

// PROJECTS SEARCHED:
// MyProject.Api
// MyProject.Shared

type INullable<T> = T | null | undefined;

// NAMESPACE: MyProject.Shared.Models

interface IContactMessage {
  name: string;
  email: string;
  message: string;
}

type HowdyDetail = {
  howdyId: number;
  sayHowdy: string;
};

// NAMESPACE: MyProject.Api.ViewModels

type OrderVM = {
  orderId: number;
  items: Array<OrderItemVM>;
  createdAt: string;
  discount: INullable<number>;
};
```

**Inheritance:** Base class properties are pulled into the derived type's declaration. If both the base and derived types are marked `[TypeScriptModel]`, the output uses `extends`:

```typescript
interface IFunInterfaceDerived extends IFunInterfaceBase {
  derivedProp: string;
}
```

**Partial classes:** All partial declarations for a class are merged before output, producing a single combined type definition.

---

## Test Fixtures

The `HowdyWorld` and `HowdyWorld.Db` projects exist solely to verify that C# language features are correctly translated. They are not referenced by TsExtractor2 at compile time — they serve as target solutions for `RunForTesting()`.

### HowdyWorld

| Class | Purpose |
|-------|---------|
| `HowdyVM` | Comprehensive test: all primitive types, nullables, arrays, lists, nested types |
| `HowdyNullables` | Nullable value types and nullable lists |
| `HowdyDetail` | Simple model; includes a nested class in a sub-namespace |
| `HowdyPartial` | Partial class split across two files — tests merging |
| `NameValueItem` | Generic key-value model |
| `HowdyDerived` | Inherits `HowdyBase` (not marked `[TypeScriptModel]`) — tests base-property inheritance |
| `ContactMessage` | `IsInterface = true`; tests `ExcludeMembersByName` and `required` fields |
| `HowdyLists` | Properties containing lists of complex types |
| `HowdyKVP` | `KeyValuePair<T, U>` — tests generic pair mapping |
| `FunInterfaceDerived` | Interface inheritance chain |
| `FunTypeDerived` | Type alias inheritance chain |

### HowdyWorld.Db

| Class | Purpose |
|-------|---------|
| `FromExternal` | References `NonTsClass` (not marked `[TypeScriptModel]`) — tests fallback to `any` |

`HowdyWorld.Db` targets .NET 7 to verify that the tool works across different target framework versions within the same solution.

---

## Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.CodeAnalysis` | 5.3.0 | Roslyn core |
| `Microsoft.CodeAnalysis.CSharp` | 5.3.0 | C# parser and semantic model |
| `Microsoft.CodeAnalysis.CSharp.Workspaces` | 5.3.0 | Solution/project workspace |
| `Microsoft.CodeAnalysis.Workspaces.MSBuild` | 5.3.0 | MSBuild project loading |
| `Microsoft.Build.Locator` | 1.11.2 | Locates the installed Visual Studio / MSBuild instance |
| `Microsoft.Build.Framework` | 17.11.48 | MSBuild types (excluded from runtime output) |

A Visual Studio or Build Tools installation is required at runtime so that `Microsoft.Build.Locator` can find MSBuild.
