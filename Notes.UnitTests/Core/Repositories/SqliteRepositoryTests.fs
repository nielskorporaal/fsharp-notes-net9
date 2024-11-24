module Notes.UnitTests.Sqlite

open Xunit
open Notes.Core.Models
open Notes.Core.Repositories
open Notes.Core.Interfaces
open Microsoft.Data.Sqlite

let createInMemorySqliteRepository () =
    let connectionString = "Data Source=:memory:" // In-memory SQLite DB
    let connection = new SqliteConnection(connectionString)
    do connection.Open()
    use cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS Notes (Id INTEGER PRIMARY KEY, Content TEXT)", connection)
    cmd.ExecuteNonQuery() |> ignore
    connection.Close()
    new SqliteRepository(connectionString)

[<Fact>]
let ``AddNote adds a note and returns a valid ID`` () =
    // Arrange
    let repository = createInMemorySqliteRepository()

    // Act
    let newNoteId = (repository :> INoteRepository).AddNote("Test Note")
    let retrievedNote = (repository :> INoteRepository).GetNote(int newNoteId)

    // Assert
    Assert.True(newNoteId > 0L)
    match retrievedNote with
    | Some(note) -> Assert.Equal("Test Note", note.content)
    | None -> Assert.Fail("Note not found")

[<Fact>]
let ``GetNote returns None for a non-existent note`` () =
    // Arrange
    let repository = createInMemorySqliteRepository()

    // Act
    let retrievedNote = (repository :> INoteRepository).GetNote(999)

    // Assert
    Assert.Null(retrievedNote)

[<Fact>]
let ``GetAllNotes returns all added notes`` () =
    // Arrange
    let repository = createInMemorySqliteRepository()
    (repository :> INoteRepository).AddNote("First Note") |> ignore
    (repository :> INoteRepository).AddNote("Second Note") |> ignore

    // Act
    let allNotes = (repository :> INoteRepository).GetAllNotes()

    // Assert
    Assert.Equal(2, List.length allNotes)
    Assert.Equal("First Note", (List.head allNotes).content)
    Assert.Equal("Second Note", (List.head (List.tail allNotes)).content)
