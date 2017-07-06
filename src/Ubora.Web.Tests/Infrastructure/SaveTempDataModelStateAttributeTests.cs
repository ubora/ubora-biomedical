using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Ubora.Web.Infrastructure;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using Ubora.Web.Tests.Helper;

namespace Ubora.Web.Tests.Infrastructure
{
    public class SaveTempDataModelStateAttributeTests
    {
        private TestSaveTempDataModelStateAttribute _saveTempDataModelStateAttribute;

        public SaveTempDataModelStateAttributeTests()
        {
            _saveTempDataModelStateAttribute = new TestSaveTempDataModelStateAttribute();
        }

        [Fact]
        public void OnActionExecuted_Saves_ModelState_If_ModelState_Is_Not_Null()
        {
            var modelStateDictionary = new ModelStateDictionary();
            modelStateDictionary.AddModelError("Error", "SomeError");

            var actionExecutedContext = GetActionExecutedContextWithModelState(modelStateDictionary, out ITempDataDictionary tempDataDictionary);

            // Act
            _saveTempDataModelStateAttribute.OnActionExecuted(actionExecutedContext);

            // Assert
            tempDataDictionary[_saveTempDataModelStateAttribute.Key].Should().Be("{\"Error\":\"SomeError\"}");
        }

        [Fact]
        public void OnActionExecuted_Does_Not_Save_ModelState_If_ModelState_Is_Empty()
        {
            var modelStateDictionary = new ModelStateDictionary();

            var actionExecutedContext = GetActionExecutedContextWithModelState(modelStateDictionary, out ITempDataDictionary tempDataDictionary);

            // Act
            _saveTempDataModelStateAttribute.OnActionExecuted(actionExecutedContext);

            // Assert
            tempDataDictionary.Should().BeEmpty();
        }

        private ActionExecutedContext GetActionExecutedContextWithModelState(ModelStateDictionary modelStateDictionary, out ITempDataDictionary tempDataDictionary)
        {
            var metaDataProvider = new EmptyModelMetadataProvider();
            var viewDataDictionary = new ViewDataDictionary(metaDataProvider, modelStateDictionary);

            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
            };
            var controller = new Mock<Controller> { CallBase = true }.Object;
            controller.ControllerContext = controllerContext;
            controller.ViewData = viewDataDictionary;

            tempDataDictionary = new TestTempDataDictionary();
            controller.TempData = tempDataDictionary;

            var filterMetadata = new List<IFilterMetadata>();
            return new ActionExecutedContext(controllerContext, filterMetadata, controller);
        }
    }

    internal class TestSaveTempDataModelStateAttribute : SaveTempDataModelStateAttribute
    {
        public new string Key => base.Key;
    }
}
