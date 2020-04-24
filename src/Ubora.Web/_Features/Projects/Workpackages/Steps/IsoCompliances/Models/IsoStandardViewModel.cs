using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances.Models
{
    public class IsoStandardViewModel
    {
        public IsoStandardViewModel(Guid id, string title, string shortDescription, Uri link, bool isMarkedAsCompliant)
        { 
            IsoStandardId = id;
            Title = title;
            ShortDescription = shortDescription;
            Link = link;
            IsMarkedAsCompliant = isMarkedAsCompliant;
        }
        
        public Guid IsoStandardId { get; }
        public string Title { get; }
        public string ShortDescription { get; }
        public Uri Link { get; }
        public bool IsMarkedAsCompliant { get; } 

        public class Projection : Projection<IsoStandard, IsoStandardViewModel>
        {
            protected override Expression<Func<IsoStandard, IsoStandardViewModel>> ToSelector()
            {
                return document => new IsoStandardViewModel(document.Id, document.Title, document.ShortDescription, document.Link, document.IsMarkedAsCompliant);
            }
        }
    }
}
