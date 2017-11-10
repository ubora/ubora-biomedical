using System.Collections.Generic;
using Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class DeviceClassificationQuestionnaireTreeFactory
    {
        public virtual DeviceClassificationQuestionnaireTree CreateDeviceClassification()
        {
            var questions = new[]
            {
                new Question("q1", new[]
                {
                    new Answer("q1", "q1_1"),
                    new Answer("q2", "q2_1")
                }),
                new Question("q1_1", new[]
                {
                    new Answer("y", "q1_1_1"),
                    new Answer("n", "q3")
                }),
                new Question("q1_1_1", new[]
                {
                    new Answer("y","q1_1_2"),
                    new Answer("n", "q1_1_2")
                }),
                new Question("q1_1_2", new[]
                {
                    new Answer("y","q1_1_3"),
                    new Answer("n", "q1_1_3")
                }),
                new Question("q1_1_3", new[]
                {
                    new Answer("y","q1_1_4"),
                    new Answer("n", "q1_1_4")
                }),
                new Question("q1_1_4", new[]
                {
                    new Answer("y", "q1_1_4_1"),
                    new Answer("n", "q1_1_5")
                }),
                new Question("q1_1_4_1", new[]
                {
                    new Answer("y","q1_1_4_2"),
                    new Answer("n", "q1_1_4_2")
                }),
                new Question("q1_1_4_2", new[]
                {
                    new Answer("y","q1_1_4_3"),
                    new Answer("n", "q1_1_4_3")
                }),
                new Question("q1_1_4_3", new[]
                {
                    new Answer("y","q1_1_5"),
                    new Answer("n", "q1_1_5")
                }),
                new Question("q1_1_5", new[]
                {
                    new Answer("y", "q1_1_5_1"),
                    new Answer("n", "q3")
                }),
                new Question("q1_1_5_1", new[]
                {
                    new Answer("y", "q1_1_5_2"),
                    new Answer("n", "q1_1_5_2")
                }),
                new Question("q1_1_5_2", new[]
                {
                    new Answer("y","q1_1_5_3"),
                    new Answer("n", "q1_1_5_3")
                }),
                new Question("q1_1_5_3", new[]
                {
                    new Answer("y","q3"),
                    new Answer("n", "q3")
                }),
                new Question("q2_1", new[]
                {
                    new Answer("q2_1", "q2_1_1"),
                    new Answer("q2_2", "q2_2_1")
                }),
                new Question("q2_1_1", new[]
                {
                    new Answer("y", "q2_1_1_1"),
                    new Answer("n", "q2_1_2")
                }),
                new Question("q2_1_1_1", new[]
                {
                    new Answer("q2_1_1_1", "q2_1_2"),
                    new Answer("q2_1_1_2", "q2_1_2"),
                    new Answer("q2_1_1_3","q2_1_2"),
                    new Answer("q2_1_1_4","q2_1_2"),
                    new Answer("q2_1_1_5","q2_1_2"),
                    new Answer("q2_1_1_6","q2_1_2"),
                }),
                new Question("q2_1_2", new[]
                {
                    new Answer("q2_1_2", "q2_1_2_1"),
                    new Answer("q2_1_3","q3")
                }),
                new Question("q2_1_2_1", new[]
                {
                    new Answer("y", "q2_1_2_2"),
                    new Answer("n", "q2_1_2_2")
                }),
                new Question("q2_1_2_2", new[]
                {
                    new Answer("y", "q2_1_2_3"),
                    new Answer("n", "q2_1_2_3")
                }),
                new Question("q2_1_2_3", new[]
                {
                    new Answer("y","q2_1_2_4"),
                    new Answer("n", "q2_1_2_4")
                }),
                new Question("q2_1_2_4", new[]
                {
                    new Answer("y","q2_1_2_5"),
                    new Answer("n", "q2_1_2_5")
                }),
                new Question("q2_1_2_5", new[]
                {
                    new Answer("y","q2_1_2_6"),
                    new Answer("n", "q2_1_2_6")
                }),
                new Question("q2_1_2_6", new[]
                {
                    new Answer("y","q3"),
                    new Answer("n", "q3")
                }),
                new Question("q2_2_1", new[]
                {
                    new Answer("q2_2_1", "q2_2_1_1"),
                    new Answer("q2_2_2", "q2_2_2_1"),
                    new Answer("q2_2_3", "q2_2_3_1")
                }),
                new Question("q2_2_1_1", new[]
                {
                    new Answer("y","q2_2_1_2"),
                    new Answer("n", "q2_2_1_2")
                }),
                new Question("q2_2_1_2", new[]
                {
                    new Answer("y", "q2_2_1_3"),
                    new Answer("n", "q2_2_1_3")
                }),
                new Question("q2_2_1_3", new[]
                {
                    new Answer("y","q2_2_1_4"),
                    new Answer("n", "q2_2_1_4")
                }),
                new Question("q2_2_1_4", new[]
                {
                    new Answer("y","q2_2_1_5"),
                    new Answer("n", "q2_2_1_5")
                }),
                new Question("q2_2_1_5", new[]
                {
                    new Answer("y","q2_2_1_6"),
                    new Answer("n", "q2_2_1_6")
                }),
                new Question("q2_2_1_6", new[]
                {
                    new Answer("y","q3"),
                    new Answer("n", "q3")
                }),
                new Question("q2_2_2_1", new[]
                {
                    new Answer("y","q2_2_2_2"),
                    new Answer("n", "q2_2_2_2")
                }),
                new Question("q2_2_2_2", new[]
                {
                    new Answer("y","q2_2_2_3"),
                    new Answer("n", "q2_2_2_3")
                }),
                new Question("q2_2_2_3", new[]
                {
                    new Answer("y","q2_2_2_4"),
                    new Answer("n", "q2_2_2_4")
                }),
                new Question("q2_2_2_4", new[]
                {
                    new Answer("y","q2_2_2_5"),
                    new Answer("n", "q3")
                }),
                new Question("q2_2_2_5", new[]
                {
                    new Answer("q2_2_2_5_1","q2_2_2_5_3"),
                    new Answer("q2_2_2_5_2","q2_2_2_5_3"),
                    new Answer("n", "q2_2_2_5_3")
                }),
                new Question("q2_2_2_5_3", new[]
                {
                    new Answer("y","q3"),
                    new Answer("n", "q3")
                }),
                new Question("q2_2_3_1", new[]
                {
                    new Answer("y","q2_2_3_2"),
                    new Answer("n", "q2_2_3_2")
                }),
                new Question("q2_2_3_2", new[]
                {
                    new Answer("y","q2_2_3_3"),
                    new Answer("n", "q2_2_3_3")
                }),
                new Question("q2_2_3_3", new[]
                {
                    new Answer("y","q2_2_3_4"),
                    new Answer("n", "q2_2_3_4")
                }),
                new Question("q2_2_3_4", new[]
                {
                    new Answer("q2_2_3_4_1","q2_2_3_5"),
                    new Answer("q2_2_3_4_2","q2_2_3_5"),
                    new Answer("n", "q2_2_3_5")
                }),
                new Question("q2_2_3_5", new[]
                {
                    new Answer("y","q2_2_3_6"),
                    new Answer("n", "q2_2_3_6")
                }),
                new Question("q2_2_3_6", new[]
                {
                    new Answer("y","q2_2_3_7"),
                    new Answer("n", "q2_2_3_7")
                }),
                new Question("q2_2_3_7", new[]
                {
                    new Answer("y","q2_2_3_8"),
                    new Answer("n", "q2_2_3_8")
                }),
                new Question("q2_2_3_8", new[]
                {
                    new Answer("y","q2_2_3_9"),
                    new Answer("n", "q2_2_3_9")
                }),
                new Question("q2_2_3_9", new[]
                {
                    new Answer("y","q2_2_3_10"),
                    new Answer("n", "q2_2_3_10")
                }),
                new Question("q2_2_3_10", new[]
                {
                    new Answer("y","q2_2_3_11"),
                    new Answer("n", "q2_2_3_11")
                }),
                new Question("q2_2_3_11", new[]
                {
                    new Answer("y","q2_2_3_12"),
                    new Answer("n", "q2_2_3_12")
                }),
                new Question("q2_2_3_12", new[]
                {
                    new Answer("y","q2_2_3_13"),
                    new Answer("n", "q2_2_3_13")
                }),
                new Question("q2_2_3_13", new[]
                {
                    new Answer("y","q3"),
                    new Answer("n", "q3")
                }),
                new Question("q3", new[]
                {
                    new Answer("y", "q3_1"),
                    new Answer("n", "q4")
                }),
                new Question("q3_1", new[]
                {
                    new Answer("y", "q3_1_1"),
                    new Answer("n", "q3_2")
                }),
                new Question("q3_1_1", new[]
                {
                    new Answer("q3_1_1","q3_1_3"),
                    new Answer("q3_1_2","q3_1_3")
                }),
                new Question("q3_1_3", new[]
                {
                    new Answer("y","q3_1_4"),
                    new Answer("n", "q3_1_4")
                }),
                new Question("q3_1_4", new[]
                {
                    new Answer("y","q3_1_5"),
                    new Answer("n", "q3_1_5")
                }),
                new Question("q3_1_5", new[]
                {
                    new Answer("y","q3_2"),
                    new Answer("n", "q3_2")
                }),
                new Question("q3_2", new[]
                {
                    new Answer("y","q3_3"),
                    new Answer("n", "q3_3")
                }),
                new Question("q3_3", new[]
                {
                    new Answer("y","q3_4"),
                    new Answer("n", "q3_4")
                }),
                new Question("q3_4", new[]
                {
                    new Answer("y", "q3_4_1"),
                    new Answer("n", "q3_5")
                }),
                new Question("q3_4_1", new[]
                {
                    new Answer("y", "q3_4_1_1"),
                    new Answer("n", "q3_4_2")
                }),
                new Question("q3_4_1_1", new[]
                {
                    new Answer("y", "q3_4_1_2"),
                    new Answer("n", "q3_4_2")
                }),
                new Question("q3_4_1_2", new[]
                {
                    new Answer("y","q3_4_1_3"),
                    new Answer("n", "q3_4_3")
                }),
                new Question("q3_4_1_3", new[]
                {
                    new Answer("y","q3_4_2"),
                    new Answer("n", "q3_4_2")
                }),
                new Question("q3_4_2", new[]
                {
                    new Answer("y", "q3_4_2_1"),
                    new Answer("n", "q3_4_3")
                }),
                new Question("q3_4_2_1", new[]
                {
                    new Answer("y","q3_4_2_2"),
                    new Answer("n", "q3_4_2_2")
                }),
                new Question("q3_4_2_2", new[]
                {
                    new Answer("y","q3_4_2_3"),
                    new Answer("n", "q3_4_3")
                }),
                new Question("q3_4_2_3", new[]
                {
                    new Answer("y","q3_4_3"),
                    new Answer("n", "q3_4_3")
                }),
                new Question("q3_4_3", new[]
                {
                    new Answer("y","q3_5"),
                    new Answer("n", "q3_5")
                }),
                new Question("q3_5", new[]
                {
                    new Answer("y", "q3_5_1"),
                    new Answer("n", "q3_6")
                }),
                new Question("q3_5_1", new[]
                {
                    new Answer("y", "q3_5_1_1"),
                    new Answer("n", "q3_5_2")
                }),
                new Question("q3_5_1_1", new[]
                {
                    new Answer("q3_5_1_1","q3_5_2"),
                    new Answer("q3_5_1_2","q3_5_2"),
                    new Answer("n","q3_5_2")
                }),
                new Question("q3_5_2", new[]
                {
                    new Answer("y", "q3_5_2_1"),
                    new Answer("n", "q3_6")
                }),
                new Question("q3_5_2_1", new[]
                {
                    new Answer("q3_5_2_1","q3_6"),
                    new Answer("q3_5_2_2","q3_6")
                }),
                new Question("q3_6", new[]
                {
                    new Answer("y", "q3_6_1"),
                    new Answer("n", "q4")
                }),
                new Question("q3_6_1", new[]
                {
                    new Answer("y","q4"),
                    new Answer("n","q4")
                }),
                new Question("q4", new[]
                {
                    new Answer("y","q5"),
                    new Answer("n", "q5")
                }),
                new Question("q5", new[]
                {
                    new Answer("y", "q5_1"),
                    new Answer("n", "q6")
                }),
                new Question("q5_1", new[]
                {
                    new Answer("y","q6"),
                    new Answer("n","q6")
                }),
                new Question("q6", new[]
                {
                    new Answer("y","q7"),
                    new Answer("n", "q7")
                }),
                new Question("q7", new[]
                {
                    new Answer("y", "q7_1"),
                    new Answer("n", "q8")
                }),
                new Question("q7_1", new[]
                {
                    new Answer("y","q7_3"),
                    new Answer("n","q7_3")
                }),
                new Question("q7_3", new[]
                {
                    new Answer("y","q8"),
                    new Answer("n", "q8")
                }),
                new Question("q8", new[]
                {
                    new Answer("y","q9"),
                    new Answer("n", "q9")
                }),
                new Question("q9",new[]
                {
                    new Answer("y", "q9_1"),
                    new Answer("n", "q10")
                }),
                new Question("q9_1", new[]
                {
                    new Answer("q9_1","q10"),
                    new Answer("q9_2","q10"),
                    new Answer("q9_3","q10")
                }),
                new Question("q10", new[]
                {
                    new Answer("y", "q10_1"),
                    new Answer("n", "q11")
                }),
                new Question("q10_1", new[]
                {
                    new Answer("y","q10_2"),
                    new Answer("n", "q10_2")
                }),
                new Question("q10_2", new[]
                {
                    new Answer("y","q11"),
                    new Answer("n", "q11")
                }),
                new Question("q11", new[]
                {
                    new Answer("y", "q11_1"),
                    new Answer("n", "q12")
                }),
                new Question("q11_1", new[]
                {
                    new Answer("y","q11_2"),
                    new Answer("n", "q11_2")
                }),
                new Question("q11_2", new[]
                {
                    new Answer("y","q11_3"),
                    new Answer("n", "q11_3")
                }),
                new Question("q11_3",new[]
                {
                    new Answer("y","q12"),
                    new Answer("n", "q12")
                }),
                new Question("q12", new []
                {
                    new Answer("y", null),
                    new Answer("n", null)
                })
            };

            var deviceClassOne = new DeviceClassOne(new[]
            {
                new ChosenAnswerDeviceClassCondition("q1_1", "n"),
                new ChosenAnswerDeviceClassCondition("q1_1_5_1", "y"),
                new ChosenAnswerDeviceClassCondition("q2_1_1_1", "q2_1_1_1"),
                new ChosenAnswerDeviceClassCondition("q2_1_1_1", "q2_1_1_2"),
                new ChosenAnswerDeviceClassCondition("q2_1_2_1", "y"),
                new ChosenAnswerDeviceClassCondition("q2_1_2_2", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_1_2", "y"),
                new ChosenAnswerDeviceClassCondition("q3_4_1_1", "y"),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { "q1_1_1", "n" },
                    { "q1_1_2", "n" },
                    { "q1_1_3", "n" },
                    { "q1_1_4", "n" },
                    { "q1_1_5", "n" }
                }),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { "q3_5_1", "n" },
                    { "q3_5_2", "n" }
                }),

                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { "q3_1", "n" },
                    { "q3_2", "n" },
                    { "q3_3", "n" },
                    { "q3_4", "n" },
                    { "q3_5", "n" },
                    { "q3_6", "n" }
                }),

            });
            var deviceClassTwoA = new DeviceClassTwoA(new[]
            {
                new ChosenAnswerDeviceClassCondition("q1_1_1", "y"),
                new ChosenAnswerDeviceClassCondition("q1_1_2", "y"),
                new ChosenAnswerDeviceClassCondition("q1_1_4_1", "y"),
                new ChosenAnswerDeviceClassCondition("q1_1_5_3", "y"),
                new ChosenAnswerDeviceClassCondition("q2_1_1_1", "q2_1_1_3"),
                new ChosenAnswerDeviceClassCondition("q2_1_1_1", "q2_1_1_4"),
                new ChosenAnswerDeviceClassCondition("q2_1_2", "q2_1_3"),
                new ChosenAnswerDeviceClassCondition("q2_1_2_3", "y"),
                new ChosenAnswerDeviceClassCondition("q2_1_2_4", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_2_5", "q2_2_2_5_1"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_1", "y"),
                new ChosenAnswerDeviceClassCondition("q3_1_1", "q3_1_2"),
                new ChosenAnswerDeviceClassCondition("q3_4_1_2", "y"),
                new ChosenAnswerDeviceClassCondition("q3_4_1_3", "y"),
                new ChosenAnswerDeviceClassCondition("q3_4_2_3", "y"),
                new ChosenAnswerDeviceClassCondition("q3_5_1_1", "n"),
                new ChosenAnswerDeviceClassCondition("q3_5_2_1", "q3_5_2_2"),
                new ChosenAnswerDeviceClassCondition("q3_6_1", "n"),
                new ChosenAnswerDeviceClassCondition("q5_1", "n"),
                new ChosenAnswerDeviceClassCondition("q7_1", "n"),
                new ChosenAnswerDeviceClassCondition("q7_3", "y"),
                new ChosenAnswerDeviceClassCondition("q9_1", "q9_3"),
                new ChosenAnswerDeviceClassCondition("q11_3", "y"),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { "q1_1_5_1", "n" },
                    { "q1_1_5_2", "n" },
                    { "q1_1_5_3", "n" }
                }),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { "q2_2_1_1", "n" },
                    { "q2_2_1_2", "n" },
                    { "q2_2_1_3", "n" },
                    { "q2_2_1_4", "n" },
                    { "q2_2_1_5", "n" },
                    { "q2_2_1_6", "n" }
                }),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { "q2_2_2_1", "n" },
                    { "q2_2_2_2", "n" },
                    { "q2_2_2_3", "n" },
                    { "q2_2_2_4", "n" },
                    { "q2_2_2_5", "n" }
                }),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { "q3_4_1", "n" },
                    { "q3_4_2", "n" },
                    { "q3_4_3", "n" }
                }),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { "q10_1", "n" },
                    { "q10_2", "n" }
                }),

                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { "q11_1", "n" },
                    { "q11_2", "n" },
                    { "q11_3", "n" }
                })
            });
            var deviceClassTwoB = new DeviceClassTwoB(new[]
            {
                new ChosenAnswerDeviceClassCondition("q1_1_3", "y"),
                new ChosenAnswerDeviceClassCondition("q1_1_4_2", "y"),
                new ChosenAnswerDeviceClassCondition("q1_1_5_2", "y"),
                new ChosenAnswerDeviceClassCondition("q2_1_1_1", "q2_1_1_5"),
                new ChosenAnswerDeviceClassCondition("q2_1_1_1", "q2_1_1_6"),
                new ChosenAnswerDeviceClassCondition("q2_1_2_5", "y"),
                new ChosenAnswerDeviceClassCondition("q2_1_2_6", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_1_4", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_1_5", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_1_6", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_2_3", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_2_5", "q2_2_2_5_2"),
                new ChosenAnswerDeviceClassCondition("q2_2_2_5_3", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_4", "q2_2_3_4_1"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_10", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_13", "y"),
                new ChosenAnswerDeviceClassCondition("q3_1_1", "q3_1_1"),
                new ChosenAnswerDeviceClassCondition("q3_1_3", "y"),
                new ChosenAnswerDeviceClassCondition("q3_1_4", "y"),
                new ChosenAnswerDeviceClassCondition("q3_1_5", "y"),
                new ChosenAnswerDeviceClassCondition("q3_2", "y"),
                new ChosenAnswerDeviceClassCondition("q3_4_2_1", "y"),
                new ChosenAnswerDeviceClassCondition("q3_4_2_2", "y"),
                new ChosenAnswerDeviceClassCondition("q3_4_3", "y"),
                new ChosenAnswerDeviceClassCondition("q3_5_1_1", "q3_5_1_2"),
                new ChosenAnswerDeviceClassCondition("q3_5_2_1", "q3_5_2_1"),
                new ChosenAnswerDeviceClassCondition("q3_6_1", "y"),
                new ChosenAnswerDeviceClassCondition("q5_1", "y"),
                new ChosenAnswerDeviceClassCondition("q6", "y"),
                new ChosenAnswerDeviceClassCondition("q7_1", "y"),
                new ChosenAnswerDeviceClassCondition("q9_1", "q9_2"),
                new ChosenAnswerDeviceClassCondition("q10_1", "y"),
                new ChosenAnswerDeviceClassCondition("q10_2", "y"),
                new ChosenAnswerDeviceClassCondition(new Dictionary<string, string>
                {
                    { "q2_2_3_1", "n" },
                    { "q2_2_3_2", "n" },
                    { "q2_2_3_3", "n" },
                    { "q2_2_3_4", "n" },
                    { "q2_2_3_5", "n" },
                    { "q2_2_3_6", "n" },
                    { "q2_2_3_7", "n" },
                    { "q2_2_3_8", "n" },
                    { "q2_2_3_9", "n" },
                    { "q2_2_3_10", "n" },
                    { "q2_2_3_11", "n" },
                    { "q2_2_3_12", "n" },
                    { "q2_2_3_13", "n" }
                }),
            });
            var deviceClassThree = new DeviceClassThree(new[]
            {
                new ChosenAnswerDeviceClassCondition("q1_1_4_3", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_1_1", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_1_3", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_2_1", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_2_2", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_2_4", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_2", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_3", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_4", "q2_2_3_4_2"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_5", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_6", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_7", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_8", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_9", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_11", "y"),
                new ChosenAnswerDeviceClassCondition("q2_2_3_12", "y"),
                new ChosenAnswerDeviceClassCondition("q3_3", "y"),
                new ChosenAnswerDeviceClassCondition("q3_5_1_1", "q3_5_1_1"),
                new ChosenAnswerDeviceClassCondition("q4", "y"),
                new ChosenAnswerDeviceClassCondition("q8", "y"),
                new ChosenAnswerDeviceClassCondition("q9_1", "q9_1"),
                new ChosenAnswerDeviceClassCondition("q11_1", "y"),
                new ChosenAnswerDeviceClassCondition("q11_2", "y"),
                new ChosenAnswerDeviceClassCondition("q12", "y"),
            });
            var deviceClasses = new DeviceClass[]
            {
                deviceClassOne, 
                deviceClassTwoA,
                deviceClassTwoB,
                deviceClassThree
            };

            return new DeviceClassificationQuestionnaireTree(questions, deviceClasses);
        }
    }
}
