using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ubora.Web.Infrastructure.TagHelpers
{
    [HtmlTargetElement("input")]
    [HtmlTargetElement("select")]
    [HtmlTargetElement("button")]
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
}
