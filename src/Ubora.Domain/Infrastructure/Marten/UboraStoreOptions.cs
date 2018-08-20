using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Marten.Services;
using Marten.Services.Events;
using Newtonsoft.Json;
using Ubora.Domain.ClinicalNeeds;
using Ubora.Domain.Discussions;
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
using Ubora.Domain.Projects.History;
using Ubora.Domain.Projects._Events;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists;
using Ubora.Domain.Projects.StructuredInformations.Events;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Infrastructure.Marten
{
    public class UboraStoreOptionsConfigurer
    {
        public Action<StoreOptions> CreateConfigureAction(
            IReadOnlyCollection<Type> eventTypes,
            IReadOnlyCollection<MappedType> notificationTypes,
            AutoCreate autoCreate)
        {
            if (eventTypes == null)
            {
                throw new ArgumentNullException(nameof(eventTypes));
            }
            if (notificationTypes == null)
            {
                throw new ArgumentNullException(nameof(notificationTypes));
            }

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

                options.Schema.For<Project>()
                    .SoftDeleted();
                 
                options.Schema.For<Candidate>().SoftDeleted();
                options.Schema.For<EventLogEntry>()
                    .Duplicate(l => l.ProjectId)
                    .Duplicate(l => l.UserId)
                    .Duplicate(l => l.Timestamp);

                options.Schema.For<IProjectEntity>()
                    .AddSubClassHierarchy(typeof(EventLogEntry));

                options.Schema.For<ResourcePage>().Duplicate(x => x.CategoryId);
                options.Schema.For<ResourceCategory>().SoftDeleted();

                options.Schema.For<ResourceFile>()
                    .Duplicate(file => file.ResourcePageId);

                options.Schema.For<IsoStandardsComplianceChecklist>()
                    .Duplicate(checklist => checklist.ProjectId);

                options.Schema.For<Discussion>()
                    .Duplicate(d => d.AttachedToEntity.EntityId)
                    .Duplicate(d => d.AttachedToEntity.EntityName);

                options.Events.InlineProjections.AggregateStreamsWith<Project>();
                options.Events.InlineProjections.AggregateStreamsWith<WorkpackageOne>();
                options.Events.InlineProjections.AggregateStreamsWith<WorkpackageTwo>();
                options.Events.InlineProjections.AggregateStreamsWith<WorkpackageThree>();
                options.Events.InlineProjections.AggregateStreamsWith<WorkpackageFour>();
                options.Events.InlineProjections.AggregateStreamsWith<ApplicableRegulationsQuestionnaireAggregate>();
                options.Events.InlineProjections.AggregateStreamsWith<DeviceClassificationAggregate>();
                options.Events.InlineProjections.AggregateStreamsWith<ResourcePage>();
                options.Events.InlineProjections.Add(new AggregateMemberProjection<Assignment, IAssignmentEvent>());
                options.Events.InlineProjections.Add(new AggregateMemberProjection<ProjectFile, IFileEvent>());
                options.Events.InlineProjections.Add(new AggregateMemberProjection<ResourceFile, IResourceFileEvent>());
                options.Events.InlineProjections.AggregateStreamsWith<Candidate>();
                options.Events.InlineProjections.AggregateStreamsWith<ResourceCategory>();
                options.Events.InlineProjections.Add(new ResourcesMenuViewProjection());
                options.Events.InlineProjections.AggregateStreamsWith<IsoStandardsComplianceChecklist>();
                options.Events.InlineProjections.Add(new DeviceStructuredInformationProjection<IDeviceStructuredInformationEvent>());
                options.Events.InlineProjections.AggregateStreamsWith<Discussion>();
                options.Events.InlineProjections.AggregateStreamsWith<ClinicalNeed>();
                options.Events.InlineProjections.Add(new ClinicalNeedQuickInfo.ViewProjection());

                options.Events.AddEventTypes(eventTypes);

                options.Schema.For<INotification>()
                    .AddSubClassHierarchy(notificationTypes.ToArray());

                foreach (var eventType in eventTypes.Where(e => e.IsSubclassOf(typeof(ProjectEvent))))
                {
                    var transformerType = typeof(EventToHistoryTransformer<>).MakeGenericType(eventType);
                    var transformer = Activator.CreateInstance(transformerType);
                    options.Events.InlineProjections.TransformEvents((dynamic)transformer);
                }
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

