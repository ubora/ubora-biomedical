using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.Questionnaires.DeviceClassifications;
using Ubora.Domain.Questionnaires.DeviceClassifications.Commands;
using Ubora.Web.Authorization;
using Ubora.Web._Features.Projects.Workpackages;
using Ubora.Web._Features.Projects._Shared;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Projects.DeviceClassifications
{
    public class DeviceClassificationsController : ProjectController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Title"] = "Device classification";
            ViewData[nameof(ProjectMenuOption)] = ProjectMenuOption.Workpackages;
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.DeviceClassification;
        }

        public virtual IActionResult Index([FromServices]DeviceClassificationIndexViewModel.Factory modelFactory)
        {
            var model = modelFactory.Create(this.ProjectId);

            return View("DeviceClassificationIndex", model);
        }

        [HttpPost]
        [Authorize(Policies.CanEditWorkpackageOne)]
        public IActionResult Start([FromServices]DeviceClassificationIndexViewModel.Factory modelFactory)
        {
            var id = Guid.NewGuid();
            ExecuteUserProjectCommand(new StartClassifyingDeviceCommand
            {
                Id = id
            }, Notice.Success(SuccessTexts.DeviceClassificationStarted));

            if (!ModelState.IsValid)
            {
                return Index(modelFactory);
            }

            return RedirectToAction(nameof(Current), new { questionnaireId = id });
        }

        public IActionResult Current(Guid questionnaireId)
        {
            var questionnaire = QueryProcessor.FindById<DeviceClassificationAggregate>(questionnaireId);
            if (questionnaire == null)
            {
                return NotFound(questionnaireId);
            }

            if (questionnaire.IsFinished)
            {
                return RedirectToAction(nameof(Review), new { questionnaireId });
            }

            var nextQuestion = questionnaire.QuestionnaireTree.FindNextUnansweredQuestion();

            return RedirectToAction(nameof(ViewQuestion), new { questionnaireId, questionId = nextQuestion.Id });
        }

        public virtual IActionResult ViewQuestion(Guid questionnaireId, string questionId, [FromServices]DeviceClassificationQuestionViewModel.Factory modelFactory)
        {
            var questionnaireAggregate = QueryProcessor.FindById<DeviceClassificationAggregate>(questionnaireId);
            if (questionnaireAggregate == null)
            {
                return NotFound();
            }

            var models = modelFactory.Create(questionnaireAggregate, questionId);

            return View("ViewQuestion", models);
        }

        [HttpPost]
        public IActionResult Answer(AnswerDeviceClassificationQuestionPostModel model, [FromServices]DeviceClassificationQuestionViewModel.Factory modelFactory)
        {
            if (!ModelState.IsValid)
            {
                return ViewQuestion(model.QuestionnaireId, model.QuestionId, modelFactory);
            }

            var questionnaireAggregate = QueryProcessor.FindById<DeviceClassificationAggregate>(model.QuestionnaireId);
            if (questionnaireAggregate == null)
            {
                return NotFound();
            }

            ExecuteUserProjectCommand(new AnswerDeviceClassificationCommand
            {
                AnswerId = model.AnswerId,
                QuestionId = model.QuestionId,
                QuestionnaireId = model.QuestionnaireId
            }, Notice.None("There is enough visual feedback."));

            if (!ModelState.IsValid)
            {
                ModelState.Remove(nameof(model.QuestionId)); // When the question is already answered, we want a new form created with the new question ID input.
                return ViewQuestion(model.QuestionnaireId, model.QuestionId, modelFactory);
            }

            return RedirectToAction(nameof(Current), new { model.QuestionnaireId });
        }

        public IActionResult Review(Guid questionnaireId, [FromServices]DeviceClassificationReviewViewModel.Factory modelFactory)
        {
            var questionnaireAggregate = QueryProcessor.FindById<DeviceClassificationAggregate>(questionnaireId);
            if (questionnaireAggregate == null)
            {
                return NotFound();
            }

            if (!questionnaireAggregate.IsFinished)
            {
                return RedirectToAction(nameof(Current), new { questionnaireId });
            }

            var model = modelFactory.Create(questionnaireAggregate);
            return View("Review", model);
        }

        [HttpPost]
        [Authorize(Policies.CanEditWorkpackageOne)]
        public IActionResult Retake(Guid oldQuestionnaireId, [FromServices]DeviceClassificationIndexViewModel.Factory modelFactory)
        {
            var deviceClassification = QueryProcessor.FindById<DeviceClassificationAggregate>(oldQuestionnaireId);
            if (deviceClassification == null)
            {
                return NotFound();
            }

            var newQuestionnaireId = Guid.NewGuid();
            ExecuteUserProjectCommand(new StopAndStartDeviceClassificationCommand
            {
                StopQuestionnaireId = oldQuestionnaireId,
                StartQuestionnaireId = newQuestionnaireId
            }, Notice.Success(SuccessTexts.DeviceClassificationRetaken));

            if (!ModelState.IsValid)
            {
                return Index(modelFactory);
            }

            return RedirectToAction(nameof(Current), new { questionnaireId = newQuestionnaireId });
        }
    }
}
