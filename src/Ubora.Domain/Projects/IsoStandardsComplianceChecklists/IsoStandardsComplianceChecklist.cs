using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Ubora.Domain.Projects.IsoStandardsComplianceChecklists.Events;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Questionnaires.ApplicableRegulations;

namespace Ubora.Domain.Projects.IsoStandardsComplianceChecklists
{
    /// <summary>
    /// WP4 step
    /// </summary>
    public class IsoStandardsComplianceChecklist : IProjectEntity
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public ImmutableList<IsoStandard> IsoStandards { get; private set; } = ImmutableList<IsoStandard>.Empty;

        private void Apply(WorkpackageFourOpenedEvent @event)
        {
            ProjectId = @event.ProjectId;

            IsoStandards = IsoStandards.AddRange(GetIsoStandardsApplicableToAllProjects());

            if (@event.LatestFinishedApplicableRegulationsQuestionnaire != null)
            {
                IsoStandards = IsoStandards.AddRange(GetIsoStandardsBasedOnQuestionnaire(@event.LatestFinishedApplicableRegulationsQuestionnaire));
            }
        }

        private void Apply(IsoStandardAddedToComplianceChecklistEvent @event)
        {
            if (@event.AggregateId != Id)
                throw new InvalidOperationException();

            if (@event.ProjectId != ProjectId)
                throw new InvalidOperationException();

            var value = new IsoStandard(@event.IsoStandardId, @event.Title, @event.Link, @event.ShortDescription, @event.InitiatedBy.UserId);
            IsoStandards = IsoStandards.Add(value);
        }

        private void Apply(IsoStandardRemovedFromComplianceChecklistEvent @event)
        {
            if (@event.AggregateId != Id)
                throw new InvalidOperationException();

            if (@event.ProjectId != ProjectId)
                throw new InvalidOperationException();

            var value = IsoStandards.Single(iso => iso.Id == @event.IsoStandardId);
            IsoStandards = IsoStandards.Remove(value);
        }

        private void Apply(IsoStandardMarkedAsCompliantEvent @event)
        {
            if (@event.AggregateId != Id)
                throw new InvalidOperationException();

            if (@event.ProjectId != ProjectId)
                throw new InvalidOperationException();

            var oldValue = IsoStandards.Single(iso => iso.Id == @event.IsoStandardId);
            var newValue = oldValue.MarkAsCompliant();
            IsoStandards = IsoStandards.Replace(oldValue, newValue);
        }

        private void Apply(IsoStandardMarkedAsNoncompliantEvent @event)
        {
            if (@event.AggregateId != Id)
                throw new InvalidOperationException();

            if (@event.ProjectId != ProjectId)
                throw new InvalidOperationException();

            var oldValue = IsoStandards.Single(iso => iso.Id == @event.IsoStandardId);
            var newValue = oldValue.MarkAsNoncompliant();
            IsoStandards = IsoStandards.Replace(oldValue, newValue);
        }

