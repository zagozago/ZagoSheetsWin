# ZagoSheetsWin

<details>
<summary>🌐 文档语言 · 选择语言</summary>

- [English](../../../README.md)
- **简体中文** — 当前页面
- [हिन्दी](../hi/README.md)
- [Español](../es/README.md)
- [العربية](../ar/README.md)
- [Français](../fr/README.md)
- [বাংলা](../bn/README.md)
- [Português (Brasil)](../pt/README.md)
- [Bahasa Indonesia](../id/README.md)
- [اردو](../ur/README.md)
- [Русский](../ru/README.md)
- [Deutsch](../de/README.md)
- [日本語](../ja/README.md)
- [Tiếng Việt](../vi/README.md)
- [Türkçe](../tr/README.md)
- [한국어](../ko/README.md)
- [Italiano](../it/README.md)
- [ไทย](../th/README.md)
- [Filipino](../fil/README.md)
- [Bahasa Melayu](../ms/README.md)
- [Kiswahili](../sw/README.md)
- [Nigerian Pidgin](../pcm/README.md)
- [मराठी](../mr/README.md)
- [తెలుగు](../te/README.md)
- [Hausa](../ha/README.md)
- [ਪੰਜਾਬੀ](../pa/README.md)
- [தமிழ்](../ta/README.md)
- [粵語](../yue/README.md)
- [فارسی](../fa/README.md)
- [አማርኛ](../am/README.md)
- [Basa Jawa](../jv/README.md)
- [ગુજરાતી](../gu/README.md)
- [ಕನ್ನಡ](../kn/README.md)
- [Yorùbá](../yo/README.md)
- [भोजपुरी](../bho/README.md)
- [پښتو](../ps/README.md)
- [ଓଡ଼ିଆ](../or/README.md)
- [မြန်မာ](../my/README.md)
- [മലയാളം](../ml/README.md)
- [Polski](../pl/README.md)
- [Basa Sunda](../su/README.md)
- [मैथिली](../mai/README.md)

</details>

**在 Windows 中直接使用 Google 表格打开本地电子表格文件。**

ZagoSheetsWin 是一款轻量级 Windows 应用程序，让打开本地电子表格变得简单：

**双击文件 → 上传并转换 → 在 Google 表格中打开**

成功导入后，ZagoSheetsWin 可以将原文件替换为指向 Google 表格文档的 Internet 快捷方式（`.url`），同时在本地保留可恢复的原文件备份。

目标很简单：让 Google 表格像 Windows 原生应用一样打开本地电子表格文件。

## 功能介绍

ZagoSheetsWin 将 Windows 中的电子表格文件与 Google 表格连接起来。

打开受支持的本地文件时，应用程序可以：

- 检测并验证电子表格；
- 创建可恢复的备份；
- 通过 Google 官方 API 将文件直接上传到 Google 云端硬盘；
- 将文件转换为原生 Google 表格文档；
- 在默认浏览器中打开转换后的电子表格；
- 创建指向 Google 文档的本地 `.url` 快捷方式；
- 后续再次打开时，避免重复上传同一个文件。

无需手动上传至云端硬盘，无需在浏览器中反复寻找文件，也无需每次手动转换。

## 支持的格式

当前开发目标包括：

- `.xlsx`
- `.xls`
- `.ods`
- `.csv`
- `.tsv`

某些格式可能存在额外的兼容性限制。如果文件包含无法安全保留的功能，应用程序会采取保守处理方式，以避免在用户不知情的情况下丢失数据。

## 专为 Windows 设计

ZagoSheetsWin 专为 Windows 构建，并通过以下方式与操作系统集成：

- **打开方式（Open with）**
- 文件类型注册
- 双击打开文件
- 可选的文件资源管理器集成
- 原生 Windows 安装程序

应用程序不会擅自更改 Windows 默认应用。用户始终可以自行控制文件关联。

## 以数据安全为核心

ZagoSheetsWin 将本地原文件替换视为一项必须能够恢复的操作。

从原文件所在文件夹移除原文件之前，应用程序会验证：

1. 已存在可恢复的备份；
2. 已成功创建 Google 表格文档；
3. 已持久保存本地文件关联信息；
4. 已成功写入并验证 Internet 快捷方式。

如果流程失败，原文件将被保留。

应用程序遵循一条简单的原则：

> 绝不在用户不知情的情况下销毁数据。

## 备份

在原文件被快捷方式替换之前，应用程序可以将其保存到私有的本地备份区域。

备份管理支持配置存储空间上限、保留期限和清理选项。

备份用于保护最初导入的原文件，**并不提供双向同步**：之后在 Google 表格中所做的修改不会写回原始电子表格文件。

## 隐私与 Google 访问权限

ZagoSheetsWin 从用户电脑直接与 Google API 通信。

- 您的电子表格内容不会发送到 Zagotools 服务器。
- OAuth 令牌保存在本地，并通过 Windows 安全机制进行保护。
- 应用程序使用 Google 云端硬盘的 `drive.file` 权限范围，将访问权限限制在通过应用程序创建或打开的文件。
- 电子表格导入流程不需要使用分析追踪服务。

隐私政策和使用条款：

https://zagotools.top/legal.html

## 项目状态

ZagoSheetsWin 正在积极开发中，目前应视为 **Alpha 测试版软件**。

Windows → Google 表格的核心工作流程已经可以使用。项目正持续改进安装、恢复、文件格式兼容性、国际化和用户体验。

在首个稳定版本发布之前，功能和行为可能发生变化。

## 与 Open in Google 的关系

ZagoSheetsWin 基于 [SwatiK425](https://github.com/SwatiK425) 开发的 [Open in Google](https://github.com/SwatiK425/open-in-google) 项目，并在其基础上继续演进。

Open in Google 为本项目提供了最初的技术基础和灵感。

此后，ZagoSheetsWin 逐步发展为独立的 Windows 应用程序，拥有自己的架构、安装程序、用户界面、备份与恢复系统、文件关联工作流程、格式处理机制，以及本地文件到 Google 表格的使用体验。

上游项目仍然独立发展。适合通用场景的改进可能会回馈上游，而 ZagoSheetsWin 也会继续独立演进。

## 开源

ZagoSheetsWin 是免费开源软件。

本项目保留原始 Open in Google 代码要求的署名与许可证信息，并明确标识后续由 ZagoSheetsWin / Zagotools 完成的开发工作。

参见：

- [LICENSE](../../../LICENSE)
- [ATTRIBUTION.md](../../../ATTRIBUTION.md)
- [NOTICE.md](../../../NOTICE.md)
- [third-party/NOTICE.md](../../../third-party/NOTICE.md)

## 许可证

MIT 许可证。

详情请参阅 [LICENSE](../../../LICENSE)。

---

**ZagoSheetsWin — Zagotools 项目**

用小巧的软件，解决真实的问题。
