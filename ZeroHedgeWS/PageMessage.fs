namespace ZeroHedgeWS


open System
open System.Collections.Generic

open Akka.FSharp
open Akka.Actor



module PageMessage  =



    let system = System.create "zerohedge" (Configuration.load())

    type Story =
        {
            Title : string;
            Introduction : string;
            Body : string;
            Reference : string;
            Published : string;
            Updated : DateTime
        }

    type EchoActor() =
        inherit UntypedActor()
            override this.OnReceive (msg:obj) = 
                printfn "Received message %A" msg
                Console.WriteLine msg

    let aref =
        spawn system "my-actor"
            (fun mailbox ->
                let rec loop() = actor {
                    let! message = mailbox.Receive()
                    // handle an incoming message
                    return! loop()
                }
                loop())

    let createActor =
        // Use Props() to create actor from type definition
        let echoActor = system.ActorOf(Props(typedefof<EchoActor>), "echo")
        echoActor

    let doSomething =
          
                        
        //let system = System.create "system" <| Configuration.defaultConfig()
                        

        // tell a message
        createActor <! "Hello World!"

