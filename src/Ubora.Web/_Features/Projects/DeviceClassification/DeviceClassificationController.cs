using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.DeviceClassification;
using Ubora.Web._Features.Projects.DeviceClassification.Services;
using Ubora.Web._Features.Projects.DeviceClassification.ViewModels;

namespace Ubora.Web._Features.Projects.DeviceClassification
{
    public class DeviceClassificationController : ProjectController
    {
        private readonly IDeviceClassification _deviceClassification;

        public DeviceClassificationController(
            ICommandQueryProcessor processor,
            IDeviceClassificationProvider deviceClassificationProvider) : base(processor)
        {
            _deviceClassification = deviceClassificationProvider.Provide();
        }

        public IActionResult GetPairedMainQuestions(Guid? pairedMainQuestionsId)
        {
            if (pairedMainQuestionsId == null)
            {
                var initialPairedMainQuestions = _deviceClassification.GetDefaultPairedMainQuestion();

                var initialMainQuestionViewModel = new PairedMainQuestionsViewModel
                {
                    PairedQuestionId = initialPairedMainQuestions.Id,
                    MainQuestionOne = initialPairedMainQuestions.MainQuestionOne?.Text,
                    MainQuestionOneId = initialPairedMainQuestions.MainQuestionOne == null ? Guid.Empty : initialPairedMainQuestions.MainQuestionOne.Id,
                    MainQuestionTwo = initialPairedMainQuestions.MainQuestionTwo?.Text,
                    MainQuestionTwoId = initialPairedMainQuestions.MainQuestionTwo == null ? Guid.Empty : initialPairedMainQuestions.MainQuestionTwo.Id,
                    Notes = initialPairedMainQuestions.GetNotes()
                };

                return View(initialMainQuestionViewModel);
            }

            var pairedMainQuestions = _deviceClassification.GetPairedMainQuestions(pairedMainQuestionsId.Value);

            var mainQuestionViewModel = new PairedMainQuestionsViewModel
            {
                PairedQuestionId = pairedMainQuestions.Id,
                MainQuestionOne = pairedMainQuestions.MainQuestionOne?.Text,
                MainQuestionOneId = pairedMainQuestions.MainQuestionOne == null ? Guid.Empty : pairedMainQuestions.MainQuestionOne.Id,
                MainQuestionTwo = pairedMainQuestions.MainQuestionTwo?.Text,
                MainQuestionTwoId = pairedMainQuestions.MainQuestionTwo == null ? Guid.Empty : pairedMainQuestions.MainQuestionTwo.Id,
                Notes = pairedMainQuestions.GetNotes()
            };

            return View(mainQuestionViewModel);
        }

        public IActionResult GetQuestions(Guid parentQuestionId, Guid? pairedMainQuestionsId)
        {
            if (parentQuestionId == default(Guid))
            {
                return BadRequest();
            }

            var questions = _deviceClassification.GetSubQuestions(parentQuestionId);

            if (questions == null)
            {
                return BadRequest("No sub question found!");
            }

            if (pairedMainQuestionsId == null)
            {
                pairedMainQuestionsId = questions.First().PairedMainQuestions.Id;
            }

            var questionsViewModel = new QuestionsViewModel
            {
                Questions = questions,
                PairedMainQuestionsId = pairedMainQuestionsId.Value,
                Notes = questions.GetNotes()
            };

            return View(questionsViewModel);
        }

        [HttpPost]
        public IActionResult MainQuestionAnswer(MainQuestionAnswerViewModel mainQuestionAnswerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("ModelState is invalid");
            }

            var subQuestions = _deviceClassification.GetSubQuestions(mainQuestionAnswerViewModel.MainQuestionId);
            if (subQuestions != null)
            {
                return RedirectToAction(nameof(GetQuestions), new { parentQuestionId = mainQuestionAnswerViewModel.MainQuestionId, pairedMainQuestionsId = mainQuestionAnswerViewModel.PairedQuestionId });
            }

            return RedirectToAction(nameof(NextMainQuestion), new { pairedMainQuestionsId = mainQuestionAnswerViewModel.PairedQuestionId });
        }

        [HttpPost]
        public IActionResult Answer(AnswerViewModel answerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("ModelState is invalid");
            }

            var questions = _deviceClassification.GetSubQuestions(answerViewModel.NextQuestionId);

            if (questions == null)
            {
                SetDeviceClassificationToProject(answerViewModel.NextQuestionId);

                if (!ModelState.IsValid)
                {
                    return BadRequest("ModelState is invalid");
                }

                return RedirectToAction(nameof(NextMainQuestion), "DeviceClassification", new { pairedMainQuestionsId = answerViewModel.PairedMainQuestionsId });
            }

