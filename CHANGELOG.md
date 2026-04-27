# 1.3.0 (2026-04-27)

> This project is based on the original [BandcampDownloader](https://github.com/otiel/bandcampdownloader) by Otiel.

## Added Languages
- Japanese (日本語)
- Kazakh Cyrillic (Қазақша)
- Kazakh Latin (Qazaqşa)

## Code Cleanup
- Removed unused `SetProxy` method from HttpService.cs
- Removed method, interface, and ISettingsService dependency
- Removed unused WebClient-related code and imports
- Updated HttpServiceTests.cs to remove unused ISettingsService mock
- Removed empty `SliderIgnoreTracksLongerThanMinutes_ValueChanged` event handler from UserControlSettingsBDF.xaml.cs
- Removed corresponding ValueChanged event handler from XAML

## Bug Fixes
- Fixed Japanese language crash (uncommented culture mapping in LanguageService.cs)
- Fixed Kazakh Latin displaying English instead of translations (culture code workaround)

## Configuration Changes
- Changed update check to use Nai64/BandcampDownloaderFixed repo instead of otiel/bandcampdownloader
