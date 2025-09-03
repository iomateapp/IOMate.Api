using AutoMapper;
using IOMate.Domain.Entities;
using IOMate.Domain.Interfaces;
using MediatR;

namespace IOMate.Application.UseCases.Events.GetEvents
{
    public class GetEventsHandler<TEntity> : IRequestHandler<GetEventsRequestDto, List<GetEventResponseDto>>
        where TEntity : BaseEntity
    {
        private readonly IBaseRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public GetEventsHandler(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<GetEventResponseDto>> Handle(GetEventsRequestDto request, CancellationToken cancellationToken)
        {
            var events = await _repository.GetEntityEventsWithOwnerAsync(request.EntityId, cancellationToken);

            var ownerIds = events.Select(e => e.OwnerId).Distinct().ToList();
            var owners = await _repository.GetOwnersByIdsAsync(ownerIds, cancellationToken);

            var ownerDict = owners.ToDictionary(u => u.Id);

            var dtos = events.Select(e =>
            {
                var dto = _mapper.Map<GetEventResponseDto>(e);
                dto.Owner = ownerDict.TryGetValue(e.OwnerId, out var owner)
                    ? _mapper.Map<UserOwnerDto>(owner)
                    : null;
                return dto;
            }).ToList();

            return dtos;
        }
    }
}