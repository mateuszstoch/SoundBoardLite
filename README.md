# SoundBoard Lite

A lightweight and modern Windows application serving as a virtual soundboard. Designed for quickly triggering your favorite sound effects, memes, and audio clips in the background using customizable global hotkeys.

## Key Features

* **Background Playback:** Runs silently as a background application in the System Tray.
* **Global Hotkeys:** Integrated support for global keyboard shortcuts (e.g., `Ctrl+Shift+D`), allowing you to trigger sounds even while playing a game or working in another program.
* **Audio Output Selection:** Ability to route audio to a specific device in the system (e.g., Virtual Audio Cable) instead of the default speakers.
* **Modern Interface:** A clean and intuitive Dark Mode built with WPF using `MaterialDesignThemes`.
* **Local Database:** All settings and assigned shortcuts are safely stored in a lightweight SQLite database.

## Installation / Usage (Pre-built Release)

You don't need to install any programming tools to use this application!
1. Head over to the [Releases](../../releases) page.
2. Download the latest `.zip` release archive.
3. Extract the downloaded archive to any folder on your computer.
4. Open the folder and double-click `SoundBoardLite.exe` to run the app. No installation is required!

## Development Mode

If you are a developer and want to build or modify the code yourself, you will need the [.NET 9.0 SDK](https://dotnet.microsoft.com/download) installed on your Windows 10/11 machine.

To run the application in development mode, open a terminal in the main project directory and execute:
```bash
dotnet run
```

## Technologies Used
- **C# & WPF** - Application logic and user interface
- **NAudio** - Audio playback and stream routing
- **NHotkey** - Global hotkey registration
- **Hardcodet.NotifyIcon.Wpf** - System Tray icon integration
- **SQLite & Dapper** - Database and ORM
