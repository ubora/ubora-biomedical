using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Users;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.ImageServices;

namespace Ubora.Web._Features.Projects.Workpackages.Steps
{
    public class CandidateViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool HasImage { get; set; }

        public AddCommentViewModel AddCommentViewModel { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }

    public class CommentViewModel
    {
        public Guid CommentatorId { get; set; }
        public string CommentatorName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string CommentText { get; set; }

        public class Factory
        {
            private readonly IQueryProcessor _queryProcessor;
            private readonly ImageStorageProvider _imageStorageProvider;

            public Factory(IQueryProcessor queryProcessor, ImageStorageProvider imageStorageProvider)
            {
                _queryProcessor = queryProcessor;
                _imageStorageProvider = imageStorageProvider;
            }

            protected Factory()
            {
            }

            public virtual CommentViewModel Create(Comment comment)
            {
                var model = new CommentViewModel();

                model.CommentatorId = comment.UserId;
                model.CommentText = comment.Text;

                var userProfile = _queryProcessor.FindById<UserProfile>(comment.UserId);
                model.CommentatorName = userProfile.FullName;
                model.ProfilePictureUrl = _imageStorageProvider.GetDefaultOrBlobUrl(userProfile);

                return model;
            }
        }
    }
}
