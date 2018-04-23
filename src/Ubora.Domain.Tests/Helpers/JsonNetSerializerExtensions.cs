using System;
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

        public static object FromJson(this JsonNetSerializer serializer, Type type, string jsonSerializedObject)
        {
            using (var jsonAsStream = ToStream(jsonSerializedObject))
            using (var streamReader = new StreamReader(jsonAsStream))
            {
                return serializer.FromJson(type, streamReader);
            }
        }

        // Taken from: https://stackoverflow.com/questions/1879395/how-do-i-generate-a-stream-from-a-string
        private static Stream ToStream(string @string)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(@string ?? ""));
        }
    }
}