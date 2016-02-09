namespace ZeroHedgeWS

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html

[<JavaScript>]
module Client =

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

    let postList = ListModel.Create (fun i ->  string i.Reference) []
    
    let v'page = Var.Create 1

    let doc'header (post : Story) = 
        JavaScript.Console.Log post.Title
        let domNode = JQuery.Of( sprintf "%s%s%s" "<div>" post.Title "</div>").Get(0)
        let href = sprintf "%s%s" "/story/" post.Reference
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
    let GetPageArticles (id : int) : Async<List<Story>> =
        async {
                let pageURL = sprintf "/api/page/%d" id
                let! response = Ajax "GET" pageURL (null)
                //JavaScript.Console.Log "Get zerohedge page"
                return Json.Deserialize<List<Story>> response 
        }

    let v'blog =
        View.Do{
            return 1 }

        |> View.MapAsync ( fun i -> async{            
            return postList
            }  )

    let Main (pageID : int) =
        let attr'divid =  Attr.Create "id" "blogItems"
        let attr'divclass = Attr.Class "col-md-12"
        let attrs_div = Seq.append [|attr'divid|] [ attr'divclass]
        Var.Set v'page pageID

        async {
            let! stories = GetPageArticles v'page.Value
            stories
            |> List.iter( fun x -> 
                JavaScript.Console.Log x.Title
                JavaScript.Console.Log x.Reference
                postList.Add x )
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
                v'blog  
                |> View.Map ( fun list -> 
                    ListModel.View postList
                    |> Doc.BindSeqCached (fun p -> 
                        doc p
                    )
                )
                |> Doc.EmbedView
            ]

            //]
        ]
