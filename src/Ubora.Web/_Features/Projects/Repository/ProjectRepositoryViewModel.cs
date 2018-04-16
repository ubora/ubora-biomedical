using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Projects.Repository;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web._Features.Projects.Repository
{
    public class ProjectRepositoryViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public AddFileViewModel AddFileViewModel { get; set; }
        public bool IsProjectLeader { get; set; }
        public IEnumerable<IGrouping<string, ProjectFileViewModel>> AllFiles { get; set; }
    }

    public class ProjectFileViewModel
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Comment { get; set; }
        public long FileSize { get; set; }
        public int RevisionNumber { get; set; }
        public long FileSizeInKbs
        {
            get
            {
                return FileSize / 1000;
            }
        }

        public class Factory
        {
            private readonly IUboraStorageProvider _uboraStorageProvider;
            private readonly IMapper _mapper;

            public Factory(IUboraStorageProvider uboraStorageProvider, IMapper mapper)
            {
                _uboraStorageProvider = uboraStorageProvider;
                _mapper = mapper;
            }

            public virtual ProjectFileViewModel Create(ProjectFile projectFile)
            {
                var projectFileViewModel = _mapper.Map<ProjectFileViewModel>(projectFile);
                return projectFileViewModel;
            }
        }

        public bool Has3DFileExtension()
        {
            var extension = System.IO.Path.GetExtension(FileName);

            var supported3dFileExtensions = new[] { ".stl", ".amf", ".nxz" };
            return supported3dFileExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }
    }
}
