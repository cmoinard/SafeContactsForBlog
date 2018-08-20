module AsyncResult

open Elmish

type AsyncResult<'InProgress, 'Done> =
| InProgress of 'InProgress
| Done of 'Done
| Error of exn

let inline ofAsyncCmd task arg toMsg = 
    Cmd.ofAsync 
        task 
        arg 
        (Done >> toMsg) 
        (Error >> toMsg) 

let inline ofAsyncCmdWithMap task arg toMsg map = 
    Cmd.ofAsync 
        task 
        (map arg)
        (fun _ -> Done arg |> toMsg) 
        (Error >> toMsg)