#pragma checksum "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Notification\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "785014d79cb88c0c1afa804dcfe53f3ce6f26794"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Notification_Index), @"mvc.1.0.view", @"/Views/Notification/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Notification/Index.cshtml", typeof(AspNetCore.Views_Notification_Index))]
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
#line 1 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Notification\Index.cshtml"
using Common.Utilities;

#line default
#line hidden
#line 2 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Notification\Index.cshtml"
using Common.Enums;

#line default
#line hidden
#line 3 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Notification\Index.cshtml"
using ViewModels;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"785014d79cb88c0c1afa804dcfe53f3ce6f26794", @"/Views/Notification/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"55f7d09cb6699920b8416cd86872bb94362cdab7", @"/Views/_ViewImports.cshtml")]
    public class Views_Notification_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<NotificationViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(99, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 6 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Notification\Index.cshtml"
  
    ViewData["Title"] = Model.Notification.Title;
    Layout = "~/Views/Shared/_LayoutNotification.cshtml";

#line default
#line hidden
            BeginContext(218, 234, true);
            WriteLiteral("\r\n<nav aria-label=\"breadcrumb\">\r\n    <div class=\"container\">\r\n        <ol class=\"breadcrumb\">\r\n            <li class=\"breadcrumb-item\"><a href=\"/\">Trang chủ</a></li>\r\n            <li class=\"breadcrumb-item active\" aria-current=\"page\">");
            EndContext();
            BeginContext(453, 24, false);
#line 15 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Notification\Index.cshtml"
                                                              Write(Model.Notification.Title);

#line default
#line hidden
            EndContext();
            BeginContext(477, 134, true);
            WriteLiteral("</li>\r\n        </ol>\r\n    </div>\r\n</nav>\r\n\r\n<div class=\"container\">\r\n    <div class=\"row\">\r\n        <div class=\"col-12\">\r\n            ");
            EndContext();
            BeginContext(612, 36, false);
#line 23 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Notification\Index.cshtml"
       Write(Html.Raw(Model.Notification.Content));

#line default
#line hidden
            EndContext();
            BeginContext(648, 30, true);
            WriteLiteral("\r\n        </div>\r\n    </div>\r\n");
            EndContext();
#line 26 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Notification\Index.cshtml"
     if (Model.Notification.Documents != null && Model.Notification.Documents.Count > 0)
    {
        foreach (var item in Model.Notification.Documents)
        {

#line default
#line hidden
            BeginContext(846, 101, true);
            WriteLiteral("            <div class=\"row mb-3\">\r\n                <div class=\"col-12\">\r\n                    <iframe");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 947, "\"", 978, 3);
#line 32 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Notification\Index.cshtml"
WriteAttributeValue("", 953, item.Path, 953, 10, false);

#line default
#line hidden
            WriteAttributeValue("", 963, "/", 963, 1, true);
#line 32 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Notification\Index.cshtml"
WriteAttributeValue("", 964, item.FileName, 964, 14, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(979, 106, true);
            WriteLiteral(" style=\"width:100%; height:900px;\" frameborder=\"0\"></iframe>\r\n                </div>\r\n            </div>\r\n");
            EndContext();
#line 35 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Notification\Index.cshtml"
        }
    }

#line default
#line hidden
            BeginContext(1103, 10, true);
            WriteLiteral("</div>\r\n\r\n");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<NotificationViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
