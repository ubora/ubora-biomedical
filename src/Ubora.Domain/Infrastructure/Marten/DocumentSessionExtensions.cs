using System;

namespace Marten
{
    public static class DocumentSessionExtensions
    {
        public static T LoadOrThrow<T>(this IDocumentSession documentSession, Guid id)
        {
            var loaded = documentSession.Load<T>(id);
            if (loaded == null)
            {
                throw new InvalidOperationException($"{typeof(T).Name} not found with ID: {id}");
            }
            return loaded;
        }

        internal static object LoadOrThrow<T>(Guid projectId)
        {
            throw new NotImplementedException();
        }
    }
}