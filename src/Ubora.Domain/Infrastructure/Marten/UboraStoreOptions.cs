using System;
using Marten;
using Marten.Services;
using Marten.Services.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Tasks;
using Ubora.Domain.Projects.DeviceClassification;

namespace Ubora.Domain.Infrastructure.Marten
{
    public class UboraStoreOptions
    {
        public Action<StoreOptions> Configuration()
        {
            var serializer = new JsonNetSerializer();
            serializer.Customize(c => c.ContractResolver = new PrivateSetterResolver());

            return options =>
            {
                options.Events.UseAggregatorLookup(AggregationLookupStrategy.UsePrivateApply);
                options.Serializer(serializer);

                options.Events.InlineProjections.AggregateStreamsWith<Project>();
                options.Events.InlineProjections.Add(new AggregateMemberProjection<ProjectTask, ITaskEvent>());

                // TODO: Add event types by convention
                options.Events.AddEventType(typeof(ProjectCreatedEvent));
                options.Events.AddEventType(typeof(TaskAddedEvent));
                options.Events.AddEventType(typeof(TaskEditedEvent));
                options.Events.AddEventType(typeof(ProjectUpdatedEvent));
                options.Events.AddEventType(typeof(MemberAddedToProjectEvent));
                options.Events.AddEventType(typeof(DeviceClassificationSetEvent));
            };
        }
    }
}
