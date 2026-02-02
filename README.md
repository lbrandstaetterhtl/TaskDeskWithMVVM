# TaskDesk with MVVM

Eine moderne Desktop-Aufgabenverwaltungsanwendung, entwickelt mit **Avalonia UI** und dem **MVVM-Architekturmuster** in C# (.NET 9.0).

## 📋 Überblick

TaskDesk ist eine plattformübergreifende Desktop-Anwendung zur Verwaltung von Aufgaben, Benutzern und Gruppen. Die Anwendung nutzt das MVVM-Pattern für eine saubere Trennung von UI-Logik und Geschäftslogik und speichert alle Daten persistent als JSON-Dateien.

## ✨ Features

### Aufgabenverwaltung
- **Aufgaben erstellen, bearbeiten und anzeigen**
- **Status-Tracking** mit fünf verschiedenen Zuständen:
  - Pending (Ausstehend)
  - In Progress (In Bearbeitung)
  - Completed (Abgeschlossen)
  - On Hold (Pausiert)
  - Cancelled (Abgebrochen)
- **Fälligkeitsdaten** für Aufgaben
- **Zuweisung von Benutzern und Gruppen** zu Aufgaben

### Benutzerverwaltung
- Benutzer mit verschiedenen Rollen:
  - Admin
  - User
  - Read-Only
- E-Mail- und Passwortverwaltung
- Gruppenzuordnung

### Gruppenverwaltung
- Erstellung und Verwaltung von Benutzergruppen
- Zuordnung von Aufgaben zu Gruppen
- Verwaltung von Gruppenmitgliedern

### Datenpersistenz
- Automatisches Speichern aller Daten als JSON
- Daten werden auf dem Desktop gespeichert (`~/Desktop/TaskDeskData/`)
- Automatisches Laden beim Programmstart

## 🏗️ Architektur

Das Projekt folgt dem **MVVM-Pattern (Model-View-ViewModel)**:

### Models
- `Task.cs` - Aufgabenmodell mit Logik
- `User.cs` - Benutzermodell
- `Group.cs` - Gruppenmodell
- `TaskState.cs` - Enum für Aufgabenstatus mit Converter
- `UserRole.cs` - Enum für Benutzerrollen mit Converter
- `MainData.cs` - Zentraler Datenspeicher

### ViewModels
- `MainWindowViewModel.cs` - Hauptfenster-Logik
- `AddTaskWindowViewModel.cs` - Logik für Aufgaben erstellen
- `OpenTaskWindowViewModel.cs` - Logik für Aufgaben bearbeiten
- `AddUserWindowViewModel.cs` - Logik für Benutzer erstellen
- `ViewModelBase.cs` - Basis-ViewModel-Klasse

### Views
- `MainWindow.axaml` - Hauptfenster mit Aufgabenliste
- `AddTaskWindow.axaml` - Dialog zum Erstellen von Aufgaben
- `OpenTaskWindow.axaml` - Dialog zum Bearbeiten von Aufgaben
- `AddUserWindow.axaml` - Dialog zum Erstellen von Benutzern
- `ErrorWindow.axaml` - Fehleranzeigedialog

### Weitere Komponenten
- `ViewLocator.cs` - Automatische Zuordnung von Views zu ViewModels

## 🛠️ Technologie-Stack

- **Framework**: .NET 9.0
- **UI-Framework**: Avalonia UI 11.3.11
- **MVVM-Toolkit**: CommunityToolkit.Mvvm 8.2.1
- **UI-Bibliothek**: FluentAvaloniaUI 2.4.1
- **Datenserialisierung**: System.Text.Json

### NuGet-Pakete
```xml
<PackageReference Include="Avalonia" Version="11.3.11" />
<PackageReference Include="Avalonia.Controls.DataGrid" Version="11.3.11" />
<PackageReference Include="Avalonia.Desktop" Version="11.3.11" />
<PackageReference Include="Avalonia.ReactiveUI" Version="11.3.8" />
<PackageReference Include="Avalonia.Themes.Fluent" Version="11.3.11" />
<PackageReference Include="Avalonia.Fonts.Inter" Version="11.3.11" />
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.1" />
<PackageReference Include="FluentAvaloniaUI" Version="2.4.1" />
```

## 🚀 Installation & Setup

### Voraussetzungen
- .NET 9.0 SDK oder höher
- Visual Studio 2022 / JetBrains Rider / VS Code

