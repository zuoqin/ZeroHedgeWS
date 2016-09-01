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
    let mutable v'mode = Var.Create 0

    let doc'header (post : Story) = 
        //JavaScript.Console.Log post.Title
        let domNode = JQuery.Of( sprintf "%s%s%s" "<div>" post.Title "</div>").Get(0)
        let href = sprintf "%s%s" "./story/" post.Reference
        let a'attr'1 =  Attr.Create "href" href
        let a'attr'list = [ a'attr'1 ] :> seq<Attr>


        if post.Reference.Length > 0 then
            divAttr[Attr.Class "panel-heading"][
                h3Attr [attr.``class`` "panel-title"] [
                    aAttr a'attr'list [
                        Doc.Static(domNode :?> Dom.Element)
                    ]
                ]            
            ]
        else
            divAttr[Attr.Class "panel-heading"][
                h3Attr [attr.``class`` "panel-title"] [
                    a [
                        Doc.Static(domNode :?> Dom.Element)
                    ]
                ]            
            ]

    let doc'body (post : Story) = 
        let attr'title1 =  Attr.Create "placeholder" "Title"
        let attr'title2 = Attr.Class "form-control"
        let attrs'title = Seq.append [| attr'title1|] [ attr'title2]


        let domNodeIntro = JQuery.Of(sprintf "%s%s%s" "<div>" post.Introduction "</div>").Get(0)
        let domNodePublished = JQuery.Of(sprintf "%s%s%s" "<div>" post.Published "</div>").Get(0)

        divAttr[Attr.Class "panel-body" ][
            Doc.Static(domNodeIntro :?> Dom.Element)
            Doc.Static(domNodePublished :?> Dom.Element)
            //p[Doc.TextNode post.Published]
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
                let toJSONresponse = (Json.Deserialize<getpage> response).Data

                return toJSONresponse
        }

    let SearchArticles (keys : string, page : int) : Async<Story list> =
        async {
                
                let pageURL = sprintf "./api/search/%s/%d" keys page
                let! response = Ajax "GET" pageURL (null)
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
            if page = 0 then
                v'mode.Value <- 0

            let mutable stories : List<Story> = []
            //JavaScript.Console.Log( "Mode: " + v'mode.Value.ToString() )
            //JavaScript.Console.Log( "Page: " + page.ToString() )

            if v'mode.Value = 0 then
                let! stories1 = GetPageArticles( page )
                stories <- stories1

            else
                let! stories1 = SearchArticles( v'search.Value, page - 1 )
                stories <- stories1

            postList.Clear()
            stories
            |> List.iter( fun x -> 
                postList.Add x )
        }
        |> Async.Start

    let OnSearchButtonClick (keys : string)= 
        async {
            v'mode.Value <- 1
            let! stories = SearchArticles( keys, 0 )
            postList.Clear()
            if stories.Length <= 1 then
                v'mode.Value <- 0

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
        List.map (fun x -> (x)) [1..9]
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

        let attr'topmenu'1 =  Attr.Create "role" "navigation"
        let attr'topmenu'2 =  Attr.Create "class" "navbar navbar-inverse navbar-fixed-top"
        let attrs'topmenu = Seq.append [|attr'topmenu'1|] [ attr'topmenu'2]

        let attr'small'btn1 =  Attr.Create "aria-controls" "bs-navbar"
        let attr'small'btn2 =  Attr.Create "aria-expanded" "true"
        let attr'small'btn3 =  Attr.Create "class" "navbar-toggle"
        let attr'small'btn4 =  Attr.Create "data-target" "#bs-navbar"
        let attr'small'btn5 =  Attr.Create "data-toggle" "collapse"
        let attr'small'btn6 =  Attr.Create "type" "button"
        let attrs'small'btn = Seq.append [|attr'small'btn1; attr'small'btn2; attr'small'btn3; attr'small'btn4; attr'small'btn5; attr'small'btn6;  |] [ ]


        let attr'small'a'1 =  Attr.Create "class" "navbar-brand"
        let attr'small'a'2 =  Attr.Create "href" "/page/0"
        let attrs'small'a = Seq.append [|attr'small'a'1; attr'small'a'2;  |] [ ]


        let attr'small'ul'1 =  Attr.Create "class" "nav navbar-nav"
        //let attr'small'ul'2 =  Attr.Create "id" "bs-navbar"
        let attrs'small'ul = Seq.append [|attr'small'ul'1;  |] [ ]

        let attr'small'div'1 =  Attr.Create "class" "navbar-collapse collapse"
        let attr'small'div'2 =  Attr.Create "id" "bs-navbar"
        let attrs'small'div = Seq.append [|attr'small'div'1;  attr'small'div'2;  |] [ ]

        let attr'page'button'1 =  Attr.Create "class" "pageButton"
        //let attr'page'button'2 =  Attr.Create "id" "pageButton"
        let attrs'page'button = Seq.append [|attr'page'button'1;  |] [ ]

        divAttr attrs'topmenu [
            divAttr [attr.``class`` "navbar-header"][
                buttonAttr attrs'small'btn [
                    spanAttr[attr.``class`` "sr-only"][Doc.TextNode "Toggle navigation"]
                    spanAttr[attr.``class`` "icon-bar"][]
                    spanAttr[attr.``class`` "icon-bar"][]
                    spanAttr[attr.``class`` "icon-bar"][]
                ]
                //aAttr attrs'small'a [Doc.TextNode "Home"]
                Doc.Button
                <| "Home"
                <| attrs'page'button
                <| fun _ -> 
                    OnPageButtonClick 0
            ]        



            divAttr attrs'small'div [
                divAttr [attr.``align`` "left"] [
                    ulAttr attrs'small'ul [
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
                                        OnSearchButtonClick( v'search.Value )
                                ]
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