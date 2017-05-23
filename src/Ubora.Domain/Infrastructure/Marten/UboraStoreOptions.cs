using System;
using System.Collections.Generic;
using Marten;
using Marten.Services;
using Marten.Services.Events;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Tasks;
using Ubora.Domain.Projects.WorkpackageOnes;

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
                options.Events.InlineProjections.Add(new AggregateMemberProjection<ProjectTask, ITaskEvent>());

                options.Events.AddEventTypes(eventTypes);

                //// TODO: Add event types by convention
                //options.Events.AddEventType(typeof(ProjectCreatedEvent));
                //options.Events.AddEventType(typeof(TaskAddedEvent));
                //options.Events.AddEventType(typeof(TaskEditedEvent));
                //options.Events.AddEventType(typeof(ProjectUpdatedEvent));
                //options.Events.AddEventType(typeof(InviteMemberToProjectCommand));
                //options.Events.AddEventType(typeof(WorkpackageOneOpenedEvent));
                //options.Events.AddEventType(typeof(DescriptionOfNeedEdited));
                //options.Events.AddEventType(typeof(DescriptionOfExistingSolutionsAndAnalysisEditedEvent));
                //options.Events.AddEventType(typeof(ProductFunctionalityEditedEvent));
                //options.Events.AddEventType(typeof(ProductPerformanceEditedEvent));
                //options.Events.AddEventType(typeof(ProductUsabilityEditedEvent));
                //options.Events.AddEventType(typeof(ProductSafetyEditedEvent));
                //options.Events.AddEventType(typeof(PatientPopulationStudyEditedEvent));
                //options.Events.AddEventType(typeof(UserRequirementStudyEditedEvent));
                //options.Events.AddEventType(typeof(AdditionalInformationEditedEvent));
            };
        }
    }
}
