using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;

namespace Ubora.Web._Features.Projects.List
{
    public class ProjectListViewModel
    {
        protected ProjectListViewModel()
        {
        }

        public string Header { get; protected set; }
        public IEnumerable<ProjectListItem> Projects { get; protected set; }

        public class ProjectListItem
        {
            public Guid Id { get; protected set; }
            public string Title { get; protected set; }
        }

        public class Factory
        {
            private readonly IMapper _mapper;
            private readonly IQueryProcessor _queryProcessor;

            public Factory(IQueryProcessor queryProcessor, IMapper mapper)
            {
                _queryProcessor = queryProcessor;
                _mapper = mapper;
            }

            public ProjectListViewModel Create(string header)
            {
                var projects = _queryProcessor.Find<Project>();

                var model = new ProjectListViewModel
                {
                    Header = header,
                    Projects = projects.Select(_mapper.Map<ProjectListItem>)
                };

                return model;
            }

            public ProjectListViewModel Create(string header, Guid userId)
            {
                var projects = _queryProcessor.Find<Project>().Where(x => x.Members.Any(m => m.UserId == userId));

                var model = new ProjectListViewModel
                {
                    Header = header,
                    Projects = projects.Select(_mapper.Map<ProjectListItem>)
                };

                return model;
            }
        }
    }
}