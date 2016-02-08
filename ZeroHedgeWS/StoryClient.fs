namespace ZeroHedgeWS

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html

[<JavaScript>]
module StoryClient =

    /// General function to send an AJAX request with a body.
    let Ajax (methodtype: string) (url: string) (serializedData: string) : Async<string> =
        Async.FromContinuations <| fun (ok, ko, _) ->
            JQuery.Ajax(
                JQuery.AjaxSettings(
                    Url = url,
                    Type = As<JQuery.RequestType> methodtype,
                    ContentType = "application/json",
                    DataType = JQuery.DataType.Text,
                    Data = serializedData,
                    Success = (fun (result, _, _) -> ok (result :?> string)),
                    Error = (fun (jqXHR, _, _) -> ko (System.Exception(jqXHR.ResponseText)))))
            |> ignore

    let doc'header (post : Story) = 
        JavaScript.Console.Log post.Title
        let domNode = JQuery.Of( sprintf "%s%s%s" "<div>" post.Title "</div>").Get(0)
        let href = sprintf "%s%s" "/story?ref=" post.Reference
        let a'attr'1 =  Attr.Create "href" href
        let a'attr'list = [ a'attr'1 ] :> seq<Attr>

        divAttr[Attr.Class "panel-heading"][
            aAttr a'attr'list [
                Doc.Static(domNode :?> Dom.Element)
            ]
            
        ]

    let doc'body (post : Story) = 
        let attr'container1 =  Attr.Create "class" "container"
        let attr'container2 = Attr.Style "margin-top" "100px"
        let attrs'container = Seq.append [| attr'container1|] [ attr'container2]

        let attr'title1 =  Attr.Create "placeholder" "Title"
        let attr'title2 = Attr.Class "form-control"
        let attrs'title = Seq.append [| attr'title1|] [ attr'title2]


        let domNode = JQuery.Of(sprintf "%s%s%s" "<div>" post.Introduction "</div>").Get(0)

        divAttr[Attr.Class "panel-body" ][
            Doc.Static(domNode :?> Dom.Element)
            p[Doc.TextNode post.Published]
        ]

    let doc post  =
        divAttr [Attr.Class "panel panel-primary";][
            doc'header post
            doc'body post
            ]

    /// Use Json.Serialize and Deserialize to send and receive data to and from the server.
    let GetStory (id : string) : Async<string> =
        async {
                let pageURL = sprintf "/api/story/%s" id
                let! response = Ajax "GET" pageURL (null)
                JavaScript.Console.Log "Get zerohedge story"                
                return Json.Deserialize<string> response 
        }

    let Main (reference :string) =

        let attr'divid =  Attr.Create "id" "blogItems"
        let attr'divclass = Attr.Class "col-md-12"
        let attrs_div = Seq.append [|attr'divid|] [ attr'divclass]

        let mutable body = ""
        let mutable title = ""
        let mutable introduction = ""

        async {
            let! story = GetStory reference
            
            body <- story
            //introduction <- story.Introduction
            //title <- story.Title
            JavaScript.Console.Log story
        }
        |> Async.Start
            
        
        

        

//        let rvInput = Var.Create ""
//        let submit = Submitter.CreateOption rvInput.View
//        let vReversed =
//            submit.View.MapAsync(function
//                | None -> async { return "" }
//                | Some input -> Server.DoSomething input
//            )
//        div [
//            Doc.Input [] rvInput
//            Doc.Button "Send" [] submit.Trigger
//            hr []
//            h4Attr [attr.``class`` "text-muted"] [text "The server responded:"]
//            divAttr [attr.``class`` "jumbotron"] [h1 [textView vReversed]]
//        ]
        divAttr[attr.``class`` "container top-padding-med ng-scope"][
            //divAttr [attr.``class`` "panel-primary"] [
            divAttr attrs_div [
                Doc.TextNode body
            ]

            //]
        ]
