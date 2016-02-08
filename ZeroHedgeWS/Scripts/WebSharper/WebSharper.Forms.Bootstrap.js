(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,UI,Next,Doc,List,T,Forms,Bootstrap,Controls,AttrModule,Doc1,Seq,View,View1;
 Runtime.Define(Global,{
  WebSharper:{
   Forms:{
    Bootstrap:{
     Controls:{
      Button:Runtime.Field(function()
      {
       return function(caption)
       {
        return function(_arg10_)
        {
         return function(_arg20_)
         {
          return Doc.Button(caption,_arg10_,_arg20_);
         };
        };
       };
      }),
      Checkbox:function(lbl,extras,target,labelExtras,targetExtras)
      {
       return Doc.Element("div",Runtime.New(T,{
        $:1,
        $0:(Controls.cls())("checkbox"),
        $1:extras
       }),List.ofArray([Doc.Element("label",labelExtras,List.ofArray([Doc.CheckBox(targetExtras,target),Doc.TextNode(lbl)]))]));
      },
      Class:Runtime.Field(function()
      {
       return function(name)
       {
        return AttrModule.Class(name);
       };
      }),
      Input:function(lbl,extras,target,labelExtras,targetExtras)
      {
       return Doc.Element("div",Runtime.New(T,{
        $:1,
        $0:(Controls.cls())("form-group"),
        $1:extras
       }),List.ofArray([Doc.Element("label",labelExtras,List.ofArray([Doc.TextNode(lbl)])),Doc.Input(Runtime.New(T,{
        $:1,
        $0:(Controls.cls())("form-control"),
        $1:targetExtras
       }),target)]));
      },
      InputPassword:function(lbl,extras,target,labelExtras,targetExtras)
      {
       return Doc.Element("div",Runtime.New(T,{
        $:1,
        $0:(Controls.cls())("form-group"),
        $1:extras
       }),List.ofArray([Doc.Element("label",labelExtras,List.ofArray([Doc.TextNode(lbl)])),Doc.PasswordBox(Runtime.New(T,{
        $:1,
        $0:(Controls.cls())("form-control"),
        $1:targetExtras
       }),target)]));
      },
      InputPasswordWithError:function(lbl)
      {
       var inputFun;
       inputFun=function(_arg00_)
       {
        return function(_arg10_)
        {
         return Doc.PasswordBox(_arg00_,_arg10_);
        };
       };
       return function(extras)
       {
        return function(tupledArg)
        {
         var target,labelExtras,targetExtras;
         target=tupledArg[0];
         labelExtras=tupledArg[1];
         targetExtras=tupledArg[2];
         return function(submitView)
         {
          return Controls.__InputWithError(inputFun,lbl,extras,target,labelExtras,targetExtras,submitView);
         };
        };
       };
      },
      InputWithError:function(lbl)
      {
       var inputFun;
       inputFun=function(_arg00_)
       {
        return function(_arg10_)
        {
         return Doc.Input(_arg00_,_arg10_);
        };
       };
       return function(extras)
       {
        return function(tupledArg)
        {
         var target,labelExtras,targetExtras;
         target=tupledArg[0];
         labelExtras=tupledArg[1];
         targetExtras=tupledArg[2];
         return function(submitView)
         {
          return Controls.__InputWithError(inputFun,lbl,extras,target,labelExtras,targetExtras,submitView);
         };
        };
       };
      },
      Radio:function(lbl,extras,target,labelExtras,targetExtras)
      {
       return Doc.Element("div",Runtime.New(T,{
        $:1,
        $0:(Controls.cls())("radio"),
        $1:extras
       }),List.ofArray([Doc.Element("label",labelExtras,List.ofArray([Doc.Radio(targetExtras,true,target),Doc.TextNode(lbl)]))]));
      },
      ShowErrors:function(extras,submit)
      {
       return Doc1.ShowErrors(submit,function(_arg1)
       {
        var _,mapping,source,ats;
        if(_arg1.$==0)
         {
          _=Doc.get_Empty();
         }
        else
         {
          mapping=function(m)
          {
           var arg20;
           arg20=List.ofArray([Doc.TextNode(m.get_Text())]);
           return Doc.Element("p",[],arg20);
          };
          source=Seq.map(mapping,_arg1);
          ats=List.ofArray([(Controls.cls())("alert alert-danger")]);
          _=Doc.Element("div",extras,List.ofArray([Doc.Element("div",ats,source)]));
         }
        return _;
       });
      },
      Simple:{
       Button:function(label,f)
       {
        return(((Controls.Button())(label))(List.ofArray([(Controls.cls())("button btn-primary")])))(f);
       },
       Checkbox:function(lbl,target)
       {
        return Controls.Checkbox(lbl,Runtime.New(T,{
         $:0
        }),target,Runtime.New(T,{
         $:0
        }),Runtime.New(T,{
         $:0
        }));
       },
       Input:function(lbl,target)
       {
        return Controls.Input(lbl,Runtime.New(T,{
         $:0
        }),target,Runtime.New(T,{
         $:0
        }),Runtime.New(T,{
         $:0
        }));
       },
       InputPassword:function(lbl,target)
       {
        return Controls.InputPassword(lbl,Runtime.New(T,{
         $:0
        }),target,Runtime.New(T,{
         $:0
        }),Runtime.New(T,{
         $:0
        }));
       },
       InputPasswordWithError:function(lbl,target,submit)
       {
        var clo1,arg10,clo2,tupledArg,arg20,arg21,arg22;
        clo1=Controls.InputPasswordWithError(lbl);
        arg10=Runtime.New(T,{
         $:0
        });
        clo2=clo1(arg10);
        tupledArg=[target,Runtime.New(T,{
         $:0
        }),Runtime.New(T,{
         $:0
        })];
        arg20=tupledArg[0];
        arg21=tupledArg[1];
        arg22=tupledArg[2];
        return(clo2([arg20,arg21,arg22]))(submit);
       },
       InputWithError:function(lbl,target,submit)
       {
        var clo1,arg10,clo2,tupledArg,arg20,arg21,arg22;
        clo1=Controls.InputWithError(lbl);
        arg10=Runtime.New(T,{
         $:0
        });
        clo2=clo1(arg10);
        tupledArg=[target,Runtime.New(T,{
         $:0
        }),Runtime.New(T,{
         $:0
        })];
        arg20=tupledArg[0];
        arg21=tupledArg[1];
        arg22=tupledArg[2];
        return(clo2([arg20,arg21,arg22]))(submit);
       },
       ShowErrors:function(submit)
       {
        return Controls.ShowErrors(Runtime.New(T,{
         $:0
        }),submit);
       },
       TextArea:function(lbl,target)
       {
        return Controls.TextArea(lbl,Runtime.New(T,{
         $:0
        }),target,Runtime.New(T,{
         $:0
        }),Runtime.New(T,{
         $:0
        }));
       },
       TextAreaWithError:function(lbl,target,submit)
       {
        var clo1,arg10,clo2,tupledArg,arg20,arg21,arg22;
        clo1=Controls.TextAreaWithError(lbl);
        arg10=Runtime.New(T,{
         $:0
        });
        clo2=clo1(arg10);
        tupledArg=[target,Runtime.New(T,{
         $:0
        }),Runtime.New(T,{
         $:0
        })];
        arg20=tupledArg[0];
        arg21=tupledArg[1];
        arg22=tupledArg[2];
        return(clo2([arg20,arg21,arg22]))(submit);
       }
      },
      TextArea:function(lbl,extras,target,labelExtras,targetExtras)
      {
       return Doc.Element("div",Runtime.New(T,{
        $:1,
        $0:(Controls.cls())("form-group"),
        $1:extras
       }),List.ofArray([Doc.Element("label",labelExtras,List.ofArray([Doc.TextNode(lbl)])),Doc.InputArea(Runtime.New(T,{
        $:1,
        $0:(Controls.cls())("form-control"),
        $1:targetExtras
       }),target)]));
      },
      TextAreaWithError:function(lbl)
      {
       var inputFun;
       inputFun=function(_arg00_)
       {
        return function(_arg10_)
        {
         return Doc.InputArea(_arg00_,_arg10_);
        };
       };
       return function(extras)
       {
        return function(tupledArg)
        {
         var target,labelExtras,targetExtras;
         target=tupledArg[0];
         labelExtras=tupledArg[1];
         targetExtras=tupledArg[2];
         return function(submitView)
         {
          return Controls.__InputWithError(inputFun,lbl,extras,target,labelExtras,targetExtras,submitView);
         };
        };
       };
      },
      __InputWithError:function(inputFun,lbl,extras,target,labelExtras,targetExtras,submitView)
      {
       var tv,view,patternInput,errorOpt,errorClassOpt,ats;
       tv=View.Through(submitView,target);
       view=View1.Map(function(res)
       {
        var _,_1,errs,mapping,reduction,list,errors;
        if(res.$==1)
         {
          if(res.$0.$==0)
           {
            _1=[{
             $:0
            },{
             $:0
            }];
           }
          else
           {
            errs=res.$0;
            mapping=function(e)
            {
             return e.get_Text();
            };
            reduction=function(a)
            {
             return function(b)
             {
              return a+"; "+b;
             };
            };
            list=List.map(mapping,errs);
            errors=Seq.reduce(reduction,list);
            _1=[{
             $:1,
             $0:errors
            },{
             $:1,
             $0:"has-error"
            }];
           }
          _=_1;
         }
        else
         {
          _=[{
           $:0
          },{
           $:0
          }];
         }
        return _;
       },tv);
       patternInput=[View1.Map(function(tuple)
       {
        return tuple[0];
       },view),View1.Map(function(tuple)
       {
        return tuple[1];
       },view)];
       errorOpt=patternInput[0];
       errorClassOpt=patternInput[1];
       ats=Seq.toList(Seq.delay(function()
       {
        return Seq.append([(Controls.cls())("form-group")],Seq.delay(function()
        {
         return Seq.append([AttrModule.DynamicClass("has-error",errorClassOpt,function(opt)
         {
          return opt.$==1;
         })],Seq.delay(function()
         {
          return extras;
         }));
        }));
       }));
       return Doc.Element("div",ats,Seq.toList(Seq.delay(function()
       {
        return Seq.append([Doc.Element("label",labelExtras,List.ofArray([Doc.TextNode(lbl)]))],Seq.delay(function()
        {
         return Seq.append([(inputFun(Runtime.New(T,{
          $:1,
          $0:(Controls.cls())("form-control"),
          $1:targetExtras
         })))(target)],Seq.delay(function()
         {
          return[Doc.BindView(function(_arg1)
          {
           var _,error;
           if(_arg1.$==1)
            {
             error=_arg1.$0;
             _=Doc.Element("span",List.ofArray([(Controls.cls())("help-block")]),List.ofArray([Doc.TextNode(error)]));
            }
           else
            {
             _=Doc.get_Empty();
            }
           return _;
          },errorOpt)];
         }));
        }));
       })));
      },
      cls:Runtime.Field(function()
      {
       return function(name)
       {
        return AttrModule.Class(name);
       };
      })
     }
    }
   }
  }
 });
 Runtime.OnInit(function()
 {
  UI=Runtime.Safe(Global.WebSharper.UI);
  Next=Runtime.Safe(UI.Next);
  Doc=Runtime.Safe(Next.Doc);
  List=Runtime.Safe(Global.WebSharper.List);
  T=Runtime.Safe(List.T);
  Forms=Runtime.Safe(Global.WebSharper.Forms);
  Bootstrap=Runtime.Safe(Forms.Bootstrap);
  Controls=Runtime.Safe(Bootstrap.Controls);
  AttrModule=Runtime.Safe(Next.AttrModule);
  Doc1=Runtime.Safe(Forms.Doc);
  Seq=Runtime.Safe(Global.WebSharper.Seq);
  View=Runtime.Safe(Forms.View);
  return View1=Runtime.Safe(Next.View);
 });
 Runtime.OnLoad(function()
 {
  Controls.cls();
  Controls.Class();
  Controls.Button();
  return;
 });
}());
