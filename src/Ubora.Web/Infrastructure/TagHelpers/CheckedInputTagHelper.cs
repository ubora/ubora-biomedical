using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ubora.Web.Infrastructure.TagHelpers
{
    [HtmlTargetElement("input")]
    public class CheckedInputTagHelper : TagHelper
    {
        [HtmlAttributeName("ubora-checked")]
        public bool Checked { set; get; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Checked)
            {
                output.Attributes.Add(new TagHelperAttribute("checked", "checked"));
            }
        }
    }
}