using System;
using System.Collections.Generic;
using Marten;
using Marten.Services;
using Marten.Services.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Tasks;
using Ubora.Domain.Projects.Workpackages;

namespace Ubora.Domain.Infrastructure.Marten
{
    public class UboraStoreOptions
    {
        public Action<StoreOptions> Configuration(IEnumerable<Type> eventTypes)
        {
            if (eventTypes == null)
            {
                throw new ArgumentNullException(nameof(eventTypes));
            }

            var serializer = new JsonNetSerializer();
            serializer.Customize(c => c.ContractResolver = new PrivateSetterResolver());

            return options =>
            {
                options.Events.UseAggregatorLookup(AggregationLookupStrategy.UsePrivateApply);
                options.Serializer(serializer);

                options.Events.InlineProjections.AggregateStreamsWith<Project>();
                options.Events.InlineProjections.AggregateStreamsWith<WorkpackageOne>();
                options.Events.InlineProjections.AggregateStreamsWith<WorkpackageTwo>();
                options.Events.InlineProjections.AggregateStreamsWith<WorkpackageThree>();
                options.Events.InlineProjections.AggregateStreamsWith<WorkpackageFour>();
                options.Events.InlineProjections.AggregateStreamsWith<WorkpackageFive>();
                options.Events.InlineProjections.Add(new AggregateMemberProjection<ProjectTask, ITaskEvent>());

                options.Events.AddEventTypes(eventTypes);
            };
        }
    }
}
