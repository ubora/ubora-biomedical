using AutoMapper;
using System;
using System.Collections.Generic;
using Ubora.Domain.Projects.Candidates;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class VotingViewModel
    {
        public IEnumerable<CandidateItemViewModel> Candidates { get; set; }
    }

    public class CandidateItemViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public class Factory
        {
            private readonly ImageStorageProvider _imageStorageProvider;
            private readonly IMapper _mapper;

            public Factory(ImageStorageProvider imageStorageProvider, IMapper mapper)
            {
                _imageStorageProvider = imageStorageProvider;
                _mapper = mapper;
            }

            public virtual CandidateItemViewModel Create(Candidate candidate)
            {
                var candidateItemViewModel = _mapper.Map<CandidateItemViewModel>(candidate);
                candidateItemViewModel.ImageUrl = _imageStorageProvider.GetDefaultOrBlobImageUrl(candidate.ImageLocation, ImageSize.Thumbnail400x300);

                return candidateItemViewModel;
            }
        }
    }
}
