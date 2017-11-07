using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Web.Data;

namespace Ubora.Web._Features.Admin.Questionnaires
{
    public class Node
    {
        public string Id { get; set; }
        public IEnumerable<int> Numbers { get; set; }
        public int Row { get; set; }
        public int Cell { get; set; }
        public string Question { get; set; }
        public string[] Answers { get; set; }
    }

    public class Edge
    {
        public string ResourceName { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Answer { get; set; }
    }

    [Authorize(Roles = ApplicationRole.Admin)]
    public class QuestionnairesController : UboraController
    {
        public IActionResult DeviceClassification()
        {
            var questionnaire = DeviceClassificationQuestionnaireTreeFactory.CreateDeviceClassification();

            return View("DeviceClassification", questionnaire);
        }

        public IActionResult DeviceClassification2()
        {
            var questionnaire = DeviceClassificationQuestionnaireTreeFactory.CreateDeviceClassification();

            return View("DeviceClassification2", questionnaire);
        }

        public IActionResult DeviceClassification3()
        {
            var questionnaire = DeviceClassificationQuestionnaireTreeFactory.CreateDeviceClassification();

            return View("DeviceClassification3", questionnaire);
        }
    }
}
