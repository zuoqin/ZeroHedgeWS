namespace ZeroHedgeWS

open System
open System.Collections.Generic
open WebSharper
open WebSharper.Sitelets

open System
open System.Collections.Generic
open System.IO
open System.Text
open System.Net

/// Type used for all JSON responses to indicate success or failure.
//[<NamedUnionCases "result">]
//type Result<'T> =
//    /// JSON: {"result": "success", /* fields of 'T... */}
//    | [<Name "success">] Success of 'T
//    /// JSON: {"result": "failure", "message": "error message..."}
//    | [<Name "failure">] Failure of message: string

module ZeroHedgeAPI =

    /// The type of actions, ie. REST API entry points.
    type Action =
        /// GET /page?id=123
        | [<EndPoint "GET /page"; Query "id">]
            GetPage of id: int
        /// GET /story?hkhkkj
        | [<EndPoint "GET /story"; Query "id">]
            GetStory of id: string

    and Story =
        {
            Title : string;
            Introduction : string;
            Body : string;
            Reference : string;
            Published : string;
            Updated : DateTime
        }

    /// Type used for all JSON responses to indicate success or failure.
    [<NamedUnionCases "result">]
    type Result<'T> =
        /// JSON: {"result": "success", /* fields of 'T... */}
        | [<Name "success">] Success of 'T
        /// JSON: {"result": "failure", "message": "error message..."}
        | [<Name "failure">] Failure of message: string


    module ApplicationLogic =

        let mutable ind5 = 0
        let DownloadURL (uri : string)=
            let s = "";
            let request = HttpWebRequest.Create(uri)  :?> HttpWebRequest 

            // Set some reasonable limits on resources used by this request
            request.AutomaticDecompression <- DecompressionMethods.GZip
            request.Method <- "GET"
            request.Accept <- "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch")
            request.Headers.Add("Accept-Language", "ru,en-US;q=0.8,en;q=0.6,zh-CN;q=0.4,zh;q=0.2")
            request.UserAgent <- "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.125 Safari/537.36";
            //request.Connection = "Keep-alive";
            // Set credentials to use for this request.
            request.Credentials <- CredentialCache.DefaultCredentials;
            let response = request.GetResponse()


            // Get the stream associated with the response.
            let receiveStream = response.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            let readStream = new StreamReader(receiveStream, Encoding.UTF8)

            let s = readStream.ReadToEnd()
            response.Close()
            readStream.Close()
            s

        
        let loadStory (refBase64 : string) : string =

            let base64EncodedBytes = System.Convert.FromBase64String(refBase64);
            let markup = DownloadURL(Encoding.UTF8.GetString(base64EncodedBytes))

            let mutable ind1 = markup.IndexOf("<h1 class=\"title\">", 0)
            ind1 <- markup.IndexOf("<div class=\"content\">", ind1)
            ind1 <- (ind1+21)
            let ind2 = markup.IndexOf("<div class=\"fivestar-static-form-item\">", ind1)
            let body = markup.Substring(ind1, ind2 - ind1)
            body;

        let parse1 (id:int) : List<Story> =

            let myWebClient = new WebClient()

            //let  myDataBuffer = fetchAsync( sprintf "http://www.zerohedge.com/?page=%d" id )

            let markup = DownloadURL( sprintf "http://www.zerohedge.com/?page=%d" id )

            let (j, ind1) = (ref 0, ref 100)
            let articles = new List<Story>()
            while(!j < 100 && !ind1 > 0) do
                ind1 := markup.IndexOf("<div class=\"picture\">", ind5);
                if ind1.Value > 0 then
                    let mutable ind2 = markup.IndexOf("<h2 class=\"title\"><a href=", ind1.Value)
                    ind2 <- (ind2 + 27)

                    let mutable ind3 = markup.IndexOf("\">", ind2)

                    let ref1 = markup.Substring(ind2, ind3 - ind2)
                    ind3 <- (ind3 + 2)

                    let ind4 = markup.IndexOf("</a>", ind3)
                    let title = markup.Substring(ind3, ind4 - ind3)

                    ind2 <- markup.IndexOf(" on ", ind4)
                    ind2 <- (ind2 + 4)
                    ind3 <- markup.IndexOf("</span>", ind2)
                    let published = markup.Substring(ind2, ind3 - ind2)

                    ind5 <- markup.IndexOf("<p>", ind4)
                    let mutable ind6 = 0

                    if markup.Substring(ind5 + 3, 3).CompareTo("<a ") = 0 then
                        ind5 <- markup.IndexOf("<p>", ind5 + 5)
                        ind6 <- ( ind5 + 3 )
                        ind5 <- markup.IndexOf("</p>", ind6)

                    elif markup.IndexOf("<img", ind5) < markup.IndexOf("</p>", ind5) then
                        ind6 <- markup.IndexOf("/>", ind5)
                        ind6 <- (ind6 + 2)
                        ind5 <- markup.IndexOf("<div class=\"clear-block\"></div>", ind6)
                    else
                        ind6 <- ind5
                        ind5 <- markup.IndexOf("<div class=\"clear-block\"></div>", ind6)

                    let body = markup.Substring(ind6, ind5 - ind6)

                    let refBase64 = System.Text.Encoding.UTF8.GetBytes(ref1)

                    let article = { Title = title; Introduction = body; Body = body;
                        Reference = System.Convert.ToBase64String(refBase64);
                        Published = published; Updated = DateTime.Now }
                    articles.Add( article )
                    printfn "kjjkhkj"
                // end of if
                printfn "kjjkhkj"
            // end of while
            printfn "kjjkhkj"
            articles



        /// The stories database
        let stories = new Dictionary<string, Story>()

        let getPage (id: int) : List<Story> =
            let CurrentPage = parse1 id
            CurrentPage

        let getStory (id: string) : Result<string> =
            let article = loadStory id //{ Title = "ssfs"; Introduction = "gjgg"; Body = "jjjhgjh"; Reference = "jhgjhgj";
                          //  Published = "hjgjhgjh"; Updated = DateTime.Now }
            Success article



    let ApiContent (ctx: Context<Action>) (action: Action)  =
        match action with
        | GetPage id ->
            Content.Json (ApplicationLogic.getPage id)
        | GetStory id ->
            Content.Json (ApplicationLogic.getStory id)

