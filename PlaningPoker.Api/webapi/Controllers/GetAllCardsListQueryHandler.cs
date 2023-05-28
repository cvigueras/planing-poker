using MediatR;

namespace webapi.Controllers;

public class GetAllCardsListQueryHandler : IRequestHandler<GetAllCardsListQuery, IEnumerable<Card>>
{
    private readonly ICardRepository cardRepository;

    public GetAllCardsListQueryHandler(ICardRepository cardRepository)
    {
        this.cardRepository = cardRepository;
    }

    public async Task<IEnumerable<Card>> Handle(GetAllCardsListQuery request, CancellationToken cancellationToken)
    {
        return await cardRepository.GetAll();
    }
}