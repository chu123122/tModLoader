using System;
using Terraria.Audio;
using Terraria.ModLoader;

namespace Terraria.UI;

public partial class ItemSlot
{
	private static bool IsAccessoryContext(int context) => Math.Abs(context) is Context.EquipAccessory or Context.EquipAccessoryVanity;
	internal static bool IsModdedLoadoutShareContext(int context) => context is Context.ModdedAccessorySlot or Context.ModdedVanityAccessorySlot or Context.ModdedDyeSlot;
	internal static bool CanModdedLoadoutShare(int context, int slot) => IsModdedLoadoutShareContext(context) && AccessorySlotLoader.ModSlotPlayer(Main.LocalPlayer).CanLoadoutShare(context, slot);


	// Copy of Acc check, but runs hooks which take the local player as a context.
	/// <inheritdoc cref="AccCheck_ForPlayer(Player, Item[], Item, int)"/>
	internal static bool AccCheck_ForLocalPlayer(Item[] itemCollection, Item item, int slot) => AccCheck_ForPlayer(Main.LocalPlayer, itemCollection, item, slot);

	/// <summary>
	/// Checks if placing <paramref name="item"/> into index <paramref name="slot"/> of <paramref name="itemCollection"/> works or not. <paramref name="itemCollection"/> corresponds to the <paramref name="player"/>'s accessories. 
	/// <br/><br/> Checks vanilla incompatibility groups, modded accessory slots, and player-specific equipment hooks.
	/// <br/><br/> Returns true if can't equip and false if placing the accessory is ok. 
	/// </summary>
	internal static bool AccCheck_ForPlayer(Player player, Item[] itemCollection, Item item, int slot)
	{
		int modCount = AccessorySlotLoader.ModSlotPlayer(player).SlotCount;
		bool modded = slot >= player.armor.Length;
		bool targetVanity = modded ? slot >= player.armor.Length + modCount : slot >= 10;
		for (int i = 3; i < itemCollection.Length; i++) {
			if (i == slot || (i >= 10 && i < 13) || itemCollection[i].IsAir)
				continue;

			Item existing = itemCollection[i];
			int incompatibility = Terraria.ID.ItemID.Sets.AccessoryIncompatibilityType[item.type];
			// Identity and the vanilla incompatibility group apply across functional and vanity slots.
			if (existing.type == item.type || (incompatibility != -1 && incompatibility == Terraria.ID.ItemID.Sets.AccessoryIncompatibilityType[existing.type]))
				return true;

			bool existingVanity = i >= player.armor.Length ? i >= player.armor.Length + modCount : i >= 10;
			if (existingVanity == targetVanity && (!CanEquipBothAccessories(existing, item, targetVanity) || !ItemLoader.CanAccessoryBeEquippedWith(player, existing, item)))
				return true;
		}

		return !ItemLoader.CanEquipAccessory(player, item, modded ? slot - player.armor.Length : slot, modded);
	}

	private static Item QuickAccessorySwap(Player player, Item item, out bool success, out int targetSlot)
	{
		success = false;
		targetSlot = -1;
		var slots = AccessorySlotLoader.ModSlotPlayer(player);
		var loader = LoaderManager.Get<AccessorySlotLoader>();
		Item[] combined = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(player.armor, slots.exAccessorySlot));
		var candidates = new System.Collections.Generic.List<int>();
		int nativeOffset = item.vanity ? 13 : 3;
		for (int i = nativeOffset; i < nativeOffset + 7; i++)
			candidates.Add(i);
		int modOffset = player.armor.Length + (item.vanity ? slots.SlotCount : 0);
		for (int i = 0; i < slots.SlotCount; i++)
			candidates.Add(modOffset + i);

		bool CanUse(int index) {
			if (index < player.armor.Length)
				return player.IsItemSlotUnlockedAndUsable(index) && !AccCheck_ForPlayer(player, combined, item, index);

			int modIndex = index - player.armor.Length;
			int context = item.vanity ? Context.ModdedVanityAccessorySlot : Context.ModdedAccessorySlot;
			return loader.ModdedIsSpecificItemSlotUnlockedAndUsable(modIndex % slots.SlotCount, player, item.vanity)
				&& loader.CanPlayerAcceptItem(player, modIndex, item, context)
				&& !AccCheck_ForPlayer(player, combined, item, index);
		}

		foreach (int index in candidates) {
			if (!combined[index].IsAir && (!CanEquipBothAccessories(combined[index], item, item.vanity) || !ItemLoader.CanAccessoryBeEquippedWith(player, combined[index], item))) {
				// More than one conflict cannot be resolved by swapping a single slot.
				if (targetSlot != -1 || !CanUse(index))
					return item;
				targetSlot = index;
			}
		}

		if (targetSlot == -1) {
			foreach (int index in candidates) {
				if (combined[index].IsAir && CanUse(index)) { targetSlot = index; break; }
			}
		}
		if (!item.vanity) {
			int preferred = targetSlot >= player.armor.Length ? targetSlot : targetSlot >= 3 ? targetSlot - 3 : -1;
			// An existing conflict takes priority over default slot preferences.
			if (!candidates.Exists(i => !combined[i].IsAir && (!CanEquipBothAccessories(combined[i], item, false) || !ItemLoader.CanAccessoryBeEquippedWith(player, combined[i], item)))) {
				loader.ModifyDefaultSwapSlot(item, ref preferred);
				int preferredIndex = preferred >= player.armor.Length ? preferred : preferred >= 0 ? preferred + 3 : -1;
				if (candidates.Contains(preferredIndex) && CanUse(preferredIndex))
					targetSlot = preferredIndex;
			}
		}

		if (targetSlot == -1) {
			foreach (int index in candidates) {
				if (CanUse(index)) { targetSlot = index; break; }
			}
		}
		if (targetSlot == -1)
			return item;

		SoundEngine.PlaySound(7);
		if (targetSlot < player.armor.Length)
			SwapClearFavorite(ref item, ref player.armor[targetSlot]);
		else
			SwapClearFavorite(ref item, ref slots.exAccessorySlot[targetSlot - player.armor.Length]);
		success = true;
		return item;
	}

	private static bool CanEquipInModdedAccessorySlot(Item checkItem, int slot, int context, bool vanity)
	{
		if (!checkItem.accessory)
			return false;

		AccessorySlotLoader accessorySlotLoader = LoaderManager.Get<AccessorySlotLoader>();
		return accessorySlotLoader.ModdedIsSpecificItemSlotUnlockedAndUsable(slot, Main.LocalPlayer, vanity)
			&& accessorySlotLoader.ModSlotCheck(checkItem, slot, context);
	}

	private static int GetSlotColorContext(int context) => context switch {
		Context.ModdedAccessorySlot => Context.EquipAccessory,
		Context.ModdedVanityAccessorySlot => Context.EquipAccessoryVanity,
		Context.ModdedDyeSlot => Context.EquipDye,
		_ => context
	};
}
