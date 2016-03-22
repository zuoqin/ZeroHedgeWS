namespace ZeroHedgeWS

open System
open System.Collections.Generic

open Akka.FSharp
open Akka.Actor

open WebSharper
open WebSharper.Sitelets

open System.IO
open System.Text
open System.Net
open System.Threading
open System.Web


type Story =
    {
        Title : string;
        Introduction : string;
        Body : string;
        Reference : string;
        Published : string;
        Updated : DateTime;
        isLoading : bool
    }

type Page =
    {
        updated : DateTime;
        stories : List<Story>;
        isLoading: bool
    }

type SearchPageIndex =
    {
        keys : string;
        page : int
    }


module ZeroHedgeAPI =


    type EchoActor() =
        inherit UntypedActor()
            override this.OnReceive (msg:obj) = 
                printfn "Received message %A" msg
                Console.WriteLine msg

    // asynchronious client messages for CRUD operations
    type CRUDBlogMessage = 
        | GetPage      of int * AsyncReplyChannel<List<Story>>
        | GetStory     of string * AsyncReplyChannel<string>
        | GetSearch    of string * int * AsyncReplyChannel<List<Story>>

    // asynchronious client messages for AKKA operations
    type AKKAMessage = 
        | GetPageX      of SearchPageIndex
        | GetStoryX     of string


    module ApplicationLogic =

        let AkkaSystem = System.create "zerohedge" (Configuration.load())


        let storiesmap = new Dictionary<string, Story>()
        let pagesmap = new Dictionary<int, Page>()
        let requestsmap = new Dictionary<SearchPageIndex, Page>()



        let CheckExpired( a: TimeSpan) =
            if a.TotalMinutes > 10. then
                true
            else
                false


        let createActor =
            // Use Props() to create actor from type definition
            let echoActor = AkkaSystem.ActorOf(Props(typedefof<EchoActor>), "echo")
            echoActor


        let DownloadURL (uri : string)=
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

            response.Close()
            receiveStream.Close()
            sb.ToString()

        let PostURL (uri : string, keys : string)=
            let request = HttpWebRequest.Create(uri)  :?> HttpWebRequest 

            // Set some reasonable limits on resources used by this request
            request.AutomaticDecompression <- DecompressionMethods.GZip
            request.Method <- "POST"
            request.Accept <- "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch")
            request.Headers.Add("Accept-Language", "ru,en-US;q=0.8,en;q=0.6,zh-CN;q=0.4,zh;q=0.2")
            request.UserAgent <- "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.125 Safari/537.36"
            request.ContentType <- "application/x-www-form-urlencoded"
            request.ServicePoint.Expect100Continue <- false
            // Set credentials to use for this request.
            //request.Credentials <- CredentialCache.DefaultCredentials;

            let postData = keys.Replace("%20", "+")
            let bodyString = "keys=" + postData //HttpUtility.UrlEncode(postData)            
            let bytedata = Encoding.UTF8.GetBytes(bodyString)
            //request.ContentLength <- int64(bytedata.Length)
            
            
            let newStream = request.GetRequestStream ()
            newStream.Write(bytedata,0,bytedata.Length)
            newStream.Close()

            let response = request.GetResponse()
            // Get the stream associated with the response.
            let receiveStream = response.GetResponseStream()
            
            let buf = [| for i in 0..8192 -> byte(0)|]
            let mutable count = 1
            let mutable sb = new StringBuilder()
            let mutable tmpString = ""
            while count > 0 do
                count <- receiveStream.Read(buf, 0, buf.Length)
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
        
        let DownloadStory (refBase64 : string) : string =

            let (bResult,theStory) = storiesmap.TryGetValue refBase64 
            if bResult && (theStory.Body.Length > 0 || theStory.isLoading) then
                theStory.Body
            else
                let mutable newStory = { Title = ""; 
                    Introduction = ""; Body = ""; Reference = refBase64;
                    Published = ""; Updated = DateTime.Now; isLoading = true}
                if bResult = true then
                    newStory <- { Title = theStory.Title; 
                    Introduction = theStory.Introduction; Body = ""; Reference = theStory.Reference;
                    Published = theStory.Published; Updated = DateTime.Now; isLoading = true}

                
                Console.WriteLine( sprintf "retrieving URL: %s"  refBase64)
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
                        if ind2 = -1 then
                            ind2 <- markup.IndexOf("<div class=\"clear-block clear\">", ind1)
                


                let mutable body = markup.Substring(ind1, ind2 - ind1)
                body <- replaceLinks body
                body <- replaceLinksBack body

                newStory <- { Title = newStory.Title; 
                Introduction = newStory.Introduction; Body = body; Reference = newStory.Reference;
                Published = newStory.Published; Updated = DateTime.Now; isLoading = false}                


                
                let bResult = storiesmap.Remove(refBase64)
                let bResult = storiesmap.Add(refBase64, newStory)
                body;


        let DownloadAndParseSearchPage (keys : string): List<Story> =
            let newSearchIndex = { keys = keys; page = 0}
            let (bResult, SearchArticles) = requestsmap.TryGetValue(newSearchIndex)
            let markup1 = PostURL( sprintf( "http://www.zerohedge.com/search/apachesolr_search/" ), keys )
            let markup = markup1.ToString()
            let articles = new List<Story>()

            let mutable ind2 = markup.IndexOf("<dl class=\"search-results apachesolr_search-results\">", 0);
             
            let ind_end = markup.IndexOf("</dl>");
            while(ind2 > 0) do                
                ind2 <- markup.IndexOf("<dt class=\"title\"", ind2)
                let mutable title = ""
                let mutable ref1 = ""
                let mutable introduction = ""
                let mutable published = ""
                if ind2 < ind_end && ind2 <> -1 then
                    ind2 <- markup.IndexOf("<a href=\"", ind2)
                    ind2 <- (ind2 + 9)
                    let mutable ind3 = markup.IndexOf("\">", ind2)

                    ref1 <- markup.Substring(ind2, ind3 - ind2)

                    ind2 <- (ind3 + 2)

                    ind3 <- markup.IndexOf("</a>", ind3)

                    title <- markup.Substring(ind2, ind3 - ind2)

                    ind2 <- markup.IndexOf("<p class=\"search-snippet\"", ind3)
                    if ind2 < ind_end then
                        ind2 <- (ind2 + 27)
                        ind3 <- markup.IndexOf("</p>", ind2)
                        introduction <- markup.Substring(ind2, ind3 - ind2)

                    ind2 <- markup.IndexOf("<p class=\"search-info\"", ind3)
                    if ind2 < ind_end then
                        ind2 <- markup.IndexOf("</a>", ind2)
                        if ind2 < ind_end then
                            ind2 <- (ind2 + 7)
                            ind3 <- (ind2 + 18)
                            published <- markup.Substring(ind2, ind3 - ind2)


                let mutable refBase64 = System.Text.Encoding.UTF8.GetBytes(ref1)
                let mutable base64Ref = System.Convert.ToBase64String(refBase64)
                let article = { Title = title; Introduction = introduction; Body = "";
                    Reference = base64Ref;
                    Published = published; Updated = DateTime.Now; isLoading = false }
                    
                    
                let (bResult, theStory) = storiesmap.TryGetValue(base64Ref)
                if bResult = false then
                    storiesmap.Add(base64Ref, article)

                if published.Length > 0 && introduction.Length > 0 then
                    articles.Add( article )


            if articles.Count > 0 then
                let newPage = { updated = DateTime.Now; stories = articles; isLoading = false }
                let bResult = requestsmap.Remove(newSearchIndex)
                requestsmap.Add(newSearchIndex, newPage)

                articles
            else
                if bResult = false then
                    null
                else
                    SearchArticles.stories


        let DownloadPostSearchPage (keys : string): List<Story> =
            let newSearchIndex = { keys = keys; page = 0}
            let (bResult, SearchArticles) = requestsmap.TryGetValue(newSearchIndex)
            if bResult = true then
                if CheckExpired(DateTime.Now - SearchArticles.updated) then
                    DownloadAndParseSearchPage(keys)
                else
                    if SearchArticles.stories.Count > 0 then
                        SearchArticles.stories
                    else
                        DownloadAndParseSearchPage(keys)
            else
                DownloadAndParseSearchPage(keys)




        let DownloadPage (id:int) : List<Story> =
            //PageMessage.doSomething

            let (bResult, thePage) = pagesmap.TryGetValue(id)
            if bResult = true && CheckExpired(DateTime.Now - thePage.updated) = false &&
                (thePage.stories.Count > 0 //|| thePage.isLoading
                ) then

                let sLine = sprintf "Will not Download page %d, return from cache" id
                Console.WriteLine sLine

                thePage.stories
            else
                let sLine = sprintf "Downloading page %d" id
                Console.WriteLine sLine
                let myWebClient = new WebClient()

                
                let articles = new List<Story>()
                let mutable continueLooping = true
                while continueLooping do
                    let (j, ind1) = (ref 0, ref 100)
                    

                    let mutable ind5 = 0
                    let markup1 = DownloadURL( sprintf "http://www.zerohedge.com/?page=%d" id )
                    let markup = markup1.ToString()
                    while(!j < 100 && !ind1 > 0 && ind5 >= 0) do
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
                                ind6 <- markup.IndexOf("<p>", ind5 + 5)
                                if ind6 < markup.IndexOf("</p>", ind5) then
                                    ind5 <- markup.IndexOf("<p>", ind5 + 5);

                                ind6 <- ( ind5 + 3 )
                                ind5 <- markup.IndexOf("</p>", ind6)

                            elif markup.IndexOf("<img", ind5) < markup.IndexOf("</p>", ind5) then
                                ind6 <- markup.IndexOf("/>", ind5)
                                ind6 <- (ind6 + 2)
                                ind5 <- markup.IndexOf("<div class=\"clear-block\"></div>", ind6)
                            else
                                ind6 <- ind5
                                ind5 <- markup.IndexOf("<div class=\"clear-block\"></div>", ind6)

                            let mutable body = ""
                            if ind5 > 0 && ind6 > 0 then
                                body <- markup.Substring(ind6, ind5 - ind6)
                                body <- replaceLinks body
                                body <- replaceLinksBack body
                            else
                                body <- ""
                            let mutable refBase64 = System.Text.Encoding.UTF8.GetBytes(ref1)
                            let mutable base64Ref = System.Convert.ToBase64String(refBase64)
                            let article = { Title = title; Introduction = body; Body = "";
                                Reference = base64Ref;
                                Published = published; Updated = DateTime.Now; isLoading = false }
                    
                    
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
                    let newPage = { updated = DateTime.Now; stories = articles; isLoading = false }
                    let bResult = pagesmap.Remove(id)
                    pagesmap.Add(id, newPage)

                    let sLine = sprintf "Cache data for page: %d has been updated with Loading = %b" id newPage.isLoading
                    Console.WriteLine sLine

                    articles
                else
                    if bResult = false then
                        null
                    else
                        thePage.stories



        let DownLoadSearchPage (keys: string, page:int) : List<Story> =
            if page = 0 then
                DownloadPostSearchPage keys
            else
                let newSearchIndex = { keys = keys; page = page}
                let (bResult, SearchArticles) = requestsmap.TryGetValue(newSearchIndex)
                if bResult = true && CheckExpired(DateTime.Now - SearchArticles.updated) = false &&
                    SearchArticles.stories.Count > 0 then
                    SearchArticles.stories
                else

                    let sLine = sprintf "Downloading search keys: %s  page: %d" keys page
                    Console.WriteLine sLine
                    
                    let keyDecode = keys //HttpUtility.UrlDecode(keys).Replace(" ", "+")
                    let markup1 = DownloadURL( sprintf "http://www.zerohedge.com/search/apachesolr_search/%s?page=%d" keyDecode page )
                    let markup = markup1.ToString()
                    let articles = new List<Story>()

                    let mutable ind2 = markup.IndexOf("<dl class=\"search-results apachesolr_search-results\">", 0);
             
                    let ind_end = markup.IndexOf("</dl>");
                    while(ind2 > 0) do                
                        ind2 <- markup.IndexOf("<dt class=\"title\"", ind2)
                        let mutable title = ""
                        let mutable ref1 = ""
                        let mutable introduction = ""
                        let mutable published = ""
                        if ind2 < ind_end && ind2 <> -1 then
                            ind2 <- markup.IndexOf("<a href=\"", ind2)
                            ind2 <- (ind2 + 9)
                            let mutable ind3 = markup.IndexOf("\">", ind2)

                            ref1 <- markup.Substring(ind2, ind3 - ind2)

                            ind2 <- (ind3 + 2)

                            ind3 <- markup.IndexOf("</a>", ind3)

                            title <- markup.Substring(ind2, ind3 - ind2)

                            ind2 <- markup.IndexOf("<p class=\"search-snippet\"", ind3)
                            if ind2 < ind_end then
                                ind2 <- (ind2 + 27)
                                ind3 <- markup.IndexOf("</p>", ind2)
                                introduction <- markup.Substring(ind2, ind3 - ind2)

                            ind2 <- markup.IndexOf("<p class=\"search-info\"", ind3)
                            if ind2 < ind_end then
                                ind2 <- markup.IndexOf("</a>", ind2)
                                if ind2 < ind_end then
                                    ind2 <- (ind2 + 7)
                                    ind3 <- (ind2 + 18)
                                    published <- markup.Substring(ind2, ind3 - ind2)


                        let mutable refBase64 = System.Text.Encoding.UTF8.GetBytes(ref1)
                        let mutable base64Ref = System.Convert.ToBase64String(refBase64)
                        let article = { Title = title; Introduction = introduction; Body = "";
                            Reference = base64Ref;
                            Published = published; Updated = DateTime.Now; isLoading = false }
                    
                    
                        let (bResult, theStory) = storiesmap.TryGetValue(base64Ref)
                        if bResult = false then
                            storiesmap.Add(base64Ref, article)

                        if published.Length > 0 && introduction.Length > 0 then
                            articles.Add( article )

                    if articles.Count > 0 then
                        let newPage = { updated = DateTime.Now; stories = articles; isLoading = false }
                        let bResult = requestsmap.Remove(newSearchIndex)
                        requestsmap.Add(newSearchIndex, newPage)

                        articles
                    else
                        if bResult = false then
                            null
                        else
                            SearchArticles.stories

        let aref =
            spawn AkkaSystem "my-actor"
                (fun mailbox ->
                    let rec loop() = actor {
                        let! msg = mailbox.Receive()
                        let sender = mailbox.Sender()

                        match msg with 
                        | GetPageX(search) ->
                            DownloadPage search.page |> ignore
                        | GetStoryX(reference) ->                      
                            let story = DownloadStory reference
                            sender <! story
//                        | GetSearch ( keys, page, reply ) ->                      
//                            let search'results = DownLoadSearchPage( keys, page )
//                            reply.Reply search'results
                        // handle an incoming message
                        //| _ ->  failwith "unknown message"
                        return! loop()
                    }
                    loop())


        let crud = MailboxProcessor.Start(fun agent ->             
            let rec loop () : Async<unit> = async {
                let! msg = agent.Receive()
                match msg with 
                | GetPage ( page'number, reply ) ->                      
                    let posts = DownloadPage page'number
                    reply.Reply posts
                | GetStory ( reference, reply ) ->                      
                    let story'content = DownloadStory reference
                    reply.Reply story'content
                | GetSearch ( keys, page, reply ) ->                      
                    let search'results = DownLoadSearchPage( keys, page )
                    reply.Reply search'results


                return! loop () }
            loop () )        

        let read'page'2 page'number =
            crud.PostAndAsyncReply( fun reply -> GetPage(page'number, reply) )

        let read'page page =
            //crud.PostAndAsyncReply( fun reply -> GetPage(page'number, reply) )

            let request = { keys = ""; page = page }
            aref <! GetPageX(request) 

        let load'search'page( keys: string, page:int) =
            crud.PostAndAsyncReply( fun reply -> GetSearch(keys, page, reply) )

        let read'story reference =
            //crud.PostAndAsyncReply( fun reply -> GetStory(reference, reply) )
            let mutable response = ""
            try
                let request = reference
                let task = (aref <? GetStoryX(request))
                response <- Async.RunSynchronously (task)
                
            with 
                | :? System.AggregateException ->
                    printfn "ask: AggregateException!"
                | :? TimeoutException ->
                    printfn "ask: timeout!"

            response


        /// The stories database
        let stories = new Dictionary<string, Story>()

        let getPage (id: int) : Async<List<Story>> =
            let sLine = sprintf "Call Get page %d" id
            Console.WriteLine sLine
            async{
                let (bResult, thePage) = pagesmap.TryGetValue(id)
                if bResult = true then
                    let sLine = sprintf "Found page %d in cache with Loading = %b" id thePage.isLoading 
                    Console.WriteLine sLine

                    if thePage.stories.Count > 0 then
                        if CheckExpired(DateTime.Now - thePage.updated) then
                            if thePage.isLoading = false then
                                //let res = crud.PostAndAsyncReply( fun reply -> GetPage(id, reply) )
                                let newPage = { updated = thePage.updated; stories = thePage.stories; isLoading = true }
                                let bResult = pagesmap.Remove(id)


                                let sLine = sprintf "Updated page %d to cache with Loading = %b" id newPage.isLoading 
                                Console.WriteLine sLine

                                pagesmap.Add(id, newPage)
                                read'page id
                            else
                                let sLine = sprintf "Page: %d is still downloading, return from cache" id 
                                Console.WriteLine sLine

                            return thePage.stories
                        else
                            let sLine = sprintf "Page: %d is not expired yet, return from cache" id 
                            Console.WriteLine sLine
                            return thePage.stories
                    else
                        let stories = new List<Story>()
                        let newPage = { updated = DateTime(1900,1,1); stories = stories; isLoading = true }
                        let bResult = pagesmap.Remove(id)
                        pagesmap.Add(id, newPage)

                        let! blog = read'page'2 id
                        return blog
                        //return thePage.stories
                else
                    let stories = new List<Story>()
                    let newPage = { updated = DateTime(1900,1,1); stories = stories; isLoading = true }
                    let bResult = pagesmap.Remove(id)
                    pagesmap.Add(id, newPage)
                    let! blog = read'page'2 id
                    return blog
                    //return thePage.stories
            }


        let getStory (id: string) : Async<string> =
            async{
                let sURL = Uri.UnescapeDataString id // Encoding.UTF8.GetString( System.Web.HttpServerUtility.UrlDecode( id))
                
                Console.WriteLine( sprintf "1st retrieving URL: %s"  sURL)
                let blog = read'story sURL
                return blog                
            }

        let AsyncSearchSite (keys: string, page : int) =
            async{
                let newSearchIndex = { keys = keys; page = page}
                let (bResult, SearchArticles) = requestsmap.TryGetValue(newSearchIndex)
                if bResult = true then
                    if SearchArticles.stories.Count > 0 then
                        if (DateTime.Now - SearchArticles.updated).TotalHours > 10.0 then
                            let res = crud.PostAndAsyncReply( fun reply -> GetSearch(keys, page, reply) )
                            return SearchArticles.stories
                        else
                            return SearchArticles.stories
                    else
                        let! blog = load'search'page( keys, page)
                        return blog
                else
                    
                    let! blog = load'search'page( keys, page)
                    return blog
            }
        let postSearch (keys: string) =
            AsyncSearchSite( keys, 0 )


        let getSearch (keys: string, page : int) =
            AsyncSearchSite( keys, page )
