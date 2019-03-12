using System;
using Newtonsoft.Json;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.IntellectualProperties.Events
{
    public class LicenseTermsChangedEvent : ProjectEvent
    {
        public static LicenseTermsChangedEvent ForCreativeCommons(UserInfo initiatedBy, Guid projectId, Guid aggregateId, CreativeCommonsLicense creativeCommonsLicense) 
        {
            return new LicenseTermsChangedEvent(initiatedBy, projectId, aggregateId, uboraLicense: null, creativeCommonsLicense: creativeCommonsLicense ?? throw new ArgumentNullException());
        }

        public static LicenseTermsChangedEvent ForUbora(UserInfo initiatedBy, Guid projectId, Guid aggregateId) 
        {
            return new LicenseTermsChangedEvent(initiatedBy, projectId, aggregateId, uboraLicense: new UboraLicense(), creativeCommonsLicense: null);
        }

        public static LicenseTermsChangedEvent ForRemoval(UserInfo initiatedBy, Guid projectId, Guid aggregateId) 
        {
            return new LicenseTermsChangedEvent(initiatedBy, projectId, aggregateId, uboraLicense: null, creativeCommonsLicense: null);
        }

        [JsonConstructor]
        protected LicenseTermsChangedEvent(UserInfo initiatedBy, Guid projectId, Guid aggregateId, UboraLicense uboraLicense, CreativeCommonsLicense creativeCommonsLicense) 
            : base(initiatedBy, projectId)
        {
            AggregateId = aggregateId;
            UboraLicense = uboraLicense;
            CreativeCommonsLicense = creativeCommonsLicense;
        }

        public Guid AggregateId { get; }
        public CreativeCommonsLicense CreativeCommonsLicense { get; }
        public UboraLicense UboraLicense { get; }

        public override string GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}