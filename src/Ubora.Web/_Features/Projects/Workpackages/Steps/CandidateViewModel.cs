using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Projects.Candidates;
using Ubora.Web.Authorization;
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
        public decimal TotalScore { get; set; }
        public decimal ScorePercentageVeryGood { get; set; }
        public decimal ScorePercentageGood { get; set; }
        public decimal ScorePercentageMediocre { get; set; }
        public decimal ScorePercentagePoor { get; set; }
        public bool IsVotingAllowed { get; set; }

        public AddCommentViewModel AddCommentViewModel { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public AddVoteViewModel AddVoteViewModel { get; set; }

        public class Factory
        {
            private readonly IMapper _mapper;
            private readonly ImageStorageProvider _imageStorageProvider;
            private readonly CommentViewModel.Factory _commentFactory;
            private readonly IAuthorizationService _authorizationService;

            public Factory(IMapper mapper, ImageStorageProvider imageStorageProvider, CommentViewModel.Factory commentFactory, IAuthorizationService authorizationService)
            {
                _mapper = mapper;
                _imageStorageProvider = imageStorageProvider;
                _commentFactory = commentFactory;
                _authorizationService = authorizationService;
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
                model.AddVoteViewModel = new AddVoteViewModel
                {
                    CandidateId = candidate.Id
                };

                CalculateScorePercentages(model, candidate);

                var comments = candidate.Comments.Select(async comment => await _commentFactory.Create(user, comment, candidate.Id));
                model.Comments = await Task.WhenAll(comments);

                model.IsVotingAllowed = (await _authorizationService.AuthorizeAsync(user, candidate, Policies.CanVoteCandidate)).Succeeded;

                return model;
            }

            public void CalculateScorePercentages(CandidateViewModel model, Candidate candidate)
            {
                var votes = candidate.Votes;
                if (!votes.Any())
                {
                    return;
                }
                var allVoteCount = candidate.Votes.Count;

                var veryGoodVotesCount = votes.Count(x => x.Score > 15);
                var goodVotesCount = votes.Count(x => x.Score > 10 && x.Score < 16);
                var mediocreVotesCount = votes.Count(x => x.Score > 5 && x.Score < 11);
                var poorVotesCount = votes.Count(x => x.Score < 6);

                model.ScorePercentageVeryGood = (decimal) veryGoodVotesCount / allVoteCount * 100;
                model.ScorePercentageGood = (decimal) goodVotesCount / allVoteCount * 100;
                model.ScorePercentageMediocre = (decimal) mediocreVotesCount / allVoteCount * 100;
                model.ScorePercentagePoor = (decimal) poorVotesCount / allVoteCount * 100;
            }
        }
    }

    public class AddVoteViewModel
    {
        public Guid CandidateId { get; set; }

        [Required]
        public int Functionality { get; set; }
        [Required]
        public int Performace { get; set; }
        [Required]
        public int Usability { get; set; }
        [Required]
        public int Safety { get; set; }
    }
}
