#pragma checksum "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "327ac5d4071b8d447133f713b4aefd821dfeb372"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_IndexDb), @"mvc.1.0.view", @"/Views/Home/IndexDb.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/IndexDb.cshtml", typeof(AspNetCore.Views_Home_IndexDb))]
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
#line 1 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\_ViewImports.cshtml"
using MssqlDatabaseAdminPanel;

#line default
#line hidden
#line 2 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\_ViewImports.cshtml"
using MssqlDatabaseAdminPanel.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"327ac5d4071b8d447133f713b4aefd821dfeb372", @"/Views/Home/IndexDb.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b8a6085341b9e44193f020a1813297051f600f18", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_IndexDb : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 8, true);
            WriteLiteral("<h2>Top ");
            EndContext();
            BeginContext(9, 29, false);
#line 1 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
   Write(ViewBag.RequiredIndexes.Count);

#line default
#line hidden
            EndContext();
            BeginContext(38, 185, true);
            WriteLiteral(" most required indexes</h2>\r\n<table class=\"table table-dark\">\r\n\t<tr>\r\n\t\t<th>Table</th>\r\n\t\t<th>Equality columns</th>\r\n\t\t<th>Inequality columns</th>\r\n\t\t<th>Included columns</th>\r\n\t</tr>\r\n");
            EndContext();
#line 9 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
     foreach (DbIndex index in ViewBag.RequiredIndexes) {

#line default
#line hidden
            BeginContext(279, 15, true);
            WriteLiteral("\t\t<tr>\r\n\t\t\t<td>");
            EndContext();
            BeginContext(295, 15, false);
#line 11 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
           Write(index.TableName);

#line default
#line hidden
            EndContext();
            BeginContext(310, 14, true);
            WriteLiteral("</td>\r\n\t\t\t<td>");
            EndContext();
            BeginContext(325, 21, false);
#line 12 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
           Write(index.EqualityColumns);

#line default
#line hidden
            EndContext();
            BeginContext(346, 14, true);
            WriteLiteral("</td>\r\n\t\t\t<td>");
            EndContext();
            BeginContext(361, 23, false);
#line 13 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
           Write(index.InequalityColumns);

#line default
#line hidden
            EndContext();
            BeginContext(384, 14, true);
            WriteLiteral("</td>\r\n\t\t\t<td>");
            EndContext();
            BeginContext(399, 21, false);
#line 14 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
           Write(index.IncludedColumns);

#line default
#line hidden
            EndContext();
            BeginContext(420, 16, true);
            WriteLiteral("</td>\r\n\t\t</tr>\r\n");
            EndContext();
#line 16 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
	}

#line default
#line hidden
            BeginContext(440, 18, true);
            WriteLiteral("</table>\r\n<h2>Top ");
            EndContext();
            BeginContext(459, 27, false);
#line 18 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
   Write(ViewBag.UnusedIndexes.Count);

#line default
#line hidden
            EndContext();
            BeginContext(486, 178, true);
            WriteLiteral(" unused indexes</h2>\r\n<table class=\"table table-dark\">\r\n\t<tr>\r\n\t\t<th>Table</th>\r\n\t\t<th>Equality columns</th>\r\n\t\t<th>Inequality columns</th>\r\n\t\t<th>Included columns</th>\r\n\t</tr>\r\n");
            EndContext();
#line 26 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
     foreach (DbIndex index in ViewBag.UnusedIndexes) {

#line default
#line hidden
            BeginContext(718, 15, true);
            WriteLiteral("\t\t<tr>\r\n\t\t\t<td>");
            EndContext();
            BeginContext(734, 15, false);
#line 28 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
           Write(index.TableName);

#line default
#line hidden
            EndContext();
            BeginContext(749, 14, true);
            WriteLiteral("</td>\r\n\t\t\t<td>");
            EndContext();
            BeginContext(764, 21, false);
#line 29 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
           Write(index.EqualityColumns);

#line default
#line hidden
            EndContext();
            BeginContext(785, 14, true);
            WriteLiteral("</td>\r\n\t\t\t<td>");
            EndContext();
            BeginContext(800, 23, false);
#line 30 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
           Write(index.InequalityColumns);

#line default
#line hidden
            EndContext();
            BeginContext(823, 14, true);
            WriteLiteral("</td>\r\n\t\t\t<td>");
            EndContext();
            BeginContext(838, 21, false);
#line 31 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
           Write(index.IncludedColumns);

#line default
#line hidden
            EndContext();
            BeginContext(859, 16, true);
            WriteLiteral("</td>\r\n\t\t</tr>\r\n");
            EndContext();
#line 33 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
	}

#line default
#line hidden
            BeginContext(879, 18, true);
            WriteLiteral("</table>\r\n<h2>Top ");
            EndContext();
            BeginContext(898, 34, false);
#line 35 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
   Write(ViewBag.MostExpensiveIndexes.Count);

#line default
#line hidden
            EndContext();
            BeginContext(932, 123, true);
            WriteLiteral(" indexes that use most resources</h2>\r\n<table class=\"table table-dark\">\r\n\t<tr>\r\n\t\t<th>Table</th>\r\n\t\t<th>Name</th>\r\n\t</tr>\r\n");
            EndContext();
#line 41 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
     foreach (DbIndex index in ViewBag.UnusedIndexes) {

#line default
#line hidden
            BeginContext(1109, 15, true);
            WriteLiteral("\t\t<tr>\r\n\t\t\t<td>");
            EndContext();
            BeginContext(1125, 15, false);
#line 43 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
           Write(index.TableName);

#line default
#line hidden
            EndContext();
            BeginContext(1140, 14, true);
            WriteLiteral("</td>\r\n\t\t\t<td>");
            EndContext();
            BeginContext(1155, 10, false);
#line 44 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
           Write(index.Name);

#line default
#line hidden
            EndContext();
            BeginContext(1165, 16, true);
            WriteLiteral("</td>\r\n\t\t</tr>\r\n");
            EndContext();
#line 46 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
	}

#line default
#line hidden
            BeginContext(1185, 54, true);
            WriteLiteral("</table>\r\n<h2>Rebuild / defragment index script</h2>\r\n");
            EndContext();
            BeginContext(1240, 137, false);
#line 49 "C:\Users\Lenovo\source\repos\MssqlDatabaseAdminPanel\MssqlDatabaseAdminPanel\Views\Home\IndexDb.cshtml"
Write(Html.TextArea("resultAlterIndex", ViewBag.AlterIndexText as string, new { @readonly = "readonly", @style = "width:700px;height:250px;" }));

#line default
#line hidden
            EndContext();
            BeginContext(1377, 2, true);
            WriteLiteral("\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
