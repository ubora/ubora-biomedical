using System;

namespace Ubora.Web._Features.Projects.DeviceClassification.Services
{
    public abstract class BaseQuestion
    {
        public Guid Id { get; }
        public string Text { get; }

        public BaseQuestion(Guid id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}