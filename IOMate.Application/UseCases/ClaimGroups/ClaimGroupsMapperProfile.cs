using AutoMapper;
using IOMate.Application.UseCases.ClaimGroups.CreateClaimGroup;
using IOMate.Application.UseCases.ClaimGroups.GetUserGroups;
using IOMate.Domain.Entities;

namespace IOMate.Application.UseCases.ClaimGroups
{
    public class ClaimGroupsMapperProfile : Profile
    {
        public ClaimGroupsMapperProfile()
        {
            CreateMap<ClaimGroup, CreateClaimGroupResponseDto>();
            CreateMap<ClaimGroup, UserClaimGroupResponseDto>();
            CreateMap<ResourceClaim, ResourceClaimDto>();
        }
    }
}