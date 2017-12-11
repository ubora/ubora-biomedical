using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ubora.Domain.Projects.Workpackages.Queries;

namespace Ubora.Web._Features.Projects.Workpackages.SideMenu
{
    public interface ISideMenuItem
    {
        string Id { get; }
        string DisplayName { get; }
        ImgIcon Icon { get; }
        NestingLevel Nesting { get; set; }
        string ATagClass { get;  }

        /// <remarks>Return object itself for fluent-API.</remarks>>
        ISideMenuItem SetStatus(WorkpackageStatus status);
        WorkpackageStatus Status { get; set; }

        bool IsSelected { get; }

        IHtmlContent GenerateHtmlMarkup(IHtmlHelper htmlHelper);
    }
}
