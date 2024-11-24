namespace Notes.Core.Repositories

open System.Collections.Generic
open Notes.Core.Interfaces
open Notes.Core.Models

type InMemoryRepository() =
    let notes = Dictionary<int, PostData>()
    let mutable nextId = 1

    interface INoteRepository with
        member this.AddNote(content) =
            let id = nextId
            nextId <- nextId + 1
            notes.[id] <- { content = content }
            id

        member this.GetNote(id) =
            match notes.TryGetValue(id) with
            | true, note -> Some note
            | false, _ -> None

        member this.GetAllNotes() =
            notes.Values |> List.ofSeq
