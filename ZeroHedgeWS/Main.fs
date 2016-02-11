namespace ZeroHedgeWS

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server



type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/about">] About
    | [<EndPoint "/page">] Page of id : int
    | [<EndPoint "/search">] Search of id : string * page : int
    | [<EndPoint "/story">] Story of id : string
    | [<EndPoint "/api">] Api of id: ApiEndPoint

and ApiEndPoint =
    | [<EndPoint "GET /page">] GetPage of int
    | [<EndPoint "GET /story">] GetStory of string
    | [<EndPoint "POST /search">] PostSearch of string
    | [<EndPoint "GET /search">] GetSearch of string * int

//and StoryPoint =
//    | [<Query("id")>] Story of string



module Templating =
    open WebSharper.UI.Next.Html

    type MainTemplate = Templating.Template<"Views\Main.html">
    type StoryTemplate = Templating.Template<"Views\Story.html">
    type PageTemplate = Templating.Template<"Views\Page.html">
    type SearchTemplate = Templating.Template<"Views\Search.html">

    // Compute a menubar where the menu item for the given endpoint is active
    let MenuBar (ctx: Context<EndPoint>) endpoint : Doc list =
        let attr'menu'style = Attr.Create "class" "divhidden"
        let attr'menu'styles = Seq.append [attr'menu'style] []
        [

            divAttr attr'menu'styles [
                Doc.TextNode "kjkj"
            ]
        ]

    let MainView ctx action title body search =
        Content.Page(
            MainTemplate.Doc(
                title = title,
                search = search,
                body = body
            )
        )

    let StoryView ctx action title body =
        Content.Page(
            StoryTemplate.Doc(
                title = title,
                menubar = MenuBar ctx action,
                body = body
            )
        )

    let PageView ctx action title body search =
        Content.Page(
            PageTemplate.Doc(
                title = title,
                search = search,
                body = body
            )
        )

    let SearchView ctx action title body search =
        Content.Page(
            SearchTemplate.Doc(
                title = title,
                search = search,
                body = body
            )
        )



module Site =
    open WebSharper.UI.Next.Html

    let ApiContent (ctx: Context<EndPoint>) (action: ApiEndPoint)  =
        match action with
        | GetPage id ->
                let result = ZeroHedgeAPI.ApplicationLogic.getPage id
                Content.Json(result.ToArray())
        | GetStory id ->            
            Content.Json (ZeroHedgeAPI.ApplicationLogic.getStory id)

        | PostSearch keys ->
            Content.Json (ZeroHedgeAPI.ApplicationLogic.postSearch keys)
        | GetSearch( keys, page ) ->
            Content.Json (ZeroHedgeAPI.ApplicationLogic.getSearch( keys, page )) 


    let HomePage (ctx :Context<EndPoint>) =
        let attr'div'right1 =  Attr.Create "role" "navigation"
        let attr'div'right2 =  Attr.Create "class" "pull-right"
        let attrs'div'right = Seq.append [|attr'div'right1|] [ attr'div'right2]


        Templating.MainView ctx EndPoint.Home "Home"
            [
                div [client <@ PageClient.Main(0) @>]
            ]
            [
                divAttr attrs'div'right [client <@ PageClient.Search @>]
            ]

    let AboutPage ctx =
        Templating.MainView ctx EndPoint.About "About"
            [
                h1 [text "About"]
            ]
            [
                p [text "This is a template WebSharper client-server application."]
            ]

    let StoryPage ctx reference =
        Templating.StoryView ctx EndPoint.Story "Story" [
            div [client <@ StoryClient.Main(reference) @>]
        ]

    let PagePage( ctx, id : int ) =
        let attr'div'right1 =  Attr.Create "role" "navigation"
        let attr'div'right2 =  Attr.Create "class" "pull-right"
        let attrs'div'right = Seq.append [|attr'div'right1|] [ attr'div'right2]
        Templating.PageView ctx EndPoint.Page "Page"
            [
                div [client <@ PageClient.Main(id) @>]
            ] 
            [
                divAttr attrs'div'right [client <@ PageClient.Search @>]
            ]

    let SearchPage( ctx, keys : string, page : int ) =
        let attr'div'container1 =  Attr.Create "class" "container"
        let attrs'div'container = Seq.append  [ attr'div'container1] []
        Templating.SearchView ctx EndPoint.Page "Page"
            [
                div [client <@ SearchClient.Main(keys, page) @>]
            ]
            [
                divAttr attrs'div'container [client <@ SearchClient.Search( keys ) @>]
            ]



    [<Website>]
    let Main =

        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Api id -> 
                let result = ApiContent(ctx)(id)
                result
            | EndPoint.Home -> HomePage (ctx)
            | EndPoint.Page id -> PagePage( ctx ,id )
            | EndPoint.Search( keys, page) -> SearchPage( ctx , keys, page )
            | EndPoint.About -> AboutPage ctx
            | EndPoint.Story id -> StoryPage ctx id
        )
