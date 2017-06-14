using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class DeviceClassification : IDeviceClassification
    {
        public Guid Id { get; set; }

        [JsonProperty(nameof(PairedMainQuestions))]
        private List<PairedMainQuestions> PairedMainQuestions { get; set; } = new List<PairedMainQuestions>();

        [JsonProperty(nameof(SubQuestions))]
        private List<SubQuestion> SubQuestions { get; set; } = new List<SubQuestion>();

        [JsonProperty(nameof(Classifications))]
        private List<Classification> Classifications { get; set; } = new List<Classification>();

        [JsonProperty(nameof(SpecialMainQuestions))]
        private List<SpecialMainQuestion> SpecialMainQuestions { get; set; } = new List<SpecialMainQuestion>();

        [JsonProperty(nameof(SpecialSubQuestions))]
        private List<SpecialSubQuestion> SpecialSubQuestions { get; set; } = new List<SpecialSubQuestion>();

        public DeviceClassification()
        {
        }

        protected DeviceClassification(
            List<PairedMainQuestions> pairedMainQuestions,
            List<SubQuestion> subQuestions,
            List<SpecialMainQuestion> specialMainQuestions,
            List<SpecialSubQuestion> specialSubQuestions,
            List<Classification> classifications)
        {
            PairedMainQuestions = pairedMainQuestions;
            SubQuestions = subQuestions;
            Classifications = classifications;
            SpecialMainQuestions = specialMainQuestions;
            SpecialSubQuestions = specialSubQuestions;
        }

        public void CreateNew()
        {
            var mainQuestion1 = new MainQuestion("Is your device NON INVASIVE?");
            var mainQuestion2 = new MainQuestion("Is your device INVASIVE?");

            var mainQuestion3 = new MainQuestion("Is your device ACTIVE?");

            var pair2 = new PairedMainQuestions(null, mainQuestion3, null);
            var pair1 = new PairedMainQuestions(pair2, mainQuestion1, mainQuestion2);

            PairedMainQuestions.Add(pair1);
            PairedMainQuestions.Add(pair2);

            var subQuestion1_1 = new SubQuestion("Is it intended for channelling or storing blood, body liquids, cells or tissues, liquids or gases for the purpose of eventual infusion, administration or introduction into the body?", pair1, mainQuestion1);
            var subQuestion1_1_1 = new SubQuestion("May it be connected to an active medical device in class IIa or a higher class?", pair1, subQuestion1_1);
            var subQuestion1_1_2 = new SubQuestion("Is it intended for use for storing or channelling blood or other body liquids or for storing organs, parts of organs or body cells and tissues, but it is not a blood bag?", pair1, subQuestion1_1);
            var subQuestion1_1_3 = new SubQuestion("Is it a blood bag?", pair1, subQuestion1_1);
            var subQuestion1_1_4 = new SubQuestion("Is it intended for modifying the biological or chemical composition of human tissues or cells, blood, other body liquids or other liquids intended for implantation or administration into the body?", pair1, subQuestion1_1);
            var subQuestion1_1_4_1 = new SubQuestion("Does the treatment consist of filtration, centrifugation or exchanges of gas, heat?", pair1, subQuestion1_1_4);
            var subQuestion1_1_4_2 = new SubQuestion("Is the treatment other than filtration, centrifugation or exchanges of gas, heat?", pair1, subQuestion1_1_4);
            var subQuestion1_1_4_3 = new SubQuestion("Is your device a substance or a mixture of substances intended to be used in vitro in direct contact with human cells, tissues or organs taken off from the human body or with human embryos before their implantation or administration into the body?", pair1, subQuestion1_1_4);
            var subQuestion1_1_5 = new SubQuestion("Is it intended come into contact with injured skin or mucous membrane?", pair1, subQuestion1_1);
            var subQuestion1_1_5_1 = new SubQuestion("Is it intended to be used as a mechanical barrier, for compression or for absorption of exudates?", pair1, subQuestion1_1_5);
            var subQuestion1_1_5_2 = new SubQuestion("Is it intended to be used principally for injuries to skin which have breached the dermis or mucous membrane and can only heal by secondary intent?", pair1, subQuestion1_1_5);
            var subQuestion1_1_5_3 = new SubQuestion("Is it principally intended to manage the micro-environment of injured skin or mucous membrane?", pair1, subQuestion1_1_5);
            var subQuestion1_1_5_4 = new SubQuestion("None of the above apply?", pair1, subQuestion1_1_5);
            var subQuestion1_1_6 = new SubQuestion("None of the above apply?", pair1, subQuestion1_1);

            SubQuestions.Add(subQuestion1_1);
            SubQuestions.Add(subQuestion1_1_1);
            SubQuestions.Add(subQuestion1_1_2);
            SubQuestions.Add(subQuestion1_1_3);
            SubQuestions.Add(subQuestion1_1_4);
            SubQuestions.Add(subQuestion1_1_4_1);
            SubQuestions.Add(subQuestion1_1_4_2);
            SubQuestions.Add(subQuestion1_1_4_3);
            SubQuestions.Add(subQuestion1_1_5);
            SubQuestions.Add(subQuestion1_1_5_1);
            SubQuestions.Add(subQuestion1_1_5_2);
            SubQuestions.Add(subQuestion1_1_5_3);
            SubQuestions.Add(subQuestion1_1_5_4);
            SubQuestions.Add(subQuestion1_1_6);

            var subQuestion2_1 = new SubQuestion("Is it SURGICALLY NON INVASIVE?", pair1, mainQuestion2);
            var subQuestion2_1_1 = new SubQuestion("Is it intended for connection to any active medical device?", pair1, subQuestion2_1);
            var subQuestion2_1_1_1 = new SubQuestion("Is it intended for transient use?", pair1, subQuestion2_1_1);
            var subQuestion2_1_1_2 = new SubQuestion("Is it intended for short-term use  only in the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity?", pair1, subQuestion2_1_1);
            var subQuestion2_1_1_3 = new SubQuestion("Is it intended for short-term use in any other site than the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity?", pair1, subQuestion2_1_1);
            var subQuestion2_1_1_4 = new SubQuestion("Is it intended for long-term use only in the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity and it not liable to be absorbed by the mucous membrane?", pair1, subQuestion2_1_1);
            var subQuestion2_1_1_5 = new SubQuestion("Is it intended for long-term use only in the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity but it is liable to be absorbed by the mucous membrane?", pair1, subQuestion2_1_1);
            var subQuestion2_1_1_6 = new SubQuestion("Is it intended for long-term use in any other site than the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity?", pair1, subQuestion2_1_1);
            var subQuestion2_1_2 = new SubQuestion("Is it intended for connection to a class I active medical device?", pair1, subQuestion2_1);
            var subQuestion2_1_2_1 = new SubQuestion("Is it intended for transient use?", pair1, subQuestion2_1_2);
            var subQuestion2_1_2_2 = new SubQuestion("Is it intended for short-term use only in the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity?", pair1, subQuestion2_1_2);
            var subQuestion2_1_2_3 = new SubQuestion("Is it intended for short-term use in any other site than the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity?", pair1, subQuestion2_1_2);
            var subQuestion2_1_2_4 = new SubQuestion("Is it intended for long-term use only in the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity and it not liable to be absorbed by the mucous membrane?", pair1, subQuestion2_1_2);
            var subQuestion2_1_2_5 = new SubQuestion("Is it intended for long-term use only in the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity but it is liable to be absorbed by the mucous membrane?", pair1, subQuestion2_1_2);
            var subQuestion2_1_2_6 = new SubQuestion("Is it intended for long-term use in any other site than the oral cavity as far as the pharynx, in an ear canal up to the ear drum or in the nasal cavity?", pair1, subQuestion2_1_2);
            var subQuestion2_1_3 = new SubQuestion("Is it intended for connection to a class IIa or higher class active medical device?", pair1, subQuestion2_1);

            SubQuestions.Add(subQuestion2_1);
            SubQuestions.Add(subQuestion2_1_1);
            SubQuestions.Add(subQuestion2_1_1_1);
            SubQuestions.Add(subQuestion2_1_1_2);
            SubQuestions.Add(subQuestion2_1_1_3);
            SubQuestions.Add(subQuestion2_1_1_4);
            SubQuestions.Add(subQuestion2_1_1_5);
            SubQuestions.Add(subQuestion2_1_1_6);
            SubQuestions.Add(subQuestion2_1_2);
            SubQuestions.Add(subQuestion2_1_2_1);
            SubQuestions.Add(subQuestion2_1_2_2);
            SubQuestions.Add(subQuestion2_1_2_3);
            SubQuestions.Add(subQuestion2_1_2_4);
            SubQuestions.Add(subQuestion2_1_2_5);
            SubQuestions.Add(subQuestion2_1_2_6);
            SubQuestions.Add(subQuestion2_1_3);


            var subQuestion2_2 = new SubQuestion("Is it SURGICALLY INVASIVE?", pair1, mainQuestion2);
            var subQuestion2_2_1 = new SubQuestion("Is it for TRANSIENT use?", pair1, subQuestion2_2);
            var subQuestion2_2_1_1 = new SubQuestion("Is it intended specifically to control, diagnose, monitor or correct a defect of the heart or of the central circulatory system through direct contact with those parts of the body?", pair1, subQuestion2_2_1);
            var subQuestion2_2_1_2 = new SubQuestion("Is it a reusable surgical instrument?", pair1, subQuestion2_2_1);
            var subQuestion2_2_1_3 = new SubQuestion("Is it intended specifically for use in direct contact with the heart or central circulatory system or the central nervous system?", pair1, subQuestion2_2_1);
            var subQuestion2_2_1_4 = new SubQuestion("Is it intended to supply energy in the form of ionising radiation?", pair1, subQuestion2_2_1);
            var subQuestion2_2_1_5 = new SubQuestion("Is it intended to have a biological effect or be wholly or mainly absorbed?", pair1, subQuestion2_2_1);
            var subQuestion2_2_1_6 = new SubQuestion("Is it intended to administer medicinal products by means of a delivery system in a manner that is potentially hazardous?", pair1, subQuestion2_2_1);
            var subQuestion2_2_1_7 = new SubQuestion("None of the above apply?", pair1, subQuestion2_2_1);

            SubQuestions.Add(subQuestion2_2);
            SubQuestions.Add(subQuestion2_2_1);
            SubQuestions.Add(subQuestion2_2_1_1);
            SubQuestions.Add(subQuestion2_2_1_2);
            SubQuestions.Add(subQuestion2_2_1_3);
            SubQuestions.Add(subQuestion2_2_1_4);
            SubQuestions.Add(subQuestion2_2_1_5);
            SubQuestions.Add(subQuestion2_2_1_6);
            SubQuestions.Add(subQuestion2_2_1_7);

            var subQuestion2_2_2 = new SubQuestion("Is it for SHORT TERM use?", pair1, subQuestion2_2);
            var subQuestion2_2_2_1 = new SubQuestion("Is it intended specifically to control, diagnose, monitor or correct a defect of the heart or of the central circulatory system through direct contact with those parts of the body?", pair1, subQuestion2_2_2);
            var subQuestion2_2_2_2 = new SubQuestion("Is it intended specifically for use in direct contact with the heart or central circulatory system or the central nervous system?", pair1, subQuestion2_2_2);
            var subQuestion2_2_2_3 = new SubQuestion("Is it intended to have a biological effect or are wholly or mainly absorbed?", pair1, subQuestion2_2_2);
            var subQuestion2_2_2_4 = new SubQuestion("Is it intended to have a biological effect or be wholly or mainly absorbed?", pair1, subQuestion2_2_2);
            var subQuestion2_2_2_5 = new SubQuestion("Is it intended to undergo chemical change in the body?", pair1, subQuestion2_2_2);
            var subQuestion2_2_2_5_1 = new SubQuestion("Is it in the teeth?", pair1, subQuestion2_2_2_5);
            var subQuestion2_2_2_5_2 = new SubQuestion("Is it in any part of the body other than the teeth?", pair1, subQuestion2_2_2_5);
            var subQuestion2_2_2_5_3 = new SubQuestion("Is it intended to administer medicines?", pair1, subQuestion2_2_2_5);
            var subQuestion2_2_2_6 = new SubQuestion("None of the above apply?", pair1, subQuestion2_2_2);

            SubQuestions.Add(subQuestion2_2_2);
            SubQuestions.Add(subQuestion2_2_2_1);
            SubQuestions.Add(subQuestion2_2_2_2);
            SubQuestions.Add(subQuestion2_2_2_3);
            SubQuestions.Add(subQuestion2_2_2_4);
            SubQuestions.Add(subQuestion2_2_2_5);
            SubQuestions.Add(subQuestion2_2_2_5_1);
            SubQuestions.Add(subQuestion2_2_2_5_2);
            SubQuestions.Add(subQuestion2_2_2_5_3);
            SubQuestions.Add(subQuestion2_2_2_6);

            var subQuestion2_2_3 = new SubQuestion("Is it implantable or LONG TERM use?", pair1, subQuestion2_2);
            var subQuestion2_2_3_1 = new SubQuestion("Is it intended to be placed in the teeth?", pair1, subQuestion2_2_3);
            var subQuestion2_2_3_2 = new SubQuestion("Is it intended for use in direct contact with the heart or central circulatory system or the central nervous system?", pair1, subQuestion2_2_3);
            var subQuestion2_2_3_3 = new SubQuestion("Is it intended to have a biological effect or be wholly or mainly absorbed?", pair1, subQuestion2_2_3);
            var subQuestion2_2_3_4 = new SubQuestion("Is it intended to undergo chemical change in the body?", pair1, subQuestion2_2_3);
            var subQuestion2_2_3_4_1 = new SubQuestion("Is it in the teeth?", pair1, subQuestion2_2_3_4);
            var subQuestion2_2_3_4_2 = new SubQuestion("Is it in any part of the body other than the teeth?", pair1, subQuestion2_2_3_4);
            var subQuestion2_2_3_5 = new SubQuestion("Is it intended to administer medicines?", pair1, subQuestion2_2_3);
            var subQuestion2_2_3_6 = new SubQuestion("Is it an active implantable device or its accessory?", pair1, subQuestion2_2_3);
            var subQuestion2_2_3_7 = new SubQuestion("Is it a breast implant?", pair1, subQuestion2_2_3);
            var subQuestion2_2_3_8 = new SubQuestion("Is it a surgical mesh?", pair1, subQuestion2_2_3);
            var subQuestion2_2_3_9 = new SubQuestion("Is it a total or partial joint replacement?", pair1, subQuestion2_2_3);
            var subQuestion2_2_3_10 = new SubQuestion("Is it an ancillary component of a total or partial joint replacement such as screws, wedges, plates and instruments?", pair1, subQuestion2_2_3);
            var subQuestion2_2_3_11 = new SubQuestion("Is it a spinal disc replacement implant?", pair1, subQuestion2_2_3);
            var subQuestion2_2_3_12 = new SubQuestion("Is it an implantable device that come into contact with the spinal column?", pair1, subQuestion2_2_3);
            var subQuestion2_2_3_13 = new SubQuestion("Is it an ancillary component of a spinal disc replacement or other device that comes in contact with the spinal column such as screws, wedges, plates and instruments?", pair1, subQuestion2_2_3);
            var subQuestion2_2_3_14 = new SubQuestion("None of the above apply?", pair1, subQuestion2_2_3);

            SubQuestions.Add(subQuestion2_2_3);
            SubQuestions.Add(subQuestion2_2_3_1);
            SubQuestions.Add(subQuestion2_2_3_2);
            SubQuestions.Add(subQuestion2_2_3_3);
            SubQuestions.Add(subQuestion2_2_3_4);
            SubQuestions.Add(subQuestion2_2_3_4_1);
            SubQuestions.Add(subQuestion2_2_3_4_2);
            SubQuestions.Add(subQuestion2_2_3_5);
            SubQuestions.Add(subQuestion2_2_3_6);
            SubQuestions.Add(subQuestion2_2_3_7);
            SubQuestions.Add(subQuestion2_2_3_8);
            SubQuestions.Add(subQuestion2_2_3_9);
            SubQuestions.Add(subQuestion2_2_3_10);
            SubQuestions.Add(subQuestion2_2_3_11);
            SubQuestions.Add(subQuestion2_2_3_12);
            SubQuestions.Add(subQuestion2_2_3_13);
            SubQuestions.Add(subQuestion2_2_3_14);

            var subQuestion3_1 = new SubQuestion("Is it therapeutic?", pair1, mainQuestion3);
            var subQuestion3_1_1 = new SubQuestion("Does it administer energy to or exchange energy with the human body in a potentially hazardous way (consider nature of energy, density of energy, site of the body)?", pair1, subQuestion3_1);
            var subQuestion3_1_2 = new SubQuestion("Does it administer energy to or exchange energy with the human body in a NON hazardous way?", pair1, subQuestion3_1);
            var subQuestion3_1_3 = new SubQuestion("Is it intended to emit ionizing radiation?", pair1, subQuestion3_1);
            var subQuestion3_1_4 = new SubQuestion("Is it intended for therapeutic radiology, including interventional radiology devices and devices which control or monitor such devices, or which directly influence their performance?", pair1, subQuestion3_1);
            var subQuestion3_1_5 = new SubQuestion("Is it intended to control or monitor a device that emits ionizing radiation, or does it directly influence the performance a device that emits ionizing radiation?", pair1, subQuestion3_1);
            var subQuestion3_2 = new SubQuestion("Is it intended to control or monitor the performance of active therapeutic class IIb devices, or intended directly to influence the performance of such devices?", pair1, mainQuestion3);
            var subQuestion3_3 = new SubQuestion("Is it intended for controlling, monitoring or directly influencing the performance of active implantable devices?", pair1, mainQuestion3);
            var subQuestion3_4 = new SubQuestion("Is it intended for diagnosis and monitoring?", pair1, mainQuestion3);
            var subQuestion3_4_1 = new SubQuestion("Is it intended to supply energy which will be absorbed by the human body?", pair1, subQuestion3_4);

            var subQuestion3_4_1_1 = new SubQuestion("Is it only intended to intended to illuminate the patient's body, in the visible spectrum?", pair1, subQuestion3_4_1);
            var subQuestion3_4_1_2 = new SubQuestion("Any other case?", pair1, subQuestion3_4_1);
            var subQuestion3_4_1_3 = new SubQuestion("Is it intended to image in vivo distribution of radiopharmaceuticals?", pair1, subQuestion3_4_1);

            SubQuestions.Add(subQuestion3_4_1_1);
            SubQuestions.Add(subQuestion3_4_1_2);
            SubQuestions.Add(subQuestion3_4_1_3);

            var subQuestion3_4_2 = new SubQuestion("Is it intended to allow direct diagnosis or monitoring of vital physiological processes?", pair1, subQuestion3_4);

            var subQuestion3_4_2_1 = new SubQuestion("Are the vital physiological processes such as the patient may be in immediate danger if the vary (example: variations in cardiac performance, respiration, activity of the central nervous system)?", pair1, subQuestion3_4_2);
            var subQuestion3_4_2_2 = new SubQuestion("Is the diagnosis and monitoring done in a clinical situation while the patient is in immediate danger?", pair1, subQuestion3_4_2);
            var subQuestion3_4_2_3 = new SubQuestion("Any other case?", pair1, subQuestion3_4_2);

            SubQuestions.Add(subQuestion3_4_2_1);
            SubQuestions.Add(subQuestion3_4_2_2);
            SubQuestions.Add(subQuestion3_4_2_3);


            var subQuestion3_4_3 = new SubQuestion("Is it intended to emit ionizing radiation for diagnostics?", pair1, subQuestion3_4);
            var subQuestion3_4_4 = new SubQuestion("None of the above apply?", pair1, subQuestion3_4);
            var subQuestion3_5 = new SubQuestion("Is it a Software?", pair1, mainQuestion3);
            var subQuestion3_5_1 = new SubQuestion("Is it intended to provide information which is used to take decisions with diagnosis or therapeutic purposes?", pair1, subQuestion3_5);
            var subQuestion3_5_1_1 = new SubQuestion("Does such decision have an impact that may cause death or an irreversible deterioration of a person's state of health?", pair1, subQuestion3_5_1);
            var subQuestion3_5_1_2 = new SubQuestion("Does such decision have an impact that may cause a serious deterioration of a person's state of health or a surgical intervention?", pair1, subQuestion3_5_1);
            var subQuestion3_5_1_3 = new SubQuestion("None of the above apply?", pair1, subQuestion3_5_1);
            var subQuestion3_5_2 = new SubQuestion("Is it intended to monitor physiological processes?", pair1, subQuestion3_5);
            var subQuestion3_5_2_1 = new SubQuestion("Does it monitor vital physiological parameters, where the nature of variations of those parameters is such that it could result in immediate danger to the patient?", pair1, subQuestion3_5_2);
            var subQuestion3_5_2_2 = new SubQuestion("Does it monitor non vital physiological parameters, so that the nature of variations of those parameters is such that it could never and in no cases result in immediate danger to the patient?", pair1, subQuestion3_5_2);
            var subQuestion3_5_3 = new SubQuestion("None of the above apply?", pair1, subQuestion3_5);
            var subQuestion3_6 = new SubQuestion("Is it intended to administer and/or remove medicinal products, body liquids or other substances to or from the body?", pair1, mainQuestion3);
            var subQuestion3_6_1 = new SubQuestion("Is this done in a manner that is potentially hazardous (taking account of the nature of the substances involved, of the part of the body concerned and of the mode of application)?", pair1, subQuestion3_6);
            var subQuestion3_6_2 = new SubQuestion("None of the above apply?", pair1, subQuestion3_6);
            var subQuestion3_7 = new SubQuestion("None of the above apply?", pair1, mainQuestion3);

            SubQuestions.Add(subQuestion3_1);
            SubQuestions.Add(subQuestion3_1_1);
            SubQuestions.Add(subQuestion3_1_2);
            SubQuestions.Add(subQuestion3_1_3);
            SubQuestions.Add(subQuestion3_1_4);
            SubQuestions.Add(subQuestion3_1_5);
            SubQuestions.Add(subQuestion3_2);
            SubQuestions.Add(subQuestion3_3);
            SubQuestions.Add(subQuestion3_4);
            SubQuestions.Add(subQuestion3_4_1);
            SubQuestions.Add(subQuestion3_4_2);
            SubQuestions.Add(subQuestion3_4_3);
            SubQuestions.Add(subQuestion3_4_4);
            SubQuestions.Add(subQuestion3_5);
            SubQuestions.Add(subQuestion3_5_1);
            SubQuestions.Add(subQuestion3_5_1_1);
            SubQuestions.Add(subQuestion3_5_1_2);
            SubQuestions.Add(subQuestion3_5_1_3);
            SubQuestions.Add(subQuestion3_5_2);
            SubQuestions.Add(subQuestion3_5_2_1);
            SubQuestions.Add(subQuestion3_5_2_2);
            SubQuestions.Add(subQuestion3_5_3);
            SubQuestions.Add(subQuestion3_6);
            SubQuestions.Add(subQuestion3_6_1);
            SubQuestions.Add(subQuestion3_6_2);
            SubQuestions.Add(subQuestion3_7);

            var specialMainQuestion12 = new SpecialMainQuestion("Is it an active therapeutic devices with an integrated or incorporated diagnostic function that includes an integrated or incorporated diagnostic function which significantly determines the patient management by the device (example: closed loop systems or automated external defibrillators)?", null);
            var specialMainQuestion11 = new SpecialMainQuestion("Is it composed of substances or of combinations of substances that are intended to be introduced into the human body via a body orifice or applied to the skin and that are absorbed by or locally dispersed in the human body?", specialMainQuestion12);
            var specialMainQuestion10 = new SpecialMainQuestion("Is it intended to administer medicinal products by inhalation, being an invasive device with respect to body orifices, other than surgically invasive devices?", specialMainQuestion11);
            var specialMainQuestion9 = new SpecialMainQuestion("Does it incorporate or contain nanomaterials?", specialMainQuestion10);
            var specialMainQuestion8 = new SpecialMainQuestion("Is it manufactured utilising tissues or cells of human or animal origin, or their derivatives, which are non-viable or rendered non-viable?", specialMainQuestion9);
            var specialMainQuestion7 = new SpecialMainQuestion("Is it intended specifically to be used for disinfecting or sterilising medical devices that are not contact lenses?", specialMainQuestion8);
            var specialMainQuestion6 = new SpecialMainQuestion("Is it intended specifically to be used for disinfecting, cleaning, rinsing or, where appropriate, hydrating contact lenses?", specialMainQuestion7);
            var specialMainQuestion5 = new SpecialMainQuestion("Is it used for contraception or prevention of the transmission of sexually transmitted diseases?", specialMainQuestion6);
            var specialMainQuestion4 = new SpecialMainQuestion("Does your device incorporate a medicinal product with an ancillary function?", specialMainQuestion5);

            SpecialMainQuestions.Add(specialMainQuestion4);
            SpecialMainQuestions.Add(specialMainQuestion5);
            SpecialMainQuestions.Add(specialMainQuestion6);
            SpecialMainQuestions.Add(specialMainQuestion7);
            SpecialMainQuestions.Add(specialMainQuestion8);
            SpecialMainQuestions.Add(specialMainQuestion9);
            SpecialMainQuestions.Add(specialMainQuestion10);
            SpecialMainQuestions.Add(specialMainQuestion11);
            SpecialMainQuestions.Add(specialMainQuestion12);

            var specialSubQuestion5_1 = new SpecialSubQuestion("Is it implantable or long term invasive?", specialMainQuestion5);
            var specialSubQuestion5_2 = new SpecialSubQuestion("None of the above apply?", specialMainQuestion5);

            var specialSubQuestion7_1 = new SpecialSubQuestion("Is it a disinfecting solution or washer-disinfector, intended specifically to be used for disinfecting invasive devices, as the end point of processing?", specialMainQuestion7);
            var specialSubQuestion7_2 = new SpecialSubQuestion("None of the above apply?", specialMainQuestion7);
            var specialSubQuestion7_3 = new SpecialSubQuestion("Is it specifically intended for recording of diagnostic images generated by X-ray radiation?", specialMainQuestion7);

            var specialSubQuestion9_1 = new SpecialSubQuestion("Does it present a high or medium potential for internal exposure?", specialMainQuestion9);
            var specialSubQuestion9_2 = new SpecialSubQuestion("Does it present a low potential for internal exposure?", specialMainQuestion9);
            var specialSubQuestion9_3 = new SpecialSubQuestion("Does it present a negliglible potential for internal exposure?", specialMainQuestion9);

            var specialSubQuestion10_1 = new SpecialSubQuestion("Does its mode of action have an essential impact on the efficacy and safety of the administered medicinal product?", specialMainQuestion10);
            var specialSubQuestion10_2 = new SpecialSubQuestion("Is it intended to treat life-threatening conditions?", specialMainQuestion10);
            var specialSubQuestion10_3 = new SpecialSubQuestion("None of the above apply?", specialMainQuestion10);

            var specialSubQuestion11_1 = new SpecialSubQuestion("Is it or it products of metabolism, systemically absorbed by the human body in order to achieve the intended purpose?", specialMainQuestion11);
            var specialSubQuestion11_2 = new SpecialSubQuestion("Does it achieve its intended purpose in the stomach or lower gastrointestinal tract and it, or its products of metabolism, is systemically absorbed by the human body?", specialMainQuestion11);
            var specialSubQuestion11_3 = new SpecialSubQuestion("Is it applied to the skin or is it are applied in the nasal or oral cavity as far as the pharynx, and achieve the intended purpose on those cavities?", specialMainQuestion11);
            var specialSubQuestion11_4 = new SpecialSubQuestion("None of the above apply?", specialMainQuestion11);

            SpecialSubQuestions.Add(specialSubQuestion5_1);
            SpecialSubQuestions.Add(specialSubQuestion5_2);

            SpecialSubQuestions.Add(specialSubQuestion7_1);
            SpecialSubQuestions.Add(specialSubQuestion7_2);
            SpecialSubQuestions.Add(specialSubQuestion7_3);

            SpecialSubQuestions.Add(specialSubQuestion9_1);
            SpecialSubQuestions.Add(specialSubQuestion9_2);
            SpecialSubQuestions.Add(specialSubQuestion9_3);

            SpecialSubQuestions.Add(specialSubQuestion10_1);
            SpecialSubQuestions.Add(specialSubQuestion10_2);
            SpecialSubQuestions.Add(specialSubQuestion10_3);

            SpecialSubQuestions.Add(specialSubQuestion11_1);
            SpecialSubQuestions.Add(specialSubQuestion11_2);
            SpecialSubQuestions.Add(specialSubQuestion11_3);
            SpecialSubQuestions.Add(specialSubQuestion11_4);

            Classifications.Add(new Classification("I", 1, new List<Guid> {
                subQuestion1_1_5_1.Id,
                subQuestion1_1_6.Id,
                subQuestion2_1_1_1.Id,
                subQuestion2_1_1_2.Id,
                subQuestion2_1_2_1.Id,
                subQuestion2_1_2_2.Id,
                subQuestion2_2_1_2.Id,
                subQuestion3_4_1_1.Id,
                subQuestion3_5_3.Id,
                subQuestion3_7.Id,
            }));
            Classifications.Add(new Classification("IIa", 2, new List<Guid> {
                subQuestion1_1_1.Id,
                subQuestion1_1_2.Id,
                subQuestion1_1_4_1.Id,
                subQuestion1_1_5_3.Id,
                subQuestion1_1_5_4.Id,
                subQuestion2_1_1_3.Id,
                subQuestion2_1_1_4.Id,
                subQuestion2_1_2_3.Id,
                subQuestion2_1_2_4.Id,
                subQuestion2_1_3.Id,
                subQuestion2_2_1_7.Id,
                subQuestion2_2_2_5_1.Id,
                subQuestion2_2_2_6.Id,
                subQuestion2_2_3_1.Id,
                subQuestion3_1_2.Id,
                subQuestion3_4_1_2.Id,
                subQuestion3_4_1_3.Id,
                subQuestion3_4_2_2.Id,
                subQuestion3_4_2_3.Id,
                subQuestion3_4_4.Id,
                subQuestion3_5_1_3.Id,
                subQuestion3_5_2_2.Id,
                subQuestion3_6_2.Id,
                specialSubQuestion5_2.Id,
                specialSubQuestion7_2.Id,
                specialSubQuestion7_3.Id,
                specialSubQuestion9_3.Id,
                specialSubQuestion10_3.Id,
                specialSubQuestion11_3.Id,

            }));
            Classifications.Add(new Classification("IIb", 3, new List<Guid> {
                subQuestion1_1_3.Id,
                subQuestion1_1_4_2.Id,
                subQuestion1_1_5_2.Id,
                subQuestion2_1_1_5.Id,
                subQuestion2_1_1_6.Id,
                subQuestion2_1_2_5.Id,
                subQuestion2_1_2_6.Id,
                subQuestion2_2_1_4.Id,
                subQuestion2_2_1_5.Id,
                subQuestion2_2_1_6.Id,
                subQuestion2_2_2_3.Id,
                subQuestion2_2_2_5_2.Id,
                subQuestion2_2_2_5_3.Id,
                subQuestion2_2_3_4_1.Id,
                subQuestion2_2_3_10.Id,
                subQuestion2_2_3_13.Id,
                subQuestion2_2_3_14.Id,
                subQuestion3_1_1.Id,
                subQuestion3_1_3.Id,
                subQuestion3_1_4.Id,
                subQuestion3_1_5.Id,
                subQuestion3_2.Id,
                subQuestion3_4_2_1.Id,
                subQuestion3_4_3.Id,
                subQuestion3_5_1_2.Id,
                subQuestion3_5_2_1.Id,
                subQuestion3_6_1.Id,
                specialSubQuestion5_1.Id,
                specialMainQuestion6.Id,
                specialSubQuestion7_1.Id,
                specialSubQuestion9_2.Id,
                specialSubQuestion10_1.Id,
                specialSubQuestion10_2.Id,
                specialSubQuestion11_4.Id,

            }));
            Classifications.Add(new Classification("III", 4, new List<Guid> {
                subQuestion1_1_4_3.Id,
                subQuestion2_2_1_1.Id,
                subQuestion2_2_1_3.Id,
                subQuestion2_2_2_1.Id,
                subQuestion2_2_2_2.Id,
                subQuestion2_2_2_4.Id,
                subQuestion2_2_3_2.Id,
                subQuestion2_2_3_3.Id,
                subQuestion2_2_3_4_2.Id,
                subQuestion2_2_3_5.Id,
                subQuestion2_2_3_6.Id,
                subQuestion2_2_3_7.Id,
                subQuestion2_2_3_8.Id,
                subQuestion2_2_3_9.Id,
                subQuestion2_2_3_11.Id,
                subQuestion2_2_3_12.Id,
                subQuestion3_3.Id,
                subQuestion3_5_1_1.Id,
                specialMainQuestion4.Id,
                specialMainQuestion8.Id,
                specialSubQuestion9_1.Id,
                specialSubQuestion11_1.Id,
                specialSubQuestion11_2.Id,
                specialMainQuestion12.Id,

            }));
        }

        public PairedMainQuestions GetDefaultPairedMainQuestion()
        {
            return GetDefaultMainQuestion(PairedMainQuestions);
        }

        public SpecialMainQuestion GetDefaultSpecialMainQuestion()
        {
            return GetDefaultMainQuestion(SpecialMainQuestions);
        }

        private T GetDefaultMainQuestion<T>(List<T> listOfElements) where T : IMainQuestion<T>
        {
            var element = listOfElements.Single(x => x.IsLastMainQuestion);

            Func<T, bool> nextMainQuestionFunc = x => x.NextMainQuestion != null && x.NextMainQuestion.Id == element.Id;

            while (listOfElements.Any(nextMainQuestionFunc))
            {
                element = listOfElements.Single(nextMainQuestionFunc);
            }

            return element;
        }

        public PairedMainQuestions GetNextPairedMainQuestions(Guid currentQuestionId)
        {
            return GetNextQuestion(PairedMainQuestions, currentQuestionId);
        }

        public SpecialMainQuestion GetNextSpecialMainQuestion(Guid currentSpecialMainQuestionId)
        {
            return GetNextQuestion(SpecialMainQuestions, currentSpecialMainQuestionId);
        }

        private T GetNextQuestion<T>(List<T> listOfElements, Guid currentQuestionId) where T : IMainQuestion<T>
        {
            if (currentQuestionId == default(Guid))
            {
                throw new ArgumentException(nameof(currentQuestionId));
            }

            var currentQuestion = listOfElements.First(x => x.Id == currentQuestionId);

            if (currentQuestion.IsLastMainQuestion)
            {
                return default(T);
            }

            return currentQuestion.NextMainQuestion;
        }

        public PairedMainQuestions GetPairedMainQuestions(Guid questionId)
        {
            return GetQuestion(PairedMainQuestions, questionId);
        }

        public SpecialMainQuestion GetSpecialMainQuestion(Guid questionId)
        {
            return GetQuestion(SpecialMainQuestions, questionId);
        }

        private T GetQuestion<T>(List<T> listOfElements, Guid questionId) where T : IMainQuestion<T>
        {
            if (questionId == default(Guid))
            {
                throw new ArgumentException(nameof(questionId));
            }

            return listOfElements.Single(x => x.Id == questionId);
        }

        public IReadOnlyCollection<SubQuestion> GetSubQuestions(Guid parentQuestionId)
        {
            return GetSubQuestions(SubQuestions, parentQuestionId);
        }

        public IReadOnlyCollection<SpecialSubQuestion> GetSpecialSubQuestions(Guid mainQuestionId)
        {
            return GetSubQuestions(SpecialSubQuestions, mainQuestionId);
        }

        private IReadOnlyCollection<T> GetSubQuestions<T>(List<T> listOfElements, Guid mainQuestionId) where T : ISubQuestion
        {
            if (mainQuestionId == default(Guid))
            {
                throw new ArgumentException(nameof(mainQuestionId));
            }

            var subQuestions = listOfElements
                .Where(x => x.ParentQuestion != null && x.ParentQuestion.Id == mainQuestionId);

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

            var classification = Classifications.Single(x => x.QuestionIds.Contains(questionId));

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
        PairedMainQuestions GetDefaultPairedMainQuestion();
        PairedMainQuestions GetNextPairedMainQuestions(Guid currentQuestionId);
        PairedMainQuestions GetPairedMainQuestions(Guid questionId);
        IReadOnlyCollection<SubQuestion> GetSubQuestions(Guid parentQuestionId);
        Classification GetClassification(Guid questionId);
        Classification GetClassification(string text);
        SpecialMainQuestion GetDefaultSpecialMainQuestion();
        SpecialMainQuestion GetNextSpecialMainQuestion(Guid currentSpecialMainQuestionId);
        SpecialMainQuestion GetSpecialMainQuestion(Guid questionId);
        IReadOnlyCollection<SpecialSubQuestion> GetSpecialSubQuestions(Guid mainQuestionId);
    }
}