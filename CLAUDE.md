# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 项目概述

这是一个基于ET8.1框架的Unity游戏项目，采用客户端-服务端双端架构。ET是一个C#开发的分布式游戏框架，支持客户端服务端代码共享、热更新、多纤程并发等特性。

## 代码架构

### 分层架构

项目采用严格的代码分层，分为以下几个关键层次：

1. **Core层** (`Unity/Assets/Scripts/Core/`, `DotNet/Core/`)
   - 框架核心功能：Entity系统、Fiber纤程、网络层、序列化等
   - 不依赖Unity特定API，可在服务端使用

2. **Model层** (`Unity/Assets/Scripts/Model/`)
   - **Client**: 纯客户端逻辑（如UI数据模型）
   - **Server**: 纯服务端逻辑（如战斗逻辑、数据库访问）
   - **Share**: 客户端服务端共享代码
   - **Generate**: Luban配置生成的代码（Client/Server/ClientServer）

3. **Hotfix层** (`Unity/Assets/Scripts/Hotfix/`)
   - 热更新逻辑代码，同样分为Client/Server/Share
   - 所有业务逻辑方法都在此层实现

4. **ModelView层** (`Unity/Assets/Scripts/ModelView/`)
   - 客户端UI相关的数据模型

5. **HotfixView层** (`Unity/Assets/Scripts/HotfixView/`)
   - 客户端UI相关的热更新逻辑

6. **Mono层** (`Unity/Assets/Scripts/Mono/`)
   - MonoBehaviour组件代码

7. **Loader层** (`Unity/Assets/Scripts/Loader/`, `DotNet/Loader/`)
   - 代码加载器，负责热更新dll的加载

8. **App层** (`Unity/Assets/Scripts/App/`, `DotNet/App/`)
   - 程序入口

### 编译模式

支持三种编译模式（通过GlobalConfig配置）：
- **Client**: 纯客户端模式
- **Server**: 纯服务端模式
- **ClientServer**: 双端模式（开发推荐）

### Entity组件系统

ET使用ECS（Entity Component System）架构：
- Entity: 只包含数据的实体对象
- Component: 挂载到Entity上的组件，只有数据无方法
- System: 通过C#扩展方法实现Entity的业务逻辑
- 使用Source Generator自动生成System代码

## 常用开发命令

### 编译与热更新

在Unity编辑器中：
- **F6**: 编译热更新dll（菜单：ET/Compile）
- **F7**: 运行时热重载代码（菜单：ET/Reload）

编译流程会：
1. 根据GlobalConfig的CodeMode刷新程序集定义（.asmdef）
2. 编译生成Unity.Model.dll、Unity.Hotfix.dll等
3. 复制到`Unity/Assets/Resources/Code/`目录（.bytes格式）

### 配置表生成

配置表使用Luban工具，Excel表格位于`Unity/Assets/Config/Excel/`：

```bash
# Windows
cd Tools/Luban
GenConfig.bat

# Linux/Mac
cd Tools/Luban
./GenConfig.sh
```

生成的代码位置：
- 客户端配置代码: `Unity/Assets/Scripts/Model/Generate/Client/Config/`
- 服务端配置代码: `Unity/Assets/Scripts/Model/Generate/Server/Config/`
- 配置数据: `Config/Excel/c/` (客户端), `Config/Excel/s/` (服务端)

### 协议生成

协议定义位于`Unity/Assets/Config/Proto/`，使用protobuf格式。

在Unity编辑器中：ET/Build Tool -> Proto2CS

或运行：
```bash
cd Tools/Proto2CS
Proto2CS.bat  # Windows
./Proto2CS.sh # Linux/Mac
```

### 服务器启动

开发环境（本地单进程）：
```bash
cd Bin
dotnet App.dll --AppType=Server --Console=0 --StartConfig=StartConfig/Localhost --Process 1
```

或使用快捷脚本：
```bash
start_server_1.bat  # Windows, 进程1
start_server_2.bat  # Windows, 进程2
```

### 打包构建

Unity编辑器中：ET/Build Tool

注意：
- 打包前必须设置CodeMode为Client
- 打包前必须设置EPlayMode为HostPlayMode（非EditorSimulateMode）
- 主场景位于`Assets/Scenes/Init.unity`

## 项目特定设置

### GlobalConfig配置

位于`Assets/Resources/GlobalConfig.asset`，关键配置项：
- **CodeMode**: Client/Server/ClientServer
- **BuildType**: Debug/Release
- **EPlayMode**: 资源加载模式（YooAsset）
- **EnableDll**: 是否启用dll加载模式

### 代码定义宏

- **ENABLE_VIEW**: 启用Entity可视化调试（在Hierarchy面板查看Entity树）
- **UNITY_COMPILE**: 编译时自动添加
- **DOTNET**: 纯服务端(.NET Core)环境

切换ENABLE_VIEW：ET/ChangeDefine/Add ENABLE_VIEW 或 Remove ENABLE_VIEW

## 网络架构

- 支持KCP、TCP、WebSocket协议
- 使用MemoryPack序列化（零GC）
- Actor消息机制实现位置透明的实体通信
- 软路由设计用于防网络攻击

## 资源管理

使用YooAsset进行资源管理，配置位于`Unity/Assets/AssetBundleCollectorSetting.asset`。

资源打包：YooAsset工具面板

## 测试与调试

### 可视化调试

开启ENABLE_VIEW宏后，运行游戏可在Hierarchy面板看到：
- Init/Global: 全局Entity
- Init/Scene(Process): 场景Entity树

### 压测工具

Unity菜单：ET/ServerTools -> Benchmark

## 重要注意事项

1. **不要手动编辑Generate目录**下的代码，这些是自动生成的
2. **Component只包含数据**，不要写方法，使用System扩展方法
3. **热更新代码**必须放在Hotfix/HotfixView层
4. **跨层引用规则**：
   - Hotfix层可以访问Model层
   - Model层不能访问Hotfix层
   - Client代码不能访问Server代码（反之亦然）
5. 修改.asmdef文件后需要F6重新编译
6. 配置表修改后必须运行Luban生成工具
7. 协议修改后必须运行Proto2CS工具

## 第三方库

项目集成了以下关键库：
- **HybridCLR**: Unity热更新方案
- **YooAsset**: 资源管理
- **MemoryPack**: 高性能序列化
- **MongoDB**: 数据库驱动
- **NLog**: 日志系统
- **DotRecast**: C#寻路库
- **I2 Localization**: 多语言支持
- **DOTween**: 动画补间
- **Spine**: 2D骨骼动画
- **Odin Inspector**: Unity编辑器增强

## 目录结构说明

- `Unity/`: Unity工程
- `DotNet/`: 纯.NET服务端工程
- `Share/`: 客户端服务端共享代码（分析器、代码生成器等）
- `Config/`: 配置数据（Excel、Json、Proto等）
- `Tools/`: 工具脚本（Luban、Proto2CS、寻路导出等）
- `Bin/`: 服务端编译输出
- `Release/`: 客户端打包输出
- `Logs/`: 运行日志
- `GameDesign/`: 游戏设计文档
- `Document/`: 技术文档

## 开发工作流

1. 修改代码
2. 按F6编译（如果修改了Model/Hotfix层代码）
3. 如果游戏正在运行，按F7热重载
4. 修改配置表后运行GenConfig.bat
5. 修改协议后运行Proto2CS
6. 提交代码前确保编译通过且服务器能正常启动
- 只添加关键注释