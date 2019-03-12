using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects.IntellectualProperties.Events;

namespace Ubora.Domain.Projects.IntellectualProperties
{
    public interface ILicense 
    {
    }

    public class UboraLicense : ValueObject, ILicense
    {
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return true;
        }
    }

    public class CreativeCommonsLicense : ValueObject, ILicense
    {
        public CreativeCommonsLicense(bool attribution, bool shareAlike, bool nonCommercial, bool noDerivativeWorks)
        {
            Attribution = attribution;
            ShareAlike = shareAlike;
            NonCommercial = nonCommercial;
            NoDerivativeWorks = noDerivativeWorks;
        }

        public bool Attribution { get; }
        public bool ShareAlike { get; }
        public bool NonCommercial { get; }
        public bool NoDerivativeWorks { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Attribution;
            yield return ShareAlike;
            yield return NonCommercial;
            yield return NoDerivativeWorks;
        }
    }

    // License in the name?
    public class IntellectualProperty : Entity<IntellectualProperty>, IProjectEntity
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public ILicense License { get; private set; }

        private void Apply(WorkpackageFiveOpenedEvent e)
        {
            ProjectId = e.ProjectId;
        }

        private void Apply(LicenseTermsChangedEvent e)
        {
            if (e.CreativeCommonsLicense != null) 
            {
                if (e.CreativeCommonsLicense.Equals(License)) 
                    throw new InvalidOperationException("Nothing actually changed.");
                License = e.CreativeCommonsLicense;
                return;
            }

            if (e.UboraLicense != null) 
            {
                if (e.UboraLicense.Equals(License)) 
                    throw new InvalidOperationException("Nothing actually changed.");
                License = e.UboraLicense;
                return;
            }

            if (License == null) 
                throw new InvalidOperationException("Nothing actually changed.");
            License = null;
        }
    }
}