using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.IsoStandardsCompliances;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances.Models
{
    public class IndexViewModel
    {
        public IReadOnlyCollection<IsoStandardIndexVieModel> IsoStandards { get; set; }

        public class Factory
        {
            public IndexViewModel Create(IsoStandardsComplianceAggregate aggregate = null)
            {
                if (aggregate == null)
                {
                    return new IndexViewModel
                    {
                        IsoStandards = new List<IsoStandardIndexVieModel>()
                    };
                }

                return new IndexViewModel
                {
                    IsoStandards = new IsoStandardIndexVieModel.Projection().Apply(aggregate.IsoStandards).ToList()
                };
            }
        }
    }

    public class IsoStandardIndexVieModel
    {
        public IsoStandardIndexVieModel(Guid id, string title, string shortDescription, Uri link, bool isMarkedAsCompliant)
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

        public class Projection : Projection<IsoStandard, IsoStandardIndexVieModel>
        {
            protected override Expression<Func<IsoStandard, IsoStandardIndexVieModel>> ToSelector()
            {
                return document => new IsoStandardIndexVieModel(document.Id, document.Title, document.ShortDescription, document.Link, document.IsMarkedAsCompliant);
            }
        }
    }
}
