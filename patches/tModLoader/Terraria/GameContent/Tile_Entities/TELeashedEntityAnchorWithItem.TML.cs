using System.IO;
using Terraria.ModLoader.IO;

namespace Terraria.GameContent.Tile_Entities;

public abstract partial class TELeashedEntityAnchorWithItem
{
	public override void SaveData(TagCompound tag)
	{
		if (itemType > 0)
			tag["item"] = ItemIO.Save(new Item(itemType));
	}

	public override void LoadData(TagCompound tag)
	{
		itemType = ItemIO.Load(tag.GetCompound("item")).type;
		// Vanilla TEs are initially restored before the .twld is loaded.
		// Recreate the leashed entity after resolving the modded item's stable name.
		OnWorldLoaded();
	}

	public override void NetSend(BinaryWriter writer) => writer.Write7BitEncodedInt(itemType);

	public override void NetReceive(BinaryReader reader) => itemType = reader.Read7BitEncodedInt();
}
