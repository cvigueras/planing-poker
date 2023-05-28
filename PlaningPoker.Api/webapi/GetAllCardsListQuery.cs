using MediatR;

namespace webapi;

public class GetAllCardsListQuery : IRequest<IEnumerable<CardReadDto>> { }