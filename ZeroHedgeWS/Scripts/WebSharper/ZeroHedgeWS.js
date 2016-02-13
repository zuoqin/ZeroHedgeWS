(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,jQuery,Exception,Concurrency,ZeroHedgeWS,Client,Json,Provider,Id,JSON,UI,Next,AttrProxy,AttrModule,Seq,List,Var1,Doc,ListModel1,View,PrintfHelpers,T,ListModel,View1,Var,String,PageClient,window,Operators,SearchClient,StoryClient;
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
      x="./api/page/"+Global.String(id);
      return Concurrency.Bind(Client.Ajax("GET",x,null),function(_arg1)
      {
       var _;
       _=Provider.get_Default();
       return Concurrency.Return(((_.DecodeList(_.DecodeRecord(undefined,[["Title",Id,0],["Introduction",Id,0],["Body",Id,0],["Reference",Id,0],["Published",Id,0],["Updated",_.DecodeDateTime(),0]])))())(JSON.parse(_arg1)));
      });
     });
    },
    Main:function(pageID)
    {
     var _attr_divid,_attr_divstyle,_attr_divclass,attrs_div,arg00,arg001,_attr_div_fixed1,_attr_div_fixed2,_attr_div_fixed,_attr_container1,_attr_container2,_attrs_container,_attr_container11,_attr_container21,_attrs_container1,ats,arg002,arg10,_arg00_1;
     _attr_divid=AttrProxy.Create("id","blogItems");
     _attr_divstyle=AttrModule.Style("margin-top","55px");
     _attr_divclass=AttrModule.Class("col-md-12");
     attrs_div=Seq.append([_attr_divid,_attr_divstyle],List.ofArray([_attr_divclass]));
     arg00=Client["v'page"]();
     Var1.Set(arg00,pageID);
     arg001=Concurrency.Delay(function()
     {
      return Concurrency.Bind(Client.GetPageArticles(Var1.Get(Client["v'page"]())),function(_arg1)
      {
       var action;
       action=function(x)
       {
        return Client.postList().Add(x);
       };
       Seq.iter(action,_arg1);
       return Concurrency.Return(null);
      });
     });
     Concurrency.Start(arg001,{
      $:0
     });
     _attr_div_fixed1=AttrProxy.Create("role","navigation");
     _attr_div_fixed2=AttrProxy.Create("class","navbar navbar-inverse navbar-fixed-top");
     _attr_div_fixed=Seq.append([_attr_div_fixed1],List.ofArray([_attr_div_fixed2]));
     _attr_container1=AttrProxy.Create("class","container");
     _attr_container2=AttrModule.Style("margin-top","100px");
     _attrs_container=Seq.append([_attr_container1],List.ofArray([_attr_container2]));
     _attr_container11=AttrProxy.Create("class","container");
     _attr_container21=AttrModule.Style("margin-top","100px");
     _attrs_container1=Seq.append([_attr_container11],List.ofArray([_attr_container21]));
     ats=List.ofArray([AttrProxy.Create("class","container top-padding-med ng-scope")]);
     arg002=function()
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
     _arg00_1=View.Map(arg002,arg10);
     return Doc.Element("div",ats,List.ofArray([Doc.Element("div",attrs_div,List.ofArray([Doc.EmbedView(_arg00_1)]))]));
    },
    doc:function(post)
    {
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel panel-primary")]),List.ofArray([Client["doc'header"](post),Client["doc'body"](post)]));
    },
    "doc'body":function(post)
    {
     var _attr_title1,_attr_title2,_attrs_title,domNode,_,arg20;
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
     var domNode,_,_1,href,_a_attr_1,_a_attr_list;
     _=post.Title;
     domNode=jQuery(PrintfHelpers.toSafe("<div>")+PrintfHelpers.toSafe(_)+PrintfHelpers.toSafe("</div>")).get(0);
     _1=post.Reference;
     href=PrintfHelpers.toSafe("./story/")+PrintfHelpers.toSafe(_1);
     _a_attr_1=AttrProxy.Create("href",href);
     _a_attr_list=List.ofArray([_a_attr_1]);
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel-heading")]),List.ofArray([Doc.Element("h3",List.ofArray([AttrProxy.Create("class","panel-title")]),List.ofArray([Doc.Element("a",_a_attr_list,List.ofArray([Doc.Static(domNode)]))]))]));
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
   PageClient:{
    AddNewPageButton:function(page)
    {
     var x,x1,_attr_href_1,_attrs_href,_attr_home_href_1,_attrs_home_href,_attr_page_button_1,_attr_page_button_2,_attrs_page_button,_,x2,ats,caption,_arg20_,arg20,_arg20_1;
     x="./page/"+Global.String(page);
     x1="Page "+Global.String(page);
     _attr_href_1=AttrProxy.Create("href",x);
     _attrs_href=Seq.append([_attr_href_1],Runtime.New(T,{
      $:0
     }));
     _attr_home_href_1=AttrProxy.Create("href","./");
     _attrs_home_href=Seq.append([_attr_home_href_1],Runtime.New(T,{
      $:0
     }));
     _attr_page_button_1=AttrProxy.Create("class","pageButton");
     _attr_page_button_2=AttrProxy.Create("id","pageButton");
     _attrs_page_button=Seq.append([_attr_page_button_1],Runtime.New(T,{
      $:0
     }));
     if(page<10?page>0:false)
      {
       x2="page"+Global.String(page)+"li";
       ats=List.ofArray([AttrProxy.Create("id",x2)]);
       caption="Page "+String(page);
       _arg20_=function()
       {
        var arg00;
        arg00=Concurrency.Delay(function()
        {
         return Concurrency.Bind(PageClient.GetPageArticles(page),function(_arg11)
         {
          var action;
          PageClient.postList().Clear();
          action=function(x3)
          {
           return PageClient.postList().Add(x3);
          };
          Seq.iter(action,_arg11);
          return Concurrency.Return(null);
         });
        });
        return Concurrency.Start(arg00,{
         $:0
        });
       };
       _=Doc.Element("li",ats,List.ofArray([Doc.Button(caption,_attrs_page_button,_arg20_)]));
      }
     else
      {
       _arg20_1=function()
       {
        var arg00;
        arg00=Concurrency.Delay(function()
        {
         return Concurrency.Bind(PageClient.GetPageArticles(0),function(_arg21)
         {
          var action;
          PageClient.postList().Clear();
          action=function(x3)
          {
           return PageClient.postList().Add(x3);
          };
          Seq.iter(action,_arg21);
          return Concurrency.Return(null);
         });
        });
        return Concurrency.Start(arg00,{
         $:0
        });
       };
       arg20=List.ofArray([Doc.Button("Home",_attrs_page_button,_arg20_1)]);
       _=Doc.Element("li",[],arg20);
      }
     return _;
    },
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
      x="./api/page/"+Global.String(id);
      return Concurrency.Bind(PageClient.Ajax("GET",x,null),function(_arg1)
      {
       var _;
       _=Provider.get_Default();
       return Concurrency.Return(((_.DecodeList(_.DecodeRecord(undefined,[["Title",Id,0],["Introduction",Id,0],["Body",Id,0],["Reference",Id,0],["Published",Id,0],["Updated",_.DecodeDateTime(),0]])))())(JSON.parse(_arg1)));
      });
     });
    },
    Main:function(pageID)
    {
     var _attr_divid,_attr_divstyle,_attr_divclass,attrs_div,arg00,arg001,_attr_div_fixed1,_attr_div_fixed2,_attr_div_fixed,_attr_container1,_attr_container2,_attrs_container,_attr_container11,_attr_container21,_attrs_container1,ats,arg002,arg10,_arg00_1;
     _attr_divid=AttrProxy.Create("id","blogItems");
     _attr_divstyle=AttrModule.Style("margin-top","60px");
     _attr_divclass=AttrModule.Class("col-md-12");
     attrs_div=Seq.append([_attr_divid,_attr_divstyle],List.ofArray([_attr_divclass]));
     arg00=PageClient["v'page"]();
     Var1.Set(arg00,pageID);
     arg001=Concurrency.Delay(function()
     {
      return Concurrency.Bind(PageClient.GetPageArticles(Var1.Get(PageClient["v'page"]())),function(_arg1)
      {
       var action;
       action=function(x)
       {
        return PageClient.postList().Add(x);
       };
       Seq.iter(action,_arg1);
       return Concurrency.Return(null);
      });
     });
     Concurrency.Start(arg001,{
      $:0
     });
     _attr_div_fixed1=AttrProxy.Create("role","navigation");
     _attr_div_fixed2=AttrProxy.Create("class","navbar navbar-inverse navbar-fixed-top");
     _attr_div_fixed=Seq.append([_attr_div_fixed1],List.ofArray([_attr_div_fixed2]));
     _attr_container1=AttrProxy.Create("class","container");
     _attr_container2=AttrModule.Style("margin-top","100px");
     _attrs_container=Seq.append([_attr_container1],List.ofArray([_attr_container2]));
     _attr_container11=AttrProxy.Create("class","container");
     _attr_container21=AttrModule.Style("margin-top","100px");
     _attrs_container1=Seq.append([_attr_container11],List.ofArray([_attr_container21]));
     ats=List.ofArray([AttrProxy.Create("class","container top-padding-med ng-scope")]);
     arg002=function()
     {
      var _arg00_,_arg10_;
      _arg00_=function(p)
      {
       return PageClient.doc(p);
      };
      _arg10_=ListModel1.View(PageClient.postList());
      return Doc.Convert(_arg00_,_arg10_);
     };
     arg10=PageClient["v'blog"]();
     _arg00_1=View.Map(arg002,arg10);
     return Doc.Element("div",ats,List.ofArray([Doc.Element("div",attrs_div,List.ofArray([Doc.EmbedView(_arg00_1)]))]));
    },
    Search:Runtime.Field(function()
    {
     var _attr_ul1,attrs_ul,_attr_div_right_1,_attr_div_right_2,_attrs_div_right,_attr_input_1_1,_attr_input_1_2,_attr_input_1_4,_attrs_input_1,_attr_input_2_1,_attr_input_2_2,_attr_input_2_3,_attrs_input_2,_attr_form_1,_attrs_form,_attr_srch_btn1,_attrs_srch_btn,ats,arg20,arg201,_arg20_;
     _attr_ul1=AttrProxy.Create("style","margin-left:0px; padding: 0px;");
     attrs_ul=Seq.append([_attr_ul1],Runtime.New(T,{
      $:0
     }));
     _attr_div_right_1=AttrProxy.Create("role","navigation");
     _attr_div_right_2=AttrProxy.Create("class","pull-right");
     _attrs_div_right=Seq.append([_attr_div_right_1],List.ofArray([_attr_div_right_2]));
     _attr_input_1_1=AttrProxy.Create("placeholder","Search");
     _attr_input_1_2=AttrProxy.Create("type","text");
     _attr_input_1_4=AttrProxy.Create("maxlength","120");
     _attrs_input_1=Seq.append([_attr_input_1_1,_attr_input_1_2],List.ofArray([_attr_input_1_4]));
     _attr_input_2_1=AttrProxy.Create("type","submit");
     _attr_input_2_2=AttrProxy.Create("value"," > ");
     _attr_input_2_3=AttrProxy.Create("class","tfbutton2");
     _attrs_input_2=Seq.append([_attr_input_2_1,_attr_input_2_2],List.ofArray([_attr_input_2_3]));
     _attr_form_1=AttrProxy.Create("class","form-wrapper cf");
     _attrs_form=Seq.append([_attr_form_1],Runtime.New(T,{
      $:0
     }));
     _attr_srch_btn1=AttrProxy.Create("type","submit");
     _attrs_srch_btn=Seq.append([_attr_srch_btn1],Runtime.New(T,{
      $:0
     }));
     ats=List.ofArray([AttrProxy.Create("class","navbar-collapse collapse")]);
     _arg20_=function()
     {
      var keyEncode,newLocation;
      keyEncode=Global.encodeURIComponent(Var1.Get(PageClient["v'search"]()));
      newLocation="./search/"+PrintfHelpers.toSafe(keyEncode)+"/0";
      window.location.href=newLocation;
      return;
     };
     arg201=List.ofArray([Doc.Element("form",_attrs_form,List.ofArray([Doc.Input(_attrs_input_1,PageClient["v'search"]()),Doc.Button("Search",_attrs_srch_btn,_arg20_)]))]);
     arg20=List.ofArray([Doc.Element("ul",attrs_ul,List.ofArray([Doc.Element("li",[],arg201)]))]);
     return Doc.Element("div",ats,List.ofArray([Doc.Element("ul",List.ofArray([AttrProxy.Create("class","nav navbar-nav")]),Seq.toList(Seq.delay(function()
     {
      return Seq.map(function(x)
      {
       return x;
      },PageClient.SitePagination());
     }))),Doc.Element("div",_attrs_div_right,List.ofArray([Doc.Element("nav",[],arg20)]))]));
    }),
    SitePagination:Runtime.Field(function()
    {
     var list1,list;
     list1=Seq.toList(Operators.range(0,9));
     list=List.map(function(x)
     {
      return x;
     },list1);
     return List.map(function(page)
     {
      return PageClient.AddNewPageButton(page);
     },list);
    }),
    doc:function(post)
    {
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel panel-primary")]),List.ofArray([PageClient["doc'header"](post),PageClient["doc'body"](post)]));
    },
    "doc'body":function(post)
    {
     var _attr_title1,_attr_title2,_attrs_title,domNode,_,arg20;
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
     var domNode,_,_1,href,_a_attr_1,_a_attr_list;
     _=post.Title;
     domNode=jQuery(PrintfHelpers.toSafe("<div>")+PrintfHelpers.toSafe(_)+PrintfHelpers.toSafe("</div>")).get(0);
     _1=post.Reference;
     href=PrintfHelpers.toSafe("./story/")+PrintfHelpers.toSafe(_1);
     _a_attr_1=AttrProxy.Create("href",href);
     _a_attr_list=List.ofArray([_a_attr_1]);
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel-heading")]),List.ofArray([Doc.Element("h3",List.ofArray([AttrProxy.Create("class","panel-title")]),List.ofArray([Doc.Element("a",_a_attr_list,List.ofArray([Doc.Static(domNode)]))]))]));
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
       return Concurrency.Return(PageClient.postList());
      });
     };
     View.get_Do();
     arg10=View1.Const(1);
     return View.MapAsync(arg00,arg10);
    }),
    "v'page":Runtime.Field(function()
    {
     return Var.Create(1);
    }),
    "v'search":Runtime.Field(function()
    {
     return Var.Create("");
    }),
    "v'search2":Runtime.Field(function()
    {
     return Var.Create("");
    })
   },
   SearchClient:{
    AddNewSearchButton:function(page,keys)
    {
     var newRef,x,_attr_href_1,_attrs_href,_attr_home_href_1,_attrs_home_href,_,arg20,_1,idAttr,idAttr1,idAttr2,idAttr3,arg201;
     newRef="./search/"+PrintfHelpers.toSafe(keys)+"/"+Global.String(page);
     x="Page "+Global.String(page);
     _attr_href_1=AttrProxy.Create("href",newRef);
     _attrs_href=Seq.append([_attr_href_1],Runtime.New(T,{
      $:0
     }));
     _attr_home_href_1=AttrProxy.Create("href","./");
     _attrs_home_href=Seq.append([_attr_home_href_1],Runtime.New(T,{
      $:0
     }));
     if(page===0)
      {
       arg20=List.ofArray([Doc.Element("a",_attrs_home_href,List.ofArray([Doc.TextNode("Home")]))]);
       _=Doc.Element("li",[],arg20);
      }
     else
      {
       if(page===6)
        {
         idAttr="page"+Global.String(page)+"li";
         _1=Doc.Element("li",List.ofArray([AttrProxy.Create("id",idAttr)]),List.ofArray([Doc.Element("a",_attrs_href,List.ofArray([Doc.TextNode(x)]))]));
        }
       else
        {
         if(page===7)
          {
           idAttr1="page"+Global.String(page)+"li";
           _1=Doc.Element("li",List.ofArray([AttrProxy.Create("id",idAttr1)]),List.ofArray([Doc.Element("a",_attrs_href,List.ofArray([Doc.TextNode(x)]))]));
          }
         else
          {
           if(page===8)
            {
             idAttr2="page"+Global.String(page)+"li";
             _1=Doc.Element("li",List.ofArray([AttrProxy.Create("id",idAttr2)]),List.ofArray([Doc.Element("a",_attrs_href,List.ofArray([Doc.TextNode(x)]))]));
            }
           else
            {
             if(page===9)
              {
               idAttr3="page"+Global.String(page)+"li";
               _1=Doc.Element("li",List.ofArray([AttrProxy.Create("id",idAttr3)]),List.ofArray([Doc.Element("a",_attrs_href,List.ofArray([Doc.TextNode(x)]))]));
              }
             else
              {
               arg201=List.ofArray([Doc.Element("a",_attrs_href,List.ofArray([Doc.TextNode(x)]))]);
               _1=Doc.Element("li",[],arg201);
              }
            }
          }
        }
       _=_1;
      }
     return _;
    },
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
    Main:function(keys,page)
    {
     var _attr_divid,_attr_divstyle,_attr_divclass,attrs_div,keyDecode,arg00,arg001,_attr_div_fixed1,_attr_div_fixed2,_attr_div_fixed,_attr_container1,_attr_container2,_attrs_container,_attr_container11,_attr_container21,_attrs_container1,ats,arg002,arg10,_arg00_1;
     _attr_divid=AttrProxy.Create("id","blogItems");
     _attr_divstyle=AttrModule.Style("margin-top","60px");
     _attr_divclass=AttrModule.Class("col-md-12");
     attrs_div=Seq.append([_attr_divid,_attr_divstyle],List.ofArray([_attr_divclass]));
     keyDecode=Global.decodeURIComponent(keys);
     arg00=SearchClient["v'search"]();
     Var1.Set(arg00,keyDecode);
     arg001=Concurrency.Delay(function()
     {
      return page===0?Concurrency.Bind(SearchClient.PostSearch(Var1.Get(SearchClient["v'search"]())),function(_arg1)
      {
       var action;
       action=function(x)
       {
        return SearchClient.postList().Add(x);
       };
       Seq.iter(action,_arg1);
       return Concurrency.Return(null);
      }):Concurrency.Bind(SearchClient.SearchArticles(Var1.Get(SearchClient["v'search"]()),page),function(_arg2)
      {
       var action;
       action=function(x)
       {
        return SearchClient.postList().Add(x);
       };
       Seq.iter(action,_arg2);
       return Concurrency.Return(null);
      });
     });
     Concurrency.Start(arg001,{
      $:0
     });
     _attr_div_fixed1=AttrProxy.Create("role","navigation");
     _attr_div_fixed2=AttrProxy.Create("class","navbar navbar-inverse navbar-fixed-top");
     _attr_div_fixed=Seq.append([_attr_div_fixed1],List.ofArray([_attr_div_fixed2]));
     _attr_container1=AttrProxy.Create("class","container");
     _attr_container2=AttrModule.Style("margin-top","100px");
     _attrs_container=Seq.append([_attr_container1],List.ofArray([_attr_container2]));
     _attr_container11=AttrProxy.Create("class","container");
     _attr_container21=AttrModule.Style("margin-top","100px");
     _attrs_container1=Seq.append([_attr_container11],List.ofArray([_attr_container21]));
     ats=List.ofArray([AttrProxy.Create("class","container top-padding-med ng-scope")]);
     arg002=function()
     {
      var _arg00_,_arg10_;
      _arg00_=function(p)
      {
       return SearchClient.doc(p);
      };
      _arg10_=ListModel1.View(SearchClient.postList());
      return Doc.Convert(_arg00_,_arg10_);
     };
     arg10=SearchClient["v'blog"]();
     _arg00_1=View.Map(arg002,arg10);
     return Doc.Element("div",ats,List.ofArray([Doc.Element("div",attrs_div,List.ofArray([Doc.EmbedView(_arg00_1)]))]));
    },
    PostSearch:function(keys)
    {
     return Concurrency.Delay(function()
     {
      var x;
      x="./api/search/"+PrintfHelpers.toSafe(keys);
      return Concurrency.Bind(SearchClient.Ajax("POST",x,null),function(_arg1)
      {
       var _;
       _=Provider.get_Default();
       return Concurrency.Return(((_.DecodeList(_.DecodeRecord(undefined,[["Title",Id,0],["Introduction",Id,0],["Body",Id,0],["Reference",Id,0],["Published",Id,0],["Updated",_.DecodeDateTime(),0]])))())(JSON.parse(_arg1)));
      });
     });
    },
    Search:function(keys)
    {
     var arg00,x,_attr_href1_1,_attrs_href1,_attr_div_right_1,_attr_div_right_2,_attrs_div_right,_attr_ul1,attrs_ul,_attr_input_1_1,_attr_input_1_2,_attr_input_1_4,_attrs_input_1,_attr_input_2_1,_attr_input_2_2,_attr_input_2_3,_attrs_input_2,_attr_form_1,_attrs_form,_attr_srch_btn1,_attrs_srch_btn,ats,arg20,arg201,_arg20_;
     arg00=SearchClient["v'search"]();
     Var1.Set(arg00,keys);
     x="./search/"+PrintfHelpers.toSafe(keys)+"/1";
     _attr_href1_1=AttrProxy.Create("href",x);
     _attrs_href1=Seq.append([_attr_href1_1],Runtime.New(T,{
      $:0
     }));
     _attr_div_right_1=AttrProxy.Create("role","navigation");
     _attr_div_right_2=AttrProxy.Create("class","pull-right");
     _attrs_div_right=Seq.append([_attr_div_right_1],List.ofArray([_attr_div_right_2]));
     _attr_ul1=AttrProxy.Create("style","margin-left:0px; padding: 0px;");
     attrs_ul=Seq.append([_attr_ul1],Runtime.New(T,{
      $:0
     }));
     _attr_input_1_1=AttrProxy.Create("placeholder","Search");
     _attr_input_1_2=AttrProxy.Create("type","text");
     _attr_input_1_4=AttrProxy.Create("maxlength","120");
     _attrs_input_1=Seq.append([_attr_input_1_1,_attr_input_1_2],List.ofArray([_attr_input_1_4]));
     _attr_input_2_1=AttrProxy.Create("type","submit");
     _attr_input_2_2=AttrProxy.Create("value"," > ");
     _attr_input_2_3=AttrProxy.Create("class","tfbutton2");
     _attrs_input_2=Seq.append([_attr_input_2_1,_attr_input_2_2],List.ofArray([_attr_input_2_3]));
     _attr_form_1=AttrProxy.Create("class","form-wrapper cf");
     _attrs_form=Seq.append([_attr_form_1],Runtime.New(T,{
      $:0
     }));
     _attr_srch_btn1=AttrProxy.Create("type","submit");
     _attrs_srch_btn=Seq.append([_attr_srch_btn1],Runtime.New(T,{
      $:0
     }));
     ats=List.ofArray([AttrProxy.Create("class","navbar-collapse collapse")]);
     _arg20_=function()
     {
      var arg001;
      arg001=Concurrency.Delay(function()
      {
       SearchClient.postList().Clear();
       return Concurrency.Bind(SearchClient.PostSearch(Global.encodeURIComponent(Var1.Get(SearchClient["v'search"]()))),function(_arg11)
       {
        var action;
        action=function(x1)
        {
         return SearchClient.postList().Add(x1);
        };
        Seq.iter(action,_arg11);
        return Concurrency.Return(null);
       });
      });
      return Concurrency.Start(arg001,{
       $:0
      });
     };
     arg201=List.ofArray([Doc.Element("form",_attrs_form,List.ofArray([Doc.Input(_attrs_input_1,SearchClient["v'search"]()),Doc.Button("Search",_attrs_srch_btn,_arg20_)]))]);
     arg20=List.ofArray([Doc.Element("ul",attrs_ul,List.ofArray([Doc.Element("li",[],arg201)]))]);
     return Doc.Element("div",ats,List.ofArray([Doc.Element("ul",List.ofArray([AttrProxy.Create("class","nav navbar-nav")]),Seq.toList(Seq.delay(function()
     {
      return Seq.map(function(x1)
      {
       return x1;
      },SearchClient.SearchPagination(keys));
     }))),Doc.Element("div",_attrs_div_right,List.ofArray([Doc.Element("nav",[],arg20)]))]));
    },
    SearchArticles:function(keys,page)
    {
     return Concurrency.Delay(function()
     {
      var pageURL;
      pageURL="./api/search/"+PrintfHelpers.toSafe(keys)+"/"+Global.String(page);
      return Concurrency.Bind(SearchClient.Ajax("GET",pageURL,null),function(_arg1)
      {
       var _;
       _=Provider.get_Default();
       return Concurrency.Return(((_.DecodeList(_.DecodeRecord(undefined,[["Title",Id,0],["Introduction",Id,0],["Body",Id,0],["Reference",Id,0],["Published",Id,0],["Updated",_.DecodeDateTime(),0]])))())(JSON.parse(_arg1)));
      });
     });
    },
    SearchPagination:function(keys)
    {
     var list1,list;
     list1=Seq.toList(Operators.range(0,9));
     list=List.map(function(x)
     {
      return[x,keys];
     },list1);
     return List.map(function(tupledArg)
     {
      var page,keys1;
      page=tupledArg[0];
      keys1=tupledArg[1];
      return SearchClient.AddNewSearchButton(page,keys1);
     },list);
    },
    doc:function(post)
    {
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel panel-primary")]),List.ofArray([SearchClient["doc'header"](post),SearchClient["doc'body"](post)]));
    },
    "doc'body":function(post)
    {
     var _attr_title1,_attr_title2,_attrs_title,domNode,_,arg20;
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
     var domNode,_,_1,href,_a_attr_1,_a_attr_list;
     _=post.Title;
     domNode=jQuery(PrintfHelpers.toSafe("<div>")+PrintfHelpers.toSafe(_)+PrintfHelpers.toSafe("</div>")).get(0);
     _1=post.Reference;
     href=PrintfHelpers.toSafe("./story/")+PrintfHelpers.toSafe(_1);
     _a_attr_1=AttrProxy.Create("href",href);
     _a_attr_list=List.ofArray([_a_attr_1]);
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel-heading")]),List.ofArray([Doc.Element("h3",List.ofArray([AttrProxy.Create("class","panel-title")]),List.ofArray([Doc.Element("a",_a_attr_list,List.ofArray([Doc.Static(domNode)]))]))]));
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
       return Concurrency.Return(SearchClient.postList());
      });
     };
     View.get_Do();
     arg10=View1.Const(1);
     return View.MapAsync(arg00,arg10);
    }),
    "v'page":Runtime.Field(function()
    {
     return Var.Create(1);
    }),
    "v'search":Runtime.Field(function()
    {
     return Var.Create("");
    }),
    "v'search2":Runtime.Field(function()
    {
     return Var.Create("");
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
      pageURL="./api/story/"+PrintfHelpers.toSafe(id);
      return Concurrency.Bind(StoryClient.Ajax("GET",pageURL,null),function(_arg1)
      {
       Provider.get_Default();
       return Concurrency.Return((Id())(JSON.parse(_arg1)));
      });
     });
    },
    Main:function(reference)
    {
     var _attr_divid,_attr_divclass,attrs_div,body,title,introduction,_v_body;
     _attr_divid=AttrProxy.Create("id","blogItems");
     _attr_divclass=AttrModule.Class("col-md-12");
     attrs_div=Seq.append([_attr_divid],List.ofArray([_attr_divclass]));
     body="";
     title="";
     introduction="";
     _v_body=Var.Create(body);
     Concurrency.Start(Concurrency.Delay(function()
     {
      return Concurrency.Bind(StoryClient.GetStory(reference),function(_arg1)
      {
       var domNode;
       jQuery("#bodyid").children().remove();
       domNode=jQuery(PrintfHelpers.toSafe("<div>")+PrintfHelpers.toSafe(_arg1)+PrintfHelpers.toSafe("</div>")).get(0);
       jQuery("#bodyid").append(domNode);
       return Concurrency.Return(null);
      });
     }),{
      $:0
     });
     return Doc.Element("div",List.ofArray([AttrProxy.Create("class","container top-padding-med ng-scope")]),List.ofArray([Doc.Element("div",List.ofArray([AttrProxy.Create("id","bodyid")]),List.ofArray([Doc.TextNode("")]))]));
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
     var domNode,_,_1,href,_a_attr_1,_a_attr_list;
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
  Doc=Runtime.Safe(Next.Doc);
  ListModel1=Runtime.Safe(Next.ListModel1);
  View=Runtime.Safe(Next.View);
  PrintfHelpers=Runtime.Safe(Global.WebSharper.PrintfHelpers);
  T=Runtime.Safe(List.T);
  ListModel=Runtime.Safe(Next.ListModel);
  View1=Runtime.Safe(Next.View1);
  Var=Runtime.Safe(Next.Var);
  String=Runtime.Safe(Global.String);
  PageClient=Runtime.Safe(ZeroHedgeWS.PageClient);
  window=Runtime.Safe(Global.window);
  Operators=Runtime.Safe(Global.WebSharper.Operators);
  SearchClient=Runtime.Safe(ZeroHedgeWS.SearchClient);
  return StoryClient=Runtime.Safe(ZeroHedgeWS.StoryClient);
 });
 Runtime.OnLoad(function()
 {
  SearchClient["v'search2"]();
  SearchClient["v'search"]();
  SearchClient["v'page"]();
  SearchClient["v'blog"]();
  SearchClient.postList();
  PageClient["v'search2"]();
  PageClient["v'search"]();
  PageClient["v'page"]();
  PageClient["v'blog"]();
  PageClient.postList();
  PageClient.SitePagination();
  PageClient.Search();
  Client["v'page"]();
  Client["v'blog"]();
  Client.postList();
  return;
 });
}());