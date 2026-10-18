# 模组目录

本目录整理个人 1.4.5.8 分支使用的迁移模组。**仓库不托管第三方模组的源码**；
安装包也只包含许可证明确允许再分发、且包内没有未核实第三方依赖的少数模组。

机器可读清单：[catalog.json](catalog.json)。工坊订阅列表：[workshop-links.md](workshop-links.md)。
本机已订阅工坊内容时，可用 [fetch-workshop.ps1](fetch-workshop.ps1) 把对应 `.tmod` 复制到目标目录。

## 版权与再分发

- 多数模组没有在包内声明许可证，按默认“保留所有权利”处理，仓库只提供链接。
- `Project tRU`（CalamityRuTranslate）明确禁止转载，只保留官方工坊链接。
- GPL-3.0 模组（BossCursor、BossesAsNPCs、ParticleLibrary）未在仓库分发二进制，因为需要同时提供对应源码。
- 仓库内 `.tmod` 仅限 MIT 且无未核实捆绑依赖的模组，附带原许可证与署名，见 [tmod/NOTICE.md](tmod/NOTICE.md)。
- 这些包是个人 1.4.5.8 移植版，不是作者官方发布；兼容性、存档和联机风险自负。

## 安装

1. 订阅 [workshop-links.md](workshop-links.md) 中的工坊页面，或从作者 GitHub/论坛下载对应版本。
2. 运行 `fetch-workshop.ps1 -Destination <tModLoader存档目录>/Mods` 从本机工坊缓存复制；
   仓库内已有的 MIT 包也会一起复制。
3. 在 tModLoader 的模组菜单启用。版本不一致时以作者官方发布为准。

## 框架与库

其他模组依赖的公共库、UI/光照/世界/粒子框架。

| 模组 | 版本 | 作者 | 许可证 | 获取 |
|---|---|---|---|---|
| InnoVault (`InnoVault`) | 1.9.101 | HoCha113 | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=3338554501) |
| Luminance (`Luminance`) | 1.0.14 | Lucille Karma | MIT | 仓库内 `tmod/Luminance.tmod` |
| Particle Library (`ParticleLibrary`) | 3.1.3.2 | SnowyStarfall | GPL-3.0 | [主页](https://github.com/SnowyStarfall/ParticleLibrary) |
| absoluteAquarian Utilities (SerousCommonLib) (`SerousCommonLib`) | 1.0.6.2 | absoluteAquarian | MIT | 仓库内 `tmod/SerousCommonLib.tmod` |
| SilkyUI (`SilkyUIFramework`) | 0.3.0 | 太阳照常升起 | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=3403460014) |
| Subworld Library (`SubworldLibrary`) | 2.3.0.1 | John Snail | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2785100219) |
| Terraria Ambience API (`TerrariaAmbienceAPI`) | 1.3.6.8 | RighteousRyan | unknown | [主页](https://github.com/RighteousRyan1/TerrariaAmbienceAPI/wiki) |

## 性能与优化

帧率与更新率优化。

| 模组 | 版本 | 作者 | 许可证 | 获取 |
|---|---|---|---|---|
| High FPS Support (`HighFPSSupport`) | 0.6.6.5 | Stellar | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=3119712528) |

## 便利与界面

界面、背包、合成、搜索、挖矿等便利功能。

| 模组 | 版本 | 作者 | 许可证 | 获取 |
|---|---|---|---|---|
| Auto Trash (`AutoTrash`) | 0.8.1.1 | jopojelly | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2565540604) |
| Basic Automated Mining (`BAM`) | 1.2.5 | Aitherix | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2838188064) |
| Boss Checklist (`BossChecklist`) | 2.2.4 | SheepishShepherd, jopojelly | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2669644269) |
| Boss Cursor (`BossCursor`) | 3.0.1 | kgoyo | GPL-3.0 | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2816694149) |
| Census - Town NPC Checklist (`Census`) | 0.5.2.8 | jopojelly | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2687866031) |
| Expanded Inventory (`ExpandedInventory`) | 0.4.1 | CoinPurse | unknown | 无公开链接 |
| HP Awareness (`HPAware`) | 1.0.6.1 | Core | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2564503881) |
| Atmospheric Torches (`ImprovedTorches`) | 1.4.7 | Orion | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2817496179) |
| Quality of Terraria 更好的体验 (`ImproveGame`) | 1.8.2 | 局长, Cyril, 夜谷紫幽, delete, Kaelis, Lacewing, Lastprismer, 合作一号, Reits, 中学生, 错数螺线, Zavarski, Pengolin | MIT | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2797518634) |
| Improved Movement Visuals (`LookMod`) | 1.3.4.1 | Orion | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2816557864) |
| Magic Builder (`MagicBuilder`) | 1.4.4.12 | GabsGoncs | unknown | 无公开链接 |
| Magic Storage (`MagicStorage`) | 0.7.0.11 | absoluteAquarian & Contributors | MIT | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2563309347) |
| Ore Excavator (1.4.3/1.4.4 Veinminer) (`OreExcavator`) | 0.8.9 | CrazyContraption | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2565639705) |
| Recipe Browser (`RecipeBrowser`) | 0.12.0.3 | jopojelly | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2619954303) |
| Smarter Cursor (`SmarterCursor`) | 1.3.3 | Quandratic | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2877850919) |
| Which Mod Is This From? (WMITF) (`WMITF`) | 2.7.1 | gardenapple | MIT | 仓库内 `tmod/WMITF.tmod` |
| Wombat's General Improvements (`WombatQOL`) | 1.3.15.1 | Wombat | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2877696929) |

