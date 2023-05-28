using MediatR;
using webapi.Cards.Models;

namespace webapi.Cards.Queries;

public class GetAllCardsListQuery : IRequest<IEnumerable<CardReadDto>> { }