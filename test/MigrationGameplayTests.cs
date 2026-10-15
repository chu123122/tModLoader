using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

[TestClass]
public class MigrationGameplayTests
{
	[ClassInitialize]
	public static void Initialize(TestContext context)
	{
		Program.SavePath = ".";
		typeof(ExtraJumpLoader).GetMethod("ResizeArrays", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)!.Invoke(null, null);
	}

	[DataTestMethod]
	[DataRow(1f)]
	[DataRow(-1f)]
	public void RamRuneStartsAirborneDownDash(float gravity)
	{
		Player player = new() {
			hasDeadCellsDownDash = true,
			controlDown = true,
			controlJump = true,
			releaseJump = true,
			gravDir = gravity,
			velocity = new Vector2(0f, -2f * gravity)
		};
		player.JumpMovement();
		Assert.IsTrue(player.isPerformingJump_DownDash);
		Assert.AreEqual(16f * gravity, player.velocity.Y);
		Assert.IsFalse(player.CanUseBootFlyingAbilities);
	}

	[TestMethod]
	public void RamRuneDoesNotTriggerOnGround()
	{
		Player player = new() {
			hasDeadCellsDownDash = true,
			controlDown = true,
			controlJump = true,
			releaseJump = true
		};
		player.JumpMovement();
		Assert.IsFalse(player.isPerformingJump_DownDash);
		Assert.IsTrue(player.CanUseBootFlyingAbilities);
	}
}