            return RedirectToAction(nameof(GetQuestions), "DeviceClassification", new { parentQuestionId = answerViewModel.NextQuestionId, pairedMainQuestionsId = answerViewModel.PairedMainQuestionsId });
        }

        [HttpPost]
        public IActionResult SpecialQuestionAnswer(SpecialAnswerViewModel specialAnswerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("ModelState is invalid");
            }

            var subQuestion = _deviceClassification.GetSpecialSubQuestions(specialAnswerViewModel.NextQuestionId);

            if (subQuestion == null)
            {
                SetDeviceClassificationToProject(specialAnswerViewModel.NextQuestionId);

                if (!ModelState.IsValid)
                {
                    return BadRequest("ModelState is invalid");
                }

                return RedirectToAction(nameof(NextSpecialMainQuestion), "DeviceClassification", new { currentSpecialMainQuestionId = specialAnswerViewModel.MainQuestionId });
            }

            return RedirectToAction(nameof(GetSpecialSubQuestions), new { mainQuestionId = specialAnswerViewModel.MainQuestionId });
        }

        public IActionResult NextMainQuestion(Guid pairedMainQuestionsId)
        {
            if (pairedMainQuestionsId == default(Guid))
            {
                return BadRequest();
            }

            var nextMainQuestion = _deviceClassification.GetNextPairedMainQuestions(pairedMainQuestionsId);

            if (nextMainQuestion == null)
            {
                return RedirectToAction(nameof(GetSpecialMainQuestion));
            }

            return RedirectToAction(nameof(GetPairedMainQuestions), "DeviceClassification", new { pairedMainQuestionsId = nextMainQuestion.Id });
        }

        public IActionResult CurrentClassification()
        {
            var currentClassificationViewModel = new CurrentClassificationViewModel
            {
                Classification = Project.DeviceClassification
            };

            return View(currentClassificationViewModel);
        }

        public IActionResult NoClass()
        {
            return View();
        }

        public IActionResult GetSpecialMainQuestion(Guid? specialMainQuestionId)
        {
            if (specialMainQuestionId == null)
            {
                var defaultSpecialMainQuestion = _deviceClassification.GetDefaultSpecialMainQuestion();

                var defaultSpecialMainQuestionViewModel = new SpecialMainQuestionViewModel
                {
                    QuestionText = defaultSpecialMainQuestion.Text,
                    CurrentSpecialMainQuestionId = defaultSpecialMainQuestion.Id,
                    Note = defaultSpecialMainQuestion.GetNote()
                };

                return View(defaultSpecialMainQuestionViewModel);
            }

            var specialMainQuestion = _deviceClassification.GetSpecialMainQuestion(specialMainQuestionId.Value);

            var specialMainQuestionViewModel = new SpecialMainQuestionViewModel
            {
                QuestionText = specialMainQuestion.Text,
                CurrentSpecialMainQuestionId = specialMainQuestion.Id,
                Note = specialMainQuestion.GetNote()
            };

            return View(specialMainQuestionViewModel);
        }

        public IActionResult NextSpecialMainQuestion(Guid currentSpecialMainQuestionId)
        {
            if (currentSpecialMainQuestionId == default(Guid))
            {
                return BadRequest();
            }

            var nextSpecialMainQuestion = _deviceClassification.GetNextSpecialMainQuestion(currentSpecialMainQuestionId);

            if (nextSpecialMainQuestion == null)
            {
                if (string.IsNullOrEmpty(Project.DeviceClassification))
                {
                    return RedirectToAction(nameof(NoClass), "DeviceClassification");
                }

                return RedirectToAction(nameof(CurrentClassification), "DeviceClassification");
            }

            return RedirectToAction(nameof(GetSpecialMainQuestion), new { specialMainQuestionId = nextSpecialMainQuestion.Id });
        }

        public IActionResult GetSpecialSubQuestions(Guid mainQuestionId)
        {
            if (mainQuestionId == default(Guid))
            {
                return BadRequest();
            }

            var subQuestions = _deviceClassification.GetSpecialSubQuestions(mainQuestionId);

            if (subQuestions == null)
            {
                return BadRequest("No sub question found!");
            }

            var questionsViewModel = new SpecialSubQuestionsViewModel
            {
                Questions = subQuestions,
                MainQuestionId = mainQuestionId,
                Notes = subQuestions.GetNotes()
            };

            return View(questionsViewModel);
        }

        private void SetDeviceClassificationToProject(Guid questionId)
        {
            var currentClassification = _deviceClassification.GetClassification(questionId);

            var command = new SetDeviceClassificationForProjectCommand
            {
                ProjectId = ProjectId,
                DeviceClassification = currentClassification,
                Actor = this.UserInfo
            };

            ExecuteUserProjectCommand(command);
        }
    }
}