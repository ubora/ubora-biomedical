using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Marten.Services;
using Marten.Services.Events;
using Newtonsoft.Json;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Repository;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects.Assignments;
using Ubora.Domain.Projects.Assignments.Events;
using Ubora.Domain.Projects.Repository.Events;
using Ubora.Domain.Questionnaires.ApplicableRegulations;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Users;
using Ubora.Domain.Projects.Candidates;

namespace Ubora.Domain.Infrastructure.Marten
{
    public class UboraStoreOptionsConfigurer
    {
        public Action<StoreOptions> CreateConfigureAction(
            IEnumerable<Type> eventTypes, 
            IEnumerable<MappedType> notificationTypes,
            AutoCreate autoCreate)
        {
            if (eventTypes == null) { throw new ArgumentNullException(nameof(eventTypes)); }
            if (notificationTypes == null) { throw new ArgumentNullException(nameof(notificationTypes)); }

            return options =>
            {
                options.AutoCreateSchemaObjects = autoCreate;
                options.NameDataLength = 100;
                options.PLV8Enabled = false;

                options.Events.UseAggregatorLookup(AggregationLookupStrategy.UsePrivateApply);
                options.Serializer(serializer: CreateConfiguredJsonSerializer());

                options.Schema.For<UserProfile>().SoftDeleted();
                options.Schema.For<ProjectFile>();
                options.Schema.For<Assignment>();
                options.Schema.For<Project>().SoftDeleted();
                options.Schema.For<Candidate>();
                options.Events.InlineProjections.AggregateStreamsWith<Project>();
                options.Events.InlineProjections.AggregateStreamsWith<WorkpackageOne>();
                options.Events.InlineProjections.AggregateStreamsWith<WorkpackageTwo>();
                options.Events.InlineProjections.AggregateStreamsWith<WorkpackageThree>();
                options.Events.InlineProjections.AggregateStreamsWith<ApplicableRegulationsQuestionnaireAggregate>();
                options.Events.InlineProjections.AggregateStreamsWith<DeviceClassificationAggregate>();
                options.Events.InlineProjections.Add(new AggregateMemberProjection<Assignment, IAssignmentEvent>());
                options.Events.InlineProjections.Add(new AggregateMemberProjection<ProjectFile, IFileEvent>());
                options.Events.InlineProjections.AggregateStreamsWith<Candidate>();

                options.Events.AddEventTypes(eventTypes);

                options.Schema.For<INotification>()
                    .AddSubClassHierarchy(notificationTypes.ToArray());
            };
        }

        public static JsonNetSerializer CreateConfiguredJsonSerializer()
        {
            var serializer = new JsonNetSerializer();
            serializer.Customize(c =>
            {
                c.NullValueHandling = NullValueHandling.Ignore;
                c.ContractResolver = new PrivateSetterResolver();
            });
            return serializer;
        }
    }
}