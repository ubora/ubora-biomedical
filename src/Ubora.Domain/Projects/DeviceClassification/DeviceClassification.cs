using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class DeviceClassification : IDeviceClassification
    {
        public Guid Id { get; set; }

        [JsonProperty(nameof(MainQuestions))]
        private List<MainQuestion> MainQuestions { get; set; } = new List<MainQuestion>();

        [JsonProperty(nameof(SubQuestions))]
        private List<SubQuestion> SubQuestions { get; set; } = new List<SubQuestion>();

        [JsonProperty(nameof(Classifications))]
        private List<Classification> Classifications { get; set; } = new List<Classification>();

        public DeviceClassification()
        {

        }

        protected DeviceClassification(List<MainQuestion> mainQuestions, List<SubQuestion> subQuestions, List<Classification> classifications)
        {
            MainQuestions = mainQuestions;
            SubQuestions = subQuestions;
            Classifications = classifications;
        }

        public void CreateNew()
        {
            var mainQuestion_12 = new MainQuestion(Guid.NewGuid(), "Is it an active therapeutic devices with an integrated or incorporated diagnostic function that includes an integrated or incorporated diagnostic function which significantly determines the patient management by the device (example: closed loop systems or automated external defibrillators)?", null);
            var mainQuestion_11 = new MainQuestion(Guid.NewGuid(), "Is it composed of substances or of combinations of substances that are intended to be introduced into the human body via a body orifice or applied to the skin and that are absorbed by or locally dispersed in the human body?", mainQuestion_12.Id);
            var mainQuestion_10 = new MainQuestion(Guid.NewGuid(), "Is it intended to administer medicinal products by inhalation, being an invasive device with respect to body orifices, other than surgically invasive devices?", mainQuestion_11.Id);
            var mainQuestion_9 = new MainQuestion(Guid.NewGuid(), "Does it incorporate or contain nanomaterials?", mainQuestion_10.Id);

            var mainQuestion_8 = new MainQuestion(Guid.NewGuid(), "Is it manufactured utilising tissues or cells of human or animal origin, or their derivatives, which are non-viable or rendered non-viable?", mainQuestion_9.Id);

            var mainQuestion_7 = new MainQuestion(Guid.NewGuid(), "Is it intended specifically to be used for disinfecting or sterilising medical devices that are not contact lenses?", mainQuestion_8.Id);
            var mainQuestion_6 = new MainQuestion(Guid.NewGuid(), "Is it intended specifically to be used for disinfecting, cleaning, rinsing or, where appropriate, hydrating contact lenses?", mainQuestion_7.Id);
            var mainQuestion_5 = new MainQuestion(Guid.NewGuid(), "Is it used for contraception or prevention of the transmission of sexually transmitted diseases?", mainQuestion_6.Id);

            var mainQuestion_4 = new MainQuestion(Guid.NewGuid(), "Does your device incorporate a medicinal product with an ancillary function?", mainQuestion_5.Id);

            var mainQuestion_2 = new MainQuestion(Guid.NewGuid(), "Is your device ACTIVE?", mainQuestion_4.Id);

            var mainQuestion_1 = new MainQuestion(Guid.NewGuid(), "Is your device INVASIVE or NON INVASIVE?", mainQuestion_2.Id);

            var question1 = new SubQuestion(Guid.NewGuid(), "Is your device INVASIVE?", mainQuestion_1.Id, mainQuestion_1.Id);
            var question2 = new SubQuestion(Guid.NewGuid(), "Is your device NON INVASIVE?", mainQuestion_1.Id, mainQuestion_1.Id);

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

            var question2_2_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it implantable or LONG TERM use?", mainQuestionId: question2.Id, parentQuestionId: question2_2.Id);

            var question2_2_3_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to be placed in the teeth?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);
            var question2_2_3_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for use in direct contact with the heart or central circulatory system or the central nervous system?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);
            var question2_2_3_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to have a biological effect or be wholly or mainly absorbed?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);
            var question2_2_3_4 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to undergo chemical change in the body?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);

            var question2_2_3_4_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it in the teeth?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3_4.Id);
            var question2_2_3_4_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it in any part of the body other than the teeth?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3_4.Id);


            var question2_2_3_5 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to administer medicines?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);
            var question2_2_3_6 = new SubQuestion(id: Guid.NewGuid(), text: "Is it an active implantable device or its accessory?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);
            var question2_2_3_7 = new SubQuestion(id: Guid.NewGuid(), text: "Is it a breast implant?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);
            var question2_2_3_8 = new SubQuestion(id: Guid.NewGuid(), text: "Is it a surgical mesh?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);
            var question2_2_3_9 = new SubQuestion(id: Guid.NewGuid(), text: "Is it a total or partial joint replacement?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);
            var question2_2_3_10 = new SubQuestion(id: Guid.NewGuid(), text: "Is it an ancillary component of a total or partial joint replacement such as screws, wedges, plates and instruments?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);
            var question2_2_3_11 = new SubQuestion(id: Guid.NewGuid(), text: "Is it a spinal disc replacement implant?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);
            var question2_2_3_12 = new SubQuestion(id: Guid.NewGuid(), text: "Is it an implantable device that come into contact with the spinal column?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);
            var question2_2_3_13 = new SubQuestion(id: Guid.NewGuid(), text: "Is it an ancillary component of a spinal disc replacement or other device that comes in contact with the spinal column such as screws, wedges, plates and instruments?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);
            var question2_2_3_14 = new SubQuestion(id: Guid.NewGuid(), text: "None of the above apply?", mainQuestionId: question2.Id, parentQuestionId: question2_2_3.Id);

            var question3_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it therapeutic?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: mainQuestion_2.Id);
            var question3_1_1 = new SubQuestion(id: Guid.NewGuid(), text: "Does it administer energy to or exchange energy with the human body in a potentially hazardous way (consider nature of energy, density of energy, site of the body)?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_1.Id);
            var question3_1_2 = new SubQuestion(id: Guid.NewGuid(), text: "Does it administer energy to or exchange energy with the human body in a NON hazardous way?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_1.Id);
            var question3_1_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to emit ionizing radiation?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_1.Id);
            var question3_1_4 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for therapeutic radiology, including interventional radiology devices and devices which control or monitor such devices, or which directly influence their performance?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_1.Id);
            var question3_1_5 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to control or monitor a device that emits ionizing radiation, or does it directly influence the performance a device that emits ionizing radiation?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_1.Id);

            var question3_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to control or monitor the performance of active therapeutic class IIb devices, or intended directly to influence the performance of such devices?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: mainQuestion_2.Id);
            var question3_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for controlling, monitoring or directly influencing the performance of active implantable devices?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: mainQuestion_2.Id);
            var question3_4 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended for diagnosis and monitoring?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: mainQuestion_2.Id);
            var question3_4_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to supply energy which will be absorbed by the human body?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_4.Id);
            var question3_4_1_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it only intended to intended to illuminate the patient's body, in the visible spectrum?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_4_1.Id);
            var question3_4_1_2 = new SubQuestion(id: Guid.NewGuid(), text: "Any other case?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_4_1.Id);
            var question3_4_1_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to image in vivo distribution of radiopharmaceuticals?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_4_1.Id);

            var question3_4_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to allow direct diagnosis or monitoring of vital physiological processes?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_4.Id);
            var question3_4_2_1 = new SubQuestion(id: Guid.NewGuid(), text: "Are the vital physiological processes such as the patient may be in immediate danger if the vary (example: variations in cardiac performance, respiration, activity of the central nervous system)?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_4_2.Id);
            var question3_4_2_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is the diagnosis and monitoring done in a clinical situation while the patient is in immediate danger?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_4_2.Id);
            var question3_4_2_3 = new SubQuestion(id: Guid.NewGuid(), text: "Any other case?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_4_2.Id);

            var question3_4_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to emit ionizing radiation for diagnostics?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_4.Id);
            var question3_4_4 = new SubQuestion(id: Guid.NewGuid(), text: "None of the above apply?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_4.Id);

            var question3_5 = new SubQuestion(id: Guid.NewGuid(), text: "Is it a Software?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: mainQuestion_2.Id);
            var question3_5_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to provide information which is used to take decisions with diagnosis or therapeutic purposes?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_5.Id);
            var question3_5_1_1 = new SubQuestion(id: Guid.NewGuid(), text: "Does such decision have an impact that may cause death or an irreversible deterioration of a person's state of health?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_5_1.Id);
            var question3_5_1_2 = new SubQuestion(id: Guid.NewGuid(), text: "Does such decision have an impact that may cause a serious deterioration of a person's state of health or a surgical intervention?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_5_1.Id);
            var question3_5_1_3 = new SubQuestion(id: Guid.NewGuid(), text: "None of the above apply?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_5_1.Id);

            var question3_5_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to monitor physiological processes?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_5.Id);
            var question3_5_2_1 = new SubQuestion(id: Guid.NewGuid(), text: "Does it monitor vital physiological parameters, where the nature of variations of those parameters is such that it could result in immediate danger to the patient?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_5_2.Id);
            var question3_5_2_2 = new SubQuestion(id: Guid.NewGuid(), text: "Does it monitor non vital physiological parameters, so that the nature of variations of those parameters is such that it could never and in no cases result in immediate danger to the patient?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_5_2.Id);

            var question3_5_3 = new SubQuestion(id: Guid.NewGuid(), text: "None of the above apply?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_5.Id);

            var question3_6 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to administer and/or remove medicinal products, body liquids or other substances to or from the body?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: mainQuestion_2.Id);
            var question3_6_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is this done in a manner that is potentially hazardous (taking account of the nature of the substances involved, of the part of the body concerned and of the mode of application)?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_6.Id);
            var question3_6_2 = new SubQuestion(id: Guid.NewGuid(), text: "None of the above apply?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: question3_6.Id);

            var question3_7 = new SubQuestion(id: Guid.NewGuid(), text: "None of the above apply?", mainQuestionId: mainQuestion_2.Id, parentQuestionId: mainQuestion_2.Id);

            var question5_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it implantable or long term invasive?", mainQuestionId: mainQuestion_5.Id, parentQuestionId: mainQuestion_5.Id);
            var question5_2 = new SubQuestion(id: Guid.NewGuid(), text: "None of the above apply?", mainQuestionId: mainQuestion_5.Id, parentQuestionId: mainQuestion_5.Id);

            var question7_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it a disinfecting solution or washer-disinfector, intended specifically to be used for disinfecting invasive devices, as the end point of processing?", mainQuestionId: mainQuestion_7.Id, parentQuestionId: mainQuestion_7.Id);
            var question7_2 = new SubQuestion(id: Guid.NewGuid(), text: "Non of the above apply?", mainQuestionId: mainQuestion_7.Id, parentQuestionId: mainQuestion_7.Id);
            var question7_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it specifically intended for recording of diagnostic images generated by X-ray radiation?", mainQuestionId: mainQuestion_7.Id, parentQuestionId: mainQuestion_7.Id);

            var question9_1 = new SubQuestion(id: Guid.NewGuid(), text: "Does it present a high or medium potential for internal exposure?", mainQuestionId: mainQuestion_9.Id, parentQuestionId: mainQuestion_9.Id);
            var question9_2 = new SubQuestion(id: Guid.NewGuid(), text: "Does it present a low potential for internal exposure?", mainQuestionId: mainQuestion_9.Id, parentQuestionId: mainQuestion_9.Id);
            var question9_3 = new SubQuestion(id: Guid.NewGuid(), text: "Does it present a negligible potential for internal exposure?", mainQuestionId: mainQuestion_9.Id, parentQuestionId: mainQuestion_9.Id);

            var question10_1 = new SubQuestion(id: Guid.NewGuid(), text: "Does its mode of action have an essential impact on the efficacy and safety of the administered medicinal product?", mainQuestionId: mainQuestion_10.Id, parentQuestionId: mainQuestion_10.Id);
            var question10_2 = new SubQuestion(id: Guid.NewGuid(), text: "Is it intended to treat life-threatening conditions?", mainQuestionId: mainQuestion_10.Id, parentQuestionId: mainQuestion_10.Id);
            var question10_3 = new SubQuestion(id: Guid.NewGuid(), text: "None of the above apply?", mainQuestionId: mainQuestion_10.Id, parentQuestionId: mainQuestion_10.Id);

            var question11_1 = new SubQuestion(id: Guid.NewGuid(), text: "Is it or it products of metabolism, systemically absorbed by the human body in order to achieve the intended purpose?", mainQuestionId: mainQuestion_11.Id, parentQuestionId: mainQuestion_11.Id);
            var question11_2 = new SubQuestion(id: Guid.NewGuid(), text: "Does it achieve its intended purpose in the stomach or lower gastrointestinal tract and it, or its products of metabolism, is systemically absorbed by the human body?", mainQuestionId: mainQuestion_11.Id, parentQuestionId: mainQuestion_11.Id);
            var question11_3 = new SubQuestion(id: Guid.NewGuid(), text: "Is it applied to the skin or is it are applied in the nasal or oral cavity as far as the pharynx, and achieve the intended purpose on those cavities?", mainQuestionId: mainQuestion_11.Id, parentQuestionId: mainQuestion_11.Id);
            var question11_4 = new SubQuestion(id: Guid.NewGuid(), text: "None of the above apply?", mainQuestionId: mainQuestion_11.Id, parentQuestionId: mainQuestion_11.Id);

            MainQuestions.Add(mainQuestion_1);
            SubQuestions.Add(question1);
            SubQuestions.Add(question1_1);
            SubQuestions.Add(question1_1_1);
            SubQuestions.Add(question1_1_2);
            SubQuestions.Add(question1_1_3);
            SubQuestions.Add(question1_1_4);
            SubQuestions.Add(question1_1_4_1);
            SubQuestions.Add(question1_1_4_2);
            SubQuestions.Add(question1_1_4_3);
            SubQuestions.Add(question1_1_5);
            SubQuestions.Add(question1_1_5_1);
            SubQuestions.Add(question1_1_5_2);
            SubQuestions.Add(question1_1_5_3);
            SubQuestions.Add(question1_1_5_4);
            SubQuestions.Add(question1_1_6);
            SubQuestions.Add(question2);
            SubQuestions.Add(question2_1);
            SubQuestions.Add(question2_1_1);
            SubQuestions.Add(question2_1_1_1);
            SubQuestions.Add(question2_1_1_2);
            SubQuestions.Add(question2_1_1_3);
            SubQuestions.Add(question2_1_1_4);
            SubQuestions.Add(question2_1_1_5);
            SubQuestions.Add(question2_1_1_6);
            SubQuestions.Add(question2_1_2);
            SubQuestions.Add(question2_1_2_1);
            SubQuestions.Add(question2_1_2_2);
            SubQuestions.Add(question2_1_2_3);
            SubQuestions.Add(question2_1_2_4);
            SubQuestions.Add(question2_1_2_5);
            SubQuestions.Add(question2_1_2_6);
            SubQuestions.Add(question2_1_3);
            SubQuestions.Add(question2_2);
            SubQuestions.Add(question2_2_1);
            SubQuestions.Add(question2_2_1_1);
            SubQuestions.Add(question2_2_1_2);
            SubQuestions.Add(question2_2_1_3);
            SubQuestions.Add(question2_2_1_4);
            SubQuestions.Add(question2_2_1_5);
            SubQuestions.Add(question2_2_1_6);
            SubQuestions.Add(question2_2_1_7);
            SubQuestions.Add(question2_2_2);
            SubQuestions.Add(question2_2_2_1);
            SubQuestions.Add(question2_2_2_2);
            SubQuestions.Add(question2_2_2_3);
            SubQuestions.Add(question2_2_2_4);
            SubQuestions.Add(question2_2_2_5);
            SubQuestions.Add(question2_2_2_5_1);
            SubQuestions.Add(question2_2_2_5_2);
            SubQuestions.Add(question2_2_2_5_3);
            SubQuestions.Add(question2_2_2_6);
            SubQuestions.Add(question2_2_3);
            SubQuestions.Add(question2_2_3_1);
            SubQuestions.Add(question2_2_3_2);
            SubQuestions.Add(question2_2_3_3);
            SubQuestions.Add(question2_2_3_4);
            SubQuestions.Add(question2_2_3_4_1);
            SubQuestions.Add(question2_2_3_4_2);
            SubQuestions.Add(question2_2_3_5);
            SubQuestions.Add(question2_2_3_6);
            SubQuestions.Add(question2_2_3_7);
            SubQuestions.Add(question2_2_3_8);
            SubQuestions.Add(question2_2_3_9);
            SubQuestions.Add(question2_2_3_10);
            SubQuestions.Add(question2_2_3_11);
            SubQuestions.Add(question2_2_3_12);
            SubQuestions.Add(question2_2_3_13);
            SubQuestions.Add(question2_2_3_14);
            SubQuestions.Add(question3_1);
            SubQuestions.Add(question3_1_1);
            SubQuestions.Add(question3_1_2);
            SubQuestions.Add(question3_1_3);
            SubQuestions.Add(question3_1_4);
            SubQuestions.Add(question3_1_5);
            SubQuestions.Add(question3_2);
            SubQuestions.Add(question3_3);
            SubQuestions.Add(question3_4);
            SubQuestions.Add(question3_4_1);
            SubQuestions.Add(question3_4_1_1);
            SubQuestions.Add(question3_4_1_2);
            SubQuestions.Add(question3_4_1_3);
            SubQuestions.Add(question3_4_2);
            SubQuestions.Add(question3_4_2_1);
            SubQuestions.Add(question3_4_2_2);
            SubQuestions.Add(question3_4_2_3);
            SubQuestions.Add(question3_4_3);
            SubQuestions.Add(question3_4_4);
            SubQuestions.Add(question3_5);
            SubQuestions.Add(question3_5_1);
            SubQuestions.Add(question3_5_1_1);
            SubQuestions.Add(question3_5_1_2);
            SubQuestions.Add(question3_5_1_3);
            SubQuestions.Add(question3_5_2);
            SubQuestions.Add(question3_5_2_1);
            SubQuestions.Add(question3_5_2_2);
            SubQuestions.Add(question3_5_3);
            SubQuestions.Add(question3_6);
            SubQuestions.Add(question3_6_1);
            SubQuestions.Add(question3_6_2);
            SubQuestions.Add(question3_7);


            MainQuestions.Add(mainQuestion_2);
            MainQuestions.Add(mainQuestion_4);
            MainQuestions.Add(mainQuestion_5);
            MainQuestions.Add(mainQuestion_6);
            MainQuestions.Add(mainQuestion_7);
            MainQuestions.Add(mainQuestion_8);
            MainQuestions.Add(mainQuestion_9);
            MainQuestions.Add(mainQuestion_10);
            MainQuestions.Add(mainQuestion_11);
            MainQuestions.Add(mainQuestion_12);

            SubQuestions.Add(question5_1);
            SubQuestions.Add(question5_2);
            SubQuestions.Add(question7_1);
            SubQuestions.Add(question7_2);
            SubQuestions.Add(question7_3);
            SubQuestions.Add(question9_1);
            SubQuestions.Add(question9_2);
            SubQuestions.Add(question9_3);
            SubQuestions.Add(question10_1);
            SubQuestions.Add(question10_2);
            SubQuestions.Add(question10_3);
            SubQuestions.Add(question11_1);
            SubQuestions.Add(question11_2);
            SubQuestions.Add(question11_3);
            SubQuestions.Add(question11_4);

            var class_I_Questions = new List<Guid>
            {
                question1_1_5_1.Id,
                question1_1_6.Id,
                question2_1_1_1.Id,
                question2_1_1_2.Id,
                question2_1_2_1.Id,
                question2_1_2_2.Id,
                question2_2_1_2.Id,
                question3_4_1_1.Id,
                question3_7.Id
            };

            var class_IIa_Questions = new List<Guid>
            {
                question1_1_1.Id,
                question1_1_2.Id,
                question1_1_4_1.Id,
                question1_1_5_3.Id,
                question1_1_5_4.Id,
                question2_1_1_3.Id,
                question2_1_1_4.Id,
                question2_1_2_3.Id,
                question2_1_2_4.Id,
                question2_1_3.Id,
                question2_2_1_7.Id,
                question2_2_2_5_1.Id,
                question2_2_2_6.Id,
                question2_2_3_1.Id,
                question3_1_2.Id,
                question3_4_1_2.Id,
                question3_4_1_3.Id,
                question3_4_2_3.Id,
                question3_4_4.Id,
                question3_5_1_3.Id,
                question3_5_2_2.Id,
                question3_6_2.Id,
                question5_2.Id,
                question7_2.Id,
                question7_3.Id,
                question9_3.Id,
                question10_3.Id,
                question11_3.Id
            };

            var class_IIb_Questions = new List<Guid>
            {
                question1_1_3.Id,
                question1_1_4_2.Id,
                question1_1_5_2.Id,
                question2_1_1_5.Id,
                question2_1_1_6.Id,
                question2_1_2_5.Id,
                question2_1_2_6.Id,
                question2_2_1_4.Id,
                question2_2_1_5.Id,
                question2_2_1_6.Id,
                question2_2_2_3.Id,
                question2_2_2_5_2.Id,
                question2_2_2_5_3.Id,
                question2_2_3_4_1.Id,
                question2_2_3_10.Id,
                question2_2_3_13.Id,
                question2_2_3_14.Id,
                question3_1_1.Id,
                question3_1_3.Id,
                question3_1_4.Id,
                question3_1_5.Id,
                question3_2.Id,
                question3_4_2_1.Id,
                question3_4_2_2.Id,
                question3_4_3.Id,
                question3_5_1_2.Id,
                question3_5_2_1.Id,
                question3_6_1.Id,
                question5_1.Id,
                mainQuestion_6.Id,
                question7_1.Id,
                question9_2.Id,
                question10_1.Id,
                question10_2.Id,
                question11_4.Id
            };

            var class_III_Questions = new List<Guid>
            {
                question1_1_4_3.Id,
                question2_2_1_1.Id,
                question2_2_1_3.Id,
                question2_2_2_1.Id,
                question2_2_2_2.Id,
                question2_2_2_4.Id,
                question2_2_3_2.Id,
                question2_2_3_3.Id,
                question2_2_3_4_2.Id,
                question2_2_3_5.Id,
                question2_2_3_6.Id,
                question2_2_3_7.Id,
                question2_2_3_8.Id,
                question2_2_3_9.Id,
                question2_2_3_11.Id,
                question2_2_3_12.Id,
                question3_3.Id,
                question3_5_1_1.Id,
                mainQuestion_4.Id,
                mainQuestion_8.Id,
                question9_1.Id,
                question11_1.Id,
                question11_2.Id,
                mainQuestion_12.Id
            };

            var class_I = new Classification(Guid.NewGuid(), "I", 1, class_I_Questions);
            var class_IIa = new Classification(Guid.NewGuid(), "IIa", 2, class_IIa_Questions);
            var class_IIb = new Classification(Guid.NewGuid(), "IIb", 3, class_IIb_Questions);
            var class_III = new Classification(Guid.NewGuid(), "III", 4, class_III_Questions);

            Classifications.Add(class_I);
            Classifications.Add(class_IIa);
            Classifications.Add(class_IIb);
            Classifications.Add(class_III);
        }

        public MainQuestion GetDefaultMainQuestion()
        {
            var mainQuestion = MainQuestions.Single(x => x.NextMainQuestion == null);

            while (MainQuestions.Any(x => x.NextMainQuestion == mainQuestion.Id))
            {
                mainQuestion = MainQuestions.Single(x => x.NextMainQuestion == mainQuestion.Id);
            }

            return mainQuestion;
        }

        public MainQuestion GetNextMainQuestion(Guid currentQuestionId)
        {
            if (currentQuestionId == default(Guid))
            {
                throw new ArgumentException(nameof(currentQuestionId));
            }

            var currentMainQuestion = MainQuestions.First(x => x.Id == currentQuestionId);

            if (currentMainQuestion.IsLastMainQuestion)
            {
                return null;
            }

            return MainQuestions.Where(x => x.Id == currentMainQuestion.NextMainQuestion).First();
        }

        public MainQuestion GetMainQuestion(Guid questionId)
        {
            if (questionId == default(Guid))
            {
                throw new ArgumentException(nameof(questionId));
            }

            return MainQuestions.Single(x => x.Id == questionId);
        }

        public IReadOnlyCollection<SubQuestion> GetSubQuestions(Guid parentQuestionId)
        {
            if (parentQuestionId == default(Guid))
            {
                throw new ArgumentException(nameof(parentQuestionId));
            }

            var subQuestions = SubQuestions
                .Where(x => x.ParentQuestionId == parentQuestionId);

            if (subQuestions == null || subQuestions.Count() == 0)
            {
                return null;
            }

            return subQuestions.ToArray();
        }

        public Classification GetClassification(Guid questionId)
        {
            if (questionId == default(Guid))
            {
                throw new ArgumentException(nameof(questionId));
            }

            var classification = Classifications.First(x => x.QuestionIds.Contains(questionId));

            return classification;
        }

        public Classification GetClassification(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException(nameof(text));
            }

            var classification = Classifications.First(x => x.Text == text);

            return classification;
        }
    }

    public interface IDeviceClassification
    {
        MainQuestion GetDefaultMainQuestion();
        MainQuestion GetNextMainQuestion(Guid currentQuestionId);
        MainQuestion GetMainQuestion(Guid questionId);
        IReadOnlyCollection<SubQuestion> GetSubQuestions(Guid parentQuestionId);
        Classification GetClassification(Guid questionId);
        Classification GetClassification(string text);
    }
}