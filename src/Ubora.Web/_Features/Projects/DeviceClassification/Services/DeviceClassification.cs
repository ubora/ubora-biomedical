using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubora.Web._Features.Projects.DeviceClassification.Services
{
    public class DeviceClassification : IDeviceClassification
    {

        private static List<MainQuestion> _mainQuestions = new List<MainQuestion>();
        private static List<SubQuestion> _subQuestions = new List<SubQuestion>();
        private static List<Classification> _classifications = new List<Classification>();

        static DeviceClassification()
        {
            var question3 = new MainQuestion(Guid.NewGuid(), "Is your device ACTIVE?", null);

            var question2 = new MainQuestion(Guid.NewGuid(), "Is your device INVASIVE?", question3.Id);

            var question1 = new MainQuestion(Guid.NewGuid(), "Is your device NON INVASIVE?", question2.Id);

            var question1_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for channelling or storing blood, body liquids, cells or tissues, liquids or gases for the purpose of eventual infusion, administration or introduction into the body?", mainQuestionId: question1.Id, parentQuestionId: question1.Id);

            var question1_1_1 = new SubQuestion(id: Guid.NewGuid(), text: "May it be connected to an active medical device in class IIa or a higher class?", mainQuestionId: question1.Id, parentQuestionId: question1_1.Id);
            var question1_1_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for use for storing or channelling blood or other body liquids or for storing organs, parts of organs or body cells and tissues, but it is not a blood bag?", mainQuestionId: question1.Id, parentQuestionId: question1_1.Id);
            var question1_1_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it a blood bag?", mainQuestionId: question1.Id, parentQuestionId: question1_1.Id);
            var question1_1_4 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for modifying the biological or chemical composition of human tissues or cells, blood, other body liquids or other liquids intended for implantation or administration into the body? ", mainQuestionId: question1.Id, parentQuestionId: question1_1.Id);

            var question1_1_4_1 = new SubQuestion(id: Guid.NewGuid(), text: "Does the treatment consist of filtration, centrifugation or exchanges of gas, heat?", mainQuestionId: question1.Id, parentQuestionId: question1_1_4.Id);
            var question1_1_4_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is the treatment other than filtration, centrifugation or exchanges of gas, heat?", mainQuestionId: question1.Id, parentQuestionId: question1_1_4.Id);
            var question1_1_4_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is your device a substance or a mixture of substances intended to be used in vitro in direct contact with human cells, tissues or organs taken off from the human body or with human embryos before their implantation or administration into the body?", mainQuestionId: question1.Id, parentQuestionId: question1_1_4.Id);

            var question1_1_5 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended come into contact with injured skin or mucous membrane?", mainQuestionId: question1.Id, parentQuestionId: question1_1.Id);

            var question1_1_5_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to be used as a mechanical barrier, for compression or for absorption of exudates?", mainQuestionId: question1.Id, parentQuestionId: question1_1_5.Id);
            var question1_1_5_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to be used principally for injuries to skin which have breached the dermis or mucous membrane and can only heal by secondary intent?", mainQuestionId: question1.Id, parentQuestionId: question1_1_5.Id);
            var question1_1_5_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it principally intended to manage the micro-environment of injured skin or mucous membrane?", mainQuestionId: question1.Id, parentQuestionId: question1_1_5.Id);
            var question1_1_5_4 = new SubQuestion(id: Guid.NewGuid(), text: "None of the above apply?", mainQuestionId: question1.Id, parentQuestionId: question1_1_5.Id);

            var question1_1_6 = new SubQuestion(id: Guid.NewGuid(), text: "None of the above apply?", mainQuestionId: question1.Id, parentQuestionId: question1_1.Id);

            var question2_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it SURGICALLY NON-INVASIVE?", mainQuestionId: question2.Id, parentQuestionId: question2.Id);
            var question2_1_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for connection to any active medical device?", mainQuestionId: question2.Id, parentQuestionId: question2_1.Id);
            var question2_1_1_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for transient use?", mainQuestionId: question2.Id, parentQuestionId: question2_1_1.Id);
            var question2_1_1_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for short-term use  only in the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity?", mainQuestionId: question2.Id, parentQuestionId: question2_1_1.Id);
            var question2_1_1_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for short-term use in any other site than the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity?", mainQuestionId: question2.Id, parentQuestionId: question2_1_1.Id);
            var question2_1_1_4 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for long-term use only in the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity and it not liable to be absorbed by the mucous membrane?", mainQuestionId: question2.Id, parentQuestionId: question2_1_1.Id);
            var question2_1_1_5 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for long-term use only in the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity but it is liable to be absorbed by the mucous membrane?", mainQuestionId: question2.Id, parentQuestionId: question2_1_1.Id);
            var question2_1_1_6 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for long-term use in any other site than the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity?", mainQuestionId: question2.Id, parentQuestionId: question2_1_1.Id);

            var question2_1_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for connection to a class I active medical device?", mainQuestionId: question2.Id, parentQuestionId: question2_1.Id);
            var question2_1_2_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for transient use?", mainQuestionId: question2.Id, parentQuestionId: question2_1_2.Id);
            var question2_1_2_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for short-term use only in the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity?", mainQuestionId: question2.Id, parentQuestionId: question2_1_2.Id);
            var question2_1_2_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for short-term use in any other site than the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity?", mainQuestionId: question2.Id, parentQuestionId: question2_1_2.Id);
            var question2_1_2_4 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for long-term use only in the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity and it not liable to be absorbed by the mucous membrane?", mainQuestionId: question2.Id, parentQuestionId: question2_1_2.Id);
            var question2_1_2_5 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for long-term use only in the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity but it is liable to be absorbed by the mucous membrane?", mainQuestionId: question2.Id, parentQuestionId: question2_1_2.Id);
            var question2_1_2_6 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for long-term use in any other site than the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity?", mainQuestionId: question2.Id, parentQuestionId: question2_1_2.Id);

            var question2_1_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for connection to a class IIa or higher class active medical device?", mainQuestionId: question2.Id, parentQuestionId: question2_1.Id);

            var question2_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it SURGICALLY INVASIVE?", mainQuestionId: question2.Id, parentQuestionId: question2.Id);
            var question2_2_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it for TRANSIENT use?", mainQuestionId: question2.Id, parentQuestionId: question2_2.Id);
            var question2_2_1_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended specifically to control, diagnose, monitor or correct a defect of the heart or of the central circulatory system through direct contact with those parts of the body?", mainQuestionId: question2.Id, parentQuestionId: question2_2_1.Id);
            var question2_2_1_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it a reusable surgical instrument?", mainQuestionId: question2.Id, parentQuestionId: question2_2_1.Id);
            var question2_2_1_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended specifically for use in direct contact with the heart or central circulatory system or the central nervous system?", mainQuestionId: question2.Id, parentQuestionId: question2_2_1.Id);
            var question2_2_1_4 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to supply energy in the form of ionising radiation?", mainQuestionId: question2.Id, parentQuestionId: question2_2_1.Id);
            var question2_2_1_5 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to have a biological effect or be wholly or mainly absorbed?", mainQuestionId: question2.Id, parentQuestionId: question2_2_1.Id);
            var question2_2_1_6 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to administer medicinal products by means of a delivery system in a manner that is potentially hazardous?", mainQuestionId: question2.Id, parentQuestionId: question2_2_1.Id);
            var question2_2_1_7 = new SubQuestion(id: Guid.NewGuid(), text: "None of the above apply?", mainQuestionId: question2.Id, parentQuestionId: question2_2_1.Id);

            var question2_2_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it for SHORT TERM use?", mainQuestionId: question2.Id, parentQuestionId: question2_2.Id);
            var question2_2_2_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended specifically to control, diagnose, monitor or correct a defect of the heart or of the central circulatory system through direct contact with those parts of the body?", mainQuestionId: question2.Id, parentQuestionId: question2_2_2.Id);
            var question2_2_2_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended specifically for use in direct contact with the heart or central circulatory system or the central nervous system?", mainQuestionId: question2.Id, parentQuestionId: question2_2_2.Id);
            var question2_2_2_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to have a biological effect or are wholly or mainly absorbed?", mainQuestionId: question2.Id, parentQuestionId: question2_2_2.Id);
            var question2_2_2_4 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to have a biological effect or be wholly or mainly absorbed?", mainQuestionId: question2.Id, parentQuestionId: question2_2_2.Id);
            var question2_2_2_5 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to undergo chemical change in the body?", mainQuestionId: question2.Id, parentQuestionId: question2_2_2.Id);
            var question2_2_2_5_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it in the teeth?", mainQuestionId: question2.Id, parentQuestionId: question2_2_2_5.Id);
            var question2_2_2_5_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it in any part of the body other than the teeth?", mainQuestionId: question2.Id, parentQuestionId: question2_2_2_5.Id);
            var question2_2_2_5_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to administer medicines?", mainQuestionId: question2.Id, parentQuestionId: question2_2_2_5.Id);
            var question2_2_2_6 = new SubQuestion(id: Guid.NewGuid(), text: "None of the above apply?", mainQuestionId: question2.Id, parentQuestionId: question2_2_2.Id);




            _mainQuestions.Add(question1);
            _subQuestions.Add(question1_1);
            _subQuestions.Add(question1_1_1);
            _subQuestions.Add(question1_1_2);
            _subQuestions.Add(question1_1_3);
            _subQuestions.Add(question1_1_4);
            _subQuestions.Add(question1_1_4_1);
            _subQuestions.Add(question1_1_4_2);
            _subQuestions.Add(question1_1_4_3);
            _subQuestions.Add(question1_1_5);
            _subQuestions.Add(question1_1_5_1);
            _subQuestions.Add(question1_1_5_2);
            _subQuestions.Add(question1_1_5_3);
            _subQuestions.Add(question1_1_5_4);
            _subQuestions.Add(question1_1_6);
            _subQuestions.Add(question2_1);
            _subQuestions.Add(question2_1_1);
            _subQuestions.Add(question2_1_1_1);
            _subQuestions.Add(question2_1_1_2);
            _subQuestions.Add(question2_1_1_3);
            _subQuestions.Add(question2_1_1_4);
            _subQuestions.Add(question2_1_1_5);
            _subQuestions.Add(question2_1_1_6);
            _subQuestions.Add(question2_1_2);
            _subQuestions.Add(question2_1_2_1);
            _subQuestions.Add(question2_1_2_2);
            _subQuestions.Add(question2_1_2_3);
            _subQuestions.Add(question2_1_2_4);
            _subQuestions.Add(question2_1_2_5);
            _subQuestions.Add(question2_1_2_6);
            _subQuestions.Add(question2_1_3);
            _subQuestions.Add(question2_2);
            _subQuestions.Add(question2_2_1);
            _subQuestions.Add(question2_2_1_1);
            _subQuestions.Add(question2_2_1_2);
            _subQuestions.Add(question2_2_1_3);
            _subQuestions.Add(question2_2_1_4);
            _subQuestions.Add(question2_2_1_5);
            _subQuestions.Add(question2_2_1_6);
            _subQuestions.Add(question2_2_1_7);
            _subQuestions.Add(question2_2_2);
            _subQuestions.Add(question2_2_2_1);
            _subQuestions.Add(question2_2_2_2);
            _subQuestions.Add(question2_2_2_3);
            _subQuestions.Add(question2_2_2_4);
            _subQuestions.Add(question2_2_2_5);
            _subQuestions.Add(question2_2_2_5_1);
            _subQuestions.Add(question2_2_2_5_2);
            _subQuestions.Add(question2_2_2_5_3);
            _subQuestions.Add(question2_2_2_6);





            _subQuestions.Add(question1_1_6);
            _subQuestions.Add(question1_1_6);

            _mainQuestions.Add(question2);
            _mainQuestions.Add(question3);

            _classifications.Add(new Classification(Guid.NewGuid(), "I", new List<Guid> { question1_1_5_1.Id, question1_1_6.Id, question2_1_1_1.Id, question2_1_1_1.Id }));
            _classifications.Add(new Classification(Guid.NewGuid(), "IIa", new List<Guid> { question1_1_1.Id, question1_1_2.Id, question1_1_4_1.Id, question1_1_5_3.Id, question1_1_5_4.Id }));
            _classifications.Add(new Classification(Guid.NewGuid(), "IIb", new List<Guid> { question1_1_3.Id, question1_1_4_2.Id, question1_1_5_2.Id }));
            _classifications.Add(new Classification(Guid.NewGuid(), "III", new List<Guid> { question1_1_4_3.Id }));
        }

        public MainQuestion GetDefaultQuestion()
        {
            return _mainQuestions.First();
        }

        public MainQuestion GetNextMainQuestion(Guid currentQuestionId)
        {
            var currentMainQuestion = _mainQuestions.First(x => x.Id == currentQuestionId);

            if (currentMainQuestion.IsLastMainQuestion)
            {
                return null;
            }

            return _mainQuestions.Where(x => x.Id == currentMainQuestion.NextMainQuestion).First();
        }

        public MainQuestion GetMainQuestion(Guid questionId)
        {
            return _mainQuestions.Single(x => x.Id == questionId);
        }

        public IReadOnlyCollection<SubQuestion> GetSubQuestions(Guid parentQuestionId)
        {
            var subQuestions = _subQuestions
                .Where(x => x.ParentQuestionId == parentQuestionId);

            if (subQuestions == null || subQuestions.Count() == 0)
            {
                return null;
            }

            return subQuestions.ToArray();
        }

        public Classification GetClassification(Guid questionId)
        {
            var classification = _classifications.First(x => x.QuestionIds.Contains(questionId));

            return classification;
        }
    }

    public interface IDeviceClassification
    {
        MainQuestion GetDefaultQuestion();
        MainQuestion GetNextMainQuestion(Guid currentQuestionId);
        MainQuestion GetMainQuestion(Guid questionId);
        IReadOnlyCollection<SubQuestion> GetSubQuestions(Guid parentQuestionId);
        Classification GetClassification(Guid questionId);
    }
}