## 内容与难度

Boss、敌怪、NPC、武器与难度内容。

| 模组 | 版本 | 作者 | 许可证 | 获取 |
|---|---|---|---|---|
| AlchemistNPC Lite (`AlchemistNPCLite`) | 1.9.9 | Gregg/VVV101, b2mauler, Rockman Zeta, somebody0214, RebooT, drh | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2599842771) |
| Bosses As NPCs (`BossesAsNPCs`) | 0.2.15 | Rijam | GPL-3.0 | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2689434541) |
| Fargo's Mutant Mod (`Fargowiltas`) | 3.3.6.7 | Fargowilta, Terry N. Muse and Javyz | MIT | 仓库内 `tmod/Fargowiltas.tmod` |
| Fargo's Music Mod (`FargowiltasMusic`) | 0.1.7.3 | Fargo & Terry & Javyz | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2782337219) |
| Fargo's Souls Mod (`FargowiltasSouls`) | 1.7.3.9 | Fargowilta, Terry N. Muse and Javyz | MIT | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2815540735) |
| Fargo's Soul Mod Extras (`FargowiltasSoulsDLC`) | 0.6.2 | Fargowilta and Terry N Muse | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2906451681) |
| Helpful NPCs (`HelpfulNPCs`) | 2.2.2 | Cas | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2817545194) |
| Useful NPCs (`UsefulNPCs`) | 3.0.4 | Gortart, Lonely Star | MIT | 仓库内 `tmod/UsefulNPCs.tmod` |
| zzp198's WeaponOut (`WeaponOut`) | 1.4.4.9 | zzp198, Flashkirby99 | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2827446882) |

## 战斗与视觉

战斗辅助、伤害显示与钓鱼扩展。

| 模组 | 版本 | 作者 | 许可证 | 获取 |
|---|---|---|---|---|
| Aimbot (`Aimbot`) | 0.4.0 | RoxyTheProxy | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2831179995) |
| Colored Damage Types (`ColoredDamageTypes`) | 1.8.5 | PvtFudgepants | unknown | [主页](https://forums.terraria.org/index.php?threads/pvt-fudgepantss-mods.59971/) |
| Multiple Lures (`MultipleLures`) | 1.4 | -Cyril- | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2795036823) |

## 联机与同步

联机配方/收藏同步。

| 模组 | 版本 | 作者 | 许可证 | 获取 |
|---|---|---|---|---|
| Recipe Browser × Magic Storage: Fully Connected (`RecipeFavoritesSync`) | 0.19 | Eric-Wu | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=3786428732) |

## 汉化与本地化

汉化或本地化补丁。

| 模组 | 版本 | 作者 | 许可证 | 获取 |
|---|---|---|---|---|
| Project tRU (`CalamityRuTranslate`) | 3.2.1.151 | tRU Team | ARR | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2831360228) |
| Fargo突变&魂石汉化补丁 (`FargoChinese`) | 336.9961.1 | Furgo_ & FallingLeaf落叶 & @马达达马达（B站同名） & -ROH- | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2847120725) |

## 兼容与修复

兼容性修复与小范围行为修正。

| 模组 | 版本 | 作者 | 许可证 | 获取 |
|---|---|---|---|---|
| Better Beds (`BedFix`) | 1.1 | -Cyril- | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2791578194) |
| Better Treasure Bag Loot (`BossBagNoRng`) | 1.2.1 | -Cyril- | unknown | [主页](https://github.com/Cyrillya/BossBagNoRng) |
| Friendly NPCs Don't Die (`FriendlyNpcsDontDie`) | 0.3 | Slayer6409 | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2729987636) |
| No More Items Stuck in Blocks (`ItemsOuttaBlocks`) | 1.1 | Cyrilly | unknown | 无公开链接 |
| No Lake Size Penalty (`NoLakeSizePenalty`) | 1.1 | -Cyril- | unknown | [工坊](https://steamcommunity.com/sharedfiles/filedetails/?id=2813360630) |
| Shorter Respawn Time (`ShorterRespawn`) | 1.2.1 | jopojelly | unknown | [主页](http://forums.terraria.org/index.php?threads/shorter-respawn-time.45962/) |

## 说明

- `localPortVersion` 是个人移植版本的版本号，工坊页面可能是作者当前版本，二者不保证一致。
- 未列出的依赖（如 StructureHelper、NetSimplified、PinyinNet 等）请按原模组说明单独获取。
- 若作者要求下架链接或包，请通过仓库 issue 联系；链接类内容可直接移除。
