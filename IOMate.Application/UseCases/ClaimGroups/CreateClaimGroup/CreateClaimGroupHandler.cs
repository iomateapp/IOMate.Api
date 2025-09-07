using AutoMapper;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.ClaimGroups.CreateClaimGroup
{
    public class CreateClaimGroupHandler : IRequestHandler<CreateClaimGroupCommand, CreateClaimGroupResponseDto>
    {
        private readonly IClaimGroupRepository _claimGroupRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateClaimGroupHandler(
            IClaimGroupRepository claimGroupRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _claimGroupRepository = claimGroupRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateClaimGroupResponseDto> Handle(CreateClaimGroupCommand request, CancellationToken cancellationToken)
        {
            var claimGroup = new ClaimGroup
            {
                Name = request.Name,
                Description = request.Description
            };

            _claimGroupRepository.Add(claimGroup);
            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<CreateClaimGroupResponseDto>(claimGroup);
        }
    }
}