### Projekt klonen und ausführen
```bash
# Repository klonen
git clone https://github.com/lbrandstaetterhtl/TaskDeskWithMVVM.git

# In das Projektverzeichnis wechseln
cd TaskDeskWithMVVM

# Projekt ausführen
dotnet run --project TaskDesk_version2/TaskDesk_version2.csproj
```

### Build
```bash
# Debug-Build
dotnet build

# Release-Build
dotnet build -c Release
```

## 📂 Projektstruktur

```
TaskDeskWithMVVM/
├── TaskDesk_version2/
│   ├── Models/                    # Datenmodelle
│   │   ├── Task.cs
│   │   ├── User.cs
│   │   ├── Group.cs
│   │   ├── TaskState.cs
│   │   ├── UserRole.cs
│   │   └── MainData.cs
│   ├── ViewModels/                # ViewModels (Logik)
│   │   ├── MainWindowViewModel.cs
│   │   ├── AddTaskWindowViewModel.cs
│   │   ├── OpenTaskWindowViewModel.cs
│   │   ├── AddUserWindowViewModel.cs
│   │   └── ViewModelBase.cs
│   ├── Views/                     # UI-Views
│   │   ├── MainWindow.axaml
│   │   ├── AddTaskWindow.axaml
│   │   ├── OpenTaskWindow.axaml
│   │   ├── AddUserWindow.axaml
│   │   └── ErrorWindow.axaml
│   ├── BluePrints/                # Zusätzliche Ressourcen
│   ├── App.axaml                  # App-Konfiguration
│   ├── App.axaml.cs
│   ├── Program.cs                 # Entry Point
│   ├── ViewLocator.cs             # View-ViewModel-Zuordnung
│   └── TaskDesk_version2.csproj
└── TaskDesk_version2.sln
```

## 💾 Datenspeicherung

Die Anwendung speichert alle Daten als JSON-Dateien im Ordner:
```
~/Desktop/TaskDeskData/
├── tasks.json
├── users.json
└── groups.json
```

### Beispiel: tasks.json
```json
[
  {
    "Id": 1,
    "Title": "Beispielaufgabe",
    "Description": "Dies ist eine Beispielaufgabe",
    "DueDate": "2026-01-31",
    "State": 1,
    "GroupIds": [1, 2],
    "UserIds": [1]
  }
]
```

## 🎯 Verwendung

### Aufgabe erstellen
1. Menü → "Add Task" anklicken
2. Titel, Beschreibung und Fälligkeitsdatum eingeben
3. Status auswählen
4. Benutzer und Gruppen zuweisen
5. "Save" klicken

### Aufgabe bearbeiten
1. Doppelklick auf eine Aufgabe in der Liste
2. Details bearbeiten
3. "Save" klicken

### Benutzer hinzufügen
1. Menü → "Add User" anklicken
2. Vollständiger Name, E-Mail, Passwort eingeben
3. Rolle auswählen
4. Gruppen zuweisen
5. "Save" klicken

## 🎨 UI-Features

- **Fluent Design** mit modernem Look
- **DataGrid** für Aufgabenliste
- **Hover-Effekte** auf Aufgabenelementen
- **Modale Dialoge** für Benutzerinteraktion
- **Fehlerbehandlung** mit benutzerfreundlichen Fehlerdialogen

## 🔧 Entwicklung

### MVVM-Pattern-Implementierung
Die Anwendung nutzt das CommunityToolkit.Mvvm für:
- `INotifyPropertyChanged` Implementierung
- `RelayCommand` für Befehle
- `ObservableCollection` für reaktive Listen

### Data Binding
Alle UI-Elemente sind über Data Binding mit den ViewModels verbunden:
```csharp
DataContext = new MainWindowViewModel();
```

## 📝 To-Do / Verbesserungen

- [ ] Suchfunktion für Aufgaben
- [ ] Filtermöglichkeiten nach Status/Gruppe/Benutzer
- [ ] Aufgaben-Sortierung
- [ ] Export-Funktion (PDF, Excel)
- [ ] Dark/Light Theme Toggle
- [ ] Benachrichtigungen für Fälligkeitstermine
- [ ] Datenbankanbindung (SQLite/PostgreSQL)
- [ ] Unit Tests hinzufügen
Contributions sind willkommen! Bitte erstelle einen Pull Request oder öffne ein Issue für Vorschläge.

---

**Hinweis**: Dies ist eine Lern-/Demonstrationsprojekt für das MVVM-Pattern mit Avalonia UI.
