using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ubora.Web.Infrastructure.TagHelpers
{
    [HtmlTargetElement("input")]
    [HtmlTargetElement("select")]
    public class DisableInputTagHelper : TagHelper
    {
        [HtmlAttributeName("ubora-disable")]
        public bool Disable { set; get; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Disable)
            {
                output.Attributes.Add(new TagHelperAttribute("disabled", "disabled"));
            }
        }
    }

    [HtmlTargetElement("input")]
    [HtmlTargetElement("select")]
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
