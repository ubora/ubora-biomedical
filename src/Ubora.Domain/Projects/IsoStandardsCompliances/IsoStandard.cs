using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Projects.IsoStandardsCompliances
{
    public class IsoStandard
    {
        public IsoStandard(Guid id, string title, Uri link, string shortDescription = "", Guid? addedByUserId = null)
            : this(id, title, shortDescription, link, false, addedByUserId)
        {
        }

        [JsonConstructor]
        private IsoStandard(Guid id, string title, string shortDescription, Uri link, bool isMarkedAsCompliant, Guid? addedByUserId)
        {
            Id = id;
            AddedByUserId = addedByUserId;
            Title = title;
            ShortDescription = shortDescription;
            Link = link;
            IsMarkedAsCompliant = isMarkedAsCompliant;
        }

        public Guid Id { get; }
        public string Title { get; }
        public string ShortDescription { get; }
        public Uri Link { get; }
        public bool IsMarkedAsCompliant { get; }
        public Guid? AddedByUserId { get; }

        public IsoStandard MarkAsCompliant()
        {
            if (IsMarkedAsCompliant)
                throw new InvalidOperationException();

            return new IsoStandard(Id, Title, ShortDescription, Link, true, AddedByUserId);
        }

        public IsoStandard MarkAsNoncompliant()
        {
            if (!IsMarkedAsCompliant)
                throw new InvalidOperationException();

            return new IsoStandard(Id, Title, ShortDescription, Link, false, AddedByUserId);
        }
    }
}