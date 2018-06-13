using System;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Assignments
{
    public class AssignmentListViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<AssignmentListItemViewModel> Assignments { get; set; }
    }

    public class AssignmentListItemViewModel
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public Guid CreatedByUserId { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}