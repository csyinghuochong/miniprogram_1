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

---

## 技能系统架构

### 核心设计

技能系统采用 **ECS + Handler模式**，通过配置表驱动实现技能行为。

#### 文件位置

```
Model/Server/MengJing/Skill/
├── SkillS.cs                      # 技能Entity（数据）
├── SkillManagerComponentS.cs      # 技能管理组件（挂载到Unit）
├── SkillDispatcherComponentS.cs   # Handler分发器（单例）
├── SkillHandlerS.cs               # Handler抽象基类
└── SkillPassiveComponent.cs       # 被动技能触发组件

Hotfix/Server/MengJing/Skill/
├── SkillSSystem.cs                # 技能System逻辑
├── SkillManagerComponentSSystem.cs
├── SkillPassiveComponentSystem.cs
└── Action/
    ├── Skill_Common.cs            # 通用Handler（配置驱动）
    ├── Skill_近战普通攻击.cs       # 特殊Handler示例
    └── Skill_远程普通攻击.cs
```

---

### 技能生命周期

```csharp
OnInit(skill)              // 初始化（创建碰撞体、设置参数）
OnExecute(skill)           // 开始执行（立即生效逻辑）
OnUpdate(skill, deltaTime) // 每帧更新（持续性技能）
OnFinished(skill)          // 结束清理
```

---

### 技能释放流程

```
玩家请求 → C2M_TryUseSkillHandler
    ↓
SkillManagerComponentS.TryUseSkill()
    ↓
检查：CD、目标、状态
    ↓
创建SkillS子Entity
    ↓
skill.OnInit() → skill.OnExecute()
    ↓
广播 M2C_OnUseSkill 给客户端
    ↓
启动Timer每帧调用OnUpdate()
    ↓
skill.SkillState = Finished 后清理
```

---

### 配置表核心字段

| 字段 | 类型 | 说明 |
|------|------|------|
| `SkillHandler` | string | Handler类名（推荐用`Skill_Common`）|
| `SkillTargetType` | enum | 目标类型（单体/AOE/队友/敌人）|
| `DamageRangeType` | int | 范围类型：0=无 1=圆形 2=矩形 3=扇形 |
| `DamageRange` | float[] | 范围参数（半径/长宽/角度）|
| `ActDamage` | float | 伤害系数 |
| `InitBuffID` | int[] | 给自己加的Buff |
| `BuffID` | int[] | 给目标加的Buff |
| `SkillLiveTime` | float | 持续时间（周期性技能）|
| `GameObjectParameter` | float[] | 自定义参数 |

---

### Skill_Common 支持的技能模式

**推荐80%的技能使用`Skill_Common`作为Handler**，通过配置表控制行为：

#### 1. 给自己加Buff
```
SkillHandler: Skill_Common
InitBuffID: [1001]
SkillTargetType: SelfOnly
```

#### 2. 给单体目标加Buff
```
SkillHandler: Skill_Common
SkillTargetType: TargetOnly
BuffID: [2001]
```

#### 3. 立即AOE伤害
```
SkillHandler: Skill_Common
DamageRangeType: 1          # 圆形
DamageRange: [10]           # 半径10
SkillTargetType: MulTarget  # 多目标（敌人）
ActDamage: 1.5              # 150%伤害
```

#### 4. 立即AOE伤害+Buff
```
SkillHandler: Skill_Common
DamageRangeType: 1
DamageRange: [8]
SkillTargetType: AllEnemy
ActDamage: 1.2
BuffID: [3001]              # 附带Buff
```

#### 5. 周期性AOE伤害
```
SkillHandler: Skill_Common
DamageRangeType: 1
DamageRange: [8]
SkillLiveTime: 5.0                # 持续5秒
GameObjectParameter: [1.0]        # 每1秒触发一次
SkillTargetType: AllEnemy
ActDamage: 0.5
```

#### 6. 全体队友Buff
```
SkillHandler: Skill_Common
SkillTargetType: AllTeam     # 不限范围
BuffID: [4001, 4002]
```

#### 7. 范围队友Buff
```
SkillHandler: Skill_Common
DamageRangeType: 1
DamageRange: [15]
SkillTargetType: AllTeam     # 关键：判定队友
BuffID: [5001]
```

