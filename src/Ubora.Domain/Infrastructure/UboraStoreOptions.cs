﻿using System;
using System.Reflection;
using Marten;
using Marten.Services;
using Marten.Services.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ubora.Domain.Projects.Events;
using Ubora.Domain.Projects.Projections;

namespace Ubora.Domain.Infrastructure
{
    public class UboraStoreOptions
    {
        public Action<StoreOptions> Configuration()
        {
            var serializer = new JsonNetSerializer();
            serializer.Customize(c => c.ContractResolver = new ResolvePrivateSetters());
            return options =>
            {
                options.Events.UseAggregatorLookup(AggregationLookupStrategy.UsePrivateApply);
                options.Serializer(serializer);
                options.Events.InlineProjections.AggregateStreamsWith<Project>();
                options.Events.InlineProjections.Add(new WorkpackagesProjection());

                // TODO: Add event types by convention
                options.Events.AddEventType(typeof(ProjectCreated));
            };
        }

        internal class ResolvePrivateSetters : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(
                MemberInfo member,
                MemberSerialization memberSerialization)
            {
                var prop = base.CreateProperty(member, memberSerialization);

                if (!prop.Writable)
                {
                    var property = member as PropertyInfo;
                    if (property != null)
                    {
                        var hasPrivateSetter = property.GetSetMethod(true) != null;
                        prop.Writable = hasPrivateSetter;
                    }
                }

                return prop;
            }
        }
    }
}
