using MediatR;

namespace webapi.Controllers;

public class GetAllCardsListQuery : IRequest<IEnumerable<Card>> { }