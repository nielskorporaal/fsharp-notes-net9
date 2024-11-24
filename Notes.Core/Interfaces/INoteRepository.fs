namespace Notes.Core.Interfaces

open Notes.Core.Models

type INoteRepository =
    abstract member AddNote : string -> int64
    abstract member GetNote : int -> PostData option
    abstract member GetAllNotes : unit -> PostData list
