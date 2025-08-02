using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.Handlers.PlayerHandlers.GetPlayerIdByIdentityId;

public record GetPlayerIdByIdentityIdQuery(Guid GameId, Guid IdentityId) : IQuery<Guid>
{
}
