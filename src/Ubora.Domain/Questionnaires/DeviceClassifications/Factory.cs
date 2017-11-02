using System.Collections.Generic;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class DeviceClassificationQuestionnaireTreeFactory
    {
        public static DeviceClassificationQuestionnaireTree CreateDeviceClassification()
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
                    new Answer("n", DeviceClass.One, "q3")
                }),
                new Question("q1_1_1", new[]
                {
                    new Answer("y", DeviceClass.TwoA, "q1_1_2"),
                    new Answer("n", "q1_1_2")
                }),
                new Question("q1_1_2", new[]
                {
                    new Answer("y", DeviceClass.TwoA, "q1_1_3"),
                    new Answer("n", "q1_1_3")
                }),
                new Question("q1_1_3", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q1_1_4"),
                    new Answer("n", "q1_1_4")
                }),
                new Question("q1_1_4", new[]
                {
                    new Answer("y", "q1_1_4_1"),
                    new Answer("n", "q1_1_5")
                }),
                new Question("q1_1_4_1", new[]
                {
                    new Answer("y", DeviceClass.TwoA, "q1_1_4_2"),
                    new Answer("n", "q1_1_4_2")
                }),
                new Question("q1_1_4_2", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q1_1_4_3"),
                    new Answer("n", "q1_1_4_3")
                }),
                new Question("q1_1_4_3", new[]
                {
                    new Answer("y", DeviceClass.Three, "q1_1_5"),
                    new Answer("n", "q1_1_5")
                }),
                new Question("q1_1_5", new[]
                {
                    new Answer("y", "q1_1_5_1"),
                    new Answer("n", "q3")
                }),
                new Question("q1_1_5_1", new[]
                {
                    new Answer("y", DeviceClass.One, "q1_1_5_2"),
                    new Answer("n", "q1_1_5_2")
                }),
                new Question("q1_1_5_2", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q1_1_5_3"),
                    new Answer("n", "q1_1_5_3")
                }),
                new Question("q1_1_5_3", new[]
                {
                    new Answer("y", DeviceClass.TwoA, "q3"),
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
                    new Answer("q2_1_1_1", DeviceClass.One, "q2_1_2"),
                    new Answer("q2_1_1_2", DeviceClass.One, "q2_1_2"),
                    new Answer("q2_1_1_3", DeviceClass.TwoA, "q2_1_2"),
                    new Answer("q2_1_1_4", DeviceClass.TwoA, "q2_1_2"),
                    new Answer("q2_1_1_5", DeviceClass.TwoB, "q2_1_2"),
                    new Answer("q2_1_1_6", DeviceClass.TwoB, "q2_1_2"),
                }),
                new Question("q2_1_2", new[]
                {
                    new Answer("q2_1_2", "q2_1_2_1"),
                    new Answer("q2_1_3", DeviceClass.TwoA, "q3")
                }),
                new Question("q2_1_2_1", new[]
                {
                    new Answer("y", DeviceClass.One, "q2_1_2_2"),
                    new Answer("n", "q2_1_2_2")
                }),
                new Question("q2_1_2_2", new[]
                {
                    new Answer("y", DeviceClass.One, "q2_1_2_3"),
                    new Answer("n", "q2_1_2_3")
                }),
                new Question("q2_1_2_3", new[]
                {
                    new Answer("y", DeviceClass.TwoA, "q2_1_2_4"),
                    new Answer("n", "q2_1_2_4")
                }),
                new Question("q2_1_2_4", new[]
                {
                    new Answer("y", DeviceClass.TwoA, "q2_1_2_5"),
                    new Answer("n", "q2_1_2_5")
                }),
                new Question("q2_1_2_5", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q2_1_2_6"),
                    new Answer("n", "q2_1_2_6")
                }),
                new Question("q2_1_2_6", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q3"),
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
                    new Answer("y", DeviceClass.Three, "q2_2_1_2"),
                    new Answer("n", "q2_2_1_2")
                }),
                new Question("q2_2_1_2", new[]
                {
                    new Answer("y", DeviceClass.One, "q2_2_1_3"),
                    new Answer("n", "q2_2_1_3")
                }),
                new Question("q2_2_1_3", new[]
                {
                    new Answer("y", DeviceClass.Three, "q2_2_1_4"),
                    new Answer("n", "q2_2_1_4")
                }),
                new Question("q2_2_1_4", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q2_2_1_5"),
                    new Answer("n", "q2_2_1_5")
                }),
                new Question("q2_2_1_5", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q2_2_1_6"),
                    new Answer("n", "q2_2_1_6")
                }),
                new Question("q2_2_1_6", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q3"),
                    new Answer("n", "q3")
                }),
                new Question("q2_2_2_1", new[]
                {
                    new Answer("y", DeviceClass.Three, "q2_2_2_2"),
                    new Answer("n", "q2_2_2_2")
                }),
                new Question("q2_2_2_2", new[]
                {
                    new Answer("y", DeviceClass.Three, "q2_2_2_3"),
                    new Answer("n", "q2_2_2_3")
                }),
                new Question("q2_2_2_3", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q2_2_2_4"),
                    new Answer("n", "q2_2_2_4")
                }),
                new Question("q2_2_2_4", new[]
                {
                    new Answer("y", DeviceClass.Three, "q2_2_2_5"),
                    new Answer("n", "q3")
                }),
                new Question("q2_2_2_5", new[]
                {
                    new Answer("q2_2_2_5_1", DeviceClass.TwoA, "q2_2_2_5_3"),
                    new Answer("q2_2_2_5_2", DeviceClass.TwoB, "q2_2_2_5_3"),
                    new Answer("n", "q2_2_2_5_3")
                }),
                new Question("q2_2_2_5_3", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q3"),
                    new Answer("n", "q3")
                }),
                new Question("q2_2_3_1", new[]
                {
                    new Answer("y", DeviceClass.TwoA, "q2_2_3_2"),
                    new Answer("n", "q2_2_3_2")
                }),
                new Question("q2_2_3_2", new[]
                {
                    new Answer("y", DeviceClass.Three, "q2_2_3_3"),
                    new Answer("n", "q2_2_3_3")
                }),
                new Question("q2_2_3_3", new[]
                {
                    new Answer("y", DeviceClass.Three, "q2_2_3_4"),
                    new Answer("n", "q2_2_3_4")
                }),
                new Question("q2_2_3_4", new[]
                {
                    new Answer("q2_2_3_4_1", DeviceClass.TwoB, "q2_2_3_5"),
                    new Answer("q2_2_3_4_2", DeviceClass.Three, "q2_2_3_5"),
                    new Answer("n", "q2_2_3_5")
                }),
                new Question("q2_2_3_5", new[]
                {
                    new Answer("y", DeviceClass.Three, "q2_2_3_6"),
                    new Answer("n", "q2_2_3_6")
                }),
                new Question("q2_2_3_6", new[]
                {
                    new Answer("y", DeviceClass.Three, "q2_2_3_7"),
                    new Answer("n", "q2_2_3_7")
                }),
                new Question("q2_2_3_7", new[]
                {
                    new Answer("y", DeviceClass.Three, "q2_2_3_8"),
                    new Answer("n", "q2_2_3_8")
                }),
                new Question("q2_2_3_8", new[]
                {
                    new Answer("y", DeviceClass.Three, "q2_2_3_9"),
                    new Answer("n", "q2_2_3_9")
                }),
                new Question("q2_2_3_9", new[]
                {
                    new Answer("y", DeviceClass.Three, "q2_2_3_10"),
                    new Answer("n", "q2_2_3_10")
                }),
                new Question("q2_2_3_10", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q2_2_3_11"),
                    new Answer("n", "q2_2_3_11")
                }),
                new Question("q2_2_3_11", new[]
                {
                    new Answer("y", DeviceClass.Three, "q2_2_3_12"),
                    new Answer("n", "q2_2_3_12")
                }),
                new Question("q2_2_3_12", new[]
                {
                    new Answer("y", DeviceClass.Three, "q2_2_3_13"),
                    new Answer("n", "q2_2_3_13")
                }),
                new Question("q2_2_3_13", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q3"),
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
                    new Answer("q3_1_1", DeviceClass.TwoB, "q3_1_3"),
                    new Answer("q3_1_2", DeviceClass.TwoA, "q3_1_3")
                }),
                new Question("q3_1_3", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q3_1_4"),
                    new Answer("n", "q3_1_4")
                }),
                new Question("q3_1_4", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q3_1_5"),
                    new Answer("n", "q3_1_5")
                }),
                new Question("q3_1_5", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q3_2"),
                    new Answer("n", "q3_2")
                }),
                new Question("q3_2", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q3_3"),
                    new Answer("n", "q3_3")
                }),
                new Question("q3_3", new[]
                {
                    new Answer("y", DeviceClass.Three, "q3_4"),
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
                    new Answer("y", DeviceClass.One, "q3_4_1_2"),
                    new Answer("n", "q3_4_2")
                }),
                new Question("q3_4_1_2", new[]
                {
                    new Answer("y", DeviceClass.TwoA, "q3_4_1_3"),
                    new Answer("n", "q3_4_3")
                }),
                new Question("q3_4_1_3", new[]
                {
                    new Answer("y", DeviceClass.TwoA, "q3_4_2"),
                    new Answer("n", "q3_4_2")
                }),
                new Question("q3_4_2", new[]
                {
                    new Answer("y", "q3_4_2_1"),
                    new Answer("n", "q3_4_3")
                }),
                new Question("q3_4_2_1", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q3_4_2_2"),
                    new Answer("n", "q3_4_2_2")
                }),
                new Question("q3_4_2_2", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q3_4_2_3"),
                    new Answer("n", "q3_4_3")
                }),
                new Question("q3_4_2_3", new[]
                {
                    new Answer("y", DeviceClass.TwoA, "q3_4_3"),
                    new Answer("n", "q3_4_3")
                }),
                new Question("q3_4_3", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q3_5"),
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
                    new Answer("q3_5_1_1", DeviceClass.Three, "q3_5_2"),
                    new Answer("q3_5_1_2", DeviceClass.TwoB, "q3_5_2"),
                    new Answer("n", DeviceClass.TwoA, "q3_5_2")
                }),
                new Question("q3_5_2", new[]
                {
                    new Answer("y", "q3_5_2_1"),
                    new Answer("n", "q3_6")
                }),
                new Question("q3_5_2_1", new[]
                {
                    new Answer("q3_5_2_1", DeviceClass.TwoB, "q3_6"),
                    new Answer("q3_5_2_2", DeviceClass.TwoA, "q3_6")
                }),
                new Question("q3_6", new[]
                {
                    new Answer("y", "q3_6_1"),
                    new Answer("n", "q4")
                }),
                new Question("q3_6_1", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q4"),
                    new Answer("n", DeviceClass.TwoA, "q4")
                }),
                new Question("q4", new[]
                {
                    new Answer("y", DeviceClass.Three, "q5"),
                    new Answer("n", "q5")
                }),
                new Question("q5", new[]
                {
                    new Answer("y", "q5_1"),
                    new Answer("n", "q6")
                }),
                new Question("q5_1", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q6"),
                    new Answer("n", DeviceClass.TwoA, "q6")
                }),
                new Question("q6", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q7"),
                    new Answer("n", "q7")
                }),
                new Question("q7", new[]
                {
                    new Answer("y", "q7_1"),
                    new Answer("n", "q8")
                }),
                new Question("q7_1", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q7_3"),
                    new Answer("n", DeviceClass.TwoA, "q7_3")
                }),
                new Question("q7_3", new[]
                {
                    new Answer("y", DeviceClass.TwoA, "q8"),
                    new Answer("n", "q8")
                }),
                new Question("q8", new[]
                {
                    new Answer("y", DeviceClass.Three, "q9"),
                    new Answer("n", "q9")
                }),
                new Question("q9",new[]
                {
                    new Answer("y", "q9_1"),
                    new Answer("n", "q10")
                }),
                new Question("q9_1", new[]
                {
                    new Answer("q9_1", DeviceClass.Three, "q10"),
                    new Answer("q9_2", DeviceClass.TwoB, "q10"),
                    new Answer("q9_3", DeviceClass.TwoA, "q10")
                }),
                new Question("q10", new[]
                {
                    new Answer("y", "q10_1"),
                    new Answer("n", "q11")
                }),
                new Question("q10_1", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q10_2"),
                    new Answer("n", "q10_2")
                }),
                new Question("q10_2", new[]
                {
                    new Answer("y", DeviceClass.TwoB, "q11"),
                    new Answer("n", "q11")
                }),
                new Question("q11", new[]
                {
                    new Answer("y", "q11_1"),
                    new Answer("n", "q12")
                }),
                new Question("q11_1", new[]
                {
                    new Answer("y", DeviceClass.Three, "q11_2"),
                    new Answer("n", "q11_2")
                }),
                new Question("q11_2", new[]
                {
                    new Answer("y", DeviceClass.Three, "q11_3"),
                    new Answer("n", "q11_3")
                }),
                new Question("q11_3",new[]
                {
                    new Answer("y", DeviceClass.TwoA, "q12"),
                    new Answer("n", "q12")
                }),
                new Question("q12", new []
                {
                    new Answer("y", DeviceClass.Three, null),
                    new Answer("n", null)
                }),
            };

            //throw new InvalidOperationException();

            var conditions = new ChosenAnswerDeviceClassCondition[]
              {
                //new ChosenAnswerDeviceClassCondition("q1_1", "n", DeviceClass.One),
                //new ChosenAnswerDeviceClassCondition("q1_1_1", "y", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q1_1_2", "y", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q1_1_3", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q1_1_4_1", "y", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q1_1_4_2", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q1_1_4_3", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q1_1_5_1", "y", DeviceClass.One),
                //new ChosenAnswerDeviceClassCondition("q1_1_5_2", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q1_1_5_3", "y", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q2_1_1_1", "q2_1_1_1", DeviceClass.One),
                //new ChosenAnswerDeviceClassCondition("q2_1_1_1", "q2_1_1_2", DeviceClass.One),
                //new ChosenAnswerDeviceClassCondition("q2_1_1_1", "q2_1_1_3", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q2_1_1_1", "q2_1_1_4", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q2_1_1_1", "q2_1_1_5", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q2_1_1_1", "q2_1_1_6", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q2_1_2", "q2_1_3", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q2_1_2_1", "y", DeviceClass.One),
                //new ChosenAnswerDeviceClassCondition("q2_1_2_2", "y", DeviceClass.One),
                //new ChosenAnswerDeviceClassCondition("q2_1_2_3", "y", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q2_1_2_4", "y", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q2_1_2_5", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q2_1_2_6", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q2_2_1_1", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_1_2", "y", DeviceClass.One),
                //new ChosenAnswerDeviceClassCondition("q2_2_1_3", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_1_4", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q2_2_1_5", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q2_2_1_6", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q2_2_2_1", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_2_2", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_2_3", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q2_2_2_4", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_2_5", "q2_2_2_5_1", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q2_2_2_5", "q2_2_2_5_2", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q2_2_2_5_3", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_1", "y", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_2", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_3", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_4", "q2_2_3_4_1", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_4", "q2_2_3_4_2", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_5", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_6", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_7", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_8", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_9", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_10", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_11", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_12", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q2_2_3_13", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q3_1_1", "q3_1_1", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q3_1_1", "q3_1_2", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q3_1_3", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q3_1_4", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q3_1_5", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q3_2", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q3_3", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q3_4_1_1", "y", DeviceClass.One),
                //new ChosenAnswerDeviceClassCondition("q3_4_1_2", "y", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q3_4_1_3", "y", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q3_4_2_1", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q3_4_2_2", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q3_4_2_3", "y", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q3_4_3", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q3_5_1_1", "q3_5_1_1", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q3_5_1_1", "q3_5_1_2", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q3_5_1_1", "q3_5_1_3", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q3_5_2_1", "q3_5_2_1", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q3_5_2_1", "q3_5_2_2", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q3_6_1", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q3_6_1", "n", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q4", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q5_1", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q5_1", "n", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q6", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q7_1", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q7_1", "n", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q7_3", "y", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q8", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q9_1", "q9_1", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q9_1", "q9_2", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q9_1", "q9_3", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q10_1", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q10_2", "y", DeviceClass.TwoB),
                //new ChosenAnswerDeviceClassCondition("q11_1", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q11_2", "y", DeviceClass.Three),
                //new ChosenAnswerDeviceClassCondition("q11_3", "y", DeviceClass.TwoA),
                //new ChosenAnswerDeviceClassCondition("q12", "y", DeviceClass.Three),

                new ChosenAnswerDeviceClassCondition("q1_1_5_4", new Dictionary<string, string>
                {
                    { "q1_1_5_1", "n" },
                    { "q1_1_5_2", "n" },
                    { "q1_1_5_3", "n" }
                }, DeviceClass.One ),

                new ChosenAnswerDeviceClassCondition("q1_1_6", new Dictionary<string, string>
                {
                    { "q1_1_1", "n" },
                    { "q1_1_2", "n" },
                    { "q1_1_3", "n" },
                    { "q1_1_4", "n" },
                    { "q1_1_5", "n" }
                }, DeviceClass.One ),

                new ChosenAnswerDeviceClassCondition("q2_2_1_7", new Dictionary<string, string>
                {
                    { "q2_2_1_1", "n" },
                    { "q2_2_1_2", "n" },
                    { "q2_2_1_3", "n" },
                    { "q2_2_1_4", "n" },
                    { "q2_2_1_5", "n" },
                    { "q2_2_1_6", "n" }
                }, DeviceClass.TwoA ),

                new ChosenAnswerDeviceClassCondition("q2_2_2_6", new Dictionary<string, string>
                {
                    { "q2_2_2_1", "n" },
                    { "q2_2_2_2", "n" },
                    { "q2_2_2_3", "n" },
                    { "q2_2_2_4", "n" },
                    { "q2_2_2_5", "n" }
                }, DeviceClass.TwoA ),

                new ChosenAnswerDeviceClassCondition("q2_2_3_14", new Dictionary<string, string>
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
                }, DeviceClass.TwoB ),

                new ChosenAnswerDeviceClassCondition("q3_4_4", new Dictionary<string, string>
                {
                    { "q3_4_1", "n" },
                    { "q3_4_2", "n" },
                    { "q3_4_3", "n" }
                }, DeviceClass.TwoA ),

                new ChosenAnswerDeviceClassCondition("q3_5_3", new Dictionary<string, string>
                {
                    { "q3_5_1", "n" },
                    { "q3_5_2", "n" }
                }, DeviceClass.One ),

                new ChosenAnswerDeviceClassCondition("q3_7", new Dictionary<string, string>
                {
                    { "q3_1", "n" },
                    { "q3_2", "n" },
                    { "q3_3", "n" },
                    { "q3_4", "n" },
                    { "q3_5", "n" },
                    { "q3_6", "n" }
                }, DeviceClass.TwoA ),

                new ChosenAnswerDeviceClassCondition("q10_3", new Dictionary<string, string>
                {
                    { "q10_1", "n" },
                    { "q10_2", "n" }
                }, DeviceClass.TwoA ),

                new ChosenAnswerDeviceClassCondition("q11_4", new Dictionary<string, string>
                {
                    { "q11_1", "n" },
                    { "q11_2", "n" },
                    { "q11_3", "n" }
                }, DeviceClass.TwoA )
              };

            return new DeviceClassificationQuestionnaireTree(questions, conditions);
        }
    }
}
