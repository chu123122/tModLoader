using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terraria.DataStructures;
using Terraria.GameContent.Items;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.ModLoader;

[TestClass]
public class MigrationCombatTests
{
	[ClassInitialize]
	public static void Initialize(TestContext context)
	{
		Program.SavePath = ".";
		Main.rand = new UnifiedRandom(1458);
	}

	[DataTestMethod]
	[DataRow(false)]
	[DataRow(true)]
	public void FirecrackerMultipliesCombinedTagsRegardlessOfOrder(bool reverse)
	{
		Player player = new();
		Projectile minion = new() { type = 864, owner = 0, minion = true };
		NPC target = new() { whoAmI = 0 };
		WhipTagEffect[] effects = { new() { TagDamage = 3 }, new() { TagDamage = 5 } };
		if (reverse)
			Array.Reverse(effects);
		var changes = TagDamageChanges.None;
		foreach (var effect in effects)
			effect.ModifyTaggedHit(player, minion, target, ref changes);
		new WhipTagEffect_Firecracker().ModifyProcHit(player, minion, target, ref changes);
		Assert.AreEqual(8, changes.AddedBaseDamage);
		Assert.AreEqual(5, changes.HighestAddedBaseDamage);
		Assert.AreEqual(2.75f, changes.TotalDamageMultiplier);
		NPC.HitModifiers modifiers = new();
		modifiers.Defense.Base = 20;
		changes.ApplyTo(ref modifiers, minion.type, 100, bonusTagDamage: 2);
		// Vanilla: floor((8 + 2) * .669) = 6; floor((100 + 6) * 2.75) = 291.
		var hit = modifiers.ToHitInfo(100, false, 0);
		Assert.AreEqual(100, hit.SourceDamage); // Procs must not multiply tags or their own multiplier again.
		Assert.AreEqual(281, hit.Damage);
	}

	[TestMethod]
	public void UntaggedHitGetsNoTagBonus()
	{
		NPC.HitModifiers modifiers = new();
		var changes = TagDamageChanges.None;
		changes.ApplyTo(ref modifiers, 864, 100, bonusTagDamage: 100);
		Assert.AreEqual(100, modifiers.GetDamage(100, false));
	}

	[DataTestMethod]
	[DataRow(0, false, 0)]
	[DataRow(200, false, 188)]
	[DataRow(200, true, 200)]
	public void VanillaManaPaymentAndDryCasting(int mana, bool freeCast, int expected)
	{
		Player player = new() { statMana = mana, manaCost = 0.6f };
		Assert.IsTrue(player.CheckMana(20, pay: !freeCast, blockQuickMana: true));
		if (mana == 0) {
			Assert.AreEqual(0, player.statMana);
			Assert.IsTrue(player.slowMagicUse);
			Assert.AreEqual(25, player.GetSlowMagicUseTime(10));
			Assert.AreEqual(0.4f, player.GetSlowMagicUseRate());
		}
		else {
			Assert.AreEqual(expected, player.statMana);
			Assert.IsFalse(player.slowMagicUse);
		}
	}

	[TestMethod]
	public void ManaRegeneratesDuringCasting()
	{
		Player player = new() { statMana = 0, statManaMax2 = 200, itemAnimation = 30, manaRegenDelay = 3 };
		for (int i = 0; i < 120; i++)
			player.UpdateManaRegen();
		Assert.IsTrue(player.statMana > 0);
		Assert.IsTrue(player.statMana < player.statManaMax2);
	}

	[DataTestMethod]
	[DataRow(ItemID.LockBox, ItemID.SilverBracer, 0.25f)]
	[DataRow(ItemID.GolemBossBag, ItemID.MobiusStrip, 1f / 6)]
	[DataRow(ItemID.FishronBossBag, ItemID.Kraken, 1f / 7)]
	public void NewContainerRewardsHaveVanillaDropRates(int container, int item, float expected)
	{
		ItemDropDatabase db = new();
		string method = container == ItemID.LockBox ? "RegisterLockbox" : "RegisterBossBags";
		typeof(ItemDropDatabase).GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(db, null);
		List<DropRateInfo> rates = new();
		foreach (var rule in db.GetRulesForItemID(container))
			rule.ReportDroprates(rates, new DropRateInfoChainFeed(1));
		var actual = rates.Where(r => r.itemId == item).Select(r => r.dropRate).ToArray();
		Assert.IsTrue(actual.Length > 0);
		foreach (float rate in actual)
			Assert.AreEqual(expected, rate, 0.00001f);
	}

