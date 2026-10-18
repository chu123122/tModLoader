using Mono.Cecil;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

if (args.Length != 3) {
    Console.Error.WriteLine("Usage: jit-optimizer input.dll output.dll runtime-directory");
    return 1;
}
string input = Path.GetFullPath(args[0]), output = Path.GetFullPath(args[1]);
if (string.Equals(input, output, StringComparison.OrdinalIgnoreCase))
    throw new ArgumentException("Input and output must be different files.");
using var resolver = new DefaultAssemblyResolver();
resolver.AddSearchDirectory(Path.GetDirectoryName(input));
resolver.AddSearchDirectory(Path.GetDirectoryName(typeof(object).Assembly.Location));
resolver.AddSearchDirectory(Path.GetFullPath(args[2]));
string libraries = Path.Combine(args[2], "Libraries");
if (Directory.Exists(libraries))
    foreach (string dir in Directory.EnumerateFiles(libraries, "*.dll", SearchOption.AllDirectories).Select(Path.GetDirectoryName).Distinct())
        resolver.AddSearchDirectory(dir);
bool symbols = File.Exists(Path.ChangeExtension(input, ".pdb"));
using var assembly = AssemblyDefinition.ReadAssembly(input, new ReaderParameters { AssemblyResolver = resolver, ReadSymbols = symbols });
IEnumerable<TypeDefinition> Types(IEnumerable<TypeDefinition> types) => types.SelectMany(t => new[] { t }.Concat(Types(t.NestedTypes)));
string Bodies(AssemblyDefinition a) => string.Join("\n", Types(a.MainModule.Types).SelectMany(t => t.Methods).Where(m => m.HasBody).Select(m =>
    m.FullName + ":" + m.Body.InitLocals + ":" + string.Join(",", m.Body.Variables.Select(v => v.VariableType.FullName)) + ":" +
    string.Join(";", m.Body.Instructions.Select(i => i.OpCode + " " + (i.Operand?.ToString() ?? "")))));
string before = Bodies(assembly);
var attribute = assembly.CustomAttributes.Single(c => c.AttributeType.FullName == "System.Diagnostics.DebuggableAttribute");
if (attribute.ConstructorArguments.Count != 1)
    throw new NotSupportedException("Expected the DebuggingModes constructor.");
int oldFlags = Convert.ToInt32(attribute.ConstructorArguments[0].Value);
int newFlags = oldFlags & ~(256 | 4);
attribute.ConstructorArguments[0] = new CustomAttributeArgument(attribute.ConstructorArguments[0].Type, newFlags);
Directory.CreateDirectory(Path.GetDirectoryName(output)!);
assembly.Write(output, new WriterParameters { WriteSymbols = symbols });
using var result = AssemblyDefinition.ReadAssembly(output, new ReaderParameters { AssemblyResolver = resolver });
if (Bodies(result) != before) throw new Exception("IL/locals changed; discard the output.");
Console.WriteLine(JsonSerializer.Serialize(new { oldFlags, newFlags, ILAndLocalsUnchanged = true,
    ILSha256 = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(before))), symbols }));
return 0;
