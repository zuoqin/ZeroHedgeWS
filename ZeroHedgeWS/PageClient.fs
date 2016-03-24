namespace ZeroHedgeWS

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html

[<JavaScript>]
module PageClient =

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
    let v'search = Var.Create ""
    // this variable just fake input
    let v'search2 = Var.Create ""

    let doc'header (post : Story) = 
        //JavaScript.Console.Log post.Title
        let domNode = JQuery.Of( sprintf "%s%s%s" "<div>" post.Title "</div>").Get(0)
        let href = sprintf "%s%s" "./story/" post.Reference
        let a'attr'1 =  Attr.Create "href" href
        let a'attr'list = [ a'attr'1 ] :> seq<Attr>

        divAttr[Attr.Class "panel-heading"][
            h3Attr [attr.``class`` "panel-title"] [
                aAttr a'attr'list [
                    Doc.Static(domNode :?> Dom.Element)
                ]
            ]            
        ]

    let doc'body (post : Story) = 
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
                let pageURL = sprintf "./api/page/%d" id
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


    let OnPageButtonClick (page : int)= 
        async {
            JS.Window.SessionStorage.SetItem("page", page.ToString())
            let! stories = GetPageArticles page
            postList.Clear()
            stories
            |> List.iter( fun x -> 
                postList.Add x )
        }
        |> Async.Start



    let AddNewPageButton (page : int) =
        let homeRef = sprintf "./"
        let newRef = sprintf "./page/%d" page
        let newTitle = sprintf "Page %d" page

        let attr'href'1 =  Attr.Create "href" newRef
        let attrs'href = Seq.append [|attr'href'1;  |] [ ]

        let attr'home'href'1 =  Attr.Create "href" homeRef
        let attrs'home'href = Seq.append [|attr'home'href'1;  |] [ ]


        let attr'page'button'1 =  Attr.Create "class" "pageButton"
        let attr'page'button'2 =  Attr.Create "id" "pageButton"
        let attrs'page'button = Seq.append [|attr'page'button'1;  |] [ ]



        match page with
        | x when (x < 10) && (x > 0) ->
            let idAttr = sprintf "page%dli" page
            liAttr [attr.``id`` idAttr ] [
                Doc.Button
                <| ("Page " + page.ToString())
                <| attrs'page'button
                <| fun _ -> 
                    OnPageButtonClick x
            ]
        | _ -> 
            li [
                Doc.Button
                <| "Home"
                <| attrs'page'button
                <| fun _ -> 
                    OnPageButtonClick 0
            ]


    let SitePagination =
        let list1 = [0..9]
        List.map (fun x -> (x)) list1
            |> List.map AddNewPageButton


    // Search buttom HTML: we show it on each Stories page at top right corner
    let Search =
        let attr'ul1 =  Attr.Create "style" "margin-left:0px; padding: 0px;"        
        let attrs_ul = Seq.append [|attr'ul1 |] [ ]
        
        let attr'div'right'1 = Attr.Create "role" "navigation"
        let attr'div'right'2 = Attr.Create "class" "pull-right"
        let attrs'div'right = Seq.append [|attr'div'right'1 |] [ attr'div'right'2]

        let attr'input'1'1 =  Attr.Create "placeholder" "Search"
        let attr'input'1'2 =  Attr.Create "type" "text"
        let attr'input'1'4 =  Attr.Create "maxlength" "120"
        let attrs'input'1 = Seq.append [|attr'input'1'1;attr'input'1'2 |] [ attr'input'1'4]

        let attr'input'2'1 =  Attr.Create "type" "submit"
        let attr'input'2'2 =  Attr.Create "value" " > "
        let attr'input'2'3 =  Attr.Create "class" "tfbutton2"
        let attrs'input'2 = Seq.append [|attr'input'2'1; attr'input'2'2 |] [ attr'input'2'3]

        let attr'form'1 =  Attr.Create "class" "form-wrapper cf"
        let attrs'form = Seq.append [|attr'form'1|] [ ]


        let attr'srch'btn1 =  Attr.Create "type" "submit"
        let attrs'srch'btn = Seq.append [|attr'srch'btn1;  |] [ ]


        divAttr [attr.``class`` "navbar-collapse collapse"][
            divAttr [attr.``align`` "left"] [
                ulAttr [attr.``class`` "nav navbar-nav" ] [
                    for x in  SitePagination do
                        yield x :> Doc
                ]
            ]
            divAttr attrs'div'right [
                nav [
                    ulAttr attrs_ul [
                        li [
                            formAttr attrs'form [
                                Doc.Input attrs'input'1 v'search
                                Doc.Button
                                <| "Search"
                                <| attrs'srch'btn
                                <| fun _ -> 
                                    let keyEncode = JS.EncodeURIComponent(v'search.Value)
                                    let newLocation = sprintf "./search/%s/0" keyEncode
                                    JS.Window.Location.Href <-newLocation
                            ]
                    

                        ]
                    ]   
                ]
            ]
        ]
    let Main (pageID : int) =
        let attr'divid =  Attr.Create "id" "blogItems"
        let attr'divstyle = Attr.Style "margin-top" "60px"
        let attr'divclass = Attr.Class "col-md-12"
        let attrs_div = Seq.append [|attr'divid; attr'divstyle |] [ attr'divclass]

        let sessionPage = JS.Window.SessionStorage.GetItem("page")
        let mutable newPageID = pageID
        if sessionPage <> null  && sessionPage.Length > 0 then
            newPageID <- JS.ParseInt(sessionPage)
        Var.Set v'page newPageID

        async {
            let! stories = GetPageArticles v'page.Value
            stories
            |> List.iter( fun x -> 
                //JavaScript.Console.Log x.Title
                //JavaScript.Console.Log x.Reference
                postList.Add x )
        }
        |> Async.Start
            
        
        let attr'div'fixed1 =  Attr.Create "role" "navigation"
        let attr'div'fixed2 =  Attr.Create "class" "navbar navbar-inverse navbar-fixed-top"
        let attr'div'fixed = Seq.append [|attr'div'fixed1|] [ attr'div'fixed2]
        
        let attr'container1 =  Attr.Create "class" "container"
        let attr'container2 = Attr.Style "margin-top" "100px"
        let attrs'container = Seq.append [| attr'container1|] [ attr'container2]


        let attr'container1 =  Attr.Create "class" "container"
        let attr'container2 = Attr.Style "margin-top" "100px"
        let attrs'container = Seq.append [| attr'container1|] [ attr'container2]

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