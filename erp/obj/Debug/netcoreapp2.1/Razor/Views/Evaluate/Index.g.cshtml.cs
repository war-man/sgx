#pragma checksum "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Evaluate\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "444906c72e314d044b70c935c1bada9f659c9a6c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Evaluate_Index), @"mvc.1.0.view", @"/Views/Evaluate/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Evaluate/Index.cshtml", typeof(AspNetCore.Views_Evaluate_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"444906c72e314d044b70c935c1bada9f659c9a6c", @"/Views/Evaluate/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"55f7d09cb6699920b8416cd86872bb94362cdab7", @"/Views/_ViewImports.cshtml")]
    public class Views_Evaluate_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Evaluate\Index.cshtml"
  
    ViewData["Title"] = "Đánh giá";
    Layout = "~/Views/Shared/_LayoutSocialFb.cshtml";

#line default
#line hidden
            BeginContext(101, 435, true);
            WriteLiteral(@"
<nav aria-label=""breadcrumb"" role=""navigation"" class=""bg-primary text-white"">
    <div class=""container"">
        <div class=""row justify-content-center"">
            <div class=""col"">
                <ol class=""breadcrumb"">
                    <li class=""breadcrumb-item"">
                        <a href=""/"">Trang chủ</a>
                    </li>
                    <li class=""breadcrumb-item active"" aria-current=""page"">");
            EndContext();
            BeginContext(537, 17, false);
#line 15 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Evaluate\Index.cshtml"
                                                                      Write(ViewData["Title"]);

#line default
#line hidden
            EndContext();
            BeginContext(554, 175, true);
            WriteLiteral("</li>\r\n                </ol>\r\n            </div>\r\n            <!--end of col-->\r\n        </div>\r\n        <!--end of row-->\r\n    </div>\r\n    <!--end of container-->\r\n</nav>\r\n\r\n");
            EndContext();
#line 25 "D:\Projects\Sgx\sourcecode\sgx\erp\Views\Evaluate\Index.cshtml"
  
    await Html.RenderPartialAsync("Empty");

#line default
#line hidden
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
