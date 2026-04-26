<div align="center">

![Icon](docs/images/Cloud.png)

# 🎵 Bandcamp Downloader

**用于从 Bandcamp 下载专辑的 Windows 应用程序**

[English](README.md) | [简体中文](README.zh-CN.md) | [Русский](README.ru.md) | [⠓⠗⠑⠊](README.br.md)

[![下载](https://img.shields.io/badge/下载-最新版本-blue?style=for-the-badge&logo=github)](https://github.com/Nai64/BandcampDownloaderFixed/releases/latest)
[![工作流状态](https://github.com/Nai64/BandcampDownloaderFixed/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/Nai64/BandcampDownloaderFixed/actions/workflows/dotnet.yml)
[![翻译状态](https://hosted.weblate.org/widgets/bandcampdownloader/-/bandcampdownloader/svg-badge.svg)](https://hosted.weblate.org/engage/bandcampdownloader/)
[![许可证](https://img.shields.io/badge/许可证-MIT-green.svg)](LICENSE)

---

**本项目基于 [Otiel/BandcampDownloader](https://github.com/Otiel/BandcampDownloader) 由 [Otiel](https://github.com/Otiel) 创建** ✨

</div>

## 📖 简介

BandcampDownloader 是一个 Windows 应用程序，通过从 [bandcamp.com](https://bandcamp.com) 检索网站上流式传输的 128 kbps MP3 文件来帮助下载专辑。它的目的是让那些喜欢在喜欢的播放器上听音乐而不是在网页浏览器上听音乐的人生活更轻松，但它只提供 Bandcamp 上已经免费提供的内容。

---

## ✨ 功能

### 🎯 核心功能

| 功能 | 描述 |
|---------|-------------|
| **📥 下载 MP3** | 从专辑、曲目或艺术家页面下载 |
| **🏷️ ID3 标签** | 专辑、艺术家、专辑艺术家、标题、曲目号、年份、歌词 |
| **🖼️ 封面艺术** | 保存到标签和/或文件夹 |
| **📋 播放列表** | M3U、PLS、WPL、ZPL 格式 |
| **🎭 群艺术家支持** | 自动分割"艺术家 - 标题"格式的曲目 |
| **🔄 弹性下载** | 出错时继续下载 |
| **⚙️ 个性化** | 包括 BDF 部分的广泛设置 |

### 🔧 BDF 设置 (BandcampDownloaderFixed 独有)

> **BDF** 部分提供可靠性和便利性选项，以增强下载体验

| 功能 | 描述 |
|---------|-------------|
| **✅ 出错时继续** | 出错时继续下载剩余的专辑/曲目 |
| **⏳ 等待文件就绪** | 通过等待文件完全写入来处理竞态条件 |
| **🎨 分割 V/A 标题** | 正确处理群艺术家发行版 |
| **⏱️ 忽略长曲目** | 跳过超过指定时长的曲目 |
| **👤 分配专辑艺术家** | 为所有曲目设置专辑艺术家（适用于合集） |
| **📊 显示曲目计数** | 在操作栏中显示下载进度 |
| **🎚️ 强制比特率** | 实验性比特率转换 |
| **📏 显示原始大小** | 显示原始与转换后的文件大小 |
| **🔔 Toast 通知** | 下载事件的 Windows 通知 |
| **🐛 详细错误对话框** | 显示详细的错误信息 |

### 🎭 有趣语言

> 为了娱乐目的，应用程序包含几个有趣的语言选项：

| 语言 | 描述 |
|----------|-------------|
| **🏴‍☠️ 海盗** | Arrr! 像海盗一样说话 |
| **🐱 LOLcat** | 我可以汉堡吗？ |
| **🎭 莎士比亚风格** | 你应该用古英语说话 |
| **⠓⠗⠑⠊ 盲文** | ⠓⠗⠑⠊⠇⠇⠑⠂（仅供娱乐！） |

---

## 🖼️ 截图

<div align="center">

### 主窗口
![Screenshot](docs/images/Screenshot.png)

### 设置窗口
![Screenshot-settings](docs/images/Screenshot-settings.png)

</div>

---

## 📜 发布说明

查看 [更新日志](CHANGELOG.md) 获取详细的版本历史。

---

## 🌍 贡献

### 🈸 翻译

您可以通过访问 [Weblate 项目](https://hosted.weblate.org/engage/bandcampdownloader) 来帮助翻译应用程序。

查看[文档](docs/help-translate.md)了解更多信息。

[![翻译状态](https://hosted.weblate.org/widget/bandcampdownloader/bandcampdownloader/multi-auto.svg)](https://hosted.weblate.org/engage/bandcampdownloader/)

---

## 📄 许可证

BandcampDownloader 在 **MIT 许可证**下获得许可 - 详见 [LICENSE](LICENSE) 文件。

---

## 👏 致谢

<div align="center">

### 🎨 原作者
**[Otiel](https://github.com/Otiel)** - 创建了这个很棒的应用程序！🚀

### 🔧 维护者
**[Nai64](https://github.com/Nai64)** - 维护和更新分支，提供修复和改进

### 🎁 特别感谢
- 图标由 [Yusuke Kamiyamane](http://p.yusukekamiyamane.com) 提供（CC BY 3.0）
- 所有贡献者和翻译者

</div>

<details>
<summary>📚 依赖项</summary>

BandcampDownloader 使用的开源库列表可以在[依赖关系图](https://github.com/Nai64/BandcampDownloaderFixed/network/dependencies)中找到。

</details>

---

## ⚠️ 盗版说明

> 您可以随意使用此应用程序，但请记住如果您想支持您喜欢的艺术家，请**购买他们的专辑**！💜

### Bandcamp 的立场

来自 [Bandcamp 的官方声明](https://get.bandcamp.help/hc/en-us/articles/23020694039575-I-heard-you-can-steal-music-on-Bandcamp-What-are-you-doing-about-this)：

> **我的一个粉丝向我展示了一种非常简单的方法，有人可以使用 RealPlayer 14.1 beta 3、RipTheWeb.com 或通过进入临时 Internet 文件并重命名等等来从 Bandcamp 上偷走我的音乐。你打算如何处理这个严重的问题？**
>
> 什么都不做。由于 Bandcamp 上的流媒体是全长度的，确实有人可以使用上述方法访问底层的 MP3-128。当然，我们可以设置一些技术障碍，但如果他们遇到这些障碍，他们不会拍拍额头打开钱包。相反，他们会转移到其他没有这些限制的网站，你会失去让你的网站成为那些看似便宜但热情、传播性强且可能以后会花钱的粉丝的首选目的地的机会。换句话说，使用上述方法的少数人应该被视为机会，而不是失去的销售。如果你仍然怀疑，[这可能会有帮助](https://newmusicstrategies.com/but-if-they-steal-it/)。

---

<div align="center">

**如果您觉得有用，请给这个仓库加星！⭐**

由开源社区用 ❤️ 制作

</div>
