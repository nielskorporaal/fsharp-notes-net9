namespace Notes.Core.Services

open Notes.Core.Interfaces

type NoteService(repository: INoteRepository) =
    member this.CreateNote(content: string) =
        repository.AddNote(content)

    member this.GetNoteById(id: int) =
        repository.GetNote(id)

    member this.GetAllNotes() =
        repository.GetAllNotes()
