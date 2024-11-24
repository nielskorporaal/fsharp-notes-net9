namespace Notes.WebApi.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open Notes.Core.Models
open Notes.Core.Services

[<ApiController>]
[<Route("[controller]")>]
type NoteController (logger : ILogger<NoteController>, noteService : NoteService) =

    inherit ControllerBase()

    [<HttpGet("{id}")>]
    member this.Get(id:int) =
        logger.LogInformation("Received GET request for id: {id}", id)
        match noteService.GetNoteById(id) with
        | Some(note) -> 
            sprintf "Note: %s" note.content |> this.Ok :> IActionResult
        | None -> 
            this.NotFound() :> IActionResult

    [<HttpPost>]
    member this.Post([<FromBody>] data : PostData) =
        logger.LogInformation("Received POST request with content: {content}", data.content)
        let newNoteId = noteService.CreateNote(data.content)
        let response: PostResponse = {
            id = newNoteId
            content = data.content
        }
        logger.LogInformation($"Created note: %A{response}")
        let location = sprintf "/note/%d" newNoteId
        this.Created(location, response) :> IActionResult