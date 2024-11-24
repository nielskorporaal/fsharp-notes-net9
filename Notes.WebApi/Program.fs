namespace Notes.WebApi
#nowarn "20"
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

module Program =
    open Notes.Core.Interfaces
    open Notes.Core.Repositories
    open Notes.Core.Services
    open System
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        let mutable isEnabled = false
        let switchFound = AppContext.TryGetSwitch("Sqlite.IsEnabled", &isEnabled)

        let repository =
            if switchFound && isEnabled then
                let connectionString = "Data Source=notes.db"
                SqliteRepository(connectionString) :> INoteRepository
            else
                InMemoryRepository() :> INoteRepository

        builder.Services.AddSingleton<INoteRepository>(repository)

        builder.Services.AddSingleton<NoteService>()

        builder.Services.AddControllers()

        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseAuthorization()
        app.MapControllers()

        app.Run()

        exitCode
