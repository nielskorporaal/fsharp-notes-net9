# fsharp-notes-net9

Simple notes taking web API built in F# using .NET 9.0. 
This project provides a basic implementation for creating and retrieving notes.

.NET 9 runtime features demonstrated in this project:
- [Feature Switches](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.codeanalysis.featureswitchdefinitionattribute?view=net-9.0)

# Feature Switches
A feature switch is a runtime toggle that can enable or disable specific functionality in a .NET application. In this web API the feature switch is used to toggle between in-memory storage and persistent storage using SQLite.
