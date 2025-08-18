using IOMate.Application.Shared.Dtos;
using MediatR;

namespace IOMate.Application.UseCases.Users.GetAllUsers
{
    public sealed record GetAllUsersRequestDto(int PageNumber = 1, int PageSize = 10)
        : IRequest<PagedResponseDto<GetAllUsersResponseDto>>;
}
