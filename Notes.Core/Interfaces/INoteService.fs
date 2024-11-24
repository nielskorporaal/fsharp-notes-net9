namespace Notes.Core.Interfaces

open Notes.Core.Models

type INoteService =
    abstract member CreateNote : string -> int
    abstract member GetNoteById : int -> Option<PostData>
    abstract member GetAllNotes : unit -> PostData list