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
open System.Threading

type Story =
    {
        Title : string;
        Introduction : string;
        Body : string;
        Reference : string;
        Published : string;
        Updated : DateTime
    }

type Page =
    {
        updated : DateTime;
        stories : List<Story>
    }
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

//    and Story =
//        {
//            Title : string;
//            Introduction : string;
//            Body : string;
//            Reference : string;
//            Published : string;
//            Updated : DateTime
//        }

    /// Type used for all JSON responses to indicate success or failure.
    [<NamedUnionCases "result">]
    type Result<'T> =
        /// JSON: {"result": "success", /* fields of 'T... */}
        | [<Name "success">] Success of 'T
        /// JSON: {"result": "failure", "message": "error message..."}
        | [<Name "failure">] Failure of message: string


    module ApplicationLogic =
        let storiesmap = new Dictionary<string, Story>()
        let pagesmap = new Dictionary<int, Page>()
        
        let DownloadURL (uri : string)=
            //let s = "";
            let request = HttpWebRequest.Create(uri)  :?> HttpWebRequest 

            // Set some reasonable limits on resources used by this request
            request.AutomaticDecompression <- DecompressionMethods.GZip
            request.Method <- "GET"
            request.Accept <- "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch")
            request.Headers.Add("Accept-Language", "ru,en-US;q=0.8,en;q=0.6,zh-CN;q=0.4,zh;q=0.2")
            request.UserAgent <- "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.125 Safari/537.36"
            //request.Connection <- "Keep-alive"
            
            // Set credentials to use for this request.
            request.Credentials <- CredentialCache.DefaultCredentials;
            let response = request.GetResponse()


            // Get the stream associated with the response.
            let receiveStream = response.GetResponseStream();
            
            let buf = [| for i in 0..8192 -> byte(0)|]
            let mutable count = 1
            let mutable sb = new StringBuilder()
            let mutable tmpString = ""
            while count > 0 do
                count <- receiveStream.Read(buf, 0, buf.Length);
                if count > 0 then                    
                    tmpString <- Encoding.UTF8.GetString(buf, 0, count)
                    sb.Append( tmpString ) |> ignore
                    //printf "dsfds"
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            //let readStream = new StreamReader(receiveStream, Encoding.UTF8)

            //let s = sb.ToString() //readStream.ReadToEnd()
            response.Close()
            receiveStream.Close()
            sb.ToString()


        let replaceLinks (inStr : string) : string =
            let mutable result = inStr
            let list1 = [ "http://www.zerohedge.com/news/"; "http://www.zerohedge.com/article/" ]
            for i in list1 do
                let mutable indref = 0
                while indref >= 0 do
                    indref <- result.IndexOf(i)
                    if indref > 0 then
                        let indref2 = result.IndexOf("\"", indref)
                        let oldref = result.Substring(indref, indref2 - indref)
                        let mutable refBase64 = System.Text.Encoding.UTF8.GetBytes(oldref)
                        let mutable base64Ref = System.Convert.ToBase64String(refBase64)

                        result <- result.Replace(oldref, sprintf "%s%s" "./story/" base64Ref)
            result


        let replaceLinksBack (inStr : string) : string =
            let mutable result = inStr
            let list1 = [ "\"/sites/default" ]
            for i in list1 do
                let mutable indref = 0
                while indref >= 0 do
                    indref <- result.IndexOf(i)
                    if indref > 0 then
                        let indref2 = result.IndexOf("\"", indref+10)
                        let oldref = result.Substring(indref+1, indref2 - indref)

                        result <- result.Replace(oldref, sprintf "%s%s" "http://www.zerohedge.com" oldref)
            result
        
        let loadStory (refBase64 : string) : string =

            let (bResult,theStory) = storiesmap.TryGetValue refBase64 
            if bResult && theStory.Body.Length > 0 then
                theStory.Body
            else
                let base64EncodedBytes = System.Convert.FromBase64String(refBase64);
                let markup1 = DownloadURL(Encoding.UTF8.GetString(base64EncodedBytes))
                let markup = markup1.ToString()
                let mutable ind1 = markup.IndexOf("<h1 class=\"title\">", 0)
                ind1 <- markup.IndexOf("<div class=\"content\">", ind1)
                ind1 <- (ind1+21)
                let mutable ind2 = markup.IndexOf("<div class=\"fivestar-static-form-item\">", ind1)
                if ind2 = -1 then
                    ind2 <- markup.IndexOf("<div class=\"comment-content\">", ind1)
                    if ind2 = -1 || ind2 > markup.IndexOf("<a href=\"/users/", ind1) then
                        ind2 <- markup.IndexOf("<a href=\"/users/", ind1)

                let mutable body = markup.Substring(ind1, ind2 - ind1)
                body <- replaceLinks body
                body <- replaceLinksBack body
                let mutable newStory = { Title = ""; 
                    Introduction = ""; Body = body; Reference = refBase64;
                    Published = ""; Updated = DateTime.Now}
                

                if bResult = true then
                    newStory <- { Title = theStory.Title; 
                    Introduction = theStory.Introduction; Body = body; Reference = theStory.Reference;
                    Published = theStory.Published; Updated = DateTime.Now}
                
                let bResult = storiesmap.Remove(refBase64)
                let bResult = storiesmap.Add(refBase64, newStory)
                body;

        let parse1 (id:int) : List<Story> =
            

            let (bResult, thePage) = pagesmap.TryGetValue(id)
            if bResult = true && (DateTime.Now - thePage.updated).TotalMinutes < 10.0 &&
                thePage.stories.Count > 0 then
                thePage.stories
            else
                let myWebClient = new WebClient()

                
                let articles = new List<Story>()
                let mutable continueLooping = true
                while continueLooping do
                    let (j, ind1) = (ref 0, ref 100)
                    

                    let mutable ind5 = 0
                    let markup1 = DownloadURL( sprintf "http://www.zerohedge.com/?page=%d" id )
                    let markup = markup1.ToString()
                    while(!j < 100 && !ind1 > 0) do
                        ind1 := markup.IndexOf("<div class=\"picture\">", ind5);
                        if ind1.Value > 0 then
                            let mutable ind2 = markup.IndexOf("<h2 class=\"title\"><a href=", ind1.Value)
                            ind2 <- (ind2 + 27)

                            let mutable ind3 = markup.IndexOf("\">", ind2)

                            let mutable ref1 = markup.Substring(ind2, ind3 - ind2)
                            ind3 <- (ind3 + 2)

                            let ind4 = markup.IndexOf("</a>", ind3)
                            let title = markup.Substring(ind3, ind4 - ind3)

                            ind2 <- markup.IndexOf(" on ", ind4)
                            ind2 <- (ind2 + 4)
                            ind3 <- markup.IndexOf("</span>", ind2)
                            let mutable published = markup.Substring(ind2, ind3 - ind2)

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

                            let mutable body = markup.Substring(ind6, ind5 - ind6)
                            body <- replaceLinks body
                            body <- replaceLinksBack body

                            let mutable refBase64 = System.Text.Encoding.UTF8.GetBytes(ref1)
                            let mutable base64Ref = System.Convert.ToBase64String(refBase64)
                            let article = { Title = title; Introduction = body; Body = "";
                                Reference = base64Ref;
                                Published = published; Updated = DateTime.Now }
                    
                    
                            let (bResult, theStory) = storiesmap.TryGetValue(base64Ref)
                            if bResult = false then
                                storiesmap.Add(base64Ref, article)

                            articles.Add( article )
                        // end of if
                    // end of while
                    if articles.Count > 0 then
                        continueLooping <- false
                    else
                        Thread.Sleep (10000)
                if articles.Count > 0 then
                    let newPage = { updated = DateTime.Now; stories = articles }
                    let bResult = pagesmap.Remove(id)
                    pagesmap.Add(id, newPage)

                    articles
                else
                    if bResult = false then
                        null
                    else
                        thePage.stories


        /// The stories database
        let stories = new Dictionary<string, Story>()

        let getPage (id: int) : List<Story> =
            let CurrentPage = parse1 id
            CurrentPage

        let getStory (id: string) : string =
            let article = loadStory id //{ Title = "ssfs"; Introduction = "gjgg"; Body = "jjjhgjh"; Reference = "jhgjhgj";
                          //  Published = "hjgjhgjh"; Updated = DateTime.Now }
            article



    let ApiContent (ctx: Context<Action>) (action: Action)  =
        match action with
        | GetPage id ->
            Content.Json (ApplicationLogic.getPage id)
        | GetStory id ->
            Content.Json (ApplicationLogic.getStory id)

