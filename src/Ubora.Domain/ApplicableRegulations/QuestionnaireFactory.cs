using System;
// ReSharper disable InconsistentNaming

namespace Ubora.Domain.ApplicableRegulations
{
    public static class QuestionnaireFactory
    {
        public static Questionnaire Create(Guid id)
        {
            var question5_1 = new Question("5.1. Does your device contain substances of animal origin?", nextQuestion: null);
            var question5 = new Question("5. Does your device come in contact to the human body (any kind of contact, brief or permanent, in any location of the human body)?", null, new[] { question5_1 });
            var question4_2 = new Question("4.2. Will it be sterilized in process?", question5);
            var question4_1_1 = new Question("4.1.1. By Ethilene Oxide?", question5);
            var question4_1_2 = new Question("4.1.2. By Irradiation?", question5);
            var question4_1_3 = new Question("4.1.3. By moist heat, steam?", question5);
            var question4_1_4 = new Question("4.1.4. By dry heat?", question5);
            var question4_1 = new Question("4.1. Will it be terminally sterilized?", question5, new[] { question4_1_1, question4_1_2, question4_1_3, question4_1_4 });
            var question4 = new Question("4. Is your device intended to be sterile?", question5, new[] { question4_1, question4_2 });
            var question3 = new Question("3. Is your device “implantable” and “active”?", question4);
            var question2_1_1 = new Question("2.1.1. Is the device containing SW intended to be part of a IT-network?", question3);
            var question2_1 = new Question("2.1. Is your device a SW or does it contain SW (applies also to firmware)?", question3, new[] { question2_1_1 });
            var question2 = new Question("2. Is your device “active” and its source of energy is electrical?", question3, new[] { question2_1 });
            var question1 = new Question("1. Is your device “implantable” and “not active”?", question2);

            return new Questionnaire(id, question1);
        }
    }
}
