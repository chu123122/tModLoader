# 个人版框架发布交接

更新：2026-10-18。用户已授权将框架个人版上传至公开 fork `chu123122/tModLoader`，并制作简易 Windows x64 实验包。README 明确个人适配由 AI 完成、代码质量低、缺少系统人工审查，仅适合自行开发，不承诺成品整合包体验。

本轮仅整理框架源码、说明和包体；46 模组仓库继续保持私有，不随框架发布。包体不含原版 Content、用户存档、模组包、日志或转储。发布复用当前已验证的本地运行文件，不重建或覆盖 Steam 安装。

当前发布操作和独立包体检查记录位于 `E:\CS-Study\Tmod-dev-tools\publish-2026-10-18`。GitHub Release 将标为 prerelease，源码快照和包内文件 SHA256 由清单记录。原生 FNA3D 补丁和 JIT 标记工具在 `personal/`；不表示完成了可重复构建验证。

旧 HANDOFF 的“没有公开推送授权”等限制属于此前任务状态，现已被本轮明确授权取代。旧测试记录保留如下，不能视为当前完整验证结论。

本轮包体检查：552 文件预检包通过 ZIP CRC 与逐文件 SHA256；随包 .NET 10.0.13 可运行；无模组服务端加载复制的 AcceptPrehard 测试世界、监听本地端口，发送正确 UTF-16 编码的 exit 后保存退出，exit 0。重定向控制台存在被捕获的 Console.Clear “句柄无效”警告，不记为无警告运行。最初退出脚本编码错误、另一次强制新控制台导致的测试失败保留在发布工作目录；没有据此修改框架游戏逻辑。

新整理的 JIT 标记工具编译成功（0 警告/错误），对临时输出的验证为 flags 3→3、IL/局部变量不变。未重新做客户端交互、模组全流程、多人或性能验收。发布目标为 `https://github.com/chu123122/tModLoader/releases/tag/chu123122-1.4.5.8-dev.20261018`；实际推送与上传结果以发布工作目录 published.json 及 GitHub 为准。

---

# 本地 Terraria 1.4.5.8 / Fargo 交接

更新：2026-10-16。当前任务进行中：先升级 Souls 1.7.3.9、补齐审计报告 1–8 项，再继续其他模组。下面旧 1.7.3.7 测试记录属于历史 smoke 证据，不能视为完整实机验收。最新状态见 `E:\CS-Study\Tmod-dev-tools\acceptance-2026-10-16\HANDOFF.md`。

当前游玩入口 `Start-Fargo-1.4.5.8.cmd` 的独立 fargo-ready 配置已将 Souls 升到 1.7.3.9；旧包保存在 acceptance-2026-10-16/delivery-backup。最新完整迁移源码在 acceptance-2026-10-16/sources/FargowiltasSouls；旧 mod-sources/FargowiltasSouls 仍是 1.7.3.7。没有加入其他模组。

## 历史 1.7.3.7 记录（已有审计指出覆盖边界）

- Mutant 3.3.6.7、Luminance 1.0.14、Souls 1.7.3.7均成功打包；包内部身份、SHA1、SHA256及内嵌DLL与构建输出的一致性已检查。
- StructureHelper从官方源码 f224c11eb09f0be12ee83665cdf097d38e6e1fe5 重建完整库并内嵌Souls，保留旧结构格式接口，场地实际生成通过。
- 真实图形客户端分阶段及跨进程保存重读全部通过，最新计数：Mutant 10、Luminance 8、组合 23。组合包含实际合成、NPC商店、五套护甲效果与原生tooltip管线、Eridanus动态状态/按键、key查找与失效代际、DestroyerGun射击和分节引用、龙卷风顺序、代表性Boss AI、翅膀/代谢球真实绘制、棺椁场地生成、世界标记和模组物品重读。
- 三份最终重读客户端日志无ERROR/FATAL、JITException、IL失败或绘制异常。截图已经查看。可选Fargo Music未装，正常出现音乐提示。
- 上一轮tML基础65核心、75Porter、71客户端检查仍是既有证据；本轮没有修改框架代码，也没有重跑该整套或新的多人测试。

## 路径和入口

- tML仓库：`E:\CS-Study\Tmod-dev`，分支local/playable-1.4.5，HEAD仍7e79059a6。
- 工具/runtime：`E:\CS-Study\Tmod-dev-tools`。
- 双Fargo源码：工具目录`mod-sources/Fargowiltas`、`mod-sources/FargowiltasSouls`。
- Luminance实际源码仍在`E:\CS-Study\Luminance-1.4.5`；新junction `E:\CS-Study\Luminance`指向它。**构建时必须通过Luminance入口，打包器按项目目录名生成包内部名。**
- StructureHelper源码：工具目录`mod-sources/StructureHelper`。
- 新游玩启动器：工具目录`Start-Fargo-1.4.5.8.cmd`；保存目录`fargo-ready`，无自动测试Mod及测试角色/世界。
- 隔离测试源码：`mod-sources/FargoSmoke`。
- 本轮所有记录：`migration-2026-10-16/README.md`、`final-validation.json`、`result-*-reload.json`、`client-*-reload.log`、`delivery-manifest.json`。

## 构建与已处理的陷阱

使用工具目录.NET10 SDK和nuget-packages。脚本`migration-2026-10-16/build-mod.py <组件> <日志标签>`依次构建，检查exit0再运行；不同构建使用独立打包目录。最新日志为luminance-04、structure-04、souls-09、smoke-12。

Souls引用必须指新构建DLL的明确路径；仅写程序集名+HintPath仍会让项目根目录旧DLL优先。先前运行时不兼容的StructureHelper.dll已替换为重建结果，改该库后先复制到Souls/lib再编Souls。

ProjectileKey在float槽中是位重解释，不能数值(int)转换。查找核对活动、owner、类型、generation；key不表示先后，龙卷风另存序号并走标准ExtraAI。此同步改动未进行多人专项。普通当前帧和缓存渲染目标的相机基准要区分。

Luminance屏幕修饰器在订阅前完成初始化，使用不可变快照并等待ShaderManager完成加载；旧ScreenShaderFixer移除，因为本地tML DoDraw已移除Retro限制。Souls菜单绘制有独立Begin/End。五套护甲说明经GlobalItem修改原生set tooltip，未使用internal LocalizedText.SetValue。

自动客户端由run-smoke.py/run-reloads.py驱动，只修改隔离配置。失焦会导致原版暂停；测试配置开启PlayWhenUnfocused并关闭ThrottleWhenInactive，不改变用户常规设置。初次欢迎和Fargo Music提示不算完整玩法验收。

## 边界与后续

不使用子agent；没有公开推送/发布/PR授权；不操作普通用户存档。原用户暂不做多人专项，编译必需的API迁移已允许。未完成完整开荒、全部Boss/武器/特效、长期稳定性、全种子和多人。其余tML PortingAudit缺口继续保留。

源码与文档目前仍是本地未提交修改；本轮没有自动提交。修改前差异、旧HANDOFF/报告保存在migration-2026-10-16，完整差异与新增文件包为migration-source-diffs-and-new-files.zip，基线见source-revisions.json。工具目录旧HANDOFF仅作历史参考。
