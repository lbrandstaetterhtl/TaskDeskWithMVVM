# TaskDesk with MVVM

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
│   │   └── MainData. cs
│   ├── ViewModels/                # ViewModels (Logik)
│   │   ├── MainWindowViewModel.cs
│   │   ├── AddTaskWindowViewModel.cs
│   │   ├── OpenTaskWindowViewModel.cs
│   │   ├── AddUserWindowViewModel. cs
│   │   └── ViewModelBase.cs
│   ├── Views/                     # UI-Views
│   │   ├── MainWindow.axaml
│   │   ├── AddTaskWindow.axaml
│   │   ├── OpenTaskWindow.axaml
│   │   ├── AddUserWindow. axaml
│   │   └── ErrorWindow.axaml
│   ├── BluePrints/                # Zusätzliche Ressourcen
│   ├── App.axaml                  # App-Konfiguration
│   ├── App.axaml. cs
│   ├── Program.cs                 # Entry Point
│   ├── ViewLocator.cs             # View-ViewModel-Zuordnung
│   └── TaskDesk_version2.csproj
└── TaskDesk_version2.sln
```

Die Anwendung speichert alle Daten als JSON-Dateien im Ordner: 
```
~/Desktop/TaskDeskData/
├── tasks.json
├── users.json
└── groups.json
```

```json
Tasks
[
  {
    "Id": 1,
    "Title": "Beispielaufgabe",
    "Description": "Dies ist eine Beispielaufgabe",
    "DueDate":  "2026-01-31",
    "State": 1,
    "GroupIds": [1, 2],
    "UserIds": [1]
  }
]

Users
[
  {
    "Id": 0,
    "FullName": "Test",
    "Email": "Test",
    "Password": "Test",
    "Role": 0,
    "GroupIds": [],
    "TaskIds": []
  },
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

---