#### 8. 复合技能（给自己+给范围敌人）
```
SkillHandler: Skill_Common
InitBuffID: [6001]           # 先给自己加Buff
DamageRangeType: 1
DamageRange: [12]
SkillTargetType: AllEnemy
BuffID: [6002]               # 再给范围内敌人加Buff
```

---

### 特殊技能需要自定义Handler

以下情况需要继承`SkillHandlerS`写新Handler：

1. **追踪弹道**（如`Skill_远程普通攻击`）
2. **多段攻击**（如`Skill_无敌击`）
3. **条件伤害**（如`Skill_淘汰之刃`：根据血量改变伤害）
4. **召唤实体**（如`Skill_混乱之雨`）
5. **复杂交互逻辑**

---

### 添加新技能的步骤

#### 常规技能（推荐）

1. **编辑配置表**（`Unity/Assets/Config/Excel/SkillConfig.xlsx`）
   - 设置 `SkillHandler = Skill_Common`
   - 配置目标类型、范围、伤害、Buff等
2. **运行Luban生成配置**（`Tools/Luban/GenConfig.bat`）
3. **测试验证**

#### 特殊技能

1. **创建新Handler**（`Hotfix/Server/MengJing/Skill/Action/`）
   ```csharp
   public class Skill_XXX : SkillHandlerS
   {
       public override void OnInit(SkillS skill) { }
       public override void OnExecute(SkillS skill) { }
       public override void OnUpdate(SkillS skill, float deltaTime) { }
       public override void OnFinished(SkillS skill) { }
   }
   ```
2. **配置表中设置 `SkillHandler = Skill_XXX`**
3. **按F6编译热更新代码**
4. **运行Luban生成配置**

---

### 范围检测（Shape）

```csharp
// 在OnInit中创建
skill.ICheckShape = skill.CreateCheckShape(0);

// 三种形状
Circle:    圆形（DamageRangeType=1, DamageRange=[半径]）
Rectangle: 矩形（DamageRangeType=2, DamageRange=[宽度, 长度]）
Fan:       扇形（DamageRangeType=3, DamageRange=[距离, 角度]）

// 检测点是否在范围内
if (skill.ICheckShape.Contains(targetPosition)) { }
```

---

### 被动技能系统

```
SkillPassiveComponent 监听触发条件：
- OnDamagedByChance       # 受伤概率触发
- OnNormalAttackByChance  # 普攻概率触发
- OnSelfHpBelowPercent    # 自身血量低于百分比
- OnTeamHpBelowPercent    # 队友血量低于百分比
- OnBattleStart           # 战斗开始

配置字段：
SkillType: Passive
SkillPassiveType: [OnDamagedByChance]
PassiveSkillPro: [0.2]    # 20%概率
PassiveSkillTriggerOnce: 1 # 只触发一次
```

---

### 技能与Buff的关系

- **InitBuffID**: 技能施放时给**施法者自己**加的Buff
- **BuffID**: 技能命中时给**目标**加的Buff
- Buff系统独立维护生命周期，详见下方Buff系统章节

---

## Buff系统架构

### 核心设计

Buff系统与技能系统结构相似，同样采用配置驱动。

#### 文件位置

```
Model/Server/MengJing/Buff/
├── BuffS.cs                    # Buff Entity
├── BuffManagerComponentS.cs    # Buff管理组件
├── BuffDispatcherComponentS.cs # Handler分发器
└── BuffSHandler.cs             # Handler抽象基类

Hotfix/Server/MengJing/Buff/
├── BuffSSystem.cs
├── BuffManagerComponentSSystem.cs
└── Action/
    ├── Buff_Attribute.cs       # 属性/状态Buff（常用）
    └── Buff_生命图腾.cs         # 特殊Buff示例
```

---

### Buff生命周期

```csharp
OnInit(buff)                 // 初始化
OnUpdate(buff, deltaTime)    // 每帧更新
OnFinished(buff)             // 结束时移除效果
```

---

### 配置表核心字段

| 字段 | 类型 | 说明 |
|------|------|------|
| `BuffHandler` | string | Handler类名（常用`Buff_Attribute`）|
| `BuffTime` | float | 持续时间（秒）|
| `BuffDelayTime` | float | 延迟生效时间 |
| `BuffLoopTime` | float | 循环触发间隔（>0则周期触发）|
| `BuffType` | int | 1=属性 2=状态 |
| `BuffParameterType` | int | 要修改的属性/状态类型 |
| `BuffParameterValue` | long | 修改值 |
| `BuffParameterValueType` | int | 基于x属性值计算（0=固定值）|
| `IsBuffStackable` | int | 是否可叠加（0=不可）|
| `BuffMaxStackCount` | int | 最大叠加层数 |
| `TargetType` | int | 1=自身 2=队友 3=敌方 |

