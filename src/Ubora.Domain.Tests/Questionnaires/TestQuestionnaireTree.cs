﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Ubora.Domain.Questionnaires;

namespace Ubora.Domain.Tests.Questionnaires
{
    public class TestQuestionnaireTree : QuestionnaireTreeBase<TestQuestion, TestAnswer>
    {
        public TestQuestionnaireTree(IEnumerable<TestQuestion> questions)
        {
            Questions = questions.ToImmutableArray();
        }
    }
}