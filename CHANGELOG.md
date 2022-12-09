# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [2.0.0] - 2022-12-08

### Added

- Can now select multiple images for the image evaluation.

### Changed

- Improved output after evaluating an image. Allows to see if an error was returned.
- Image evaluation tab : the last selected directory is remembered.
- Image evaluation tab : selecting a directory will only return images in the top directory (it used to be a recursive enumeration).
- Image evaluation tab : progress bar is now at the bottom of the screen and can now display an accurate progression.
- Updated to .NET Framework 4.8.
- Some texts were changed.

### Fixed

- Paths with spaces were ignored when evaluating images or creating an imgdb file.

### Removed

- Removed unused log functions.
- Removed unecessary try/catch.
- Removed Dragablz dependency.
- Removed MahApps dependency.
- Removed ControlzEx dependency.
- Removed System.Windows.Interactivity dependency.
- Removed MaterialDesignTheme dependency.

## [1.1.0] - 2020-07-10

This is the last version that was published by JacksiroKe. The changelog here was not written by JacksiroKe and could be incomplete.

### Added

- Can now evaluate images by selecting a directory.

### Changed

- Improved UI for the second and third tabs.

### Fixed

- Fix an issue that made the application not create imgdb file when a folder source had spaces.
- Fix a visual problem on the Progress Bar UI.
- CMD terminal is now hidden from the user.

## [1.0] - 2020-04-07

First version published by JacksiroKe

[Unreleased]: https://github.com/Mythique/arcoreimg/compare/v2.0.0...HEAD
[2.0.0]: https://github.com/Mythique/arcoreimg/compare/v1.1.0...v2.0.0
[1.1.0]: https://github.com/Mythique/arcoreimg/compare/v1.0.0...v1.1.0
[1.0]: https://github.com/Mythique/arcoreimg/releases/tag/v1.0