﻿using AutoMapper;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Assignments;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Users;
using Ubora.Web._Features.Projects.Dashboard;
using Ubora.Web._Features.Projects.Workpackages.Reviews;
using Ubora.Web._Features.Projects.Workpackages.Steps;
using Ubora.Web._Features.Users.Profile;
using Ubora.Web._Features.Projects.Repository;
using Ubora.Domain.Projects.Repository;
using Ubora.Domain.Projects._Commands;
using Ubora.Web._Features.Projects.Assignments;
using Ubora.Domain.Projects.Candidates;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Web._Features.Projects.Workpackages.Candidates;
using Ubora.Web._Features.Users.UserList.Models;

namespace Ubora.Web._Features._Shared
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Assignment, AssignmentListItemViewModel>();

            CreateMap<Assignment, AssignmentViewModel>()
                .ForMember(dest => dest.AssigneeIds, o => o.Ignore())
                .ForMember(dest => dest.ProjectMembers, o => o.Ignore());

            CreateMap<Assignment, EditAssignmentViewModel>()
                .ForMember(dest => dest.AssigneeIds, o => o.Ignore())
                .ForMember(dest => dest.ProjectMembers, o => o.Ignore());

            CreateMap<ProjectFile, ProjectFileViewModel>();

            CreateMap<ProjectFile, UpdateFileViewModel>()
                .ForMember(dest => dest.ProjectFile, o => o.Ignore())
                .ForMember(dest => dest.FileId, o => o.MapFrom(src => src.Id));

            CreateMap<Project, UpdateProjectCommand>()
                .ForMember(dest => dest.ProjectId, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Actor, o => o.Ignore());

            CreateMap<WorkpackageStep, EditStepViewModel>()
                .ForMember(dest => dest.StepId, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.ReadStepUrl, o => o.Ignore())
                .ForMember(dest => dest.EditStepUrl, o => o.Ignore())
                .ForMember(dest => dest.ContentQuillDelta, o => o.Ignore());

            CreateMap<WorkpackageStep, ReadStepViewModel>()
                .IncludeBase<WorkpackageStep, EditStepViewModel>()
                .ForMember(dest => dest.EditButton, o => o.Ignore())
                .ForMember(dest => dest.ContentHtml, o => o.Ignore());

            CreateMap<UserProfile, UserListItemViewModel>()
                .ForMember(dest => dest.ProfilePictureLink, o => o.Ignore())
                .ForMember(dest => dest.IsInvited, o => o.Ignore());

            CreateMap<UserProfile, ProfileViewModel>()
                .ForMember(dest => dest.ProfilePictureLink, o => o.Ignore())
                .ForMember(dest => dest.IsVerifiedMentor, o => o.Ignore())
                .ForMember(dest => dest.IsUnverifedMentor, o => o.Ignore())
                .ForMember(dest => dest.CountryEnglishName, o => o.MapFrom(src => src.Country.DisplayName))
                .ForMember(dest => dest.UserProjects, o => o.Ignore());

            CreateMap<UserProfile, UserProfileViewModel>()
                .ForMember(dest => dest.CountryCode, o => o.MapFrom(src => src.Country.Code));

            CreateMap<Project, ProjectOverviewViewModel>();
            CreateMap<Project, ProjectDashboardViewModel>()
                .ForMember(dest => dest.IsProjectMember, o => o.Ignore())
                .ForMember(dest => dest.ImagePath, o => o.Ignore())
                .ForMember(dest => dest.DescriptionHtml, o => o.Ignore())
                .ForMember(dest => dest.DeviceClassification, o => o.Ignore());

            CreateMap<WorkpackageReview, WorkpackageReviewViewModel>();

            CreateMap<Candidate, CandidateItemViewModel>()
                .ForMember(dest => dest.ImageUrl, o => o.Ignore());

            CreateMap<Candidate, EditCandidateViewModel>();
            CreateMap<Candidate, EditCandidateImageViewModel>()
                .ForMember(dest => dest.Image, o => o.Ignore());
            CreateMap<Candidate, RemoveCandidateImageViewModel>();

            CreateMap<WhereWillTechnologyBeUsed, WhereWillTechnologyBeUsedViewModel>();

            CreateMap<FacilityRequirementsInformation, FacilityRequirementsViewModel>();
        }
    }
}
