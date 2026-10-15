# PortingNotes 逐条核查

原始笔记保留不删。本表对应本地工作分支。“静态核对”不等于运行验收；“待核查”不代表已复现 Bug。全部迁移尚未完成。

状态统计：静态核对 20 项；待核查 134 项；已修复并单测 1 项；限定范围已验证 41 项；部分修复 1 项；已实现，待视觉验收 1 项；记录已建立 1 项。这些条目大小差别很大，不应换算成开发完成百分比。

| ID | 原文行号 | 项目 | 状态与证据 |
|---|---|---|---|
| P001 | 5 | NPCSpawnParams.gameModeData no longer exists. This was potentially used in IBestiaryInfoElement. | 静态核对：NPCSpawnParams已改为difficultyOverride，IBestiaryInfoElement本身不再传gameModeData；具体图鉴数值待全覆盖 |
| P002 | 6 | NPCSpawnParams.strengthMultiplierOverride renamed to difficultyOverride. Investigate if behavior changed. | 静态核对：NPCSpawnParams.difficultyOverride 经 NPC.ScaleStats 使用浮点难度采样；原版Boss三难度检查已覆盖部分 |
| P003 | 7 | NPCHitCount = 58 --> (and others) needs comment explaining what the value should be. Why is it 1 more when no sound 0? | 待核查： |
| P004 | 8 | Remove all Obsolete methods, including hooks and vanilla changes. | 待核查： |
| P005 | 9 | Doublecheck methods marked as "Unused": SwitchTilesNew, AddStructure/AddProtectedStructure | 待核查： |
| P006 | 10 | Test if https://github.com/tModLoader/tModLoader/pull/4626 is fixed in vanilla. I think you need to test with non-player chat to test properly, or maybe another player? | 待核查： |
| P007 | 11 | ItemSourceID is now static readonly, not a const. Do any other IDs change? | 静态核对：ItemSourceID在原版为static readonly；现有代码与ExampleMod可编译，尚未穷举所有ID迁移 |
| P008 | 12 | Consider updating FlexibleTileWand.Reload | 已修复并单测：FlexibleTileWand.Reload重建全部30原生Wand；核心反射枚举逐一验证实例更新 |
| P009 | 13 | https://github.com/tModLoader/tModLoader/pull/1675 seemed to fix a bug that is apparently now fixed in vanilla. Patches in AWorkshopPublishInfoState deleted. Verify that existing workshop publicity still correctly updates UI without requiring a click. | 待核查： |
| P010 | 14 | Check for any remaining TML added ID sets that aren't in TML.cs files. | 待核查： |
| P011 | 15 | BuffLoader.ReApply (NPC) logic seems changed, likely to fix desync issues. The server sync for MessageID.NPCBuffs when !quiet now happens after the reapply logic. Modded ReApply will need doc updates or maybe new parameters to properly adjust to these changes. Maybe a ref time parameter instead? | 限定范围已验证：NPC.AddBuff 重上后发送 NPCBuffs；既有真实TCP自定义Buff重上用例通过 |
| P012 | 16 | Recipe.requiredTile no longer supports multiple tiles. Only a single crafting station is the new approach. In theory it is possible to restore multiple required tiles, but we'd have to rule that recipes can show in the filtered crafting station UI if _any_ of their required tiles meet the filter. | 限定范围已验证：多站必须同时满足、UI任一站过滤；客户端合成检查通过 |
| P013 | 17 | Zone calculations seem to have been reorganized a bit. Verify functionality of hooks (TileCountsAvailable, ResetNearbyTileEffects, UpdateSceneEffect) | 静态核对：SceneMetrics.ScanAndExportToMain调用ResetNearbyTileEffects/TileCountsAvailable，Player更新SceneEffect；hook时序未专门断言 |
| P014 | 18 | FileUtilities.Copy and Move no longer have an `overwrite` parameter. | 待核查： |
| P015 | 19 | LegacyAudioSystem now has TrackLoopCounts and PlayCallbacks. They seem to involve counting how many times a specific music has looped. Investigate. Modders might be interested. Used with RainbowBoulderMusicPlayCallback | 待核查： |
| P016 | 20 | SoundEngine.Initialize now returns the IAudioSystem. We should test if it is still necessary to show an error message for !IsAudioSupported. 1.4.5 change log claims "Terraria no longer fails to launch when it fails to detect an available audio device.", we should see if tModLoader can work without audio support too. | 限定范围已验证：图形客户端模拟 NoAudioHardwareException 返回 DisabledAudioSystem |
| P017 | 21 | https://github.com/tModLoader/tModLoader/pull/3500 seems to have changed ItemSourceID.PlayerDropItemCheck to ThrowItem. In 1.4.5 it was renamed to PlayerDrop and there is a new InventoryOverflow. Double check that the #3500 logic applies to the fixed 1.4.5 code. Maybe see if ThrowItem is a better name than PlayerDrop and can be fixed in Terraria, otherwise tModPorter it or double check that it is patched everywhere to use the new names. | 待核查： |
| P018 | 22 | Also, ItemSourceID.SortingWithNoSpace has been removed. | 待核查： |
| P019 | 23 | TileLoader.SpecialDraw (and other tile methods I assume) now takes a TileBatch instead of Main.spriteBatch. What does this affect? How will mods need to change? Why do some methods in TileDrawing still use Main.spriteBatch? | 待核查： |
| P020 | 24 | ShaderData classes now have `if (Main.dedServ)` checks. Are these overzealous, or do we need to adjust other places or inform modders that shader code might attempt to run on servers. | 待核查： |
| P021 | 25 | Player.voiceOverride. Currently an sbyte, might need to be an int like the other equipment slot IDs. Also an example would be nice. | 待核查： |
| P022 | 26 | Player.revolverCritChanceBonus needs a quick test now that it has been implemented as a Projectile.CritChance bonus. Need to hookup `Item.GetVisualCritChance` | 限定范围已验证：VisualCritChanceTests 与实际弹丸暴击检查 |
| P023 | 27 | Player.adjTile patches are weird. It shouldn't be necessary to resize, they should be correct when the Player is initialized anyway. | 待核查： |
| P024 | 28 | Player.coat added. It might also need and EquipType | 限定范围已验证：Coat/Body共用槽位分配、下摆、染色和实际角色绘制通过；专用服务器初始化与双图形客户端实际TCP复验通过 |
| P025 | 29 | What is Player._pendingRefunds? Does it require modded item support? | 待核查： |
| P026 | 30 | Player.ApplyEquipVanity now calls RefreshInfoAccsFromItemType. Is this new behavior, will our existing hooks now call things multiple times by accident? | 待核查： |
| P027 | 31 | Player.ApplyItemTime has been updated, we might not need as many patches? | 待核查： |
| P028 | 32 | Integrate new `private void SetItemAnimation(int baseFrames, float multiplier)` method into our usetime hooks. Make public. | 静态核对：SetItemAnimation(baseFrames,multiplier)已公开；ApplyItemAnimation仍通过CombinedHooks.TotalAnimationTime和UseAnimation |
| P029 | 33 | What does `Main.item[num].OverrideWith(theItemWeDrop);` do differently than `Main.item[num] = theItemWeDrop;`? Do we need to document or adjust how modders interact with Main.item[]? | 待核查： |
| P030 | 34 | ProjectileLoader.CanUseGrapple can be reworked. The vanilla code now consolidates "max hooks" checks, so we should be able to make the logic for most modded grappling hooks easier by supplying those parameters to the hook or using a set. | 待核查： |
| P031 | 35 | https://github.com/tModLoader/tModLoader/issues/4494 should be easily fixable with the new QuickGrapple code organization | 限定范围已验证：CanUseItem/CanUseGrapple拒绝后保持坐骑，客户端检查通过 |
| P032 | 36 | Should Ram Rune be a VanillaExtraJump? Adjust Player.isPerformingJump_DownDash if necessary. What is CanUseBootFlyingAbilities? Also double check CancelAllJumpVisualEffects logic | 限定范围已验证：RamRune正常/反向重力和落地行为核心测试 |
| P033 | 37 | TileLoader.HasWalkDust and WalkDust never pass in the Tile or Frame values. New vanilla logic sometimes checks frameX/Y, so hook could be updated with more parameters to facilitate. | 待核查： |
| P034 | 38 | Collision.SwitchTiles `objType` parameter in Player.Update changed from 1 to 5. Why, isn't it still Player? Adjust docs accordingly after investigating. | 待核查： |
| P035 | 40 | Adjacent tiles are now contained in Recipe.TileCountsAs rather than hard-coded. Need to adjust TileLoader.AdjTiles hooks/docs/exampels to prioritize using Recipe.TileCountsAs | 限定范围已验证：ModTile.AdjTiles静态/动态等效、UI过滤和循环保护，客户端检查通过 |
| P036 | 42 | Terraria added a CanConsumeConsumableItem stub method. Should it check item.consumable or only ItemLoader.ConsumeItem. | 待核查： |
| P037 | 43 | We'll need to check all bag drops and update the drop database. | 待核查： |
| P038 | 44 | A lot of the GetItemSettings presets have been renamed or removed. Might be something to document. | 待核查： |
| P039 | 45 | SystemLoader.ModifyLightingBrightness might need `_negLight3` parameter. | 待核查： |
| P040 | 46 | SystemLoader.ModifyLightingBrightness and LoaderManager.Get<WaterStylesLoader>().LightColorMultiplier might need perspectivePlayer parameter or docs to use Main.SceneMetrics.PerspectivePlayer. This is for supporting spectator mode I believe. | 待核查： |
| P041 | 47 | There seems to be a new `flag` we should add to TileLoader.ModifyLight. It seems to determine if paint should override the native light color from a tile. | 待核查： |
| P042 | 48 | BiomeConversionID.PurificationPowder (8) and Chlorophyte (9) might not match up with Terraria-added values. Need to double check where these were used against the new ID values. Chlorophyte is now either 8/9/10 and PurificationPowder is 11. | 限定范围已验证：实际腐化石经 PurificationPowder=11 恢复石块 |
| P043 | 49 | Lange.CreateDialogFilter now has a checkConditions parameter. It seems that there is a new system for object substitutions. We'll need to document these and make sure they work for modded substitutions. LocalizedText.CanFormatWith usages seem to be replaced with ConditionsMetWith. Some Language.GetTextValueWith usages changed to GetTextValue but still somehow support substitutions. | 限定范围已验证：Lang.CreateDialogFilter 对象替换与条件真/假/前缀检查通过 |
| P044 | 50 | Should PlayerLoader.SyncPlayer in SyncOnePlayer be after syncing owner Projectiles? | 待核查： |
| P045 | 51 | Vanilla Fishing drops are now declarative | 待核查： |
| P046 | 56 | TELeashedEntityAnchorWithItem (used by TECritterAnchor and TEKiteAnchor) will need to be updated to support modded items. | 限定范围已验证：Mod物品锚点稳定名称存读档、40000网络ID检查通过；完整世界重载尚未单独覆盖 |
| P047 | 59 | ItemID.BannerEffect changed, might need an example. Docs need to be updated for LinearCurve. | 待核查： |
| P048 | 60 | Chest.maxItems no longer const. DefaultMaxItems/AbsoluteMaxItemsWeCanEverReachInAChestForNow added. Might need to find for loops to 40 and change if we want to support this. Chest ctors changed. | 待核查： |
| P049 | 61 | Verify that https://github.com/tModLoader/tModLoader/issues/4383 is fixed: `if (Language.GetText("CLI.NewWorld_Command").EqualsCommand(text3))` in vanilla replaced `if (text2 == "n" \|\| text2 == "N" \|\| string.Equals(text2, Language.GetTextValue("CLI.NewWorld_Command"), StringComparison.CurrentCultureIgnoreCase))` fix in tmod. (New world command) | 待核查： |
| P050 | 62 | Main.ExecuteCommand changed, doesn't have both a lowercase and raw input string anymore. Double check how capitalization is handled now, such as in CommandLoader.HandleCommand.  | 待核查： |
| P051 | 63 | Main menu music logic now checks Main.titleMusicStyle and titleMusicStyleRandom, document and adjust ModMenu logic if necessary. | 待核查： |
| P052 | 64 | Item.armorPenetration added, should we keep tml-added ArmorPenetration property? | 待核查： |
| P053 | 66 | Vanilla CanHavePrefixes logic changed, might be able to use it rather than tml changes. | 待核查： |
| P054 | 67 | ItemSlot has new flip parameter, what is it used for? PreDrawInInventory needs flip parameter. (and itemFade parameter? And secondColor?) | 待核查： |
| P055 | 68 | "// Sound is played on animation start #ItemTimeOnAllClients" comments around "SoundEngine.PlaySound(item6.UseSound" in MessageBuffer's `ShotAnimationAndSound` code. ShotAnimationAndSound was renamed, we might need to verify that this is still fixed in tmod. | 待核查： |
| P056 | 69 | ApplyDifficultyAndPlayerScaling needs to be revisited. | 限定范围已验证：缩放hook移至原生多人缩放后；ModNPC专家/大师x1/2/4人六组合、原版GlobalNPC观察最终生命值检查通过 |
| P057 | 70 | WorldGen.StopWaterfallAmbienceAudio might be a better place for some existing patches. Need to verify save and quit stopping waterfall sounds properly. | 限定范围已验证：实际瀑布循环声音开始、保存退出后停止检查通过 |
| P058 | 71 | TileLoader.DropCritterChance could be updated with LuckyClover chance. Also Lavafly/HellButterfly chance | 待核查： |
| P059 | 72 | TileID.Sets.SpreadsCrimson added. Need docs and possibly adjust biome spread logic. SpreadsHallow | 待核查： |
| P060 | 73 | OreRunner changed, new parameters should make the method more useful, need docs. Also in 1.4.4 OreRunner was missing a tileMoss check, so that might affect mods when fixed. | 待核查： |
| P061 | 74 | Code in Projectile claiming "// Moved to CombinedHooks.ModifyHitByProjectile" will need to be copied over again if that is still the intention. It seems that deadMansSweater is also nearby, should it also be commented? | 待核查： |
| P062 | 75 | Not sure about the order for "VanillaOnHitEffectsResume:" and other labels. SpawnHitVisuals method added in between existing patches. | 待核查： |
| P063 | 76 | CombinedHooks.CanHitNPCWithProj patches might need to be reworked, it seems like they should be able to be simplified | 待核查： |
| P064 | 77 | Looks like we might want to split out the collision hitbox modification from TileCollideStyle. There is a new Projectile.GetCollisionParams method. | 待核查： |
| P065 | 78 | Double check new DoScrollingInInventory logic against PlayerInput.MouseInModdedUI | 待核查： |
| P066 | 79 | Patches checking ActiveWorldFileData being null and initializing it might be superfluous now. Seems like there were some vanilla changes. | 待核查： |
| P067 | 80 | MapRenderer class now contains what was Main.mapSectionTexture and mapTarget. Most static fields there should probably be public. | 待核查： |
| P068 | 81 | DrawBlockReplacementIcon return changed from void to bool. Does that affect how the builders toggle works? Did vanilla behavior change? New state bool in logic, and DoStatefulTickSound | 待核查： |
| P069 | 82 | See updated `toolTipNames[numLines] = "UseMana";` patch. Look into IsSpaceGun and GetManaCost, might need updates.  | 限定范围已验证：GetManaCost Mod倍率和原版倍率仅应用一次，实际扣蓝/OnConsumeMana检查通过 |
| P070 | 83 | Double check PlayerLoader.ModifyZoom logic. Seems like there is only 1 callsite now, code was cleaned up? | 待核查： |
| P071 | 84 | There are new music tracks and some might have been moved. Update SceneEffectPriority enum docs and double check that they are correct for both methods. | 待核查： |
| P072 | 85 | DrawPlayer_14_2_GlassSlipperSparkles gone? | 待核查： |
| P073 | 86 | Need to find where ProjectileLoader.DrawHeldProjInFrontOfHeldItemAndArms (ModProjectile.DrawHeldProjInFrontOfHeldItemAndArms) should go. PlayerDrawSet removed heldProjOverHand and there are new fields as well. Seems like `SelectedDrawnProjectile.drawLayer == 8` replaced it in DrawPlayer_31_ProjectileOverArm? ProjectileDrawLayerID.HeldProjOverHand exists. | 静态核对：旧 DrawHeldProjInFrontOfHeldItemAndArms 已迁移为 Projectile.drawLayer=HeldProjOverHand，Porter配置已有提示 |
| P074 | 87 | SoundID.TML: NPCHit58, NPCDeath67, NPCDeath68, Item179-199 | 静态核对：SoundID.TML 已有 NPCHit58、NPCDeath67/68、Item199 等新增条目 |
| P075 | 88 | CreateTrackable now has maxInstances parameter, need to make sure they are applied to our changes. | 待核查： |
| P076 | 89 | Item.IsTheSameAs removed | 待核查： |
| P077 | 90 | RefreshInfoAccsFromItemType is now being called on vanity equipment too. Does this affect any of our changes? Is any slot now being checked twice? Do we need to adjust ModAccessorySlotPlayer for the same behavior? | 待核查： |
| P078 | 91 | `//TML: Eventide and nightglow handled by Item.useLimitPerAnimation.` comment now commenting out item 5669. Might need to make changes to that item similar to 4956 | 限定范围已验证：3852/4952/4953/5669/6152 每动画次数游戏内调用检查通过 |
| P079 | 92 | clientClone changed. I think the `_clientClone` field is no longer needed, or extraneous. | 待核查： |
| P080 | 93 | Player.nonTorch removed | 待核查： |
| P081 | 94 | What is ApplyRapidAttackBonus? | 待核查： |
| P082 | 95 | GameTipsDisplay patches need to be redone | 限定范围已验证：GameTips ConditionsMet 过滤修复，旧阶段客户端通过 |
| P083 | 96 | Main.OpenPlayerSelectFromNet changed how our patches can be implemented for invite joining. Need to be reimplemented. | 静态核对：OpenPlayerSelectFromNet 已恢复Mod加载后延迟网络角色选择；真实Steam邀请未测试 |
| P084 | 97 | DrawColorCodedStringWithShadow methods no longer return Vector2 string size. Is this because of some reason? All patches in ChatManager need to be revisited. | 静态核对：ChatManager已改为LayoutSnippets统一测量与绘制，UniqueDraw正确传scale；独立聊天边界用例待补 |
| P085 | 98 | Paladin shield patches might have been mixed up. Double Check. | 限定范围已验证：圣盾保护者消失时的实际 Hurt 路径检查通过 |
| P086 | 99 | New GetItemManaUsageDetails and ItemCheck_PayMana_X methods split mana costs into multiple methods. Should be able to remove a lot of Player.TML.cs patches and use them directly. | 限定范围已验证：普通物品和持续施法 mana hooks 已接入；客户端扣蓝检查通过 |
| P087 | 100 | Vanilla now uses Player.clientCloneItem() instead of Item.Clone(). I think we can just use that instead of swapping them for CopyNetStateTo and adjust `clientCloneItem` with `NetStateVersion`, but this may need more testing. | 待核查： |
| P088 | 101 | Recipe Changes: | 限定范围已验证：消耗材料、批准后回调/Prefix/OnCreated、DestinationStack、远端批准/拒绝退款检查通过 |
| P089 | 108 | ItemSlot flow changed a lot. AccCheck no longer exists, replaced by CanEquipAccessoryInSlot? | 限定范围已验证：原生与Mod槽约束、快速装备/翅膀冲突/偏好和染料交换3项真实客户端检查通过 |
| P090 | 109 | DyeSwap/ModSlotDyeSwap needs new approach | 限定范围已验证：原生与Mod槽约束、快速装备/翅膀冲突/偏好和染料交换3项真实客户端检查通过 |
| P091 | 110 | Item192 uses Projectile.kiteSoundPitch. How do we do that? | 待核查： |
| P092 | 111 | New AmmoID.Sets.IsSpecialist doesn't contain Sand anymore. Is that expected? | 待核查： |
| P093 | 112 | Everything in NPCSpawnHelper will need to be checked against any 1.4.5 changes, as well as any new conditions that are still missing. | 待核查： |
| P094 | 113 | TileSnapshot will need more thought to restore functionality. Commented out erroring code for now. | 部分修复：快照恢复方块/完整墙ID/帧/电线/油漆/涂料/液体，3核心用例通过；Mod箱子/TileEntity/自定义ITileData快照仍待核对 |
| P095 | 114 | Vanilla now has a NativeLibraries class, conflicting with our own. | 待核查： |
| P096 | 115 | Modded tip logic might need changes. There can now be player creation specific tips in UICharacterCreation._tips (a GameTipsDisplay class with a different `ITipProvider`. Supporting this would require more thought. | 待核查： |
| P097 | 116 | How do we update ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling? Needs dev comment and to be updated. | 待核查： |
| P098 | 117 | SurfaceBackgroundStylesLoader.DrawCloseBackground needs to be fixed or recreated from new vanilla logic/math | 已实现，待视觉验收：恢复近景背景Y计算，接入新版verticalParallaxOverride；尚未单独展示并核对自定义背景 |
| P099 | 118 | The "#4640"/PostTileFrame fix caused the game to get stuck on settling liquids 50%, it has been commented out. | 限定范围已验证：PostTileFrame改用当前tileCache，邻居重绘检查通过；新普通小世界完整生成/存读/退出通过 |
| P100 | 119 | Initialize_AlmostEverything has many new methods we'll likely need to integrate into mod reloading. We might want to see if there is a "cleaner" way than just copying over specific method calls into ModContent.Load. | 静态核对：ModContent已有 ArmorSetBonuses/ItemID.Sets/TileID.Sets.PostSetupContent；新增Coat提前构造。并未完成所有Count引用审计 |
| P101 | 122 | Make a checklist of all TML hooks and have others QC each method behavior? | 记录已建立：本表逐条保留199个顶层条目，未核销项不表示已完成或已复现缺陷 |
| P102 | 123 | ItemID.Sets.BonusAttackSpeedMultiplier renamed to BonusMeleeSpeedMultiplier. tModPorter done. (double check that this doesn't only apply to melee weapons. I think it isn't limited currently) | 静态核对：Porter已有 BonusAttackSpeedMultiplier -> BonusMeleeSpeedMultiplier，尚未穷举非近战倍率场景 |
| P103 | 124 | Run NPCShopDatabase.Test tests. | 限定范围已验证：全部已注册原版NPC商店，32种昼夜/肉前后/月相状态与本机原版oracle对照通过，修复8盆栽销售条件 |
| P104 | 125 | Player.setBonus is unused by vanilla. We will likely remove it (and comment out UpdateArmorSetsOld since it is misleading) and migrate all ExampleMod set bonuses to the new system. We'll need examples of various common set bonus setups (multiple helments, partial sets, typical head/chest/leg set, etc.) | 限定范围已验证：PR5141套装API、共享有效头部、实例回调与示例编译通过 |
| P105 | 126 | Check ModifyEquipTextureDraw to determine if there is any other locations where the hook needs to be applied in PlayerDrawLayers.cs | 待核查： |
| P106 | 127 | PlayerDrawLayers.cs DrawPlayer_13_Leggings | 待核查： |
| P107 | 130 | Should NetMessage.LogMessageError Invariant.Assert be directed to ModNet.LogMessageError somehow? | 待核查： |
| P108 | 131 | TileDrawing.IsTileDangerous vanilla now has tilex/y, but different order, so we might need to adjust TML variant or other callsites | 待核查： |
| P109 | 132 | Why did ProjectileID.SummonTagDamageMultiplier change? | 待核查： |
| P110 | 133 | Clound.cs, there is now CloudID.Sets.RareClouds_Normal, CloudID.Sets.RareClouds_Celebration for more control. Document and maybe use in ExampleMod. | 静态核对：Cloud 已使用 RareClouds_Normal/Celebration 两表 |
| P111 | 134 | public static LanguageSearchFilter CreateDialogFilter(string startsWith, object substitutions) removed from Lang.cs? Could still be useful, add back into Lang.TML.cs? | 限定范围已验证：Lang.TML.cs 对象替换 overload 与实际 LocalizedText 条件检查通过 |
| P112 | 135 | WorldGen.SwapTileData might not be necessary anymore. ErrorWorldSwapTiles exists. The new name isn't as useful, however. | 待核查： |
| P113 | 136 | Projectile.identity removed, Projectile.key added. How to use? Also Projectile.IndexForVisuals. Also Main.projectileIdentity. Need to fix Projectile.key docs, it is the identity docs. | 静态核对：Projectile.key文档已说明跨服务端客户端与slot复用的引用，并指向TryGetActive |
| P114 | 137 | Projectile.TransformType added, replaces direct setting of Projectile.type in AI. Might need a hook or more logic if used with modded projectiles. | 待核查： |
| P115 | 138 | Code near "Yoyos with effective top speed" comment in AI_009_2_Yoyos seems weird. Was this fixed in vanilla and the "< 1.01f" code left in? Probably can remove the duplicate bounds check, check blame | 待核查： |
| P116 | 139 | Item.hasVanityEffects no longer marked as [Old], why? Did something change? | 待核查： |
| P117 | 140 | Item.active property removed | 待核查： |
| P118 | 141 | Move TML added Item ctor patches near others or to Item.TML.cs | 待核查： |
| P119 | 142 | Main.rand can now have key-specific UnifiedRandom instances. Not sure how a mod might use it. | 待核查： |
| P120 | 143 | SystemLoader.PostUpdatePlayers(); call, move TimeLogger.UpdatePlayers.AddTime call? | 待核查： |
| P121 | 144 | PreUpdateNPCs doesn't exactly match 1.4.4 location because of UpdateWorld_SpawnNPCs being added. Do we want 2 more hooks there? Probably need to double check that all Main Update methods have a Pre and Post method, and that they are documented and any deviations from 1.4.4. order are documented as well (UpdateWorld_FloatingText, UpdateWorld_SpawnNPCs, etc) | 待核查： |
| P122 | 145 | Could NPC.lifeRegenExpectedLossPerSecond or interactedWithPlayerLocally be useful to modders? | 待核查： |
| P123 | 146 | Is NPC.OnSpawn useful for tmod? | 待核查： |
| P124 | 147 | Chest.AddItemToShop changes need more thought to fit in vanilla changes. | 待核查： |
| P125 | 148 | NPCLoader.UpdateLifeRegen will need to be changed. This has changed a lot. | 限定范围已验证：DoT hook 后移至原版 tally 结算之后，集成检查通过 |
| P126 | 149 | Player.hasWings might be useful. | 待核查： |
| P127 | 150 | Player.strongestMoveSpeedDebuff? Seems like a split between move speed buffs and debuffs | 待核查： |
| P128 | 151 | Player.maxRunSpeed should mention originalRunSpeed maybe. | 待核查： |
| P129 | 152 | We'll need to recheck all the container drop loot tables again. At least OpenLockBox has changed | 待核查： |
| P130 | 153 | Seems like shared loadout slots don't actually populate Player.armor slots. Modders will have to use Player.GetEffectiveArmor to get the actual armor that is active. | 限定范围已验证：护甲套装读取 GetEffectiveArmor，客户端共享头部用例通过；其他mod自行使用有效装备API |
| P131 | 154 | Player.manaPotionDelay is new. | 待核查： |
| P132 | 155 | Player.ApplyManaRegenerationDelay() replaces setting Player.manaRegenDelay directly. | 待核查： |
| P133 | 156 | Item 3852 Tome of Infinite Wisdom ItemCheck_OwnerOnlyCode changed. Does Item.useLimitPerAnimation still make sense? | 限定范围已验证：智慧宝典每动画使用次数用例通过 |
| P134 | 157 | 4953, 4952, 4956, 5669 ItemCheck_OwnerOnlyCode also changed, check if useLimitPerAnimation still makes sense. What about 6152? | 限定范围已验证：4952/4953/5669/6152使用次数用例通过 |
| P135 | 158 | Where should chlorophyteBladeCounter-- fit in the new ItemCheck_inner code. | 限定范围已验证：实际客户端帧中叶绿军刀计数5到4 |
| P136 | 159 | Is the "// Stealth knockback. Moved from GetWeaponKnockback" change fine? It seems the Player.stealth value is being remapped, so moving this logic and applying it using the remapped value might be an issue. | 限定范围已验证：真实客户端蘑菇矿隐身击退使用原始stealth检查通过 |
| P137 | 160 | Player.CollideWithNPCs changed, new parameters | 待核查： |
| P138 | 161 | Player.ApplyDamageToNPC changed, new parameters, new overloads | 待核查： |
| P139 | 162 | Double check that allDamage and allCrit are used in new armor set code | 限定范围已验证：ArmorSetBonuses.Effects.CrystalAssassin/Pumpkin保留Generic allDamage/allCrit，所有原版套装查询通过 |
| P140 | 163 | GetNanoFlaskDamageBoost in ProcessHitAgainstNPC might not be accounted for correctly.  | 限定范围已验证：真实弹丸命中NPC的纳米药剂加成通过 |
| P141 | 164 | New PickAmmo_PickAmmoItem, PickAmmo_IterateRange, PlayerAmmoCyclingMode. ItemLoader.CanChooseAmmo patches need to be reworked  | 限定范围已验证：循环弹药 CanChooseAmmo veto 真实检查通过 |
| P142 | 165 | Nano bullet might need some changes for the new 25% damage increase  | 限定范围已验证：纳米弹武器与弹药合计25%加成测试 |
| P143 | 166 | New PickAmmo_PickAmmoItem method seems to replicate our old PickAmmo/ChooseAmmo split. | 限定范围已验证：循环候选计数/选择均调用 CanChooseAmmo |
| P144 | 167 | Our PickAmmo overload patch lost. Maybe just put in .TML.cs file anyway. | 待核查： |
| P145 | 168 | Not sure about VanillaOnHitEffectsWhipsStart label location | 待核查： |
| P146 | 169 | WhipTagEffect.ModifyTaggedHit patches are wrong, method changed a lot | 限定范围已验证：原版鞭子合算/爆竹倍率测试与饰品最大标签检查通过；全部鞭子组合未遍历 |
| P147 | 170 | DustID.Spider and InfernoFlame are new and differ from names we made up in TML.cs file, remove our entries? | 待核查： |
| P148 | 171 | SpriteCharacterData is class now. | 待核查： |
| P149 | 172 | Terraria.Testing.Cloning.CloneByReference shares name with tmod class. Problem? Same purpose? Investigate. | 待核查： |
| P150 | 176 | UIElement.PassThroughMouseInteraction --> What does it do? How does it differ from IgnoresMouseInteraction? | 待核查： |
| P151 | 177 | TileEntity.Read now has a gameversion parameter. For modded tiles, I don't think this affects anything. Vanilla TEs have updated save and load code, need to verify poses and other changes work with modded items. | 待核查： |
| P152 | 178 | FlexibleTileWand is now used to place many other tiles that used to rely solely on RandomStyleRange. We should add an example of a custom FlexibleTileWand item/tile and document when to use it. | 待核查： |
| P153 | 179 | TileID.Sets.DontDrawTileSlopes. | 待核查： |
| P154 | 180 | Player.selectedItem is not a getter property instead of a field. We might need to document selectedItemState and other related new fields. | 待核查： |
| P155 | 181 | Add docs for new GetItemSettings parameters | 待核查： |
| P156 | 182 | Main.menuChat | 待核查： |
| P157 | 183 | Need to fix documentation for various secret and special seeds, like Main.specialSeedWorld. Need to change secret to special in most cases, and fix wiki links. | 待核查： |
| P158 | 184 | We need docs on how to use /checkpoint and StateSnapshot. Wiki page most likely, teaching why and how to use, limitations, etc. | 待核查： |
| P159 | 188 | UIElement.OnDraw is now a DrawEvent not a ElementEvent (can this be tModPorted?) | 待核查： |
| P160 | 189 | Point16.X and Y are no longer readonly | 待核查： |
| P161 | 190 | Entity.Center changed, taking into account 0.5 from odd widths and heights now instead of using integer division | 待核查： |
| P162 | 194 | Many of the unique vanilla hairstyles are no longer available at character creation. Set ModHair.AvailableDuringCharacterCreation to false if you wish to follow suit. | 待核查： |
| P163 | 195 | SlimeBodyItemDropRule can be applied to any NPCID.Sets.SlimeCanContainItems now and ItemID.Sets.OreDropsFromSlime has been updated to include the alternate ores as well as Hellstone and DesertFossil | 待核查： |
| P164 | 196 | Magic and Summon prefixes have been split into separate categories | 待核查： |
| P165 | 198 | Pylons no longer require happiness to be sold. Remove Condition.HappyEnoughToSellPylons from ModPylon.GetNPCShopEntry() to match vanilla. | 静态核对：ModPylon.GetNPCShopEntry仅要求AnotherTownNPCNearby/NotInEvilBiome，底座已有 |
| P166 | 199 | Removed Condition.HappyEnough and Condition.HappyEnoughToSellPylons. Replaced with Condition.CurrentPriceAdjustmentUnder(float priceModifier) and Condition.CurrentPriceAdjustmentOver(float priceModifier). | 静态核对：Condition已提供CurrentPriceAdjustmentUnder/Over，底座已有 |
| P167 | 200 | UnifiedRandom.Next methods are no longer virtual | 待核查： |
| P168 | 201 | UIWrappedSearchBar, is it useful to modders? | 限定范围已验证：实际新版工作台使用 UIWrappedSearchBar 搜索wood并截取带配方帧 |
| P169 | 202 | Various text rendering methods have been changed or improved. Investigate new functionality and previous bug fixes. | 待核查： |
| P170 | 203 | Player.IsAllowedToHoldItems | 待核查： |
| P171 | 204 | Need to determine if hooks need to act on ModItem or WorldItem. For example: `ItemIO.SendModData(item3, writer);` | 待核查： |
| P172 | 206 | The number3 parameter of the SyncEquipment message seems to have changed meaning. Docs needed. | 待核查： |
| P173 | 207 | EntitySource_FishedOut will now apply to fishing item spawns instead of just npc spawns. | 待核查： |
| P174 | 208 | Main.blackTarget removed. All other render targets are now static and WorldSceneLayerTarget instead of RenderTarget2D. (What does WorldSceneLayerTarget do?) | 待核查： |
| P175 | 209 | ArmorIDs.Head.Sets.DrawHead renamed to ArmorIDs.Head.Sets.HidesHead and values inverted | 待核查： |
| P176 | 210 | HardmodeAnnouncementTask is no longer a HardmodeTask. | 待核查： |
| P177 | 211 | AssetRepository a huge mess of patches. _changeWatcher patch might need to be restored. | 待核查： |
| P178 | 212 | LanguageManager.GetText changed, now it stores on miss. Before it didn't and we kept that behavior. Do we want the old behavior still? | 待核查： |
| P179 | 213 | Replace Main.hasFocus with FocusHelper.AllowUIInputs (or another property) | 待核查： |
| P180 | 214 | Player.QuickSpawnItem no longer returns an int indicating the index of the item in Main.item. This is because the spawned item can now potentially go directly into player inventory. | 待核查： |
| P181 | 215 | Item.width and height no longer have any relation to the in-world hitbox of dropped items. All items now have a 16x16 hitbox in the game world. | 待核查： |
| P182 | 220 | Verify that ExampleZombieThief still works with changes | 待核查： |
| P183 | 226 | In Step_Torch, change "bool flag = !ItemID.Sets.WaterTorches[type];" to "bool flag = !ItemID.Sets.WaterTorches[providedInfo.player.BiomeTorchHoldStyle(providedInfo.item.type)];" to "Allow underwater biome torches to work" (Coral Torches don't work while in the ocean) | 静态核对：SmartCursorHelper.Step_Torch已使用BiomeTorchHoldStyle后的WaterTorches，底座已有 |
| P184 | 227 | de-DE/Main.json has a lot of fixes in it, should some of these fixes be brought into Terraria? Are they actually correct? Some English. | 待核查： |
| P185 | 228 | https://github.com/tModLoader/tModLoader/commit/fced57a0725a6fabc171616487adf5166cbb89ef has several changes similar to `new MemoryStream(FileUtilities.ReadAllBytes(text, isCloudSave));` -> `FileUtilities.ReadAllBytes(text, isCloudSave).ToMemoryStream();`, to "Fix bug where BinaryIO failed to access the buffer of non-public MemoryStream". Do these need to be fixed in Terraria as well? | 待核查： |
| P186 | 229 | https://github.com/tModLoader/tModLoader/commit/a532a537df39d3787829299e0835a3e29263fe7d has a fix for "Fix map saving if loading corrupted map file.". Check if this is still the case and suggest a fix in Terraria. | 待核查： |
| P187 | 230 | https://github.com/tModLoader/tModLoader/commit/a532a537df39d3787829299e0835a3e29263fe7d has a fix for "Fix map saving if loading old map file.". Check if this is still the case and suggest a fix in Terraria. | 待核查： |
| P188 | 231 | NPC.catchItem, change from short to int? | 静态核对：NPC.catchItem已改为int，类型转换迁移底座已有 |
| P189 | 232 | `private static readonly bool[] SafeDust` and `private static readonly bool[] SafeGore` being private and readonly, unlike every other set, is a bit odd. | 静态核对：ChildSafety.SafeDust/SafeGore已public且可resize，DustLoader/GoreLoader扩展数组 |
| P190 | 233 | More simple typos: "GameUI.PrecentFishingPower" | 待核查： |
| P191 | 234 | PlayerDrawSet.missingHand and missingArm are the opposite of what they sound like apparently. tModLoader changes them as follows: | 待核查： |
| P192 | 252 | There are still several TileID.Sets.Torches missing: TorchAttack (x2), UpdateTorchLuck_ConsumeCountersAndCalculate, TryRecalculatingTorchLuck, PlaceTile | 待核查： |
| P193 | 258 | See what happens when a worldgen step fails now with the new world generator code. We removed this patch since the new code seems to handle it, but maybe we need to restore the error reporting (Utils.ShowFancyErrorMessage) | 限定范围已验证：故意抛异常的 WorldGenerator.RunPass 可向上传播；LoadWorld 自动生成失败不再继续保存。尚未故障注入完整加载入口 |
| P194 | 288 | Need to restore this patch. The code has been moved into ItemSorting.AddSortingPrioritiesBasedOnPlayerDamage and DamageTypeSortingLayerEntry is also new: | 静态核对：ItemSorting.DamageTypeSortingLayerEntry已用StatModifier并按Additive*Multiplicative排序，底座已有 |
| P195 | 312 | These buffimmune change are new and likely need to be used to update TML-specific sets. | 限定范围已验证：新增原版Poisoned/Bleeding/Hemorrhage与Ichor/BrokenArmor继承，并修复双免疫OR/反向继承下标；客户端通过 |
| P196 | 323 | Move this patch to PrepareAliases, or adjust ModCommand to use new system: | 静态核对：EmojiCommand.PrepareAliases已遍历EmoteBubbleLoader.EmoteBubbleCount，Mod表情别名由统一入口刷新 |
| P197 | 341 | PatchReviewer crashes when trying to move this patch "@@ -30673,9 +32745,14 @@": | 待核查： |
| P198 | 358 | Banner syncing has changed. Check what `NetManager.Instance.SendToClient(BannerSystem.NetBannersModule.WriteFullState(), whoAmI);` does and make sure it supports modded bannerids. | 限定范围已验证：按Mod名称保存可领取旗帜数量、本地存读与全量网络状态检查通过；两个实际TCP图形客户端都收到正确Mod旗帜计数 |
| P199 | 371 | Chest.chestItemSpawn/chestItemSpawn2/dresserItemSpawn removed. Document whatever replaced it. | 待核查： |
