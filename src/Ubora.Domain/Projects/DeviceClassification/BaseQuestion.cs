﻿using System;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public abstract class BaseQuestion
    {
        public Guid Id { get; private set; }
        public string Text { get; private set; }
        public Note Note { get; private set; }

        public BaseQuestion(string questionText, Note note)
        {
            Id = Guid.NewGuid();
            Text = questionText;
            Note = note;
        }
    }
}