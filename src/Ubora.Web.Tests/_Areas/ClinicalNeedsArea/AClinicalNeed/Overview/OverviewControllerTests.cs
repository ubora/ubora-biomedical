using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ubora.Domain.ClinicalNeeds;
using Ubora.Web.Tests._Features;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview.Models;
using Xunit;

namespace Ubora.Web.Tests._Areas.ClinicalNeedsArea.AClinicalNeed.Overview
{
    public class OverviewControllerTests : UboraControllerTestsBase
    {
        private readonly Mock<OverviewController> _controllerMock;
        private readonly OverviewController _controller;

        public OverviewControllerTests()
        {
            _controllerMock = new Mock<OverviewController>
            {
                CallBase = true
            };
            _controller = _controllerMock.Object;
            SetUpForTest(_controller);

        }

        [Fact]
        public void Overview_Returns_View_With_Model()
        {
            var clinicalNeed = new ClinicalNeed()
                    .Set(x => x.IndicatedAt, DateTimeOffset.Now)
                    .Set(x => x.Keywords, "keywords")
                    .Set(x => x.PotentialTechnologyTag, "tech")
                    .Set(x => x.AreaOfUsageTag, "area")
                    .Set(x => x.ClinicalNeedTag, "clinical");

            _controller.ClinicalNeed = clinicalNeed;

            // Act
            var result = (ViewResult) _controller.Overview();

            // Assert
            var model = (OverviewViewModel) result.Model;

            model.ShouldBeEquivalentTo(clinicalNeed);
        }
    }
}
