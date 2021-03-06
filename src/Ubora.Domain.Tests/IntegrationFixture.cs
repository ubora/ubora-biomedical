﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using Autofac;
using AutoFixture;
using Marten;
using Moq;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Marten;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Users;
using Ubora.Domain.Users.Queries;

namespace Ubora.Domain.Tests
{
    /// <summary>
    /// Use this for testing with database and domain IoC service registrations.
    /// </summary>
    public abstract class IntegrationFixture : DocumentSessionIntegrationFixture
    {
        private IContainer _innerContainer;
        public IComponentContext Container => _innerContainer ?? (_innerContainer = InitializeContainer());

        private ICommandQueryProcessor _processor;
        public ICommandQueryProcessor Processor => _processor ?? (_processor = Container.Resolve<ICommandQueryProcessor>());

        private readonly DomainAutofacModule _domainAutofacModule;

        public IFixture AutoFixture { get; } = new Fixture();

        static IntegrationFixture()
        {
            DomainAutofacModule.ShouldInitializeAndRegisterDocumentStoreOnLoad = false;
        }

        protected IntegrationFixture()
        {
            AutoFixture.Register<QuillDelta>(() => new QuillDelta("{" + Guid.NewGuid() + "}"));

            _domainAutofacModule = new DomainAutofacModule(ConnectionSource.ConnectionString, Mock.Of<IStorageProvider>());
            var eventTypes = DomainAutofacModule.FindDomainEventConcreteTypes();
            var notificationTypes = DomainAutofacModule.FindDomainNotificationConcreteTypes();
            StoreOptions(new UboraStoreOptionsConfigurer().CreateConfigureAction(eventTypes.ToList(), notificationTypes.ToList(), AutoCreate.CreateOnly));
            InitializeContainer();
        }

        private IContainer InitializeContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(_domainAutofacModule);

            builder.RegisterType<TestFindUboraMentorProfilesQueryHandler>()
                .As<IQueryHandler<FindUboraMentorProfilesQuery, IReadOnlyCollection<UserProfile>>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TestFindUboraManagementGroupHandler>()
                .As<IQueryHandler<FindUboraManagementGroupQuery, IReadOnlyCollection<UserProfile>>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TestFindUboraAdministratorsQueryHandler>().As<IQueryHandler<FindUboraAdministratorsQuery, IReadOnlyCollection<UserProfile>>>()
                .InstancePerLifetimeScope();

            // Register Marten DocumentStore/Session
            builder.Register(_ => (TestingDocumentStore)theStore).As<DocumentStore>().As<IDocumentStore>().SingleInstance();
            builder.Register(_ => Session).As<IDocumentSession>().As<IQuerySession>().InstancePerLifetimeScope();

            var storageProviderMock = new Mock<IStorageProvider>().Object;
            builder.RegisterInstance(storageProviderMock).As<IStorageProvider>();

            RegisterAdditional(builder);

            var container = builder.Build();
            return container;
        }

        // Extension point for test-specific registrations (e.g. mocks)
        protected virtual void RegisterAdditional(ContainerBuilder builder)
        {
        }

        public override void Dispose()
        {
            _innerContainer?.Dispose();

            base.Dispose();
        }
    }
}