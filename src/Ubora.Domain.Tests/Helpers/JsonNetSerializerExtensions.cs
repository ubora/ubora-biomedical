using System.IO;
using System.Text;
using Marten.Services;

namespace Ubora.Domain.Tests.Helpers
{
    public static class JsonNetSerializerExtensions
    {
        public static T FromJson<T>(this JsonNetSerializer serializer, string jsonSerializedObject)
        {
            using (var jsonAsStream = ToStream(jsonSerializedObject))
            {
                return serializer.FromJson<T>(jsonAsStream);
            }
        }

        // Taken from: https://stackoverflow.com/questions/1879395/how-do-i-generate-a-stream-from-a-string
        private static Stream ToStream(string @string)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(@string ?? ""));
        }
    }
}