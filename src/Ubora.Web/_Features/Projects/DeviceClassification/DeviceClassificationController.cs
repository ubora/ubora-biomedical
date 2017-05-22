using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.DeviceClassification;
using Ubora.Web._Features.Projects.DeviceClassification.Services;

namespace Ubora.Web._Features.Projects.DeviceClassification
{


    public class DeviceClassificationController : ProjectController
    {
        private readonly IDeviceClassification _deviceClassification;
        private readonly IMapper _mapper;

        public DeviceClassificationController(
            ICommandQueryProcessor processor,
            IDeviceClassification deviceClassification,
            IMapper mapper) : base(processor)
        {
            _deviceClassification = deviceClassification;
            _mapper = mapper;
        }

        public IActionResult GetMainQuestion(Guid? questionId, Guid id)
        {
            if (id == default(Guid))
            {
                throw new ArgumentException(nameof(id));
            }

            if (questionId == null)
            {
                var initialMainQuestion = _deviceClassification.GetDefaultQuestion();

                var initialMainQuestionViewModel = new MainQuestionViewModel
                {
                    ProjectId = id,
                    MainQuestionText = initialMainQuestion.Text,
                    MainQuestionId = initialMainQuestion.Id
                };

                return View(initialMainQuestionViewModel);
            }

            var mainQuestion = _deviceClassification.GetMainQuestion(questionId.Value);

            var mainQuestionViewModel = new MainQuestionViewModel
            {
                ProjectId = id,
                MainQuestionText = mainQuestion.Text,
                MainQuestionId = mainQuestion.Id
            };

            return View(mainQuestionViewModel);
        }

        public IActionResult NextMainQuestion(Guid mainQuestionId, Guid id)
        {
            if (id == default(Guid))
            {
                throw new ArgumentException(nameof(id));
            }

            if (mainQuestionId == default(Guid))
            {
                throw new ArgumentException(nameof(mainQuestionId));
            }

            var nextMainQuestion = _deviceClassification.GetNextMainQuestion(mainQuestionId);

            if (nextMainQuestion == null)
            {
                var project = FindById<Project>(id);
                if (string.IsNullOrEmpty(project.DeviceClassification))
                {
                    return RedirectToAction(nameof(NoClass), "DeviceClassification", new { id = id });
                }

                return RedirectToAction(nameof(CurrentClassification), "DeviceClassification", new { id = id });
            }

            return RedirectToAction(nameof(GetMainQuestion), "DeviceClassification", new { questionId = nextMainQuestion.Id, id = id });
        }

        public IActionResult GetQuestions(Guid questionId, Guid? mainQuestionId, Guid id)
        {
            if (id == default(Guid))
            {
                throw new ArgumentException(nameof(id));
            }

            if (questionId == default(Guid))
            {
                throw new ArgumentException(nameof(questionId));
            }
            var questions = _deviceClassification.GetSubQuestions(questionId);

            if (questions == null)
            {
                return RedirectToAction(nameof(Classification), "DeviceClassification", new { questionId = questionId, mainQuestionId = mainQuestionId, id = id });
            }

            if (mainQuestionId == null)
            {
                mainQuestionId = questions.First().MainQuestionId;
            }

            var questionsViewModel = new QuestionsViewModel
            {
                ProjectId = id,
                Questions = questions,
                MainQuestionId = mainQuestionId.Value
            };

            return View(questionsViewModel);
        }

        public IActionResult CurrentClassification(Guid id)
        {
            if (id == default(Guid))
            {
                throw new ArgumentException(nameof(id));
            }

            var project = FindById<Project>(id);

            var currentClassificationViewModel = new CurrentClassificationViewModel
            {
                ClassificationText = project.DeviceClassification,
                ProjectId = id
            };

            return View(currentClassificationViewModel);
        }

        public IActionResult Classification(Guid questionId, Guid mainQuestionId, Guid id)
        {
            if (id == default(Guid))
            {
                throw new ArgumentException(nameof(id));
            }

            if (mainQuestionId == default(Guid))
            {
                throw new ArgumentException(nameof(mainQuestionId));
            }

            if (questionId == default(Guid))
            {
                throw new ArgumentException(nameof(questionId));
            }

            var currentClassification = _deviceClassification.GetClassification(questionId);

            var projectDeviceClassificationText = FindById<Project>(id).DeviceClassification;

            var command = new SaveDeviceClassificationToProjectCommand
            {
                Id = Guid.NewGuid(),
                ProjectId = id,
                DeviceClassification = currentClassification.Text,
                Actor = this.UserInfo
            };

            if (string.IsNullOrEmpty(projectDeviceClassificationText))
            {
                ExecuteUserProjectCommand(command);
                return RedirectToAction(nameof(NextMainQuestion), "DeviceClassification", new { mainQuestionId = mainQuestionId, id = id });
            }

            var projectDeviceClassification = _deviceClassification.GetClassification(projectDeviceClassificationText);

            if (currentClassification > projectDeviceClassification)
            {
                ExecuteUserProjectCommand(command);
            }

            return RedirectToAction(nameof(NextMainQuestion), "DeviceClassification", new { mainQuestionId = mainQuestionId, id = id });
        }

        public IActionResult NoClass(Guid id)
        {
            return View(id);
        }
    }
}