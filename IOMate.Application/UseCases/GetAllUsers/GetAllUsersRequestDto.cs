using MediatR;

namespace IOMate.Application.UseCases.GetAllUsers
{
    public sealed record GetAllUsersRequestDto : IRequest<List<GetAllUsersResponseDto>>;
}
