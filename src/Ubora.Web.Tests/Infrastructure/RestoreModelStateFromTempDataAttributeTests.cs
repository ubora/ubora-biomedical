using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using Ubora.Web.Infrastructure;
using Ubora.Web.Tests.Helper;
using Xunit;
using System.Linq;

namespace Ubora.Web.Tests.Infrastructure
{
    public class RestoreModelStateFromTempDataAttributeTests
    {
        private TestRestoreModelStateFromTempDataAttribute _restoreModelStateFromTempDataAttribute;

        public RestoreModelStateFromTempDataAttributeTests()
        {
            _restoreModelStateFromTempDataAttribute = new TestRestoreModelStateFromTempDataAttribute();
        }

        [Fact]
        public void OnActionExecuting_Restores_To_Controller_ModelState_If_Temporary_ModelState_Exists()
        {
            var tempDataDictionary = new TestTempDataDictionary();
            tempDataDictionary[_restoreModelStateFromTempDataAttribute.Key] = "{\"Error\":\"SomeError\"}";

            var context = GetActionExecutingContextWithTempData(tempDataDictionary, out Controller controller);

            // Act
            _restoreModelStateFromTempDataAttribute.OnActionExecuting(context);

            // Assert
            var entry = controller.ModelState["Error"];
            entry.Errors.First().ErrorMessage.Should().Be("SomeError");
        }

        [Fact]
        public void OnActionExecuting_Does_Nothing_If_TempData_Is_Empty()
        {
            var tempDataDictionary = new TestTempDataDictionary();

            var context = GetActionExecutingContextWithTempData(tempDataDictionary, out Controller controller);

            // Act
            _restoreModelStateFromTempDataAttribute.OnActionExecuting(context);

            // Assert
            controller.ModelState.Should().BeEmpty();
        }

        private ActionExecutingContext GetActionExecutingContextWithTempData(TestTempDataDictionary tempDataDictionary, out Controller controller)
        {
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
            };
            controller = new Mock<Controller> { CallBase = true }.Object;
            controller.ControllerContext = controllerContext;

            controller.TempData = tempDataDictionary;

            var filterMetadata = new List<IFilterMetadata>();
            var actionArguments = new Dictionary<string, object>();
            return new ActionExecutingContext(controllerContext, filterMetadata, actionArguments, controller);
        }
    }

    internal class TestRestoreModelStateFromTempDataAttribute : RestoreModelStateFromTempDataAttribute
    {
        public new string Key => base.Key;
    }
}
