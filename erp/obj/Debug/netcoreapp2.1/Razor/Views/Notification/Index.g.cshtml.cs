#pragma checksum "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Notification\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "291ae3f46de9992783fd52fd4532bb38bf7f28d3"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"291ae3f46de9992783fd52fd4532bb38bf7f28d3", @"/Views/Notification/Index.cshtml")]
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
  
    ViewData["Title"] = "Thông báo";


#line default
#line hidden
            BeginContext(148, 1055, true);
            WriteLiteral(@"
<div class=""main-container"">
    <nav aria-label=""breadcrumb"" role=""navigation"" class=""bg-primary text-white"">
        <div class=""container"">
            <div class=""row justify-content-center"">
                <div class=""col"">
                    <ol class=""breadcrumb"">
                        <li class=""breadcrumb-item"">
                            <a href=""/"">Trang chủ</a>
                        </li>
                        <li class=""breadcrumb-item active"" aria-current=""page"">
                            Thông báo
                        </li>
                    </ol>
                </div>
            </div>
        </div>
    </nav>

    <section class=""space-sm"">
        <div class=""container"">
            <div class=""row"">
                <div class=""col-12"">
                    <i class=""far fa-clock""></i>
                    <span>
                        Đang cập nhật thông tin.
                    </span>
                </div>
            </div>
        </div>");
            WriteLiteral("\n    </section>\r\n\r\n\r\n</div>\r\n\r\n");
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
