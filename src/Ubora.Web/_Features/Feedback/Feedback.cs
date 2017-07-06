using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Web._Features.Feedback
{
    public class Feedback
    {
        public Guid Id { get; private set; }
        public string Value { get; private set; }
        public string FromPath { get; private set; }
        public UserInfo By { get; private set; }
        public DateTimeOffset SentAt { get; set; }

        public Feedback(string value, string fromPath, UserInfo by)
        {
            Id = Guid.NewGuid();
            SentAt = DateTimeOffset.Now;
            Value = value;
            FromPath = fromPath;
            By = by;
        }
    }
}