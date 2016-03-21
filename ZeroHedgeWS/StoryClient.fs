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
        //JavaScript.Console.Log post.Title
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
                let id1 = JavaScript.JS.DecodeURIComponent id
                JavaScript.Console.Log id1
                let pageURL = sprintf "./api/story/%s" id1
                let! response = Ajax "GET" pageURL (null)
                //JavaScript.Console.Log "Get zerohedge story"                
                return Json.Deserialize<string> response 
        }

    let Main (reference :string) =

        let attr'divid =  Attr.Create "id" "blogItems"
        let attr'divclass = Attr.Class "col-md-12"
        let attrs_div = Seq.append [|attr'divid|] [ attr'divclass]

        let mutable body = ""
        let mutable title = ""
        let mutable introduction = ""

        let v'body = Var.Create body

        async {
            let! story = GetStory reference
            
            JQuery.JQuery.Of("#bodyid").Children().Remove().Ignore
            let domNode = JQuery.Of( sprintf "%s%s%s" "<div>" story "</div>").Get(0)
            JQuery.JQuery.Of("#bodyid").Append( domNode :?> Dom.Element ).Ignore
            //JavaScript.Console.Log story
        }
        |> Async.Start
            
        
        divAttr[attr.``class`` "container top-padding-med ng-scope"][
            divAttr [attr.``id`` "bodyid"] [
              Doc.TextNode ""
            ]
        ]
