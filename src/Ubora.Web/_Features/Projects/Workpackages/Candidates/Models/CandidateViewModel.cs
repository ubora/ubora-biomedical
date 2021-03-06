﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ubora.Domain.Discussions;
using Ubora.Domain.Projects.Candidates;
using Ubora.Web.Authorization;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Projects.Workpackages.Candidates
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
        public bool HasUserVoted { get; set; }
        public bool CanEditProjectCandidate { get; set; }
        public bool CanChangeProjectCandidateImage { get; set; }
        public bool CanRemoveProjectCandidateImage { get; set; }
        public bool CanRemoveCandidate { get; set; }
        public bool CanAddComment { get; set; }

        public IEnumerable<Ubora.Web._Components.Discussions.Models.CommentViewModel> Comments { get; set; }
        public AddVoteViewModel AddVoteViewModel { get; set; }
        public UserVotesViewModel UserVotesViewModel { get; set; }

        public class Factory
        {
            private readonly ImageStorageProvider _imageStorageProvider;
            private readonly CommentViewModelFactory _commentFactory;
            private readonly IAuthorizationService _authorizationService;

            public Factory(ImageStorageProvider imageStorageProvider, CommentViewModelFactory commentFactory, IAuthorizationService authorizationService)
            {
                _imageStorageProvider = imageStorageProvider;
                _commentFactory = commentFactory;
                _authorizationService = authorizationService;
            }

            protected Factory()
            {
            }

            public virtual async Task<CandidateViewModel> Create(Candidate candidate, Discussion candidateDiscussion, ClaimsPrincipal user)
            {
                var model = new CandidateViewModel();

                model.Id = candidate.Id;
                model.ProjectId = candidate.ProjectId;
                model.Title = candidate.Title;
                model.Description = candidate.Description;
                model.TotalScore = candidate.TotalScore;
                model.HasImage = candidate.HasImage;

                model.ImageUrl = _imageStorageProvider.GetDefaultOrBlobImageUrl(candidate.ImageLocation, ImageSize.Thumbnail400x300);

                model.AddVoteViewModel = new AddVoteViewModel
                {
                    CandidateId = candidate.Id
                };

                CalculateScorePercentages(model, candidate);
                
                var comments = candidateDiscussion.Comments.Select(async comment => await _commentFactory.Create(user, comment, candidate.Id));
                model.Comments = await Task.WhenAll(comments);

                model.IsVotingAllowed = await _authorizationService.IsAuthorizedAsync(user, candidate, Policies.CanVoteCandidate);
                model.CanEditProjectCandidate = await _authorizationService.IsAuthorizedAsync(user, candidate, Policies.CanEditProjectCandidate);
                model.CanChangeProjectCandidateImage = await _authorizationService.IsAuthorizedAsync(user, candidate, Policies.CanChangeProjectCandidateImage);
                model.CanRemoveProjectCandidateImage = await _authorizationService.IsAuthorizedAsync(user, candidate, Policies.CanRemoveProjectCandidateImage);
                model.CanRemoveCandidate = await _authorizationService.IsAuthorizedAsync(user, candidate, Policies.CanRemoveCandidate);
                model.CanAddComment = await _authorizationService.IsAuthorizedAsync(user, candidate, Policies.CanWorkOnProjectContent);

                model.HasUserVoted = candidate.Votes.Any(x => x.UserId == user.GetId());
                if (model.HasUserVoted)
                {
                    SetUserVotes(model, candidate, user.GetId());
                }

                return model;
            }

            private void SetUserVotes(CandidateViewModel model, Candidate candidate, Guid userId)
            {
                var userVote = candidate.Votes.Single(x => x.UserId == userId);
                model.UserVotesViewModel = new UserVotesViewModel
                {
                    Functionality = userVote.Functionality,
                    Performace = userVote.Performance,
                    Usability = userVote.Usability,
                    Safety = userVote.Safety
                };
            }

            private void CalculateScorePercentages(CandidateViewModel model, Candidate candidate)
            {
                if (!candidate.Votes.Any())
                {
                    return;
                }
                var allVoteCount = candidate.Votes.Count;

                var veryGoodVotesCount = candidate.Votes.Count(x => x.Score > 15);
                var goodVotesCount = candidate.Votes.Count(x => x.Score > 10 && x.Score < 16);
                var mediocreVotesCount = candidate.Votes.Count(x => x.Score > 5 && x.Score < 11);
                var poorVotesCount = candidate.Votes.Count(x => x.Score < 6);

                model.ScorePercentageVeryGood = (decimal) veryGoodVotesCount / allVoteCount * 100;
                model.ScorePercentageGood = (decimal) goodVotesCount / allVoteCount * 100;
                model.ScorePercentageMediocre = (decimal) mediocreVotesCount / allVoteCount * 100;
                model.ScorePercentagePoor = (decimal) poorVotesCount / allVoteCount * 100;
            }
        }
    }
}