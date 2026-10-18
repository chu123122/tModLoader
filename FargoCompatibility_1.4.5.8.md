# 本地 Fargo / Terraria 1.4.5.8 适配状态

正式组合：Mutant 3.3.6.7、Luminance 1.0.14、Souls 1.7.3.9。Souls完整发行DLL恢复及迁移源码在 E:\CS-Study\Tmod-dev-tools\acceptance-2026-10-16\sources\FargowiltasSouls；旧 mod-sources/FargowiltasSouls 1.7.3.7 保留。

已完成相互独立的API、三进度夹具、平台IL、五套护甲tooltip/技能、八类武器命中/引用代际、四个Boss攻击/转阶段/死亡/宝藏袋、渲染同进程重载及跨进程存档检查。第6项用户实际操作后整体反馈通过，未提供逐项截图或交易计数。

本轮新增实际修复：ItemMapIconRenderer在模组数组扩容后重新绑定；FNA3D/MojoShader的DXIL创建保留稀疏sampler槽位范围，保留Souls原shader。Mutant测试生存辅助已修正，不改Boss机制。原始失败和skip均保留，不等于全部玩法、平台或联机长期稳定性已验收。

详细报告：[1–8补测报告](/mnt/e/CS-Study/Tmod-dev-tools/acceptance-2026-10-16/1-8补测报告.md)。各次输入hash、runtime日志与断言位于同目录 runs；native构建/备份见 native。

用户2026-10-17已授权继续当前保存启用列表的其他模组迁移，具体状态在 enabled-port-2026-10-16。不得把其他模组编译通过写成完整实机验收。
