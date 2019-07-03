using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Marten.Events;
using Moq;
using Ubora.Domain.Projects.History;
using Ubora.Domain.Projects.History.SortSpecifications;
using Ubora.Domain.Projects._Events;
using Xunit;

namespace Ubora.Domain.Tests.Projects.History.SortSpecifications
{
    public class SortByTimestampDescendingSpecificationTests
    {
        [Fact]
        public void Sorts_By_Timestamp()
        {
            var eventLog1 = EventLogEntry.FromEvent(new Mock<Event<ProjectEvent>>(new Mock<ProjectEvent>(new DummyUserInfo(), Guid.NewGuid()).Object).Object)
                .Set(l => l.Timestamp, DateTimeOffset.UtcNow);
            var eventLog2 = EventLogEntry.FromEvent(new Mock<Event<ProjectEvent>>(new Mock<ProjectEvent>(new DummyUserInfo(), Guid.NewGuid()).Object).Object)
                .Set(l => l.Timestamp, DateTimeOffset.UtcNow.AddMinutes(1));
            var eventLog3 = EventLogEntry.FromEvent(new Mock<Event<ProjectEvent>>(new Mock<ProjectEvent>(new DummyUserInfo(), Guid.NewGuid()).Object).Object)
                .Set(l => l.Timestamp, DateTimeOffset.UtcNow.AddMinutes(2));

            var sut = new SortByTimestampDescendingSpecification();

            // Act
            var sortedResult = sut.Sort(new[] {eventLog1, eventLog3, eventLog2}.AsQueryable());

            // Assert
            sortedResult.ShouldBeEquivalentTo(new[] {eventLog3, eventLog2, eventLog1});
        }
    }
}
