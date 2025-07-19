
using FluentResults;
using Mafia.Games.Domain.ValueObjects;
using Mafia.Games.Contracts.CommandDTOs;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Contracts.Commands;

// Публичные команды для межмодульного взаимодействия:

public record StartGameCommand(GameSettingsDto GameSettings, List<Guid> PlayersIdentityId) : ICommand<Result> { };





// Internal команды для внутри модульных действии:

public record PerformActionCommand(Guid GameId, Guid ActorId, Guid TargetId, ActionType ActionType) : ICommand<Result> { };