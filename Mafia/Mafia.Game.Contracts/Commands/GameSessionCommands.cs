
using FluentResults;
using Mafia.Game.Contracts.CommandDTOs;
using Mafia.Game.Domain.ValueObjects;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Game.Contracts.Commands;

// Публичные команды для межмодульного взаимодействия:

public record StartGameCommand(GameSettingsDto GameSettings, List<Guid> PlayersIdentityId) : ICommand<Result> { };





// Internal команды для внутри модульных действии:

public record PerformActionCommand(Guid GameId, Guid ActorId, Guid TargetId, ActionType ActionType) : ICommand<Result> { };