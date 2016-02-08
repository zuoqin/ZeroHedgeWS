namespace ZeroHedgeWS

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server



type EndPoint =
    | [<EndPoint "/">] Home
    | [<EndPoint "/about">] About
    | [<EndPoint "/api">] Api of id: ApiEndPoint

and ApiEndPoint =
    | [<EndPoint "GET /page">] GetPage of int
    | [<EndPoint "GET /story">] GetStory of string




module Templating =
    open WebSharper.UI.Next.Html

    type MainTemplate = Templating.Template<"Main.html">

    // Compute a menubar where the menu item for the given endpoint is active
    let MenuBar (ctx: Context<EndPoint>) endpoint : Doc list =
        //let ( => ) txt act =
        //     liAttr [if endpoint = act then yield attr.``class`` "active"] [
        //        aAttr [attr.href (ctx.Link act)] [text txt]
        //     ]
        //[
        //    li ["Home" => EndPoint.Home]
        //    li ["About" => EndPoint.About]
        //]

        let attr'menu'style = Attr.Create "class" "hiddenmenu"

        let attr'menu'styles = Seq.append [attr'menu'style] []

        [

            divAttr attr'menu'styles [
                Doc.TextNode "kjkj"
            ]
        ]

    let Main ctx action title body =
        Content.Page(
            MainTemplate.Doc(
                title = title,
                menubar = MenuBar ctx action,
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

    let HomePage ctx =
        Templating.Main ctx EndPoint.Home "Home" [
            h1 [text "Say Hi to the server!"]
            div [client <@ Client.Main() @>]
        ]

    let AboutPage ctx =
        Templating.Main ctx EndPoint.About "About" [
            h1 [text "About"]
            p [text "This is a template WebSharper client-server application."]
        ]


    [<Website>]
    let Main =
        //let Sitelet1 = Sitelet.Infer ZeroHedgeAPI.ApiContent

        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Api id -> 
                let result = ApiContent(ctx)(id)
                result
            | EndPoint.Home -> HomePage ctx
            | EndPoint.About -> AboutPage ctx
        )
