using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Terraria.ModLoader;

[TestClass]
public class MigrationItemStateTests
{
	[TestMethod]
	public void ArmorPenetrationPropertyUsesTheNativePrefixField()
	{
		Item item = new();
		item.armorPenetration = 17;
		Assert.AreEqual(17, item.ArmorPenetration);
		item.ArmorPenetration = 23;
		Assert.AreEqual(23, item.armorPenetration);
		item.ArmorPenetration = -4;
		Assert.AreEqual(0, item.armorPenetration);
	}

	[TestMethod]
	public void VoiceAccessoryDefaultsPreserveExtendedSlotIDs()
	{
		Item item = new();
		item.DefaultToVoiceOverrideAccessory(500);
		Assert.AreEqual(500, item.voiceSlot);
		Assert.IsTrue(item.accessory && item.vanity);
	}
}
