#pragma checksum "C:\work\作業\DeborahCall\Views\Top\Menu.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4b5fbbcaa272ef3ed61d1ae82b39bea5190f030f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Top_Menu), @"mvc.1.0.view", @"/Views/Top/Menu.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Top/Menu.cshtml", typeof(AspNetCore.Views_Top_Menu))]
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
#line 1 "C:\work\作業\DeborahCall\Views\_ViewImports.cshtml"
using DeborahCall;

#line default
#line hidden
#line 2 "C:\work\作業\DeborahCall\Views\_ViewImports.cshtml"
using DeborahCall.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4b5fbbcaa272ef3ed61d1ae82b39bea5190f030f", @"/Views/Top/Menu.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2292f7435552424ea705ca07fad0a896443faf0f", @"/Views/_ViewImports.cshtml")]
    public class Views_Top_Menu : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Deborah.Models.Tra_Entry>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(45, 634, true);
            WriteLiteral(@"<p>未登録お知らせが○○件あります</p>

<table>
    <tr>
        <th>会社名</th>
        <th>担当者名</th>
        <th>電話番号</th>
        <th>着信時間</th>
        <th>終了時間</th>
        <th>選択</th>
    </tr>
    <tr>
        <td>テスト会社</td>
        <td>テスト</td>
        <td>000-0000-0000</td>
        <td>10/1 10:00</td>
        <td>10/1 10:05</td>
        <td><input type=""button"" value=""SELECT"" class=""select_btn""></td>
    </tr>
    <tr>
        <td>テスト会社</td>
        <td>テスト</td>
        <td>000-0000-0000</td>
        <td>10/1 10:00</td>
        <td>10/1 10:05</td>
        <td><input type=""button"" value=""SELECT"" class=""select_btn""></td>
    </tr>
</table>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Deborah.Models.Tra_Entry>> Html { get; private set; }
    }
}
#pragma warning restore 1591
