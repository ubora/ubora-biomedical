﻿using System;
using Autofac;
using Marten;
using Marten.Events;
using Microsoft.Extensions.DependencyInjection;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Queries;

namespace Ubora.Domain
{
    public class DomainModule : Module
    {
        private readonly string _connectionString;

        public DomainModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var options = new StoreOptions();
            options.Connection(_connectionString);

            options.NameDataLength = 100;

            Action<StoreOptions> configuration = new UboraStoreOptions().Configuration();

            configuration.Invoke(options);

            builder.RegisterInstance(new DocumentStore(options)).SingleInstance();
            builder.Register(x => x.Resolve<DocumentStore>().OpenSession()).As<IDocumentSession>().As<IQuerySession>().InstancePerLifetimeScope();
            builder.Register(x => x.Resolve<IDocumentSession>().Events).As<IEventStore>();

            builder.RegisterType<Query>().As<IQuery>().InstancePerLifetimeScope();
            builder.RegisterType<EventStreamQuery>().As<IEventStreamQuery>().InstancePerLifetimeScope();
            builder.RegisterType<CommandBus>().As<ICommandBus>().InstancePerLifetimeScope();
            builder.RegisterType<CreateProjectCommandHandler>().As<ICommandHandler<CreateProjectCommand>>().InstancePerLifetimeScope();
        }
    }
}
