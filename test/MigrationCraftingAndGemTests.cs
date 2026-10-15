using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terraria.ID;

namespace Terraria.ModLoader;

[TestClass]
public class MigrationCraftingAndGemTests
{
	[TestMethod]
	public void ReloadResetsEveryVanillaPlacementWand()
	{
		var fields = typeof(Terraria.GameContent.FlexibleTileWand).GetFields(BindingFlags.Static | BindingFlags.Public);
		var before = new System.Collections.Generic.Dictionary<FieldInfo, object>();
		foreach (var field in fields)
			if (field.FieldType == typeof(Terraria.GameContent.FlexibleTileWand))
				before.Add(field, field.GetValue(null)!);
		Terraria.GameContent.FlexibleTileWand.Reload();
		Assert.IsTrue(before.Count >= 30);
		foreach (var pair in before)
			Assert.AreNotSame(pair.Value, pair.Key.GetValue(null), pair.Key.Name + " retains the previous mod list's state");
	}

	[TestMethod]
	public void AllSevenGemStavesCombineWithAllSevenRobes()
	{
		Program.SavePath = ".";
		int[] shots = { 121, 122, 123, 124, 125, 126, 597 };
		int[] robes = { 1282, 1283, 1284, 1285, 1286, 1287, 4256 };
		var method = typeof(Player).GetMethod("PackGemStaffFeatures", BindingFlags.Instance | BindingFlags.NonPublic)!;
		for (int staff = 0; staff < shots.Length; staff++) {
			for (int robe = 0; robe < robes.Length; robe++) {
				Player player = new();
				player.armor[1] = new Item { type = robes[robe], bodySlot = 1, stack = 1 };
				var features = new Projectile.GemStaffFeatures((float)method.Invoke(player, new object[] { shots[staff] })!);
				bool[] actual = { features.Homing, features.AoeExplosion, features.SwirlTwins, features.FastThenSlow, features.ArPenSpread, features.BiggerHitbox, features.CanBounce };
				for (int effect = 0; effect < shots.Length; effect++)
					Assert.AreEqual(effect == staff || effect == robe, actual[effect], $"Staff {staff}, robe {robe}, effect {effect}");
				Assert.AreEqual(staff == robe, features.RepeatsGem);
			}
		}
	}
}
