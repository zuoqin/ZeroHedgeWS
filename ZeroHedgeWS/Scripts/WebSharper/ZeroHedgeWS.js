(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,jQuery,Exception,Concurrency,ZeroHedgeWS,Client,Json,Provider,Id,JSON,UI,Next,AttrProxy,AttrModule,Seq,List,Var1,console,Doc,ListModel1,View,PrintfHelpers,T,ListModel,View1,Var,StoryClient;
 Runtime.Define(Global,{
  ZeroHedgeWS:{
   Client:{
    Ajax:function(methodtype,url,serializedData)
    {
     var arg00;
     arg00=function(tupledArg)
     {
      var ok,ko,value;
      ok=tupledArg[0];
      ko=tupledArg[1];
      tupledArg[2];
      value=jQuery.ajax({
       url:url,
       type:methodtype,
       contentType:"application/json",
       dataType:"text",
       data:serializedData,
       success:function(result)
       {
        return ok(result);
       },
       error:function(jqXHR)
       {
        return ko(Exception.New1(jqXHR.responseText));
       }
      });
      return;
     };
     return Concurrency.FromContinuations(arg00);
    },
    GetPageArticles:function(id)
    {
     return Concurrency.Delay(function()
     {
      var x;
      x="/api/page/"+Global.String(id);
      return Concurrency.Bind(Client.Ajax("GET",x,null),function(_arg1)
      {
       var _;
       _=Provider.get_Default();
       return Concurrency.Return(((_.DecodeList(_.DecodeRecord(undefined,[["Title",Id,0],["Introduction",Id,0],["Body",Id,0],["Reference",Id,0],["Published",Id,0],["Updated",_.DecodeDateTime(),0]])))())(JSON.parse(_arg1)));
      });
     });
    },
    Main:function()
    {
     var _attr_divid,_attr_divclass,attrs_div,arg00,ats,arg001,arg10,_arg00_1;
     _attr_divid=AttrProxy.Create("id","blogItems");
     _attr_divclass=AttrModule.Class("col-md-12");
     attrs_div=Seq.append([_attr_divid],List.ofArray([_attr_divclass]));
     arg00=Concurrency.Delay(function()
     {
      return Concurrency.Bind(Client.GetPageArticles(Var1.Get(Client["v'page"]())),function(_arg1)
      {
       var action;
       action=function(x)
       {
        var a;
        a=x.Title;
        console?console.log(a):undefined;
        return Client.postList().Add(x);
       };
       Seq.iter(action,_arg1);
       return Concurrency.Return(null);
      });
     });
     Concurrency.Start(arg00,{
      $:0
     });
     ats=List.ofArray([AttrProxy.Create("class","container top-padding-med ng-scope")]);
     arg001=function()
     {
      var _arg00_,_arg10_;
      _arg00_=function(p)
      {
       return Client.doc(p);
      };
      _arg10_=ListModel1.View(Client.postList());
      return Doc.Convert(_arg00_,_arg10_);
     };
     arg10=Client["v'blog"]();
     _arg00_1=View.Map(arg001,arg10);
     return Doc.Element("div",ats,List.ofArray([Doc.Element("div",attrs_div,List.ofArray([Doc.EmbedView(_arg00_1)]))]));
    },
    doc:function(post)
    {
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel panel-primary")]),List.ofArray([Client["doc'header"](post),Client["doc'body"](post)]));
    },
    "doc'body":function(post)
    {
     var _attr_container1,_attr_container2,_attrs_container,_attr_title1,_attr_title2,_attrs_title,domNode,_,arg20;
     _attr_container1=AttrProxy.Create("class","container");
     _attr_container2=AttrModule.Style("margin-top","100px");
     _attrs_container=Seq.append([_attr_container1],List.ofArray([_attr_container2]));
     _attr_title1=AttrProxy.Create("placeholder","Title");
     _attr_title2=AttrModule.Class("form-control");
     _attrs_title=Seq.append([_attr_title1],List.ofArray([_attr_title2]));
     _=post.Introduction;
     domNode=jQuery(PrintfHelpers.toSafe("<div>")+PrintfHelpers.toSafe(_)+PrintfHelpers.toSafe("</div>")).get(0);
     arg20=List.ofArray([Doc.TextNode(post.Published)]);
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel-body")]),List.ofArray([Doc.Static(domNode),Doc.Element("p",[],arg20)]));
    },
    "doc'header":function(post)
    {
     var a,domNode,_,_1,href,_a_attr_1,_a_attr_list;
     a=post.Title;
     console?console.log(a):undefined;
     _=post.Title;
     domNode=jQuery(PrintfHelpers.toSafe("<div>")+PrintfHelpers.toSafe(_)+PrintfHelpers.toSafe("</div>")).get(0);
     _1=post.Reference;
     href=PrintfHelpers.toSafe("/story/")+PrintfHelpers.toSafe(_1);
     _a_attr_1=AttrProxy.Create("href",href);
     _a_attr_list=List.ofArray([_a_attr_1]);
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel-heading")]),List.ofArray([Doc.Element("a",_a_attr_list,List.ofArray([Doc.Static(domNode)]))]));
    },
    postList:Runtime.Field(function()
    {
     var arg00,arg10;
     arg00=function(i)
     {
      return Global.String(i.Reference);
     };
     arg10=Runtime.New(T,{
      $:0
     });
     return ListModel.Create(arg00,arg10);
    }),
    "v'blog":Runtime.Field(function()
    {
     var arg00,arg10;
     arg00=function()
     {
      return Concurrency.Delay(function()
      {
       return Concurrency.Return(Client.postList());
      });
     };
     View.get_Do();
     arg10=View1.Const(1);
     return View.MapAsync(arg00,arg10);
    }),
    "v'page":Runtime.Field(function()
    {
     return Var.Create(1);
    })
   },
   StoryClient:{
    Ajax:function(methodtype,url,serializedData)
    {
     var arg00;
     arg00=function(tupledArg)
     {
      var ok,ko,value;
      ok=tupledArg[0];
      ko=tupledArg[1];
      tupledArg[2];
      value=jQuery.ajax({
       url:url,
       type:methodtype,
       contentType:"application/json",
       dataType:"text",
       data:serializedData,
       success:function(result)
       {
        return ok(result);
       },
       error:function(jqXHR)
       {
        return ko(Exception.New1(jqXHR.responseText));
       }
      });
      return;
     };
     return Concurrency.FromContinuations(arg00);
    },
    GetStory:function(id)
    {
     return Concurrency.Delay(function()
     {
      var pageURL;
      pageURL="/api/story/"+PrintfHelpers.toSafe(id);
      return Concurrency.Bind(StoryClient.Ajax("GET",pageURL,null),function(_arg1)
      {
       if(console)
        {
         console.log("Get zerohedge story");
        }
       Provider.get_Default();
       return Concurrency.Return((Id())(JSON.parse(_arg1)));
      });
     });
    },
    Main:function(reference)
    {
     var _attr_divid,_attr_divclass,attrs_div,body,title,introduction;
     _attr_divid=AttrProxy.Create("id","blogItems");
     _attr_divclass=AttrModule.Class("col-md-12");
     attrs_div=Seq.append([_attr_divid],List.ofArray([_attr_divclass]));
     body=[""];
     title="";
     introduction="";
     Concurrency.Start(Concurrency.Delay(function()
     {
      return Concurrency.Bind(StoryClient.GetStory(reference),function(_arg1)
      {
       body[0]=_arg1;
       if(console)
        {
         console.log(_arg1);
        }
       return Concurrency.Return(null);
      });
     }),{
      $:0
     });
     return Doc.Element("div",List.ofArray([AttrProxy.Create("class","container top-padding-med ng-scope")]),List.ofArray([Doc.Element("div",attrs_div,List.ofArray([Doc.TextNode(body[0])]))]));
    },
    doc:function(post)
    {
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel panel-primary")]),List.ofArray([StoryClient["doc'header"](post),StoryClient["doc'body"](post)]));
    },
    "doc'body":function(post)
    {
     var _attr_container1,_attr_container2,_attrs_container,_attr_title1,_attr_title2,_attrs_title,domNode,_,arg20;
     _attr_container1=AttrProxy.Create("class","container");
     _attr_container2=AttrModule.Style("margin-top","100px");
     _attrs_container=Seq.append([_attr_container1],List.ofArray([_attr_container2]));
     _attr_title1=AttrProxy.Create("placeholder","Title");
     _attr_title2=AttrModule.Class("form-control");
     _attrs_title=Seq.append([_attr_title1],List.ofArray([_attr_title2]));
     _=post.Introduction;
     domNode=jQuery(PrintfHelpers.toSafe("<div>")+PrintfHelpers.toSafe(_)+PrintfHelpers.toSafe("</div>")).get(0);
     arg20=List.ofArray([Doc.TextNode(post.Published)]);
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel-body")]),List.ofArray([Doc.Static(domNode),Doc.Element("p",[],arg20)]));
    },
    "doc'header":function(post)
    {
     var a,domNode,_,_1,href,_a_attr_1,_a_attr_list;
     a=post.Title;
     console?console.log(a):undefined;
     _=post.Title;
     domNode=jQuery(PrintfHelpers.toSafe("<div>")+PrintfHelpers.toSafe(_)+PrintfHelpers.toSafe("</div>")).get(0);
     _1=post.Reference;
     href=PrintfHelpers.toSafe("/story?ref=")+PrintfHelpers.toSafe(_1);
     _a_attr_1=AttrProxy.Create("href",href);
     _a_attr_list=List.ofArray([_a_attr_1]);
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel-heading")]),List.ofArray([Doc.Element("a",_a_attr_list,List.ofArray([Doc.Static(domNode)]))]));
    }
   }
  }
 });
 Runtime.OnInit(function()
 {
  jQuery=Runtime.Safe(Global.jQuery);
  Exception=Runtime.Safe(Global.WebSharper.Exception);
  Concurrency=Runtime.Safe(Global.WebSharper.Concurrency);
  ZeroHedgeWS=Runtime.Safe(Global.ZeroHedgeWS);
  Client=Runtime.Safe(ZeroHedgeWS.Client);
  Json=Runtime.Safe(Global.WebSharper.Json);
  Provider=Runtime.Safe(Json.Provider);
  Id=Runtime.Safe(Provider.Id);
  JSON=Runtime.Safe(Global.JSON);
  UI=Runtime.Safe(Global.WebSharper.UI);
  Next=Runtime.Safe(UI.Next);
  AttrProxy=Runtime.Safe(Next.AttrProxy);
  AttrModule=Runtime.Safe(Next.AttrModule);
  Seq=Runtime.Safe(Global.WebSharper.Seq);
  List=Runtime.Safe(Global.WebSharper.List);
  Var1=Runtime.Safe(Next.Var1);
  console=Runtime.Safe(Global.console);
  Doc=Runtime.Safe(Next.Doc);
  ListModel1=Runtime.Safe(Next.ListModel1);
  View=Runtime.Safe(Next.View);
  PrintfHelpers=Runtime.Safe(Global.WebSharper.PrintfHelpers);
  T=Runtime.Safe(List.T);
  ListModel=Runtime.Safe(Next.ListModel);
  View1=Runtime.Safe(Next.View1);
  Var=Runtime.Safe(Next.Var);
  return StoryClient=Runtime.Safe(ZeroHedgeWS.StoryClient);
 });
 Runtime.OnLoad(function()
 {
  Client["v'page"]();
  Client["v'blog"]();
  Client.postList();
  return;
 });
}());
