namespace ZeroHedgeWS

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Client
open WebSharper.UI.Next.Html

[<JavaScript>]
module SearchClient =

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
                return Json.Deserialize<List<Story>> response 
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



    let AddNewSearchButton (page : int, keys:string) =
        let homeRef = sprintf "./"
        let newRef = sprintf "./search/%s/%d" keys page
        let newTitle = sprintf "Page %d" page

        let attr'href'1 =  Attr.Create "href" newRef
        let attrs'href = Seq.append [|attr'href'1;  |] [ ]

        let attr'home'href'1 =  Attr.Create "href" homeRef
        let attrs'home'href = Seq.append [|attr'home'href'1;  |] [ ]

        match page with
        | 0 ->
            li [ 
                aAttr attrs'home'href [
                    Doc.TextNode "Home"
                ]
            ]
        | _ -> 
            li [
                aAttr attrs'href [
                    Doc.TextNode newTitle
                ]
            ]
        
    let SearchPagination (keys : string)=
        let list1 = [0..9]
        List.map (fun x -> (x, keys)) list1
            |> List.map AddNewSearchButton
 
    // Search buttom HTML: we show it on each Stories page at top right corner
    let Search( keys: string ) =
        Var.Set v'search keys

        let newRef1 = sprintf "./search/%s/1" keys

        let attr'href1'1 =  Attr.Create "href" newRef1
        let attrs'href1 = Seq.append [|attr'href1'1;  |] [ ]

        let attr'div'right'1 = Attr.Create "role" "navigation"
        let attr'div'right'2 = Attr.Create "class" "pull-right"
        let attrs'div'right = Seq.append [|attr'div'right'1 |] [ attr'div'right'2]

        let attr'ul1 =  Attr.Create "id" "utilitiesbar"
        let attr'ul2 = Attr.Class "nav navbar-nav sm sm-collapsible"
        let attrs_ul = Seq.append [|attr'ul1 |] [ attr'ul2]
        
        let attr'input'1'1 =  Attr.Create "placeholder" "Search"
        let attr'input'1'2 =  Attr.Create "type" "text"
        //let attr'input'1'3 =  Attr.Create "class" "tftextinput2"
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
            ulAttr [attr.``class`` "nav navbar-nav" ] [
                for x in  SearchPagination(keys) do
                    yield x :> Doc

//                li [
//                    aAttr attrs'href1 [
//                        Doc.TextNode "Page 1"
//                    ]
//                ]
//                li [
//                    aAttr attrs'href1 [
//                        Doc.TextNode "Page 2"
//                    ]
//                ]
//                li [
//                    aAttr attrs'href1 [
//                        Doc.TextNode "Page 3"
//                    ]
//                ]
//                li [
//                    aAttr attrs'href1 [
//                        Doc.TextNode "Page 4"
//                    ]
//                ]
//                li [
//                    aAttr attrs'href1 [
//                        Doc.TextNode "Page 5"
//                    ]
//                ]
//                li [
//                    aAttr attrs'href1 [
//                        Doc.TextNode "Page 6"
//                    ]
//                ]
//                li [
//                    aAttr attrs'href1 [
//                        Doc.TextNode "Page 7"
//                    ]
//                ]
//                li [
//                    aAttr attrs'href1 [
//                        Doc.TextNode "Page 8"
//                    ]
//                ]
//                li [
//                    aAttr attrs'href1 [
//                        Doc.TextNode "Page 9"
//                    ]
//                ]
            ]
            divAttr attrs'div'right [
                nav [
                    ulAttr [] [
                        li [
                            formAttr attrs'form [
                                Doc.Input attrs'input'1 v'search
                                Doc.Button
                                <| "Search"
                                <| attrs'srch'btn
                                <| fun _ ->
                                    async {
                                        postList.Clear()                                
                                        let! stories = SearchArticles( JS.EncodeURIComponent(v'search.Value), 1 )
                                        stories
                                        |> List.iter( fun x -> 
                                            //JavaScript.Console.Log x.Introduction
                                            //JavaScript.Console.Log x.Reference
                                            postList.Add x )
                                    }
                                    |> Async.Start

                            ]
                    

                        ]
                    ]   
                ]
            ]        
        ]

    let Main (keys : string, page :int) =
        let attr'divid =  Attr.Create "id" "blogItems"
        let attr'divstyle = Attr.Style "margin-top" "60px"
        let attr'divclass = Attr.Class "col-md-12"
        let attrs_div = Seq.append [|attr'divid; attr'divstyle |] [ attr'divclass]
        Var.Set v'search keys

        async {
            let! stories = SearchArticles( v'search.Value, page )
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
