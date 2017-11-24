using System.Collections.Generic;
using Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses;
using Ubora.Domain.Questionnaires.DeviceClassifications.Texts;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class DeviceClassificationQuestionnaireTreeFactory
    {
        public virtual DeviceClassificationQuestionnaireTree CreateDeviceClassificationVersionOne()
        {
            var questions = new[]
            {
                new Question(nameof(QuestionTexts.q1), new[]
                {
                    new Answer(nameof(AnswerTexts.q1), nextQuestionId: nameof(QuestionTexts.q1_1)),
                    new Answer(nameof(AnswerTexts.q2), nextQuestionId: nameof(QuestionTexts.q2_1))
                }),
                new Question(nameof(QuestionTexts.q1_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q1_1_1)),
                    new Answer("n", nameof(QuestionTexts.q3))
                }),
                new Question(nameof(QuestionTexts.q1_1_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q1_1_2)),
                    new Answer("n", nameof(QuestionTexts.q1_1_2))
                }),
                new Question(nameof(QuestionTexts.q1_1_2), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q1_1_3)),
                    new Answer("n", nameof(QuestionTexts.q1_1_3))
                }),
                new Question(nameof(QuestionTexts.q1_1_3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q1_1_4)),
                    new Answer("n", nameof(QuestionTexts.q1_1_4))
                }),
                new Question(nameof(QuestionTexts.q1_1_4), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q1_1_4_1)),
                    new Answer("n", nameof(QuestionTexts.q1_1_5))
                }),
                new Question(nameof(QuestionTexts.q1_1_4_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q1_1_4_2)),
                    new Answer("n", nameof(QuestionTexts.q1_1_4_2))
                }),
                new Question(nameof(QuestionTexts.q1_1_4_2), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q1_1_4_3)),
                    new Answer("n", nameof(QuestionTexts.q1_1_4_3))
                }),
                new Question(nameof(QuestionTexts.q1_1_4_3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q1_1_5)),
                    new Answer("n", nameof(QuestionTexts.q1_1_5))
                }),
                new Question(nameof(QuestionTexts.q1_1_5), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q1_1_5_1)),
                    new Answer("n", nameof(QuestionTexts.q3))
                }),
                new Question(nameof(QuestionTexts.q1_1_5_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q1_1_5_2)),
                    new Answer("n", nameof(QuestionTexts.q1_1_5_2))
                }),
                new Question(nameof(QuestionTexts.q1_1_5_2), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q1_1_5_3)),
                    new Answer("n", nameof(QuestionTexts.q1_1_5_3))
                }),
                new Question(nameof(QuestionTexts.q1_1_5_3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3)),
                    new Answer("n", nameof(QuestionTexts.q3))
                }),
                new Question(nameof(QuestionTexts.q2_1), new[]
                {
                    new Answer(nameof(AnswerTexts.q2_1), nextQuestionId: nameof(QuestionTexts.q2_1_1)),
                    new Answer(nameof(AnswerTexts.q2_2), nextQuestionId: nameof(QuestionTexts.q2_2_1))
                }),
                new Question(nameof(QuestionTexts.q2_1_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_1_1_1)),
                    new Answer("n", nameof(QuestionTexts.q2_1_2))
                }),
                new Question(nameof(QuestionTexts.q2_1_1_1), new []
                {
                    new Answer("y", nameof(QuestionTexts.q2_1_2)), 
                    new Answer("n", nameof(QuestionTexts.q2_1_1_2))
                }),
                new Question(nameof(QuestionTexts.q2_1_1_2), new []
                {
                    new Answer(nameof(AnswerTexts.q2_1_1_2), nameof(QuestionTexts.q2_1_2)),
                    new Answer(nameof(AnswerTexts.q2_1_1_3), nameof(QuestionTexts.q2_1_2)),
                    new Answer("n", nameof(QuestionTexts.q2_1_1_4))
                }),
                new Question(nameof(QuestionTexts.q2_1_1_4), new []
                {
                    new Answer(nameof(AnswerTexts.q2_1_1_4), nameof(QuestionTexts.q2_1_2)),
                    new Answer(nameof(AnswerTexts.q2_1_1_5), nameof(QuestionTexts.q2_1_2)),
                    new Answer(nameof(AnswerTexts.q2_1_1_6), nameof(QuestionTexts.q2_1_2)),
                    new Answer("n", nameof(QuestionTexts.q2_1_2))
                }),
                new Question(nameof(QuestionTexts.q2_1_2), new[]
                {
                    new Answer(nameof(AnswerTexts.q2_1_2), nextQuestionId: nameof(QuestionTexts.q2_1_2_1)),
                    new Answer(nameof(AnswerTexts.q2_1_3), nextQuestionId: nameof(QuestionTexts.q3))
                }),
                new Question(nameof(QuestionTexts.q2_1_2_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3)),
                    new Answer("n", nameof(QuestionTexts.q2_1_2_2))
                }),
                new Question(nameof(QuestionTexts.q2_1_2_2), new[]
                {
                    new Answer(nameof(AnswerTexts.q2_1_2_2), nameof(QuestionTexts.q3)),
                    new Answer(nameof(AnswerTexts.q2_1_2_3), nameof(QuestionTexts.q3)),
                    new Answer("n", nameof(QuestionTexts.q2_1_2_4))
                }),
                new Question(nameof(QuestionTexts.q2_1_2_4), new[]
                {
                    new Answer(nameof(AnswerTexts.q2_1_2_4), nameof(QuestionTexts.q3)),
                    new Answer(nameof(AnswerTexts.q2_1_2_5), nameof(QuestionTexts.q3)),
                    new Answer(nameof(AnswerTexts.q2_1_2_6), nameof(QuestionTexts.q3)),
                    new Answer("n", nameof(QuestionTexts.q3))
                }),
                new Question(nameof(QuestionTexts.q2_2_1), new[]
                {
                    new Answer(nameof(AnswerTexts.q2_2_1), nextQuestionId: nameof(QuestionTexts.q2_2_1_1)),
                    new Answer(nameof(AnswerTexts.q2_2_2), nextQuestionId: nameof(QuestionTexts.q2_2_2_1)),
                    new Answer(nameof(AnswerTexts.q2_2_3), nextQuestionId: nameof(QuestionTexts.q2_2_3_1))
                }),
                new Question(nameof(QuestionTexts.q2_2_1_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_1_2)),
                    new Answer("n", nameof(QuestionTexts.q2_2_1_2))
                }),
                new Question(nameof(QuestionTexts.q2_2_1_2), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_1_3)),
                    new Answer("n", nameof(QuestionTexts.q2_2_1_3))
                }),
                new Question(nameof(QuestionTexts.q2_2_1_3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_1_4)),
                    new Answer("n", nameof(QuestionTexts.q2_2_1_4))
                }),
                new Question(nameof(QuestionTexts.q2_2_1_4), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_1_5)),
                    new Answer("n", nameof(QuestionTexts.q2_2_1_5))
                }),
                new Question(nameof(QuestionTexts.q2_2_1_5), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_1_6)),
                    new Answer("n", nameof(QuestionTexts.q2_2_1_6))
                }),
                new Question(nameof(QuestionTexts.q2_2_1_6), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3)),
                    new Answer("n", nameof(QuestionTexts.q3))
                }),
                new Question(nameof(QuestionTexts.q2_2_2_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_2_2)),
                    new Answer("n", nameof(QuestionTexts.q2_2_2_2))
                }),
                new Question(nameof(QuestionTexts.q2_2_2_2), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_2_3)),
                    new Answer("n", nameof(QuestionTexts.q2_2_2_3))
                }),
                new Question(nameof(QuestionTexts.q2_2_2_3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_2_4)),
                    new Answer("n", nameof(QuestionTexts.q2_2_2_4))
                }),
                new Question(nameof(QuestionTexts.q2_2_2_4), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_2_5)),
                    new Answer("n", nameof(QuestionTexts.q3))
                }),
                new Question(nameof(QuestionTexts.q2_2_2_5), new[]
                {
                    new Answer(nameof(AnswerTexts.q2_2_2_5_1), nextQuestionId: nameof(QuestionTexts.q2_2_2_5_3)),
                    new Answer(nameof(AnswerTexts.q2_2_2_5_2), nextQuestionId: nameof(QuestionTexts.q2_2_2_5_3)),
                    new Answer("n", nameof(QuestionTexts.q2_2_2_5_3))
                }),
                new Question(nameof(QuestionTexts.q2_2_2_5_3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3)),
                    new Answer("n", nameof(QuestionTexts.q3))
                }),
                new Question(nameof(QuestionTexts.q2_2_3_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_3_2)),
                    new Answer("n", nameof(QuestionTexts.q2_2_3_2))
                }),
                new Question(nameof(QuestionTexts.q2_2_3_2), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_3_3)),
                    new Answer("n", nameof(QuestionTexts.q2_2_3_3))
                }),
                new Question(nameof(QuestionTexts.q2_2_3_3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_3_4)),
                    new Answer("n", nameof(QuestionTexts.q2_2_3_4))
                }),
                new Question(nameof(QuestionTexts.q2_2_3_4), new[]
                {
                    new Answer(nameof(AnswerTexts.q2_2_3_4_1), nextQuestionId: nameof(QuestionTexts.q2_2_3_5)),
                    new Answer(nameof(AnswerTexts.q2_2_3_4_2), nextQuestionId: nameof(QuestionTexts.q2_2_3_5)),
                    new Answer("n", nameof(QuestionTexts.q2_2_3_5))
                }),
                new Question(nameof(QuestionTexts.q2_2_3_5), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_3_6)),
                    new Answer("n", nameof(QuestionTexts.q2_2_3_6))
                }),
                new Question(nameof(QuestionTexts.q2_2_3_6), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_3_7)),
                    new Answer("n", nameof(QuestionTexts.q2_2_3_7))
                }),
                new Question(nameof(QuestionTexts.q2_2_3_7), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_3_8)),
                    new Answer("n", nameof(QuestionTexts.q2_2_3_8))
                }),
                new Question(nameof(QuestionTexts.q2_2_3_8), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_3_9)),
                    new Answer("n", nameof(QuestionTexts.q2_2_3_9))
                }),
                new Question(nameof(QuestionTexts.q2_2_3_9), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_3_10)),
                    new Answer("n", nameof(QuestionTexts.q2_2_3_10))
                }),
                new Question(nameof(QuestionTexts.q2_2_3_10), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_3_11)),
                    new Answer("n", nameof(QuestionTexts.q2_2_3_11))
                }),
                new Question(nameof(QuestionTexts.q2_2_3_11), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_3_12)),
                    new Answer("n", nameof(QuestionTexts.q2_2_3_12))
                }),
                new Question(nameof(QuestionTexts.q2_2_3_12), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q2_2_3_13)),
                    new Answer("n", nameof(QuestionTexts.q2_2_3_13))
                }),
                new Question(nameof(QuestionTexts.q2_2_3_13), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3)),
                    new Answer("n", nameof(QuestionTexts.q3))
                }),
                new Question(nameof(QuestionTexts.q3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_1)),
                    new Answer("n", nameof(QuestionTexts.q4))
                }),
                new Question(nameof(QuestionTexts.q3_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_1_1)),
                    new Answer("n", nameof(QuestionTexts.q3_2))
                }),
                new Question(nameof(QuestionTexts.q3_1_1), new[]
                {
                    new Answer(nameof(AnswerTexts.q3_1_1), nextQuestionId: nameof(QuestionTexts.q3_1_3)),
                    new Answer(nameof(AnswerTexts.q3_1_2), nextQuestionId: nameof(QuestionTexts.q3_1_3)),
                    new Answer("n", nextQuestionId: nameof(QuestionTexts.q3_1_3))
                }),
                new Question(nameof(QuestionTexts.q3_1_3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_1_4)),
                    new Answer("n", nameof(QuestionTexts.q3_1_4))
                }),
                new Question(nameof(QuestionTexts.q3_1_4), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_1_5)),
                    new Answer("n", nameof(QuestionTexts.q3_1_5))
                }),
                new Question(nameof(QuestionTexts.q3_1_5), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_2)),
                    new Answer("n", nameof(QuestionTexts.q3_2))
                }),
                new Question(nameof(QuestionTexts.q3_2), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_3)),
                    new Answer("n", nameof(QuestionTexts.q3_3))
                }),
                new Question(nameof(QuestionTexts.q3_3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_4)),
                    new Answer("n", nameof(QuestionTexts.q3_4))
                }),
                new Question(nameof(QuestionTexts.q3_4), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_4_1)),
                    new Answer("n", nameof(QuestionTexts.q3_5))
                }),
                new Question(nameof(QuestionTexts.q3_4_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_4_1_1)),
                    new Answer("n", nameof(QuestionTexts.q3_4_2))
                }),
                new Question(nameof(QuestionTexts.q3_4_1_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_4_1_2)),
                    new Answer("n", nameof(QuestionTexts.q3_4_2))
                }),
                new Question(nameof(QuestionTexts.q3_4_1_2), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_4_1_3)),
                    new Answer("n", nameof(QuestionTexts.q3_4_3))
                }),
                new Question(nameof(QuestionTexts.q3_4_1_3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_4_2)),
                    new Answer("n", nameof(QuestionTexts.q3_4_2))
                }),
                new Question(nameof(QuestionTexts.q3_4_2), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_4_2_1)),
                    new Answer("n", nameof(QuestionTexts.q3_4_3))
                }),
                new Question(nameof(QuestionTexts.q3_4_2_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_4_2_2)),
                    new Answer("n", nameof(QuestionTexts.q3_4_2_2))
                }),
                new Question(nameof(QuestionTexts.q3_4_2_2), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_4_3)),
                    new Answer("n", nameof(QuestionTexts.q3_4_2_3))
                }),
                new Question(nameof(QuestionTexts.q3_4_2_3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_4_3)),
                    new Answer("n", nameof(QuestionTexts.q3_4_3))
                }),
                new Question(nameof(QuestionTexts.q3_4_3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_5)),
                    new Answer("n", nameof(QuestionTexts.q3_5))
                }),
                new Question(nameof(QuestionTexts.q3_5), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_5_1)),
                    new Answer("n", nameof(QuestionTexts.q3_6))
                }),
                new Question(nameof(QuestionTexts.q3_5_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_5_1_1)),
                    new Answer("n", nameof(QuestionTexts.q3_5_2))
                }),
                new Question(nameof(QuestionTexts.q3_5_1_1), new[]
                {
                    new Answer(nameof(AnswerTexts.q3_5_1_1), nextQuestionId: nameof(QuestionTexts.q3_5_2)),
                    new Answer(nameof(AnswerTexts.q3_5_1_2), nextQuestionId: nameof(QuestionTexts.q3_5_2)),
                    new Answer("n", nameof(QuestionTexts.q3_5_2))
                }),
                new Question(nameof(QuestionTexts.q3_5_2), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_5_2_1)),
                    new Answer("n", nameof(QuestionTexts.q3_6))
                }),
                new Question(nameof(QuestionTexts.q3_5_2_1), new[]
                {
                    new Answer(nameof(AnswerTexts.q3_5_2_1), nextQuestionId: nameof(QuestionTexts.q3_6)),
                    new Answer(nameof(AnswerTexts.q3_5_2_2), nextQuestionId: nameof(QuestionTexts.q3_6)),
                    new Answer("n", nextQuestionId: nameof(QuestionTexts.q3_6)) // TODO: class?
                }),
                new Question(nameof(QuestionTexts.q3_6), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q3_6_1)),
                    new Answer("n", nameof(QuestionTexts.q4))
                }),
                new Question(nameof(QuestionTexts.q3_6_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q4)),
                    new Answer("n", nameof(QuestionTexts.q4))
                }),
                new Question(nameof(QuestionTexts.q4), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q5)),
                    new Answer("n", nameof(QuestionTexts.q5))
                }),
                new Question(nameof(QuestionTexts.q5), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q5_1)),
                    new Answer("n", nameof(QuestionTexts.q6))
                }),
                new Question(nameof(QuestionTexts.q5_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q6)),
                    new Answer("n", nameof(QuestionTexts.q6))
                }),
                new Question(nameof(QuestionTexts.q6), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q7)),
                    new Answer("n", nameof(QuestionTexts.q7))
                }),
                new Question(nameof(QuestionTexts.q7), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q7_1)),
                    new Answer("n", nameof(QuestionTexts.q8))
                }),
                new Question(nameof(QuestionTexts.q7_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q7_3)),
                    new Answer("n", nameof(QuestionTexts.q7_3))
                }),
                new Question(nameof(QuestionTexts.q7_3), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q8)),
                    new Answer("n", nameof(QuestionTexts.q8))
                }),
                new Question(nameof(QuestionTexts.q8), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q9)),
                    new Answer("n", nameof(QuestionTexts.q9))
                }),
                new Question(nameof(QuestionTexts.q9),new[]
                {
                    new Answer("y", nameof(QuestionTexts.q9_1)),
                    new Answer("n", nameof(QuestionTexts.q10))
                }),
                new Question(nameof(QuestionTexts.q9_1), new[]
                {
                    new Answer(nameof(AnswerTexts.q9_1), nextQuestionId: nameof(QuestionTexts.q10)),
                    new Answer(nameof(AnswerTexts.q9_2), nextQuestionId: nameof(QuestionTexts.q10)),
                    new Answer(nameof(AnswerTexts.q9_3), nextQuestionId: nameof(QuestionTexts.q10))
                }),
                new Question(nameof(QuestionTexts.q10), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q10_1)),
                    new Answer("n", nameof(QuestionTexts.q11))
                }),
                new Question(nameof(QuestionTexts.q10_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q10_2)),
                    new Answer("n", nameof(QuestionTexts.q10_2))
                }),
                new Question(nameof(QuestionTexts.q10_2), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q11)),
                    new Answer("n", nameof(QuestionTexts.q11))
                }),
                new Question(nameof(QuestionTexts.q11), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q11_1)),
                    new Answer("n", nameof(QuestionTexts.q12))
                }),
                new Question(nameof(QuestionTexts.q11_1), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q11_2)),
                    new Answer("n", nameof(QuestionTexts.q11_2))
                }),
                new Question(nameof(QuestionTexts.q11_2), new[]
                {
                    new Answer("y", nameof(QuestionTexts.q11_3)),
                    new Answer("n", nameof(QuestionTexts.q11_3))
                }),
                new Question(nameof(QuestionTexts.q11_3),new[]
                {
                    new Answer("y", nameof(QuestionTexts.q12)),
                    new Answer("n", nameof(QuestionTexts.q12))
                }),
                new Question(nameof(QuestionTexts.q12), new []
                {
                    new Answer("y", null),
                    new Answer("n", null)
                })
            };

            var deviceClassOneWithConditions = DeviceClass.One.WithConditions(new []
            {
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q1_1), "n"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q1_1_5_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_1_1_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_1_1_2), nameof(AnswerTexts.q2_1_1_2)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_1_2_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_1_2_2), nameof(AnswerTexts.q2_1_2_2)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_1_2), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_4_1_1), "y"),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { nameof(QuestionTexts.q1_1_1), "n" },
                    { nameof(QuestionTexts.q1_1_2), "n" },
                    { nameof(QuestionTexts.q1_1_3), "n" },
                    { nameof(QuestionTexts.q1_1_4), "n" },
                    { nameof(QuestionTexts.q1_1_5), "n" }
                }),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { nameof(QuestionTexts.q3_5_1), "n" },
                    { nameof(QuestionTexts.q3_5_2), "n" }
                }),

                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { nameof(QuestionTexts.q3_1), "n" },
                    { nameof(QuestionTexts.q3_2), "n" },
                    { nameof(QuestionTexts.q3_3), "n" },
                    { nameof(QuestionTexts.q3_4), "n" },
                    { nameof(QuestionTexts.q3_5), "n" },
                    { nameof(QuestionTexts.q3_6), "n" }
                })
            });

            var deviceClassTwoAWithConditions = DeviceClass.TwoA.WithConditions(new []
            {
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q1_1_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q1_1_2), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q1_1_4_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q1_1_5_3), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_1_1_2), nameof(AnswerTexts.q2_1_1_3)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_1_1_4), nameof(AnswerTexts.q2_1_1_4)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_1_2), nameof(AnswerTexts.q2_1_3)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_1_2_2), nameof(AnswerTexts.q2_1_2_3)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_1_2_4), nameof(AnswerTexts.q2_1_2_4)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_2_5), nameof(AnswerTexts.q2_2_2_5_1)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_1_1), nameof(AnswerTexts.q3_1_2)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_4_1_2), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_4_1_3), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_4_2_3), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_5_1_1), "n"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_5_2_1), nameof(AnswerTexts.q3_5_2_2)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_6_1), "n"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q5_1), "n"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q7_1), "n"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q7_3), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q9_1), nameof(AnswerTexts.q9_3)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q11_3), "y"),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { nameof(QuestionTexts.q1_1_5_1), "n" },
                    { nameof(QuestionTexts.q1_1_5_2), "n" },
                    { nameof(QuestionTexts.q1_1_5_3), "n" }
                }),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { nameof(QuestionTexts.q2_2_1_1), "n" },
                    { nameof(QuestionTexts.q2_2_1_2), "n" },
                    { nameof(QuestionTexts.q2_2_1_3), "n" },
                    { nameof(QuestionTexts.q2_2_1_4), "n" },
                    { nameof(QuestionTexts.q2_2_1_5), "n" },
                    { nameof(QuestionTexts.q2_2_1_6), "n" }
                }),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { nameof(QuestionTexts.q2_2_2_1), "n" },
                    { nameof(QuestionTexts.q2_2_2_2), "n" },
                    { nameof(QuestionTexts.q2_2_2_3), "n" },
                    { nameof(QuestionTexts.q2_2_2_4), "n" },
                    { nameof(QuestionTexts.q2_2_2_5), "n" }
                }),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { nameof(QuestionTexts.q3_4_1), "n" },
                    { nameof(QuestionTexts.q3_4_2), "n" },
                    { nameof(QuestionTexts.q3_4_3), "n" }
                }),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { nameof(QuestionTexts.q10_1), "n" },
                    { nameof(QuestionTexts.q10_2), "n" }
                })
            });

            var deviceClassTwoBWithConditions = DeviceClass.TwoB.WithConditions(new []
            {
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q1_1_3), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q1_1_4_2), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q1_1_5_2), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_1_1_4), nameof(AnswerTexts.q2_1_1_5)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_1_1_4), nameof(AnswerTexts.q2_1_1_6)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_1_2_4), nameof(AnswerTexts.q2_1_2_5)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_1_2_4), nameof(AnswerTexts.q2_1_2_6)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_1_4), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_1_5), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_1_6), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_2_3), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_2_5), nameof(AnswerTexts.q2_2_2_5_2)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_2_5_3), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_4), nameof(AnswerTexts.q2_2_3_4_1)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_10), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_13), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_1_1), nameof(AnswerTexts.q3_1_1)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_1_3), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_1_4), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_1_5), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_2), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_4_2_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_4_2_2), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_4_3), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_5_1_1), nameof(AnswerTexts.q3_5_1_2)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_5_2_1), "q3_5_2_1"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_6_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q5_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q6), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q7_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q9_1), nameof(AnswerTexts.q9_2)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q10_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q10_2), "y"),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { nameof(QuestionTexts.q2_2_3_1), "n" },
                    { nameof(QuestionTexts.q2_2_3_2), "n" },
                    { nameof(QuestionTexts.q2_2_3_3), "n" },
                    { nameof(QuestionTexts.q2_2_3_4), "n" },
                    { nameof(QuestionTexts.q2_2_3_5), "n" },
                    { nameof(QuestionTexts.q2_2_3_6), "n" },
                    { nameof(QuestionTexts.q2_2_3_7), "n" },
                    { nameof(QuestionTexts.q2_2_3_8), "n" },
                    { nameof(QuestionTexts.q2_2_3_9), "n" },
                    { nameof(QuestionTexts.q2_2_3_10), "n" },
                    { nameof(QuestionTexts.q2_2_3_11), "n" },
                    { nameof(QuestionTexts.q2_2_3_12), "n" },
                    { nameof(QuestionTexts.q2_2_3_13), "n" }
                }),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { nameof(QuestionTexts.q11_1), "n" },
                    { nameof(QuestionTexts.q11_2), "n" },
                    { nameof(QuestionTexts.q11_3), "n" }
                })
            });

            var deviceClassThreeWithConditions = DeviceClass.Three.WithConditions(new []
            {
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q1_1_4_3), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_1_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_1_3), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_2_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_2_2), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_2_4), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_2), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_3), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_4), nameof(AnswerTexts.q2_2_3_4_2)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_5), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_6), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_7), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_8), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_9), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_11), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q2_2_3_12), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_3), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q3_5_1_1), nameof(AnswerTexts.q3_5_1_1)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q4), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q8), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q9_1), nameof(AnswerTexts.q9_1)),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q11_1), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q11_2), "y"),
                new ChosenAnswerDeviceClassCondition(nameof(QuestionTexts.q12), "y")
            });

            var deviceClassesWithConditions = new[]
            {
                deviceClassOneWithConditions,
                deviceClassTwoAWithConditions,
                deviceClassTwoBWithConditions,
                deviceClassThreeWithConditions
            };

            return new DeviceClassificationQuestionnaireTree(questions, deviceClassesWithConditions);
        }
    }
}
