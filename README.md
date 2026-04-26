<div align="center">

![Icon](docs/images/Cloud.png)

# 🎵 Bandcamp Downloader

**A Windows application to download albums from Bandcamp**

[English](README.md) | [简体中文](README.zh-CN.md) | [Русский](README.ru.md)

[![Download](https://img.shields.io/badge/Download-Latest-blue?style=for-the-badge&logo=github)](https://github.com/Nai64/BandcampDownloaderFixed/releases/latest)
[![Workflow status](https://github.com/Nai64/BandcampDownloaderFixed/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/Nai64/BandcampDownloaderFixed/actions/workflows/dotnet.yml)
[![Translation status](https://hosted.weblate.org/widgets/bandcampdownloader/-/bandcampdownloader/svg-badge.svg)](https://hosted.weblate.org/engage/bandcampdownloader/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

**This project is based on [Otiel/BandcampDownloader](https://github.com/Otiel/BandcampDownloader) by [Otiel](https://github.com/Otiel)** ✨

</div>

## 📖 Description

BandcampDownloader is a Windows application that helps downloading albums from [bandcamp.com](https://bandcamp.com) by retrieving the 128 kbps MP3 files streamed on the website. It aims to ease the life of those who prefer to listen to music on their favorite player rather than on their web browser, but offers only what is already freely available on Bandcamp.

---

## ✨ Features

### 🎯 Core Functionality

| Feature | Description |
|---------|-------------|
| **📥 Download MP3s** | From album, track, or artist pages |
| **🏷️ ID3 Tags** | Album, Artist, Album Artist, Title, Track #, Year, Lyrics |
| **🖼️ Cover Art** | Save to tags and/or folder |
| **📋 Playlists** | M3U, PLS, WPL, ZPL formats |
| **🎭 Various Artists** | Auto-split "Artist - Title" format |
| **🔄 Resilient** | Continues on errors |
| **⚙️ Personalization** | Extensive settings including BDF section |

### 🔧 BDF Settings (BandcampDownloaderFixed Exclusive)

> The **BDF** section provides reliability and convenience options for enhanced downloading experience

| Feature | Description |
|---------|-------------|
| **✅ Continue on Error** | Keeps downloading remaining albums/tracks when errors occur |
| **⏳ Wait for File Ready** | Handles race conditions by waiting for files to be fully written |
| **🎨 Split V/A Titles** | Properly handles Various Artists releases |
| **⏱️ Ignore Long Tracks** | Skips tracks exceeding specified duration |
| **👤 Assign Album Artist** | Sets album artist for all tracks (compilations) |
| **📊 Show Track Count** | Displays download progress in action bar |
| **🎚️ Force Bitrate** | Experimental bitrate conversion |
| **📏 Show Original Size** | Displays original vs converted file size |
| **🔔 Toast Notifications** | Windows notifications for download events |
| **🐛 Detailed Error Dialog** | Shows detailed error information |

---

## 🖼️ Screenshots

<div align="center">

### Main Window
![Screenshot](docs/images/Screenshot.png)

### Settings Window
![Screenshot-settings](docs/images/Screenshot-settings.png)

</div>

---

## 📜 Release Notes

See the [changelog](CHANGELOG.md) for detailed version history.

---

## 🌍 Contributing

### 🈸 Translation

You can help translating the application by going to the [Weblate project](https://hosted.weblate.org/engage/bandcampdownloader).

See the [documentation](docs/help-translate.md) for more info.

[![Translation status](https://hosted.weblate.org/widget/bandcampdownloader/bandcampdownloader/multi-auto.svg)](https://hosted.weblate.org/engage/bandcampdownloader/)

---

## 📄 License

BandcampDownloader is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 👏 Credits

<div align="center">

### 🎨 Original Author
**[Otiel](https://github.com/Otiel)** - Created this awesome application! 🚀

### 🔧 Maintained by
**[Nai64](https://github.com/Nai64)** - Maintaining and updating the fork with fixes and improvements

### 🎁 Special Thanks
- Icons by [Yusuke Kamiyamane](http://p.yusukekamiyamane.com) (CC BY 3.0)
- All contributors and translators

</div>

<details>
<summary>📚 Dependencies</summary>

The list of open-source libraries used by BandcampDownloader can be found on the [dependency graph](https://github.com/Nai64/BandcampDownloaderFixed/network/dependencies).

</details>

---

## ⚠️ Piracy Note

> You'll do what you want to do with this app, but remember to **buy albums from your favorite artists** if you want to support them! 💜

### Bandcamp's Stance

From [Bandcamp's official statement](https://get.bandcamp.help/hc/en-us/articles/23020694039575-I-heard-you-can-steal-music-on-Bandcamp-What-are-you-doing-about-this):

> **One of my fans showed me a totally easy way that someone could STEAL my music off of Bandcamp using RealPlayer 14.1 beta 3, or RipTheWeb.com, or by going into Temporary Internet Files and renaming blah blah blah. What are you doing about this grave problem?**
>
> Nothing. Since streams on Bandcamp are full-length, it's true that someone could use one of the above methods to access the underlying MP3-128. And sure, we could throw some technical hurdles in their way, but if they hit one of those hurdles, it's not like they'd slap their forehead and open their wallet. Instead, they'd just move on to some other site where those restrictions aren't in place, and you'll have squandered the chance to make your own site the premier destination for those seemingly cheap, but enthusiastic, word-spreading, and potentially later money-spending fans. In other words, the few people employing the above methods are better thought of as an opportunity, not a lost sale. If you're still skeptical, [this may help](https://newmusicstrategies.com/but-if-they-steal-it/).

---

<div align="center">

**⭐ Star this repo if you find it useful!**

Made with ❤️ by the open-source community

</div>