        /// <remarks>Definitely extract this logic when there are multiple versions of the questionnaire.</remarks>
        private IEnumerable<IsoStandard> GetIsoStandardsBasedOnQuestionnaire(ApplicableRegulationsQuestionnaireTree questionnaire)
        {
            var questionsAnsweredYesIds = questionnaire.AnsweredQuestions.Where(q => q.ChosenAnswer.IsYes()).Select(q => q.Id);

            foreach (var questionId in questionsAnsweredYesIds)
            {
                switch (questionId)
                {
                    case "q1":
                        yield return new IsoStandard(
                            Guid.Parse("32dd2f9c-42a9-442c-96d0-a728d44f666d"),
                            "EN ISO 14630:2012",
                            new Uri("http://shop.bsigroup.com/ProductDetail?pid=000000000030261816"),
                            "This standard specifies requirements for all devices that are implanted in the human body but are not active.");
                        break;

                    case "q2":
                        yield return new IsoStandard(
                            Guid.Parse("7fae1bd7-13c2-41ea-a2fe-6023a55a683c"),
                            "IEC 60601-1:2005+AMD1:2012 CSV (consolidated version)",
                            new Uri("https://webstore.iec.ch/publication/2612"),
                            "This standard specifies requirements for electromedical devices; it has more than 60 related publications, that describe very specific areas of electromedical devices.");
                        break;

                    case "q2_1":
                        yield return new IsoStandard(
                            Guid.Parse("3531dcfc-3f2b-40e0-8be5-73a22fdf39ee"),
                            "EN 62304:2006+A1:2015",
                            new Uri("http://shop.bsigroup.com/ProductDetail?pid=000000000030287754"),
                            "This standard specifies how to design and code software for medical devices and sets requirements for SW change control.");
                        break;

                    case "q2_1_1":
                        yield return new IsoStandard(
                            Guid.Parse("f28ff7bb-dacf-43c0-b11f-5cadf0ccfb02"),
                            "BS EN 80001-1:2011",
                            new Uri("http://shop.bsigroup.com/ProductDetail?pid=000000000030157836"));
                        yield return new IsoStandard(
                            Guid.Parse("9de49e25-bb18-449f-abbc-5a6cd0fda180"),
                            "PD IEC/TR 80001-2-1:2012",
                            new Uri("http://shop.bsigroup.com/ProductDetail?pid=000000000030244875"));
                        break;

                    case "q3":
                        yield return new IsoStandard(
                            Guid.Parse("9de49e25-bb18-449f-abbc-5a6cd0fda180"),
                            "ISO 14708-1:2014",
                            new Uri("http://shop.bsigroup.com/SearchResults/?q=14708"),
                            "This standard specifies requirements for all devices that are implanted in the human body and are also active; there are specific chapters for some medical devices (for example, -2 for cardiac pacemakers).");
                        break;

                    case "q4":
                        break;

                    case "q4_1":
                        yield return new IsoStandard(
                            Guid.Parse("1ca891fd-13f9-4d69-909d-586aff9331ef"),
                            "EN ISO 11607-1:2009+A1:2014",
                            new Uri("http://shop.bsigroup.com/ProductDetail?pid=000000000030255136"),
                            "This standard specifies how the devices shall be packaged to allow sterilization and ensure that they remain sterile.");
                        break;

                    case "q4_1_1":
                        yield return new IsoStandard(
                            Guid.Parse("9b1a0bcb-2241-4e61-b427-e67136c7c8f3"),
                            "EN ISO 11135:2014",
                            new Uri("http://shop.bsigroup.com/ProductDetail?pid=000000000030318543"));
                        break;

                    case "q4_1_2":
                        yield return new IsoStandard(
                            Guid.Parse("b6413266-df22-4fa1-a6c6-a0fd0ad78ed4"),
                            "ISO 11137",
                            new Uri("https://www.iso.org/obp/ui/#iso:std:iso:11137:-1:ed-1:v1:en"));
                        break;

                    case "q4_1_3":
                        yield return new IsoStandard(
                            Guid.Parse("b5882864-1d52-4092-b655-62a0f8402a23"),
                            "ISO 17665-1:2006(en)",
                            new Uri("https://www.iso.org/obp/ui/#iso:std:iso:17665:-1:ed-1:v1:en"));
                        break;

                    case "q4_1_4":
                        yield return new IsoStandard(
                            Guid.Parse("20033733-fb34-48fc-9a65-5d326145ded2"),
                            "ISO 20857:2010(en)",
                            new Uri("https://www.iso.org/obp/ui/#iso:std:iso:20857:ed-1:v1:en"));
                        break;

                    case "q4_2":
                        yield return new IsoStandard(
                            Guid.Parse("4d798e6d-8698-4830-b645-7bc2d99f6c78"),
                            "ISO 13408-1:2008(en)",
                            new Uri("https://www.iso.org/obp/ui/#iso:std:iso:13408:-1:ed-2:v1:en"));
                        break;

                    case "q5":
                        yield return new IsoStandard(
                            Guid.Parse("e149e7a7-1216-4e8d-86e1-82a0a5eed542"),
                            "ISO 10993-1",
                            new Uri("https://www.iso.org/obp/ui/#iso:std:iso:10993:-1:ed-4:v1:en"));
                        break;

                    case "q5_1":
                        yield return new IsoStandard(
                            Guid.Parse("57e59214-29f9-4097-9d75-08e32db435c4"),
                            "ISO 22442-1:2015(en)",
                            new Uri("https://www.iso.org/obp/ui/#iso:std:iso:22442:-1:ed-2:v1:en"));
                        break;
                }
            }
        }

        private IEnumerable<IsoStandard> GetIsoStandardsApplicableToAllProjects()
        {
            yield return new IsoStandard(
                Guid.Parse("c4b42810-1223-4009-8ecf-f835f6f9e5e6"),
                "EN ISO 13485:2016",
                new Uri("http://shop.bsigroup.com/ProductDetail?pid=000000000030353196"),
                "This standard specifies requirements for all entities involved in medical devices, in all stages of the product life cycle: from design to manufacture to installation to disposal. Ubora Platform is structured to be a guideline for design activities in compliance to this standard.");

            yield return new IsoStandard(
                Guid.Parse("329fe3f6-665d-41a5-b4de-58dff0b0f558"),
                "EN ISO 14971:2012",
                new Uri("http://shop.bsigroup.com/ProductDetail?pid=000000000030268035"),
                "This standard specifies requirements for designers and manufacturers of medical devices, in order to minimize the risk of the device itself. There is no “risk zero” device but many activities can be implemented to reduce and manage risk. This standard provides useful checklists and also guidance on the most widespread risk management techniques such as FMEA.");

            yield return new IsoStandard(
                Guid.Parse("e457a508-5042-465b-8bf4-0c3c6c76b40e"),
                "MEDDEV 2.7.1 rev 4 CLINICAL EVALUATION: A GUIDE FOR MANUFACTURERS AND NOTIFIED BODIES UNDER DIRECTIVES 93/42/EEC and 90/385/EEC",
                new Uri("http://ec.europa.eu/docsroom/documents/17522/attachments/1/translations/en/renditions/native"),
                "This guideline provides information on methods used to assess the clinical performance and the clinical benefit of a medical device.");

            yield return new IsoStandard(
                Guid.Parse("da747f2f-d256-4d91-b76a-a92f9407a2d0"),
                "IEC 62366-1",
                new Uri("https://www.iso.org/obp/ui/#iso:std:iec:62366:-1:ed-1:v1:en,fr"),
                "This standard provides guidance on how to manage the human factors while designing a medical device (usability engineering).");

            yield return new IsoStandard(
                Guid.Parse("b7857158-89b3-4473-bf19-825645fe4808"),
                "EN ISO 15223-1:2016",
                new Uri("http://shop.bsigroup.com/ProductDetail/?pid=000000000030358535"),
                "This standard lists a series of symbols that may be applicable in labels of medical devices.");
        }
    }
}