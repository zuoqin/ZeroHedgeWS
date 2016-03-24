printf "Loading references..."; stdout.Flush()
#r "bin/Debug/Microsoft.Owin.dll"
#r "bin/Debug/Mono.Cecil.Mdb.dll"
#r "bin/Debug/Mono.Cecil.Pdb.dll"
#r "bin/Debug/Mono.Cecil.Rocks.dll"

#r "bin/Debug/WebSharper.Core.JavaScript.dll"
#r "bin/Debug/WebSharper.Core.dll"
#r "bin/Debug/WebSharper.JavaScript.dll"
#r "bin/Debug/WebSharper.Collections.dll"
#r "bin/Debug/WebSharper.Control.dll"
#r "bin/Debug/WebSharper.JQuery.dll"
#r "bin/Debug/WebSharper.Main.dll"
#r "bin/Debug/WebSharper.Web.dll"
#r "bin/Debug/WebSharper.Sitelets.dll"
#r "bin/Debug/Mono.Cecil.dll"


#r "bin/Debug/WebSharper.UI.Next.dll"
#r "bin/Debug/WebSharper.UI.Next.Templating.dll"

#r "bin/Debug/WebSharper.InterfaceGenerator.dll"
#r "bin/Debug/WebSharper.Owin.dll"
#r "bin/Debug/Suave.dll"
#r "bin/Debug/WebSharper.Suave.dll"


#r "bin/Debug/Akka.dll"
#r "bin/Debug/Akka.FSharp.dll"
#r "bin/Debug/Newtonsoft.Json.dll"
//#r "bin/Debug/FSharp.Core.dll"
#r "bin/Debug/FsPickler.dll"
#r "bin/Debug/HttpMultipartParser.dll"

printfn "Done."

#load "Controllers\ZeroHedgeAPI.fs"

#load "Main.fs"