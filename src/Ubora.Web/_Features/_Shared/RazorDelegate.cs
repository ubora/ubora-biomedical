using Microsoft.AspNetCore.Html;

// https://haacked.com/archive/2011/02/27/templated-razor-delegates.aspx
// http://vibrantcode.com/blog/2010/8/2/inside-razor-part-3-templates.html
public delegate IHtmlContent RazorDelegate(dynamic item = null);