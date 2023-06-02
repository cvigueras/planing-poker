using AutoMapper;
using MediatR;
using PlaningPoker.Api.Cards.Models;
using PlaningPoker.Api.Cards.Repositories;

namespace PlaningPoker.Api.Cards.Queries;

public class GetAllCardsListQueryHandler : IRequestHandler<GetAllCardsListQuery, IEnumerable<CardReadDto>>
{
    private readonly ICardRepository cardRepository;
    private readonly IMapper mapper;

    public GetAllCardsListQueryHandler(ICardRepository cardRepository, IMapper mapper)
    {
        this.cardRepository = cardRepository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<CardReadDto>> Handle(GetAllCardsListQuery request, CancellationToken cancellationToken)
    {
        var cards = await cardRepository.GetAll();
        return mapper.Map<IEnumerable<CardReadDto>>(cards);
    }
}