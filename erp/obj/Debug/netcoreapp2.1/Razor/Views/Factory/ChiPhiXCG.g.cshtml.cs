#pragma checksum "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3fdde2b53ffbcb884c5beac38a1ad2e02647dc6e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Factory_ChiPhiXCG), @"mvc.1.0.view", @"/Views/Factory/ChiPhiXCG.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Factory/ChiPhiXCG.cshtml", typeof(AspNetCore.Views_Factory_ChiPhiXCG))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\_ViewImports.cshtml"
using Models;

#line default
#line hidden
#line 1 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
using Common.Utilities;

#line default
#line hidden
#line 2 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
using ViewModels;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3fdde2b53ffbcb884c5beac38a1ad2e02647dc6e", @"/Views/Factory/ChiPhiXCG.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"55f7d09cb6699920b8416cd86872bb94362cdab7", @"/Views/_ViewImports.cshtml")]
    public class Views_Factory_ChiPhiXCG : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ChiPhiXCGViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("hidedatepicker"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control form-control-lg js-select2-basic-single"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "get", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("form-main"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("text/javascript"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(74, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 5 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
  
    ViewData["Title"] = "Chi phí xe cơ giới";
    Layout = "~/Views/Shared/_LayoutData.cshtml";

#line default
#line hidden
            BeginContext(181, 61, true);
            WriteLiteral("\r\n<div class=\"card\">\r\n    <div class=\"card-body\">\r\n        <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 242, "\"", 340, 6);
            WriteAttributeValue("", 249, "/", 249, 1, true);
#line 12 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
WriteAttributeValue("", 250, Constants.LinkFactory.Main, 250, 27, false);

#line default
#line hidden
            WriteAttributeValue("", 277, "/", 277, 1, true);
#line 12 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
WriteAttributeValue("", 278, Constants.LinkFactory.ChiPhiXCG, 278, 32, false);

#line default
#line hidden
            WriteAttributeValue("", 310, "/", 310, 1, true);
#line 12 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
WriteAttributeValue("", 311, Constants.LinkFactory.Create, 311, 29, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(341, 79, true);
            WriteLiteral("><i class=\"icon-add-to-list mr-1\"></i> Nhập số liệu</a>\r\n    </div>\r\n</div>\r\n\r\n");
            EndContext();
            BeginContext(420, 2147, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e07c250a9b0548f98bc4d1194a3f7c25", async() => {
                BeginContext(525, 130, true);
                WriteLiteral("\r\n    <div class=\"form-row mb-3\">\r\n        <div class=\"col-md-3 date-area\">\r\n            <label class=\"control-label\">Từ</label>\r\n");
                EndContext();
#line 20 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
              
                if (Model.from.HasValue)
                {

#line default
#line hidden
                BeginContext(732, 88, true);
                WriteLiteral("                    <input class=\"form-control form-control-lg datepicker datepicker-lg\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 820, "\"", 868, 1);
#line 23 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
WriteAttributeValue("", 828, Model.from.Value.ToString("dd/MM/yyyy"), 828, 40, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(869, 5, true);
                WriteLiteral(" />\r\n");
                EndContext();
#line 24 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                }
                else
                {

#line default
#line hidden
                BeginContext(934, 93, true);
                WriteLiteral("                    <input class=\"form-control form-control-lg datepicker datepicker-lg\" />\r\n");
                EndContext();
#line 28 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                }
            

#line default
#line hidden
                BeginContext(1061, 12, true);
                WriteLiteral("            ");
                EndContext();
                BeginContext(1073, 61, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "20c6161151944fa7be49a8e84bbb1fc2", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 30 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.from);

#line default
#line hidden
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(1134, 114, true);
                WriteLiteral("\r\n        </div>\r\n        <div class=\"col-md-3 date-area\">\r\n            <label class=\"control-label\">Đến</label>\r\n");
                EndContext();
#line 34 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
              
                if (Model.to.HasValue)
                {

#line default
#line hidden
                BeginContext(1323, 88, true);
                WriteLiteral("                    <input class=\"form-control form-control-lg datepicker datepicker-lg\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 1411, "\"", 1457, 1);
#line 37 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
WriteAttributeValue("", 1419, Model.to.Value.ToString("dd/MM/yyyy"), 1419, 38, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1458, 5, true);
                WriteLiteral(" />\r\n");
                EndContext();
#line 38 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                }
                else
                {

#line default
#line hidden
                BeginContext(1523, 93, true);
                WriteLiteral("                    <input class=\"form-control form-control-lg datepicker datepicker-lg\" />\r\n");
                EndContext();
#line 42 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                }
            

#line default
#line hidden
                BeginContext(1650, 12, true);
                WriteLiteral("            ");
                EndContext();
                BeginContext(1662, 59, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "70a2227acfdc46768156652853bb5650", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 44 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.to);

#line default
#line hidden
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(1721, 127, true);
                WriteLiteral("\r\n        </div>\r\n        <div class=\"col-md-6\">\r\n            <label class=\"control-label\">Xe cơ giới/máy</label>\r\n            ");
                EndContext();
                BeginContext(1848, 372, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("select", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fe8eb66dda1d4b3fa3cbf4f5fe9267f5", async() => {
                    BeginContext(1931, 18, true);
                    WriteLiteral("\r\n                ");
                    EndContext();
                    BeginContext(1949, 32, false);
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "612c30d1190a495e9b18cdb2a3d58300", async() => {
                        BeginContext(1966, 6, true);
                        WriteLiteral("Tất cả");
                        EndContext();
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_2.Value;
                    __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
                    await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                    if (!__tagHelperExecutionContext.Output.IsContentModified)
                    {
                        await __tagHelperExecutionContext.SetOutputContentAsync();
                    }
                    Write(__tagHelperExecutionContext.Output);
                    __tagHelperExecutionContext = __tagHelperScopeManager.End();
                    EndContext();
                    BeginContext(1981, 2, true);
                    WriteLiteral("\r\n");
                    EndContext();
#line 50 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                  
                    foreach (var item in Model.Vehicles)
                    {

#line default
#line hidden
                    BeginContext(2084, 24, true);
                    WriteLiteral("                        ");
                    EndContext();
                    BeginContext(2108, 47, false);
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1805bba32b72454cb6f2dda28833884f", async() => {
                        BeginContext(2137, 9, false);
#line 53 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                                               Write(item.Name);

#line default
#line hidden
                        EndContext();
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                    BeginWriteTagHelperAttribute();
#line 53 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                           WriteLiteral(item.Alias);

#line default
#line hidden
                    __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                    __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                    await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                    if (!__tagHelperExecutionContext.Output.IsContentModified)
                    {
                        await __tagHelperExecutionContext.SetOutputContentAsync();
                    }
                    Write(__tagHelperExecutionContext.Output);
                    __tagHelperExecutionContext = __tagHelperScopeManager.End();
                    EndContext();
                    BeginContext(2155, 2, true);
                    WriteLiteral("\r\n");
                    EndContext();
#line 54 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                    }
                

#line default
#line hidden
                    BeginContext(2199, 12, true);
                    WriteLiteral("            ");
                    EndContext();
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.SelectTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper);
#line 48 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.xcg);

#line default
#line hidden
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_SelectTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(2220, 340, true);
                WriteLiteral(@"
        </div>
    </div>
    <div class=""form-row mb-3"">
        <div class=""col-md-12"">
            <label class=""control-label""><small>Bấm nút hoặc Enter</small></label>
            <button class=""btn btn-lg btn-info form-control"" type=""submit""><i class=""icon-magnifying-glass""></i> Tìm kiếm</button>
        </div>
    </div>
");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "action", 4, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 447, "/", 447, 1, true);
#line 16 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
AddHtmlAttributeValue("", 448, Constants.LinkFactory.Main, 448, 27, false);

#line default
#line hidden
            AddHtmlAttributeValue("", 475, "/", 475, 1, true);
#line 16 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
AddHtmlAttributeValue("", 476, Constants.LinkFactory.ChiPhiXCG, 476, 32, false);

#line default
#line hidden
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2567, 42, true);
            WriteLiteral("\r\n\r\n<small><span class=\"badge badge-info\">");
            EndContext();
            BeginContext(2610, 13, false);
#line 67 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                                 Write(Model.Records);

#line default
#line hidden
            EndContext();
            BeginContext(2623, 527, true);
            WriteLiteral(@"</span> được tìm thấy.</small>
<div class=""table-responsive"">
    <table class=""table table-sm table-bordered table-striped table-hover"">
        <thead>
            <tr>
                <th></th>
                <th>Tháng</th>
                <th>
                    Xe cơ giới/Máy
                </th>
                <th>
                    Chi phí/tháng
                </th>
                <th>
                    Chi phí/1h
                </th>
            </tr>
        </thead>
        <tbody>
");
            EndContext();
#line 86 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
             foreach (var item in Model.List)
            {
                if (!string.IsNullOrEmpty(item.ChungLoaiXe))
                {

#line default
#line hidden
            BeginContext(3293, 119, true);
            WriteLiteral("                    <tr>\r\n                        <td></td>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(3413, 10, false);
#line 93 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                       Write(item.Month);

#line default
#line hidden
            EndContext();
            BeginContext(3423, 1, true);
            WriteLiteral("/");
            EndContext();
            BeginContext(3425, 9, false);
#line 93 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                                   Write(item.Year);

#line default
#line hidden
            EndContext();
            BeginContext(3434, 91, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(3526, 16, false);
#line 96 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                       Write(item.ChungLoaiXe);

#line default
#line hidden
            EndContext();
            BeginContext(3542, 91, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(3634, 48, false);
#line 99 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                       Write(String.Format("{0:#,###,###}", item.ChiPhiThang));

#line default
#line hidden
            EndContext();
            BeginContext(3682, 91, true);
            WriteLiteral("\r\n                        </td>\r\n                        <td>\r\n                            ");
            EndContext();
            BeginContext(3774, 45, false);
#line 102 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                       Write(String.Format("{0:#,###,###}", item.ChiPhi1H));

#line default
#line hidden
            EndContext();
            BeginContext(3819, 60, true);
            WriteLiteral("\r\n                        </td>\r\n                    </tr>\r\n");
            EndContext();
#line 105 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
                }
            }

#line default
#line hidden
            BeginContext(3913, 122, true);
            WriteLiteral("        </tbody>\r\n    </table>\r\n</div>\r\n\r\n<small>\r\n    Có VAT 10%. Lương tài xế 10,000,000 xe nhà, 8h/1 ngày\r\n</small>\r\n\r\n");
            EndContext();
            DefineSection("scripts", async() => {
                BeginContext(4053, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
                BeginContext(4059, 90, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "06a5d090b4cb42f887ac3604ab529c5c", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 4095, "~/js/factory-chi-phi.js?", 4095, 24, true);
#line 116 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Factory\ChiPhiXCG.cshtml"
AddHtmlAttributeValue("", 4119, DateTime.Now.Ticks, 4119, 19, false);

#line default
#line hidden
                EndAddHtmlAttributeValues(__tagHelperExecutionContext);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(4149, 2, true);
                WriteLiteral("\r\n");
                EndContext();
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ChiPhiXCGViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
