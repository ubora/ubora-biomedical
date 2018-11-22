using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Ubora.Domain.Discussions
{
    public class AttachedToEntity
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityName EntityName { get; }
        public Guid EntityId { get; }

        public AttachedToEntity(EntityName entityName, Guid entityId)
        {
            EntityName = entityName;
            EntityId = entityId;
        }

    }

    public enum EntityName
    {
        Candidate
    }
}