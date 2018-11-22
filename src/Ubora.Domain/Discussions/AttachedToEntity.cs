using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Ubora.Domain.Discussions
{
    public class AttachedToEntity : ValueObject
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityName EntityName { get; }
        public Guid EntityId { get; }

        public AttachedToEntity(EntityName entityName, Guid entityId)
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EntityName;
            yield return EntityId;
        }
    }

    public enum EntityName
    {
        Candidate,
        ClinicalNeed
    }
}