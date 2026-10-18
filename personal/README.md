# 个人构建附记

这里只记录发布包所需的少量额外步骤，不是完整编译教程。

- 框架基于上游 `1.4.5` 分支基线 `fdcf523ec5c8f3fe613ce16d4eac6e07eef8de93`。`patches/` 是需要保留的迁移改动，`src/` 是本地生成文件，不上传原版反编译源码。
- Windows 本地运行包使用 .NET 10。发布包复用已测试的本地二进制快照；`PACKAGE-MANIFEST.json` 记录来源提交、构建说明和各文件 SHA256。提交号是源码快照标识，不表示已完成可重复构建验证。
- `jit-optimizer` 只调整 `DebuggableAttribute` 中的禁用 JIT 优化等标记，并比较方法 IL/局部变量；它不等价于重新编译 Release，也不保证性能改善或挂钩完全兼容。输入、输出必须分开，先备份再使用。
- 原生补丁 `native/mojoshader-sampler-span.patch` 对应 FNA3D 26.06（`84124dd88cbb5eb244d114e1d44331ad76bf8f45`）、MojoShader `abdc80360c1d4560ab8f356035dcd53ae6e9b87f`。修正 SDL GPU 路径中稀疏采样器槽位的跨度，并移除孤立的栈释放。包内 Windows FNA3D.dll SHA256：`8b9869113c5b4ff438f3b17e01ac194805621f26ea5e998cd3fb38f14ac977ce`。原生重编需要自行准备对应工具链。

JIT 工具示例（.NET 10 SDK）：

```powershell
dotnet run --project personal/jit-optimizer -- input.dll output.dll path/to/runtime
```

打包只收集明确列出的框架文件、依赖和 .NET 运行时，不收集存档、日志、模组或原版 Content：

```text
python personal/package.py --runtime path/to/runtime --dotnet path/to/dotnet --nuget path/to/nuget-cache --output path/to/package.zip --revision SOURCE_COMMIT
```

SDK、原版游戏、模组源码和编译环境需另行准备。重新构建后的文件应自行验证，尤其是 IL/反射挂钩、弹幕引用、绘制状态、合成和保存重读。
