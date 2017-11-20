using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ubora.Domain.Projects.Candidates;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class CandidateViewModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool HasImage { get; set; }

        public AddCommentViewModel AddCommentViewModel { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public class Factory
        {
            private readonly IMapper _mapper;
            private readonly ImageStorageProvider _imageStorageProvider;
            private readonly CommentViewModel.Factory _commentFactory;

            public Factory(IMapper mapper, ImageStorageProvider imageStorageProvider, CommentViewModel.Factory commentFactory)
            {
                _mapper = mapper;
                _imageStorageProvider = imageStorageProvider;
                _commentFactory = commentFactory;
            }

            protected Factory()
            {
            }

            public virtual async Task<CandidateViewModel> Create(Candidate candidate, ClaimsPrincipal user)
            {
                var model = _mapper.Map<CandidateViewModel>(candidate);
                model.ImageUrl = _imageStorageProvider.GetDefaultOrBlobImageUrl(candidate.ImageLocation, ImageSize.Thumbnail400x300);
                model.AddCommentViewModel = new AddCommentViewModel
                {
                    CandidateId = candidate.Id
                };

                var comments = candidate.Comments.Select(async comment => await _commentFactory.Create(user, comment, candidate.Id));
                model.Comments = await Task.WhenAll(comments);

                return model;
            }
        }
    }
}
