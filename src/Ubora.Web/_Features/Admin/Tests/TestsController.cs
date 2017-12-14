using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Ubora.Web.Data;
using Ubora.Web._Features.Notifications._Base;
using Ubora.Web._Features.Projects.History._Base;

namespace Ubora.Web._Features.Admin.Tests
{
    [Authorize(Roles = ApplicationRole.Admin)]
    public class TestsController : UboraController
    {
        public IActionResult RunTests()
        {
            ViewData["Title"] = "Manage UBORA";

            var testActions = typeof(TestsController)
                .GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(DiagnosticsTestAttribute), inherit: false).Any())
                .Select(x => new DiagnosticsTestAction
                {
                    Name = x.Name,
                    ActionUrl = Url.Action(x.Name)
                }).ToList();

            return View(testActions);
        }

        private JsonResult RunTest(Func<string> action)
        {
            try
            {
                var actionResult = action.Invoke();

                return Json(new DiagnosticsTestResult
                {
                    Message = actionResult,
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                var message = ex is DiagnosticsException
                    ? ex.Message
                    : ex.ToString();

                return Json(new DiagnosticsTestResult
                {
                    Message = message,
                    IsSuccess = false
                });
            }
        }

        [DiagnosticsTest]
        public JsonResult CanFindPartialViewsForNotificationViewModels([FromServices]IHtmlHelper htmlHelper)
        {
            ((HtmlHelper)htmlHelper).Contextualize(new ViewContext());

            var assembly = typeof(TestsController).GetTypeInfo().Assembly;

            var notificationViewModelTypes = assembly.GetTypes()
                .Where(t => typeof(INotificationViewModel).IsAssignableFrom(t))
                .Where(t =>
                {
                    var info = t.GetTypeInfo();
                    return !(info.IsInterface || info.IsAbstract);
                });

            return RunTest(() =>
            {
                foreach (var type in notificationViewModelTypes)
                {
                    var viewModel = (INotificationViewModel)Activator.CreateInstance(type);
                    try
                    {
                        viewModel.GetPartialView(htmlHelper, true);
                        viewModel.GetPartialView(htmlHelper, false);
                    }
                    catch (InvalidOperationException)
                    {
                        // InvalidOperationException is thrown when view is not found.
                        throw;
                    }
                    catch (Exception)
                    {
                    }
                }
                return "OK";
            });
        }

        [DiagnosticsTest]
        public JsonResult CanFindPartialViewsForEventViewModels([FromServices]IHtmlHelper htmlHelper)
        {
            ((HtmlHelper)htmlHelper).Contextualize(new ViewContext());

            var assembly = typeof(TestsController).GetTypeInfo().Assembly;

            var eventViewModelTypes = assembly.GetTypes()
                .Where(t => typeof(IEventViewModel).IsAssignableFrom(t))
                .Where(t =>
                {
                    var info = t.GetTypeInfo();
                    return !(info.IsInterface || info.IsAbstract);
                });

            return RunTest(() =>
            {
                foreach (var type in eventViewModelTypes)
                {
                    var viewModel = (IEventViewModel)Activator.CreateInstance(type);
                    try
                    {
                        viewModel.GetPartialView(htmlHelper);
                    }
                    catch (InvalidOperationException)
                    {
                        // InvalidOperationException is thrown when view is not found.
                        throw;
                    }
                    catch (Exception)
                    {
                    }
                }
                return "OK";
            });
        }
    }
}