---

### Buff叠加规则

```
BuffManagerComponentS.BuffFactory() 工厂方法逻辑：

1. 检查叠加上限（BuffMaxStackCount）
2. 同ID Buff处理：
   - IsBuffStackable=0 → 移除旧Buff，添加新Buff
   - IsBuffStackable=1 → 允许共存
3. 状态类Buff（BuffType=2）特殊处理：
   - 同类型状态只保留一个（最新的）
```

---

### Buff_Attribute 支持的模式

**推荐90%的Buff使用`Buff_Attribute`作为Handler**：

#### 1. 持续属性加成
```
BuffHandler: Buff_Attribute
BuffType: 1                  # 属性
BuffTime: 10.0               # 持续10秒
BuffParameterType: 110001    # NumericType.Now_AtkAdd
BuffParameterValue: 500      # 增加500攻击力
BuffDelayTime: 0             # 立即生效
```

#### 2. 持续回血/扣血
```
BuffHandler: Buff_Attribute
BuffType: 1
BuffTime: 5.0
BuffLoopTime: 1.0            # 每1秒触发一次
BuffParameterType: 110001    # Now_Hp
BuffParameterValue: 100      # 每次回100血
```

#### 3. 基于属性百分比
```
BuffHandler: Buff_Attribute
BuffType: 1
BuffTime: 8.0
BuffParameterType: 110001    # Now_AtkAdd
BuffParameterValue: 2000     # 20%（/10000）
BuffParameterValueType: 110010  # 基于当前攻击力
```

#### 4. 状态异常（眩晕/定身）
```
BuffHandler: Buff_Attribute
BuffType: 2                  # 状态
BuffTime: 3.0
BuffParameterType: 4         # StateType.Vertigo（眩晕）
```

#### 5. 嘲讽
```
BuffHandler: Buff_Attribute
BuffType: 2
BuffTime: 5.0
BuffParameterType: 9         # StateType.Taunt
```

---

### 添加新Buff的步骤

1. **编辑配置表**（`Unity/Assets/Config/Excel/BuffConfig.xlsx`）
   - 设置 `BuffHandler = Buff_Attribute`（大部分情况）
   - 配置类型、时间、参数
2. **运行Luban生成配置**（`Tools/Luban/GenConfig.bat`）
3. **测试验证**

特殊Buff（如召唤物）需要自定义Handler。

---

### 状态类型枚举（StateType）

常用状态值（在`BuffParameterType`中使用）：

```csharp
// 需要查看具体定义：Model/Share/MengJing/Enum/StateType.cs
1  - 无敌
2  - 沉默
3  - 禁锢
4  - 眩晕
9  - 嘲讽
...
```

---

### 技能调用Buff示例

```csharp
// 在技能中给目标添加Buff
skill.SkillBuff(buffId, targetUnit);

// Buff会自动：
// 1. 检查TargetType是否匹配
// 2. 处理叠加规则
// 3. 广播给客户端
// 4. 启动生命周期计时
```

---

## 技能/Buff开发最佳实践

1. **优先使用配置驱动** - 80%技能用`Skill_Common`，90%Buff用`Buff_Attribute`
2. **复用现有Handler** - 新技能前先检查是否有类似逻辑
3. **参数化设计** - 用`GameObjectParameter`传递特殊参数
4. **分离表现和逻辑** - 服务端只做数值计算，特效/动画由客户端处理
5. **文档注释** - 特殊Handler必须在顶部注释参数含义
6. **测试覆盖** - 修改通用Handler需要全面回归测试

---

## 常见问题排查

### 技能不触发
1. 检查`SkillHandler`名称是否正确
2. 确认Handler类已编译（F6）
3. 查看服务端日志是否有Error

### Buff不生效
1. 检查`BuffParameterType`是否在`NumericType`枚举中存在
2. 确认`TargetType`是否匹配实际目标
3. 检查是否被叠加规则拦截

### 周期性技能不循环
1. 确认`SkillLiveTime > 0`
2. 检查`GameObjectParameter[0]`是否设置
3. 确认`DamageRangeType > 0`（周期伤害必需）

---