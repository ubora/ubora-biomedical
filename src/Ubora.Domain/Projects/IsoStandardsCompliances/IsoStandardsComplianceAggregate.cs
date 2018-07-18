using System;
using System.Collections.Immutable;
using System.Linq;
using Ubora.Domain.Projects.IsoStandardsCompliances.Events;

namespace Ubora.Domain.Projects.IsoStandardsCompliances
{
    /// <summary>
    /// WP4 step
    /// </summary>
    public class IsoStandardsComplianceAggregate : IProjectEntity
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public ImmutableList<IsoStandard> IsoStandards { get; private set; } = ImmutableList<IsoStandard>.Empty;

        private void Apply(IsoStandardAddedToChecklistEvent @event)
        {
            if (@event.AggregateId != Id)
                throw new InvalidOperationException();

            ProjectId = @event.ProjectId;
            var value = new IsoStandard(@event.Title, @event.ShortDescription, @event.Link, @event.InitiatedBy.UserId);
            IsoStandards = IsoStandards.Add(value);
        }

        private void Apply(IsoStandardRemovedFromChecklistEvent @event)
        {
            if (@event.AggregateId != Id)
                throw new InvalidOperationException();

            if (@event.ProjectId != ProjectId)
                throw new InvalidOperationException();

            var value = IsoStandards.Single(iso => iso.Id == @event.IsoStandardId);
            IsoStandards = IsoStandards.Remove(value);
        }

        private void Apply(IsoStandardMarkedAsCompliantEvent @event)
        {
            if (@event.AggregateId != Id)
                throw new InvalidOperationException();

            if (@event.ProjectId != ProjectId)
                throw new InvalidOperationException();

            var oldValue = IsoStandards.Single(iso => iso.Id == @event.IsoStandardId);
            var newValue = oldValue.MarkAsCompliant();
            IsoStandards = IsoStandards.Replace(oldValue, newValue);
        }

        private void Apply(IsoStandardMarkedAsNoncompliantEvent @event)
        {
            if (@event.AggregateId != Id)
                throw new InvalidOperationException();

            if (@event.ProjectId != ProjectId)
                throw new InvalidOperationException();

            var oldValue = IsoStandards.Single(iso => iso.Id == @event.IsoStandardId);
            var newValue = oldValue.MarkAsNoncompliant();
            IsoStandards = IsoStandards.Replace(oldValue, newValue);
        }
    }
}