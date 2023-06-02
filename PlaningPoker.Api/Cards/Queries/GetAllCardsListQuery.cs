using MediatR;
using PlaningPoker.Api.Cards.Models;

namespace PlaningPoker.Api.Cards.Queries;

public class GetAllCardsListQuery : IRequest<IEnumerable<CardReadDto>> { }