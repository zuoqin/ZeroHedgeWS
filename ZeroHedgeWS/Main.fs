namespace ZeroHedgeWS

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server



type EndPoint =
    | [<EndPoint "/about">] About
    | [<EndPoint "/api">] Api of id: ApiEndPoint

and ApiEndPoint =
    | [<EndPoint "GET /page">] GetPage of int
    | [<EndPoint "GET /story">] GetStory of string
    | [<EndPoint "POST /search">] PostSearch of string
    | [<EndPoint "GET /search">] GetSearch of string * int




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
            async{
                let! result = ZeroHedgeAPI.ApplicationLogic.getPage id
                return Content.Json(result.ToArray())          
            }
        | GetStory id ->
            async{
                let! result = ZeroHedgeAPI.ApplicationLogic.getStory id
                return Content.Json(result)          
            }

        | PostSearch keys ->
            async{
                let! result = ZeroHedgeAPI.ApplicationLogic.postSearch keys
                return Content.Json(result.ToArray())
            }

        | GetSearch( keys, page ) ->
            async{
                let! result = ZeroHedgeAPI.ApplicationLogic.getSearch(keys, page)
                return Content.Json(result.ToArray())          
            }
        |> Async.RunSynchronously


    let AboutPage ctx =
        Templating.MainView ctx EndPoint.About "About"
            [
                h1 [text "About"]
            ]
            [
                p [text "This is a template WebSharper client-server application."]
            ]


    let Main =
        
        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Api id -> ApiContent(ctx)(id)
            | EndPoint.About -> AboutPage ctx
        )


//    open Suave.Web
//    open WebSharper.Suave
//
//    do startWebServer defaultConfig (WebSharperAdapter.ToWebPart Main)


    open Suave.Web
    open WebSharper.Suave
    
    open System
    open System.Net

    open Suave.Operators
    open Suave.Sockets.Control
    open Suave.WebSocket
    open Suave.Utils
    open Suave.Files
    open Suave.RequestErrors
    open Suave.Filters
    open Suave.Http

    let cfg =
      { defaultConfig with
          bindings =
            [ HttpBinding.mk HTTP (IPAddress.Parse "0.0.0.0") 8083us
            ]
          listenTimeout = TimeSpan.FromMilliseconds 3000. }

    do startWebServer cfg (WebSharperAdapter.ToWebPart Main)