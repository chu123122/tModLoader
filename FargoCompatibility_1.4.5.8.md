# Fargo 1.4.5.8 本地兼容性检查

检查日期：2026-10-15。本轮顺序为迁移笔记审查与自动检查 → Fargo 源码编译兼容性。**没有生成可玩的 Fargo 兼容包，也没有执行 Fargo 游戏内验收。** 联机同步、协议、序列化标识部分按用户要求保留。

## 基础迁移检查

PortingNotes 全部199个顶层条目已逐条记入 PortingAudit_1.4.5.md，其中包含源码检查、真实运行、未完成API和待人工验收项；不等于199条全部通过。65核心测试、75 Porter测试、71图形客户端检查通过。本轮变更的5份补丁/整文件精确重放通过。

修复了原生/Mod穿甲字段分裂、退款物品完整.tplr存档、共享装备和虚荣信息饰品刷新、语音配饰默认值int限制。没有修改请求处理、网络包或同步行为。

## 编译兼容性结果

| 组件 | 当前暴露的编译诊断 | 运行状态 |
|---|---:|---|
| Fargo Mutant 3.3.6.7 | 3 | 编译未通过，未启动模组游戏 |
| Fargo Souls 1.7.3.7 | 71 | 编译未通过，未启动模组游戏 |
| Luminance 1.0.14 | 2 | 编译未通过，未启动模组游戏 |

这些是编译器当前阶段暴露的诊断，**不是剩余Bug总数或工作量**。修复签名层错误后可能继续出现方法体错误。

突变体已迁移新版NPC对话按钮和动态商店/事件动作、新合成列表及区域计数入口、世界物品hook、配方组工厂注册与命名冲突、选择栏、幸福度移除等本地API。编译诊断由153减至3。尚未验证这些Mod改动的实际游戏行为。

保留的3处恰在联机相关路径：FargoNet.SyncAI仍使用旧Projectile.identity，手里剑销毁包仍使用旧identity，NPC生成仍设置已移除的netUpdate2。新ProjectileKey包含spawner/index/generation，旧包把标识转short；不能为了过编译把key简单强转后宣称正确。本轮不改这三处。

魂石已运行官方tModPorter：1805文件中修改669份，7轮完成；首批诊断由609变为71，主要为DrawBehind列表签名、旧护甲套装hook、NPC对话按钮、AddBuff detour和一处MSBuild命名空间引用。源码仍引用随仓库带的旧Fargowiltas/Luminance DLL，仅能做此阶段源码检查；新依赖未构建成功，不能算组合兼容。

Luminance的CheckMonoliths已移位，FilterManager.EndCapture签名/重载改变，目前停在2个detour声明错误；图形渲染目标/捕获生命周期需适配后再验证。

## 可复查证据

工具目录：E:\CS-Study\Tmod-dev-tools。完整诊断保存为 fargo-compatibility-diagnostics.json；每轮原始日志均保留：
- fargo-mutant-build-4.log
- fargo-souls-post-porter-build.log
- luminance-build-1.log
- notes-core-tests.log / notes-porter-tests.log / notes-client-driver.log
- expanded-patch-replay-validation.json（本轮5份） / pre-notes-54-patch-replay-validation.json（上一轮54份）

源码基线：突变体0153d4aacd4bfaa02984ea1dd610b92e4deca201；魂石226fadeadbe3422785a7708ba2cdf53bd8548c00；Luminance b2468dfd2f299597602dc6826af781d436c29a57。三个源码目录中的适配保持本地开发状态，未公开发布。
