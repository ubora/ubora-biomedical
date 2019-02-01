using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Moq;
using Ubora.Domain;
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
        public async Task Overview_Returns_View_With_Model()
        {
            var description = new QuillDelta("{description}");

            NodeServicesMock
                .Setup(ns => ns.InvokeAsync<string>("./Scripts/backend/ConvertQuillDeltaToHtml.js", It.Is<object[]>(
                    a => a.Single().Equals("{description}"))))
                .ReturnsAsync("DescriptionQuillDelta");


            var clinicalNeed = 
                new ClinicalNeed()
                    .Set(x => x.IndicatedAt, DateTimeOffset.Now)
                    .Set(x => x.Keywords, "keywords")
                    .Set(x => x.PotentialTechnologyTag, "tech")
                    .Set(x => x.AreaOfUsageTag, "area")
                    .Set(x => x.ClinicalNeedTag, "clinical")
                    .Set(x => x.Description, description);

            _controller.Set(c => c.ClinicalNeed, clinicalNeed);

            // Act
            var result = (ViewResult) await _controller.Overview(Mock.Of<INodeServices>());

            // Assert
            var model = (OverviewViewModel) result.Model;

            model.ShouldBeEquivalentTo(clinicalNeed, opt => opt.Excluding(x => x.DescriptionQuillDelta));
            model.DescriptionQuillDelta.Should().Be("DescriptionQuillDelta");
        }
    }
}
