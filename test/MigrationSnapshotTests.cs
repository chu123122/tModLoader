using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.ModLoader;

[TestClass]
public class MigrationSnapshotTests
{
	[TestMethod]
	public void SnapshotRoundTripsPersistentTileStateAndFullWallID()
	{
		Main.tileFrameImportant = new bool[TileID.Count];
		Main.tileFrameImportant[TileID.WorkBenches] = true;
		Main.tile = new Tilemap(2, 2);
		Tile source = Main.tile[0, 0], destination = Main.tile[1, 1];
		source.TileType = TileID.WorkBenches;
		source.HasTile = true;
		source.WallType = 60000;
		source.TileFrameX = 54; source.TileFrameY = 18;
		source.TileColor = 12; source.WallColor = 23;
		source.RedWire = source.BlueWire = source.GreenWire = source.YellowWire = true;
		source.HasActuator = source.IsActuated = true;
		source.IsTileInvisible = source.IsWallInvisible = source.IsTileFullbright = source.IsWallFullbright = true;
		source.LiquidAmount = 211; source.LiquidType = LiquidID.Shimmer;
		var snapshot = TileSnapshot.TileStruct.From(source);
		using var stream = new MemoryStream();
		using var writer = new BinaryWriter(stream);
		snapshot.Write(writer);
		Assert.AreEqual(Marshal.SizeOf<TileSnapshot.TileStruct>(), stream.Length);
		stream.Position = 0;
		var restored = TileSnapshot.TileStruct.Read(new BinaryReader(stream));
		destination.CheckingLiquid = destination.SkipLiquid = true;
		restored.Apply(destination);
		Assert.AreEqual(snapshot, TileSnapshot.TileStruct.From(destination));
		Assert.IsTrue(destination.HasTile && destination.HasActuator && destination.IsActuated);
		Assert.AreEqual((ushort)60000, destination.WallType);
		Assert.AreEqual((short)54, destination.TileFrameX);
		Assert.AreEqual((short)18, destination.TileFrameY);
		Assert.IsTrue(destination.RedWire && destination.BlueWire && destination.GreenWire && destination.YellowWire);
		Assert.IsTrue(destination.IsTileInvisible && destination.IsWallInvisible && destination.IsTileFullbright && destination.IsWallFullbright);
		Assert.AreEqual(211, (int)destination.LiquidAmount);
		Assert.AreEqual(LiquidID.Shimmer, destination.LiquidType);
		Assert.IsFalse(destination.CheckingLiquid || destination.SkipLiquid);
	}

	[DataTestMethod]
	[DataRow((int)SlopeType.SlopeDownLeft)]
	[DataRow((int)SlopeType.SlopeUpRight)]
	public void SnapshotRestoresSlopes(int slope)
	{
		Main.tileFrameImportant = new bool[TileID.Count];
		TileID.Sets.SaveSlopes = new bool[TileID.Count];
		TileID.Sets.SaveSlopes[TileID.Stone] = true;
		Main.tile = new Tilemap(2, 1);
		Tile source = Main.tile[0, 0], destination = Main.tile[1, 0];
		source.HasTile = true; source.TileType = TileID.Stone; source.Slope = (SlopeType)slope;
		TileSnapshot.TileStruct.From(source).Apply(destination);
		Assert.AreEqual((SlopeType)slope, destination.Slope);
	}
}
