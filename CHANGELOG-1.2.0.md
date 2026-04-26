# Changelog v1.2.0

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
