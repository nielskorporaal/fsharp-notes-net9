namespace Notes.Core.Repositories

open Microsoft.Data.Sqlite
open Notes.Core.Interfaces
open Notes.Core.Models

type SqliteRepository(connectionString: string) =
    
    let connection = new SqliteConnection(connectionString)

    do connection.Open()

    let createTable() =
        use cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS Notes (Id INTEGER PRIMARY KEY, Content TEXT)", connection)
        cmd.ExecuteNonQuery() |> ignore

    do createTable()

    interface INoteRepository with
        member this.AddNote(content) =
            use cmd = new SqliteCommand("INSERT INTO Notes (Content) VALUES (@content); SELECT last_insert_rowid();", connection)
            cmd.Parameters.AddWithValue("@content", content) |> ignore
            let id = cmd.ExecuteScalar() :?> int64
            id

        member this.GetNote(id) =
            use cmd = new SqliteCommand("SELECT Content FROM Notes WHERE Id = @id", connection)
            cmd.Parameters.AddWithValue("@id", id) |> ignore
            use reader = cmd.ExecuteReader()
            if reader.Read() then
                Some { content = reader.GetString(0) }
            else
                None

        member this.GetAllNotes() =
            use cmd = new SqliteCommand("SELECT Id, Content FROM Notes", connection)
            use reader = cmd.ExecuteReader()
            let mutable notes = []
            while reader.Read() do
                let note = { content = reader.GetString(1) }
                notes <- note :: notes
            List.rev notes

