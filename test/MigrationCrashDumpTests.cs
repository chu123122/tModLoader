using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terraria.Utilities;

namespace Terraria.ModLoader;
[TestClass]
public class MigrationCrashDumpTests
{
	[TestMethod]
	public void LocalBranchVersionProducesValidDumpFileName()
	{
		string name = (string)typeof(CrashDump).GetMethod("CreateDumpName", BindingFlags.Static | BindingFlags.NonPublic)!.Invoke(null, null)!;
		Assert.AreEqual(name, Path.GetFileName(name));
		Assert.IsFalse(name.Any(Path.GetInvalidFileNameChars().Contains));
		Assert.IsTrue(name.EndsWith(".dmp"));
	}
}
