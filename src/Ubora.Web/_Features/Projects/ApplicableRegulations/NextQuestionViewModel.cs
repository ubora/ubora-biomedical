﻿using System;
using Org.BouncyCastle.Asn1.X509;

namespace Ubora.Web._Features.Projects.ApplicableRegulations
{
    public class NextQuestionViewModel
    {
        public Guid Id { get; set; }
        public Guid QuestionnaireId { get; set; }
       
        public string Text { get; set; }
        public bool Answer { get; set; }
    }
}