using AutoMapper;
using IOMate.Application.Shared.Dtos;
using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.GetAllUsers
{
    public sealed class GetAllUsersHandler : IRequestHandler<GetAllUsersRequestDto, PagedResponseDto<GetAllUsersResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponseDto<GetAllUsersResponseDto>> Handle(GetAllUsersRequestDto request, CancellationToken cancellationToken)
        {
            var total = await _userRepository.CountAsync(cancellationToken);
            var users = await _userRepository.GetPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
            var mapped = _mapper.Map<List<GetAllUsersResponseDto>>(users);

            return new PagedResponseDto<GetAllUsersResponseDto>
            {
                TotalCount = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Results = mapped
            };
        }
    }
}