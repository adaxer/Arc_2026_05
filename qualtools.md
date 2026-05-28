| Tool/Feature | Eingebaut in VS? | Kostenlos? | Zweck | Typ |
|---|---:|---:|---|---|
| Roslyn / .NET Analyzers | Ja | Ja | Code Quality, Style, Maintainability | Statische Analyse |
| Code Metrics | Ja | Ja | Maintainability Index, Cyclomatic Complexity, Coupling, LOC | Metriken |
| Code Cleanup | Ja | Ja | Formatierung, Usings, Style-Fixes | Style |
| EditorConfig | Ja | Ja | Teamweite Regeln für Coding Style | Konfiguration |
| StyleCop Analyzers | Nein (NuGet) | Ja | Strenge C#-Style-Regeln | Statische Analyse |
| SonarAnalyzer for .NET | Nein (NuGet) | Ja | Bugs, Code Smells, Security Hotspots | Statische Analyse |
| SonarQube Community | Extern Server + CI | Ja | Zentrale Analyseplattform, Trends, Technical Debt, Security, Coverage | Plattform |
| SonarQube Developer/Enterprise | Extern Server + CI | Nein | Erweiterte Regeln, Branch-/PR-Analyse, Security | Plattform |
| SonarLint | VS Extension | Ja | Lokale Sonar-Prüfung direkt im Editor | IDE-Integration |
| Meziantou.Analyzer | Nein (NuGet) | Ja | Viele praxisnahe .NET-Regeln | Statische Analyse |
| Roslynator | Extension / NuGet / CLI | Ja | Refactorings + Analyzer | Refactoring |
| ReSharper | Extension | Nein | Analyse, Navigation, Refactoring | IDE-Produktivität |
| NDepend | Extension / Extern | Nein | Architekturprüfung, Dependency Graphs, Qualitätsmetriken | Architektur |
| dotnet format | CLI | Ja | Formatierung und automatische Fixes | CLI |
| Security Code Scan | NuGet | Ja | Sicherheitsanalyse für .NET | Security |
| GitHub CodeQL | CI / GitHub | Teilweise | Security- und Datenflussanalyse | Security |

Meine pragmatische Kombi wäre: eingebaute .NET Analyzers + .editorconfig + Code Cleanup + Meziantou.Analyzer + SonarAnalyzer. Das ist kostenlos, CI-tauglich und nervt nicht ganz so sehr wie ein pedantischer Compiler mit Kaffee-Entzug.