# SoundBoard Lite

Lekka i nowoczesna aplikacja na system Windows pełniąca rolę wirtualnego panelu z dźwiękami (soundboard). Stworzona z myślą o szybkim wyzwalaniu ulubionych efektów dźwiękowych, memów i wstawek w tle, za pomocą zdefiniowanych skrótów klawiszowych.

## Główne Funkcje

* **Odtwarzanie w tle:** Działa jako ukryta aplikacja w systemowym pasku zadań (System Tray).
* **Globalne Skróty Klawiszowe:** Zintegrowana obsługa globalnych skrótów (np. `Ctrl+Shift+D`), pozwalająca na wyzwalanie dźwięków nawet gdy grasz w grę lub pracujesz w innym programie.
* **Wybór Wyjścia Audio:** Możliwość skierowania dźwięku do konkretnego urządzenia w systemie (np. Virtual Audio Cable) zamiast do domyślnych głośników.
* **Nowoczesny Interfejs:** Przejrzysty i intuicyjny Dark Mode zbudowany w WPF przy użyciu `MaterialDesignThemes`.
* **Lokalna Baza Danych:** Wszystkie ustawienia i przypisane skróty zapisywane są bezpiecznie w lekkiej bazie SQLite.

## Wymagania

* System operacyjny Windows 10/11
* Zestaw narzędzi [.NET 9.0 SDK](https://dotnet.microsoft.com/download)

## Uruchomienie

Aby uruchomić aplikację w trybie deweloperskim, otwórz terminal w głównym katalogu projektu i wpisz:
```bash
dotnet run
```

## Budowanie Wersji Finalnej (Release)
Aby utworzyć pojedynczy plik gotowy do udostępnienia (bez potrzeby instalowania .NET):
```bash
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

## Wykorzystane Technologie
- **C# & WPF** - Aplikacja i interfejs
- **NAudio** - Obsługa i routing strumieni audio
- **NHotkey** - Rejestracja globalnych skrótów klawiszowych
- **Hardcodet.NotifyIcon.Wpf** - Obsługa ikonki w System Tray
- **SQLite & Dapper** - Baza danych i ORM