	[TestMethod]
	public void ArmorBonusesUseGenericStats()
	{
		// These vanilla sets increase all damage / crit, including inherited damage classes.
		Player player = new();
		var before = player.GetTotalDamage(DamageClass.Melee).ApplyTo(100);
		ArmorSetBonuses.Benefits.CrystalAssassin(player);
		Assert.AreEqual(before + 10f, player.GetTotalDamage(DamageClass.Melee).ApplyTo(100), 0.001f);
		Assert.AreEqual(14f, player.GetTotalCritChance(DamageClass.Ranged));
	}
	[DataTestMethod]
	[DataRow(1f, 143)]
	[DataRow(1.5f, 215)]
	public void NanoBulletsBoostWeaponAndAmmoDamage(float multiplier, int expected)
	{
		Player player = new();
		player.GetDamage(DamageClass.Ranged) *= multiplier;
		Item weapon = new() { type = ItemID.Minishark, damage = 100, DamageType = DamageClass.Ranged, useAmmo = AmmoID.Bullet, shoot = ProjectileID.Bullet };
		player.inventory[54] = new Item { type = ItemID.NanoBullet, damage = 15, DamageType = DamageClass.Ranged, ammo = AmmoID.Bullet, shoot = 285, stack = 9999 };
		Assert.IsTrue(player.PickAmmo(weapon, out int projectile, out _, out int damage, out _, out _, dontConsume: true));
		Assert.AreEqual(285, projectile);
		Assert.AreEqual(expected, damage);
		Assert.AreEqual(9999, player.inventory[54].stack);
	}

	[TestMethod]
	public void SharedFavoritedArmorIsUsedByArmorQueries()
	{
		Player player = new();
		var previousLocalPlayer = Main.player[Main.myPlayer];
		Main.player[Main.myPlayer] = player;
		try {
		Item sharedHelmet = new() { type = ItemID.CopperHelmet, headSlot = 1, stack = 1, favorited = true };
		player.Loadouts[1].Armor[0] = sharedHelmet;
		Assert.AreSame(sharedHelmet, player.GetEffectiveArmor(0, out int? loadout));
		Assert.AreEqual(1, loadout);
		Assert.AreEqual(ItemID.CopperHelmet, new ArmorSetBonus.QueryContext(player).HeadItem);
		Item ownHelmet = new() { type = ItemID.IronHelmet, headSlot = 2, stack = 1 };
		player.armor[0] = ownHelmet;
		Assert.AreSame(ownHelmet, player.GetEffectiveArmor(0));
		}
		finally { Main.player[Main.myPlayer] = previousLocalPlayer; }
	}

	[TestMethod]
	public void ChannelledWeaponManaOverloadSupportsDryCasting()
	{
		Player player = new() { statMana = 0 };
		Item item = new() { mana = 20, DamageType = DamageClass.Magic };
		Assert.IsTrue(player.CheckMana(item, pay: true, blockQuickMana: true));
		Assert.IsTrue(player.slowMagicUse);
		Assert.AreEqual(0, player.statMana);
		player.statMana = 100;
		Assert.IsTrue(player.CheckMana(item, pay: true));
		Assert.IsFalse(player.slowMagicUse);
		Assert.AreEqual(80, player.statMana);
	}

	[TestMethod]
	public void DryCastingAnimationMatchesSlowerUseTime()
	{
		Player player = new() { slowMagicUse = true };
		Item item = new() { mana = 20, useTime = 20, useAnimation = 20, DamageType = DamageClass.Magic };
		player.ApplyItemAnimation(item);
		Assert.AreEqual(50, player.itemAnimationMax);
		Assert.AreEqual(0.4f, player.GetSlowMagicUseRate());
	}

	[DataTestMethod]
	[DataRow(ItemID.WoodenCrate, ItemID.PoisonBarb)]
	[DataRow(ItemID.WoodenCrateHard, ItemID.PoisonBarb)]
	[DataRow(ItemID.FrozenCrate, ItemID.GlacierFang)]
	[DataRow(ItemID.FrozenCrateHard, ItemID.GlacierFang)]
	public void NewCrateRewardsArePresent(int container, int reward)
	{
		ItemDropDatabase db = new();
		typeof(ItemDropDatabase).GetMethod("RegisterCrateDrops", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(db, null);
		List<DropRateInfo> rates = new();
		foreach (var rule in db.GetRulesForItemID(container))
			rule.ReportDroprates(rates, new DropRateInfoChainFeed(1));
		Assert.IsTrue(rates.Any(rate => rate.itemId == reward && rate.dropRate > 0));
	}

}
