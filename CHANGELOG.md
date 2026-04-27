# 1.3.0 (2026-04-27)

> This project is based on the original [BandcampDownloader](https://github.com/otiel/bandcampdownloader) by Otiel.

## New Features
- Added new color themes: Blue, Green, Purple, Orange
- Added Remember Last URL option to save last used album URL
- Added Clear URLs button to quickly clear URL textbox
- Added Open downloads folder button for quick access to download location
- Added Open URL button to discography selection dialog for quick album access

## UI Improvements
- Made discography selection dialog fixed size (600x500 pixels)
- Increased discography selection dialog width to 700px to accommodate new Actions column
- Repositioned Clear URLs button to action bar for better accessibility

## Build Configuration
- Added dual build configurations: full self-contained (75MB) and light framework-dependent (8.3MB)
- Full version includes .NET runtime for standalone use
- Light version requires .NET 10 runtime for smaller download size

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
