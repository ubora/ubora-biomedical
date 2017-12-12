using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members.Queries;

namespace Ubora.Web._Features.Admin
{
    public class ProjectsUnderReviewViewModel
    {
        public IEnumerable<ProjectUnderReviewViewModel> ProjectsUnderReview { get; set; }
    }

    public class ProjectUnderReviewViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IReadOnlyDictionary<Guid, string> ProjectMentors { get; set; }

        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor)
            {
                _queryProcessor = queryProcessor;
            }

            public virtual ProjectUnderReviewViewModel Create(Project project)
            {
                var mentorIds = project.Members.Where(x => x.IsMentor).Select(x => x.UserId);
                var mentors = _queryProcessor.ExecuteQuery(new FindFullNamesQuery(mentorIds));

                var model = new ProjectUnderReviewViewModel
                {
                    Id = project.Id,
                    Title = project.Title,
                    ProjectMentors = mentors
                };

                return model;
            }
        }
    }
}
