(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,console,alert,Forms,Pervasives,Form1,Form,Validation,List,Bootstrap,Controls,T,Application2,Client,UI,Next,AttrProxy,Simple,Doc,Var,Submitter,Remoting,AjaxRemotingProvider,Concurrency,View,AttrModule,Seq,ListModel1,String,Var1,jQuery,ListModel,View1,DateTimeHelpers,ListPage;
 Runtime.Define(Global,{
  Application2:{
   Client:{
    LoginForm:function()
    {
     var _arg00_,_arg00_1,_arg10_,_arg10_1,_arg10_2,x,renderFunction;
     if(console)
      {
       console.log("Trying to create LoginForm");
      }
     _arg00_=function(tupledArg)
     {
      var user;
      user=tupledArg[0];
      tupledArg[1];
      tupledArg[2];
      return alert("Welcome, "+user+"!");
     };
     _arg10_=Form.Yield("");
     _arg10_1=Form.Yield("");
     _arg00_1=Pervasives.op_LessMultiplyGreater(Pervasives.op_LessMultiplyGreater(Pervasives.op_LessMultiplyGreater(Form1.Return(function(user)
     {
      return function(pass)
      {
       return function(check)
       {
        return[user,pass,check];
       };
      };
     }),Validation.IsNotEmpty("Must enter a username",_arg10_)),Validation.IsNotEmpty("Must enter a password",_arg10_1)),Form.Yield(false));
     _arg10_2=Form.WithSubmit(_arg00_1);
     x=Form.Run(_arg00_,_arg10_2);
     renderFunction=function()
     {
      return function(pass)
      {
       return function(check)
       {
        return function(submit)
        {
         var arg20,clo1,arg10,clo2,tupledArg,arg201,arg21,arg22;
         clo1=Controls.TextAreaWithError("Username - echoed");
         arg10=Runtime.New(T,{
          $:0
         });
         clo2=clo1(arg10);
         tupledArg=[Client.rvUsername(),Runtime.New(T,{
          $:0
         }),Runtime.New(T,{
          $:0
         })];
         arg201=tupledArg[0];
         arg21=tupledArg[1];
         arg22=tupledArg[2];
         arg20=List.ofArray([Controls.Input("Username",Runtime.New(T,{
          $:0
         }),Client.rvUsername(),List.ofArray([(Client.cls())("sr-only")]),List.ofArray([(Client.cls())("input-lg"),AttrProxy.Create("readonly","")])),Simple.TextAreaWithError("Username - echoed",Client.rvUsername(),submit.get_View()),(clo2([arg201,arg21,arg22]))(submit.get_View()),Simple.InputWithError("Username - echoed",Client.rvUsername(),submit.get_View()),Simple.InputPasswordWithError("Password",pass,submit.get_View()),Controls.Checkbox("Keep me logged in",Runtime.New(T,{
          $:0
         }),check,List.ofArray([(Client.cls())("input-lg")]),Runtime.New(T,{
          $:0
         })),Controls.Radio("This is a radio button",Runtime.New(T,{
          $:0
         }),check,Runtime.New(T,{
          $:0
         }),Runtime.New(T,{
          $:0
         })),(((Controls.Button())("Log in"))(List.ofArray([(Controls.Class())("btn btn-primary")])))(function()
         {
          return submit.Trigger();
         }),Controls.ShowErrors(List.ofArray([AttrProxy.Create("style","margin-top:1em;")]),submit.get_View())]);
         return Doc.Element("form",[],arg20);
        };
       };
      };
     };
     return Form1.Render(renderFunction,x);
    },
    Main:function()
    {
     var blog,rvInput,submit,arg00,arg10,vReversed,_attr_my1,_attr_my2,attrs,_attr_divid,_attr_divclass,attrs_div,_attr_login_divid,_attr_login_divclass,_attrs_login_div,_attr_login_style,_attrs_login_style_seq,ats,ats1,ats2,ats3,ats4,arg20,_arg00_,arg001,arg101,_arg00_2;
     Client.postList().Add(Client.post1());
     Client.postList().Add(Client.post2());
     blog={
      Posts:Client.postList()
     };
     rvInput=Var.Create("");
     submit=Submitter.CreateOption(rvInput.get_View());
     arg00=function(_arg1)
     {
      var _,input;
      if(_arg1.$==1)
       {
        input=_arg1.$0;
        _=AjaxRemotingProvider.Async("Application2:0",[input]);
       }
      else
       {
        _=Concurrency.Delay(function()
        {
         return Concurrency.Return("");
        });
       }
      return _;
     };
     arg10=submit.get_View();
     vReversed=View.MapAsync(arg00,arg10);
     _attr_my1=AttrProxy.Create("placeholder","Search");
     _attr_my2=AttrModule.Class("mainsearch");
     attrs=Seq.append([_attr_my1],List.ofArray([_attr_my2]));
     _attr_divid=AttrProxy.Create("id","blogItems");
     _attr_divclass=AttrModule.Class("col-md-12");
     attrs_div=Seq.append([_attr_divid],List.ofArray([_attr_divclass]));
     _attr_login_divid=AttrProxy.Create("id","loginform");
     _attr_login_divclass=AttrModule.Class("divhidden");
     _attrs_login_div=Seq.append([_attr_login_divid],List.ofArray([_attr_login_divclass]));
     _attr_login_style=AttrModule.Style("visibility","hidden");
     _attrs_login_style_seq=Seq.append(List.ofArray([_attr_login_divclass]),List.ofArray([_attr_login_divid]));
     ats=List.ofArray([AttrModule.Class("container")]);
     ats1=List.ofArray([AttrModule.Class("navbar navbar-default navbar-fixed-top")]);
     ats2=List.ofArray([AttrModule.Class("navbar-collapse collapse")]);
     ats3=List.ofArray([AttrModule.Class("navbar-collapse collapse")]);
     ats4=List.ofArray([AttrModule.Class("pull-right")]);
     arg20=List.ofArray([Doc.Input(attrs,Client.rvUsername())]);
     _arg00_=Client.loaddata();
     arg001=function()
     {
      var _arg00_1,_arg10_;
      _arg00_1=function(p)
      {
       return Client.doc(p);
      };
      _arg10_=ListModel1.View(blog.Posts);
      return Doc.Convert(_arg00_1,_arg10_);
     };
     arg101=Client["v'blog"]();
     _arg00_2=View.Map(arg001,arg101);
     return Doc.Element("div",ats,List.ofArray([Doc.Element("div",ats1,List.ofArray([Client["doc'nav'left"](),Doc.Element("div",ats2,List.ofArray([Doc.Element("div",ats3,List.ofArray([Doc.Element("div",ats4,List.ofArray([Doc.Element("div",List.ofArray([AttrModule.Class("nav")]),List.ofArray([Doc.Element("ul",List.ofArray([AttrModule.Class("nav navbar-nav sm sm-collapsible")]),List.ofArray([Doc.Element("li",[],arg20)])),Doc.EmbedView(_arg00_)]))]))]))]))])),Doc.Element("div",attrs_div,List.ofArray([Doc.EmbedView(_arg00_2)]))]));
    },
    addNewBlog:Runtime.Field(function()
    {
     var _,a;
     _=Client.iid()+1;
     Client.iid=function()
     {
      return _;
     };
     Client.postList().Add({
      Id:Client.iid(),
      Title:Var.Create("New item "+String(Client.iid())+" title"),
      Content:Var.Create("44444fsfsdf"),
      CreateDate:"2015-01-01",
      EditDate:Var.Create("2015-01-01"),
      Num:Var.Create(1),
      EditedTitle:Var.Create("dsadsdsa"),
      EditedContent:Var.Create("dsadsad"),
      IsEditMode:Var.Create(false)
     });
     a=Client.iid();
     return console?console.log(a):undefined;
    }),
    cls:Runtime.Field(function()
    {
     return function(name)
     {
      return AttrModule.Class(name);
     };
    }),
    doc:function(post)
    {
     var arg10;
     arg10="post-"+Global.String(post.Id)+"-article";
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel panel-primary"),AttrProxy.Create("id",arg10)]),List.ofArray([Client["doc'header"](post),Client["doc'body"](post)]));
    },
    "doc'body":function(post)
    {
     var ats,arg20,arg201,_arg10_,_arg20_;
     ats=List.ofArray([AttrModule.Class("panel-body")]);
     arg20=List.ofArray([Doc.TextView(post.Content.get_View())]);
     arg201=List.ofArray([Doc.TextView(post.EditDate.get_View())]);
     _arg10_=List.ofArray([AttrModule.Class("btn btn-default")]);
     _arg20_=function()
     {
      Var1.Set(Client.rvUsername(),Var1.Get(post.Content));
      jQuery("#blogItems").hide();
      jQuery("#loginform").removeClass("divhidden").addClass("divshown");
      return console?console.log(1):undefined;
     };
     return Doc.Element("div",ats,List.ofArray([Doc.Element("div",[],arg20),Doc.Element("p",[],arg201),Doc.Button("New Item",_arg10_,_arg20_)]));
    },
    "doc'header":function(post)
    {
     var arg20;
     arg20=List.ofArray([Doc.TextView(post.Title.get_View())]);
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel-heading")]),List.ofArray([Doc.Element("div",[],arg20)]));
    },
    "doc'main'menu":Runtime.Field(function()
    {
     var ats,ats1,ats2,arg20,arg201,arg202,ats3,ats4,arg203,arg204,arg205,arg206,arg207,arg208,arg209;
     ats=List.ofArray([AttrModule.Class("nav navbar-nav")]);
     ats1=List.ofArray([AttrModule.Class("dropdown open")]);
     ats2=List.ofArray([AttrModule.Class("dropdown-menu")]);
     arg20=List.ofArray([Doc.TextNode("System Configuration")]);
     arg201=List.ofArray([Doc.TextNode("System Configuration")]);
     arg202=List.ofArray([Doc.TextNode("System Configuration")]);
     ats3=List.ofArray([AttrModule.Class("dropdown open")]);
     ats4=List.ofArray([AttrModule.Class("dropdown-menu")]);
     arg203=List.ofArray([Doc.TextNode("Interface report")]);
     arg204=List.ofArray([Doc.TextNode("Process Schedule Maintenance")]);
     arg205=List.ofArray([Doc.TextNode("Calendar Management")]);
     arg206=List.ofArray([Doc.TextNode("System Utility")]);
     arg207=List.ofArray([Doc.TextNode("System Maintenance")]);
     arg208=List.ofArray([Doc.TextNode("Payroll Process Management")]);
     arg209=List.ofArray([Doc.TextNode("Human Resource Management")]);
     return Doc.Element("ul",ats,List.ofArray([Doc.Element("li",ats1,List.ofArray([Doc.Element("a",List.ofArray([AttrModule.Class("dropdown-toggle")]),List.ofArray([Doc.TextNode("System Configuration")])),Doc.Element("ul",ats2,List.ofArray([Doc.Element("li",[],arg20),Doc.Element("li",[],arg201),Doc.Element("li",[],arg202)]))])),Doc.Element("li",ats3,List.ofArray([Doc.Element("a",List.ofArray([AttrModule.Class("dropdown-toggle")]),List.ofArray([Doc.TextNode("System Utility")])),Doc.Element("ul",ats4,List.ofArray([Doc.Element("li",[],arg203),Doc.Element("li",[],arg204),Doc.Element("li",[],arg205)]))])),Doc.Element("li",[],arg206),Doc.Element("li",[],arg207),Doc.Element("li",[],arg208),Doc.Element("li",[],arg209)]));
    }),
    "doc'nav'left":Runtime.Field(function()
    {
     var arg20;
     arg20=List.ofArray([Doc.TextNode("jkhkjhjk")]);
     return Doc.Element("div",[],arg20);
    }),
    iid:Runtime.Field(function()
    {
     return 5;
    }),
    loaddata:Runtime.Field(function()
    {
     var x,arg00;
     x=Client["v'menu"]().get_View();
     arg00=function()
     {
      return Concurrency.Delay(function()
      {
       var x1;
       x1=AjaxRemotingProvider.Async("Application2:1",[0,"WEBSITE"]);
       return Concurrency.Bind(x1,function(_arg1)
       {
        var patternInput,english,a;
        patternInput=_arg1.$0;
        patternInput[4];
        patternInput[0];
        patternInput[3];
        english=patternInput[1];
        patternInput[2];
        _arg1.$0;
        a="trying to get menu from server "+english;
        console?console.log(a):undefined;
        return Concurrency.Return(Doc.get_Empty());
       });
      });
     };
     return View.MapAsync(arg00,x);
    }),
    post1:Runtime.Field(function()
    {
     return{
      Id:1,
      Title:Var.Create("111ssdfds"),
      Content:Var.Create("333fsfsdf"),
      CreateDate:"2015-01-01",
      EditDate:Var.Create("2015-01-01"),
      Num:Var.Create(1),
      EditedTitle:Var.Create("dsadsdsa"),
      EditedContent:Var.Create("dsadsad"),
      IsEditMode:Var.Create(false)
     };
    }),
    post2:Runtime.Field(function()
    {
     return{
      Id:2,
      Title:Var.Create("222ssdfds"),
      Content:Var.Create("44444fsfsdf"),
      CreateDate:"2015-01-01",
      EditDate:Var.Create("2015-01-01"),
      Num:Var.Create(1),
      EditedTitle:Var.Create("dsadsdsa"),
      EditedContent:Var.Create("dsadsad"),
      IsEditMode:Var.Create(false)
     };
    }),
    postList:Runtime.Field(function()
    {
     var arg00,arg10;
     arg00=function(i)
     {
      return i.Id<<0;
     };
     arg10=Runtime.New(T,{
      $:0
     });
     return ListModel.Create(arg00,arg10);
    }),
    rvSubmit:Runtime.Field(function()
    {
     return Var.Create(null);
    }),
    rvUsername:Runtime.Field(function()
    {
     return Var.Create("");
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
    "v'menu":Runtime.Field(function()
    {
     return Var.Create("");
    })
   },
   ListPage:{
    ListDataToPosts:function(_,_1,_2,_3,_4)
    {
     var data,Title,Id,EditDate,CreateDate,Content,CreateDate1,EditDate1,Content1,post;
     data=[_,_1,_2,_3,_4];
     Title=data[4];
     Id=data[0];
     EditDate=data[2];
     CreateDate=data[1];
     Content=data[3];
     CreateDate1=DateTimeHelpers.LongDate(CreateDate);
     EditDate1=Var.Create(DateTimeHelpers.LongDate(EditDate));
     Content1=Var.Create(Content);
     post={
      Id:Id,
      Title:Var.Create(Title),
      Content:Content1,
      CreateDate:CreateDate1,
      EditDate:EditDate1
     };
     return post;
    },
    Main:function()
    {
     var blog,rvInput,submit,arg00,arg10,vReversed,_attr_container1,_attr_container2,_attrs_container,_attr_title1,_attr_title2,_attrs_title,_attr_content1,_attr_content2,_attrs_content,_attr_search1,_attr_search2,_attrs_search,_attr_divid,_attr_divclass,attrs_div,_attr_login_divid,_attr_login_divclass,_attrs_login_div,_attr_login_style,_attrs_login_style_seq,replacePost,ats,arg20,arg201,_arg10_,_arg20_,ats1,ats2,ats3,ats4,arg202;
     Concurrency.Start(Concurrency.Delay(function()
     {
      return Concurrency.Bind(AjaxRemotingProvider.Async("Application2:3",[]),function(_arg1)
      {
       Seq.iter(function(x)
       {
        return ListPage.postList().Add(x);
       },List.map(function(tupledArg)
       {
        return ListPage.ListDataToPosts(tupledArg[0],tupledArg[1],tupledArg[2],tupledArg[3],tupledArg[4]);
       },_arg1));
       return Concurrency.Return(null);
      });
     }),{
      $:0
     });
     blog={
      Posts:ListPage.postList()
     };
     rvInput=Var.Create("");
     submit=Submitter.CreateOption(rvInput.get_View());
     arg00=function(_arg2)
     {
      return _arg2.$==1?AjaxRemotingProvider.Async("Application2:0",[_arg2.$0]):Concurrency.Delay(function()
      {
       return Concurrency.Return("");
      });
     };
     arg10=submit.get_View();
     vReversed=View.MapAsync(arg00,arg10);
     _attr_container1=AttrProxy.Create("class","container");
     _attr_container2=AttrModule.Style("margin-top","100px");
     _attrs_container=Seq.append([_attr_container1],List.ofArray([_attr_container2]));
     _attr_title1=AttrProxy.Create("placeholder","Title");
     _attr_title2=AttrModule.Class("form-control");
     _attrs_title=Seq.append([_attr_title1],List.ofArray([_attr_title2]));
     _attr_content1=AttrProxy.Create("placeholder","Content");
     _attr_content2=AttrModule.Class("form-control");
     _attrs_content=Seq.append([_attr_content1],List.ofArray([_attr_content2]));
     _attr_search1=AttrProxy.Create("placeholder","Search");
     _attr_search2=AttrModule.Class("mainsearch");
     _attrs_search=Seq.append([_attr_search1],List.ofArray([_attr_search2]));
     _attr_divid=AttrProxy.Create("id","blogItems");
     _attr_divclass=AttrModule.Class("col-md-12");
     attrs_div=Seq.append([_attr_divid],List.ofArray([_attr_divclass]));
     _attr_login_divid=AttrProxy.Create("id","loginform");
     _attr_login_divclass=AttrModule.Class("divhidden");
     _attrs_login_div=Seq.append([_attr_login_divid],List.ofArray([_attr_login_divclass]));
     _attr_login_style=AttrModule.Style("display","none");
     _attrs_login_style_seq=Seq.append(List.ofArray([_attr_login_divclass]),List.ofArray([_attr_login_divid]));
     replacePost=function(post)
     {
      var newPost;
      newPost={
       Id:post.Id,
       Title:Var.Create(Var1.Get(ListPage.rvPostTitle())),
       Content:Var.Create(Var1.Get(ListPage.rvPostContent())),
       CreateDate:"2015-01-01",
       EditDate:Var.Create("2015-01-01")
      };
      return newPost.Id>0?{
       $:1,
       $0:newPost
      }:{
       $:0
      };
     };
     ats=List.ofArray([AttrProxy.Create("class","form-group")]);
     arg20=List.ofArray([Doc.TextNode("Title")]);
     arg201=List.ofArray([Doc.TextNode("Content")]);
     _arg10_=List.ofArray([AttrModule.Class("btn btn-default")]);
     _arg20_=function()
     {
      jQuery("#blogItems").show();
      jQuery("#loginform").removeClass("divshown").addClass("divhidden");
      return console?console.log(1):undefined;
     };
     ats1=List.ofArray([AttrModule.Class("navbar navbar-default navbar-fixed-top")]);
     ats2=List.ofArray([AttrModule.Class("navbar-collapse collapse")]);
     ats3=List.ofArray([AttrModule.Class("navbar-collapse collapse")]);
     ats4=List.ofArray([AttrModule.Class("pull-right")]);
     arg202=List.ofArray([Doc.Input(_attrs_search,ListPage.rvSearch())]);
     return Doc.Element("div",_attrs_container,List.ofArray([Doc.Element("div",_attrs_login_style_seq,List.ofArray([Doc.Element("div",ats,List.ofArray([Doc.Element("div",List.ofArray([AttrProxy.Create("class","col-sm-12")]),List.ofArray([Doc.Element("label",[],arg20)])),Doc.Element("div",List.ofArray([AttrProxy.Create("class","col-sm-12")]),List.ofArray([Doc.InputArea(_attrs_title,ListPage.rvPostTitle())])),Doc.Element("div",List.ofArray([AttrProxy.Create("class","col-sm-12")]),List.ofArray([Doc.Element("label",[],arg201)])),Doc.Element("div",List.ofArray([AttrProxy.Create("class","col-sm-12")]),List.ofArray([Doc.InputArea(_attrs_title,ListPage.rvPostContent())])),Doc.Button("Save",_arg10_,function()
     {
      jQuery("#blogItems").show();
      jQuery("#loginform").removeClass("divshown").addClass("divhidden");
      return Concurrency.Start(Concurrency.Delay(function()
      {
       return Concurrency.Bind(AjaxRemotingProvider.Async("Application2:4",[Var1.Get(ListPage.rvRowIndex()),Var1.Get(ListPage.rvPostTitle()),Var1.Get(ListPage.rvPostContent())]),function()
       {
        var arg101;
        ListPage.postList().FindByKey(Var1.Get(ListPage.rvRowIndex()));
        arg101=Var1.Get(ListPage.rvRowIndex());
        ListPage.postList().UpdateBy(replacePost,arg101);
        return Concurrency.Return(null);
       });
      }),{
       $:0
      });
     }),Doc.Button("Cancel",List.ofArray([AttrModule.Class("btn btn-default")]),_arg20_)]))])),Doc.Element("div",ats1,List.ofArray([ListPage["doc'nav'left"](),Doc.Element("div",ats2,List.ofArray([Doc.Element("div",ats3,List.ofArray([Doc.Element("div",ats4,List.ofArray([Doc.Element("div",List.ofArray([AttrModule.Class("nav")]),List.ofArray([Doc.Element("ul",List.ofArray([AttrModule.Class("nav navbar-nav sm sm-collapsible")]),List.ofArray([Doc.Element("li",[],arg202)])),Doc.EmbedView(ListPage.loaddata())]))]))]))]))])),Doc.Element("div",attrs_div,List.ofArray([Doc.EmbedView(View.Map(function()
     {
      return Doc.Convert(function(p)
      {
       return ListPage.doc(p);
      },ListModel1.View(blog.Posts));
     },ListPage["v'blog"]()))]))]));
    },
    addNewBlog:Runtime.Field(function()
    {
     var _,a;
     _=ListPage.iid()+1;
     ListPage.iid=function()
     {
      return _;
     };
     ListPage.postList().Add({
      Id:ListPage.iid(),
      Title:Var.Create("New item "+String(ListPage.iid())+" title"),
      Content:Var.Create("44444fsfsdf"),
      CreateDate:"2015-01-01",
      EditDate:Var.Create("2015-01-01")
     });
     a=ListPage.iid();
     return console?console.log(a):undefined;
    }),
    cls:Runtime.Field(function()
    {
     return function(name)
     {
      return AttrModule.Class(name);
     };
    }),
    doc:function(post)
    {
     var arg10;
     arg10="post-"+Global.String(post.Id)+"-article";
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel panel-primary"),AttrProxy.Create("id",arg10)]),List.ofArray([ListPage["doc'header"](post),ListPage["doc'body"](post)]));
    },
    "doc'body":function(post)
    {
     var _attr_container1,_attr_container2,_attrs_container,_attr_title1,_attr_title2,_attrs_title,ats,arg20,arg201,_arg10_,_arg20_;
     _attr_container1=AttrProxy.Create("class","container");
     _attr_container2=AttrModule.Style("margin-top","100px");
     _attrs_container=Seq.append([_attr_container1],List.ofArray([_attr_container2]));
     _attr_title1=AttrProxy.Create("placeholder","Title");
     _attr_title2=AttrModule.Class("form-control");
     _attrs_title=Seq.append([_attr_title1],List.ofArray([_attr_title2]));
     ats=List.ofArray([AttrModule.Class("panel-body")]);
     arg20=List.ofArray([Doc.TextView(post.Content.get_View())]);
     arg201=List.ofArray([Doc.TextView(post.EditDate.get_View())]);
     _arg10_=List.ofArray([AttrModule.Class("btn btn-default")]);
     _arg20_=function()
     {
      var arg10;
      Var1.Set(ListPage.rvPostContent(),Var1.Get(post.Content));
      Var1.Set(ListPage.rvPostTitle(),Var1.Get(post.Title));
      arg10=post.Id;
      Var1.Set(ListPage.rvRowIndex(),arg10);
      jQuery("#blogItems").hide();
      jQuery("#loginform").removeClass("divhidden").addClass("divshown");
      return console?console.log(1):undefined;
     };
     return Doc.Element("div",ats,List.ofArray([Doc.Element("div",[],arg20),Doc.Element("p",[],arg201),Doc.Button("New Item",_arg10_,_arg20_)]));
    },
    "doc'header":function(post)
    {
     var arg20;
     arg20=List.ofArray([Doc.TextView(post.Title.get_View())]);
     return Doc.Element("div",List.ofArray([AttrModule.Class("panel-heading")]),List.ofArray([Doc.Element("div",[],arg20)]));
    },
    "doc'main'menu":Runtime.Field(function()
    {
     var ats,ats1,ats2,arg20,arg201,arg202,ats3,ats4,arg203,arg204,arg205,arg206,arg207,arg208,arg209;
     ats=List.ofArray([AttrModule.Class("nav navbar-nav")]);
     ats1=List.ofArray([AttrModule.Class("dropdown open")]);
     ats2=List.ofArray([AttrModule.Class("dropdown-menu")]);
     arg20=List.ofArray([Doc.TextNode("System Configuration")]);
     arg201=List.ofArray([Doc.TextNode("System Configuration")]);
     arg202=List.ofArray([Doc.TextNode("System Configuration")]);
     ats3=List.ofArray([AttrModule.Class("dropdown open")]);
     ats4=List.ofArray([AttrModule.Class("dropdown-menu")]);
     arg203=List.ofArray([Doc.TextNode("Interface report")]);
     arg204=List.ofArray([Doc.TextNode("Process Schedule Maintenance")]);
     arg205=List.ofArray([Doc.TextNode("Calendar Management")]);
     arg206=List.ofArray([Doc.TextNode("System Utility")]);
     arg207=List.ofArray([Doc.TextNode("System Maintenance")]);
     arg208=List.ofArray([Doc.TextNode("Payroll Process Management")]);
     arg209=List.ofArray([Doc.TextNode("Human Resource Management")]);
     return Doc.Element("ul",ats,List.ofArray([Doc.Element("li",ats1,List.ofArray([Doc.Element("a",List.ofArray([AttrModule.Class("dropdown-toggle")]),List.ofArray([Doc.TextNode("System Configuration")])),Doc.Element("ul",ats2,List.ofArray([Doc.Element("li",[],arg20),Doc.Element("li",[],arg201),Doc.Element("li",[],arg202)]))])),Doc.Element("li",ats3,List.ofArray([Doc.Element("a",List.ofArray([AttrModule.Class("dropdown-toggle")]),List.ofArray([Doc.TextNode("System Utility")])),Doc.Element("ul",ats4,List.ofArray([Doc.Element("li",[],arg203),Doc.Element("li",[],arg204),Doc.Element("li",[],arg205)]))])),Doc.Element("li",[],arg206),Doc.Element("li",[],arg207),Doc.Element("li",[],arg208),Doc.Element("li",[],arg209)]));
    }),
    "doc'nav'left":Runtime.Field(function()
    {
     var arg20;
     arg20=List.ofArray([Doc.TextNode("jkhkjhjk")]);
     return Doc.Element("div",[],arg20);
    }),
    iid:Runtime.Field(function()
    {
     return 5;
    }),
    loaddata:Runtime.Field(function()
    {
     var x,arg00;
     x=ListPage["v'menu"]().get_View();
     arg00=function()
     {
      return Concurrency.Delay(function()
      {
       var x1;
       x1=AjaxRemotingProvider.Async("Application2:1",[0,"WEBSITE"]);
       return Concurrency.Bind(x1,function(_arg1)
       {
        var patternInput,english,a;
        patternInput=_arg1.$0;
        patternInput[4];
        patternInput[0];
        patternInput[3];
        english=patternInput[1];
        patternInput[2];
        _arg1.$0;
        a="trying to get menu from server inside ListPage"+english;
        console?console.log(a):undefined;
        return Concurrency.Return(Doc.get_Empty());
       });
      });
     };
     return View.MapAsync(arg00,x);
    }),
    postList:Runtime.Field(function()
    {
     var arg00,arg10;
     arg00=function(i)
     {
      return i.Id<<0;
     };
     arg10=Runtime.New(T,{
      $:0
     });
     return ListModel.Create(arg00,arg10);
    }),
    rvPostContent:Runtime.Field(function()
    {
     return Var.Create("");
    }),
    rvPostTitle:Runtime.Field(function()
    {
     return Var.Create("");
    }),
    rvRowIndex:Runtime.Field(function()
    {
     return Var.Create(0);
    }),
    rvSearch:Runtime.Field(function()
    {
     return Var.Create("");
    }),
    rvSubmit:Runtime.Field(function()
    {
     return Var.Create(null);
    }),
    "v'blog":Runtime.Field(function()
    {
     var arg00,arg10;
     arg00=function()
     {
      return Concurrency.Delay(function()
      {
       return Concurrency.Return(ListPage.postList());
      });
     };
     View.get_Do();
     arg10=View1.Const(1);
     return View.MapAsync(arg00,arg10);
    }),
    "v'menu":Runtime.Field(function()
    {
     return Var.Create("");
    })
   }
  }
 });
 Runtime.OnInit(function()
 {
  console=Runtime.Safe(Global.console);
  alert=Runtime.Safe(Global.alert);
  Forms=Runtime.Safe(Global.WebSharper.Forms);
  Pervasives=Runtime.Safe(Forms.Pervasives);
  Form1=Runtime.Safe(Forms.Form1);
  Form=Runtime.Safe(Forms.Form);
  Validation=Runtime.Safe(Forms.Validation);
  List=Runtime.Safe(Global.WebSharper.List);
  Bootstrap=Runtime.Safe(Forms.Bootstrap);
  Controls=Runtime.Safe(Bootstrap.Controls);
  T=Runtime.Safe(List.T);
  Application2=Runtime.Safe(Global.Application2);
  Client=Runtime.Safe(Application2.Client);
  UI=Runtime.Safe(Global.WebSharper.UI);
  Next=Runtime.Safe(UI.Next);
  AttrProxy=Runtime.Safe(Next.AttrProxy);
  Simple=Runtime.Safe(Controls.Simple);
  Doc=Runtime.Safe(Next.Doc);
  Var=Runtime.Safe(Next.Var);
  Submitter=Runtime.Safe(Next.Submitter);
  Remoting=Runtime.Safe(Global.WebSharper.Remoting);
  AjaxRemotingProvider=Runtime.Safe(Remoting.AjaxRemotingProvider);
  Concurrency=Runtime.Safe(Global.WebSharper.Concurrency);
  View=Runtime.Safe(Next.View);
  AttrModule=Runtime.Safe(Next.AttrModule);
  Seq=Runtime.Safe(Global.WebSharper.Seq);
  ListModel1=Runtime.Safe(Next.ListModel1);
  String=Runtime.Safe(Global.String);
  Var1=Runtime.Safe(Next.Var1);
  jQuery=Runtime.Safe(Global.jQuery);
  ListModel=Runtime.Safe(Next.ListModel);
  View1=Runtime.Safe(Next.View1);
  DateTimeHelpers=Runtime.Safe(Global.WebSharper.DateTimeHelpers);
  return ListPage=Runtime.Safe(Application2.ListPage);
 });
 Runtime.OnLoad(function()
 {
  ListPage["v'menu"]();
  ListPage["v'blog"]();
  ListPage.rvSubmit();
  ListPage.rvSearch();
  ListPage.rvRowIndex();
  ListPage.rvPostTitle();
  ListPage.rvPostContent();
  ListPage.postList();
  ListPage.loaddata();
  ListPage.iid();
  ListPage["doc'nav'left"]();
  ListPage["doc'main'menu"]();
  ListPage.cls();
  ListPage.addNewBlog();
  Client["v'menu"]();
  Client["v'blog"]();
  Client.rvUsername();
  Client.rvSubmit();
  Client.postList();
  Client.post2();
  Client.post1();
  Client.loaddata();
  Client.iid();
  Client["doc'nav'left"]();
  Client["doc'main'menu"]();
  Client.cls();
  Client.addNewBlog();
  return;
 });
}());
